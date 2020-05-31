using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace microclr
{
	struct ExecutionContext
	{
		MethodInfo method;
		Variable[] args;

		byte[] il;
		int ip;

		MicroClr VM;
		MicroClrStack Stack;
		MicroClrHeap Heap;

		private static readonly MethodInfo initArray = typeof(RuntimeHelpers).GetMethod("InitializeArray")!;
		private static readonly MethodInfo indexStr = typeof(string).GetMethod("get_Chars", new Type[] { typeof(int) })!;
		private static readonly MethodInfo strLength = typeof(string).GetMethod("get_Length")!;

		public ExecutionContext(MethodInfo method, object[] args, MicroClr vm) : this(method, ConvertArgs(vm, method, args), vm)
		{
		}

		public ExecutionContext(MethodInfo method, Variable[] args, MicroClr vm)
		{
			if (!method.IsStatic)
			{
				throw new NonStaticMethodException(method);
			}

			this.method = method;
			this.args = args;
			VM = vm;
			Stack = vm.Stack;
			Heap = vm.Heap;

			il = method.GetMethodBody().GetILAsByteArray();
			ip = 0;
		}

		private static Variable[] ConvertArgs(MicroClr vm, MethodInfo method, object[] args)
		{
			if (method.GetParameters().Length != args.Length)
			{
				throw new ParameterCountException(method.GetParameters().Length, args.Length);
			}

			for (int i = 0; i < args.Length; i++)
			{
				var p = method.GetParameters()[i];
				if (!p.ParameterType.IsAssignableFrom(args[i].GetType()))
				{
					throw new ParameterTypeException(i, p.ParameterType, args[i].GetType());
				}
			}

			var ret = new Variable[args.Length];
			for (int i = 0; i < args.Length; i++)
			{
				var argT = method.GetParameters()[i].ParameterType;
				if (argT == typeof(sbyte))
				{
					ret[i] = new Variable((sbyte)args[i]);
				}
				else if (argT == typeof(byte))
				{
					ret[i] = new Variable((byte)args[i]);
				}
				else if (argT == typeof(short))
				{
					ret[i] = new Variable((short)args[i]);
				}
				else if (argT == typeof(ushort))
				{
					ret[i] = new Variable((ushort)args[i]);
				}
				else if (argT == typeof(int))
				{
					ret[i] = new Variable((int)args[i]);
				}
				else if (argT == typeof(uint))
				{
					ret[i] = new Variable((uint)args[i]);
				}
				else if (argT == typeof(long))
				{
					ret[i] = new Variable((long)args[i]);
				}
				else if (argT == typeof(ulong))
				{
					ret[i] = new Variable((ulong)args[i]);
				}
				else if (argT == typeof(float))
				{
					ret[i] = new Variable((float)args[i]);
				}
				else if (argT == typeof(double))
				{
					ret[i] = new Variable((double)args[i]);
				}
				else if (argT == typeof(bool))
				{
					ret[i] = new Variable((bool)args[i] ? 1 : 0);
				}
				else if (argT == typeof(char))
				{
					ret[i] = new Variable((int)(char)args[i]);
				}
				else if (!argT.IsValueType)
				{
					ret[i] = new Variable((ulong)vm.Heap.Add(args[i]), VariableType.Object);
				}
				else
				{
					throw new NotImplementedException();
				}
			}

			return ret;
		}

		private static void ValidateArgs(MethodInfo method, object[] args)
		{
			if (method.GetParameters().Length != args.Length)
			{
				throw new ParameterCountException(method.GetParameters().Length, args.Length);
			}

			for (int i = 0; i < args.Length; i++)
			{
				var p = method.GetParameters()[i];
				if (p.ParameterType != args[i].GetType())
				{
					throw new ParameterTypeException(i, p.ParameterType, args[i].GetType());
				}
			}
		}

		public object Execute()
		{
			Span<Variable> locals = stackalloc Variable[method.GetMethodBody().LocalVariables.Count];

			while (true)
			{
				OpCodeValues opcode = ReadOpCode();

				// This page describes the behaviour of all the instructions.
				// https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes?view=netcore-3.1

				switch (opcode)
				{
					#region Miscellaneous
					// Do nothing
					case OpCodeValues.Nop:
						break;

					case OpCodeValues.Pop:
						Stack.Pop();
						break;

					case OpCodeValues.Dup:
						Stack.Push(Stack.Peek());
						break;

					case OpCodeValues.Ldtoken:
						Stack.PushInt(ReadInt());
						break;
					#endregion

					#region Argument loading
					case OpCodeValues.Ldarg:
						Stack.Push(args[ReadShort()]);
						break;
					case OpCodeValues.Ldarg_S:
						Stack.Push(args[il[ip++]]);
						break;
					case OpCodeValues.Ldarg_0:
						Stack.Push(args[0]);
						break;
					case OpCodeValues.Ldarg_1:
						Stack.Push(args[1]);
						break;
					case OpCodeValues.Ldarg_2:
						Stack.Push(args[2]);
						break;
					case OpCodeValues.Ldarg_3:
						Stack.Push(args[3]);
						break;
					#endregion

					#region Store a value to a local variable
					case OpCodeValues.Stloc:
						locals[ReadUShort()] = Stack.Pop();
						break;
					case OpCodeValues.Stloc_S:
						locals[il[ip++]] = Stack.Pop();
						break;
					case OpCodeValues.Stloc_0:
						locals[0] = Stack.Pop();
						break;
					case OpCodeValues.Stloc_1:
						locals[1] = Stack.Pop();
						break;
					case OpCodeValues.Stloc_2:
						locals[2] = Stack.Pop();
						break;
					case OpCodeValues.Stloc_3:
						locals[3] = Stack.Pop();
						break;
					#endregion

					#region Push a local to the stack
					case OpCodeValues.Ldloc:
						Stack.Push(locals[ReadUShort()]);
						break;
					case OpCodeValues.Ldloc_S:
						Stack.Push(locals[il[ip++]]);
						break;
					case OpCodeValues.Ldloc_0:
						Stack.Push(locals[0]);
						break;
					case OpCodeValues.Ldloc_1:
						Stack.Push(locals[1]);
						break;
					case OpCodeValues.Ldloc_2:
						Stack.Push(locals[2]);
						break;
					case OpCodeValues.Ldloc_3:
						Stack.Push(locals[3]);
						break;
					#endregion

					#region Push literal to the stack
					case OpCodeValues.Ldnull:
						//Stack.PushULong(0);
						Stack.Push(new Variable(0, VariableType.Object));
						break;
					case OpCodeValues.Ldc_I4:
						Stack.PushInt(ReadInt());
						break;
					case OpCodeValues.Ldc_I4_S:
						Stack.PushInt((sbyte)il[ip++]);
						break;
					case OpCodeValues.Ldc_I4_0:
						Stack.PushInt(0);
						break;
					case OpCodeValues.Ldc_I4_1:
						Stack.PushInt(1);
						break;
					case OpCodeValues.Ldc_I4_2:
						Stack.PushInt(2);
						break;
					case OpCodeValues.Ldc_I4_3:
						Stack.PushInt(3);
						break;
					case OpCodeValues.Ldc_I4_4:
						Stack.PushInt(4);
						break;
					case OpCodeValues.Ldc_I4_5:
						Stack.PushInt(5);
						break;
					case OpCodeValues.Ldc_I4_6:
						Stack.PushInt(6);
						break;
					case OpCodeValues.Ldc_I4_7:
						Stack.PushInt(7);
						break;
					case OpCodeValues.Ldc_I4_8:
						Stack.PushInt(8);
						break;
					case OpCodeValues.Ldc_I4_M1:
						Stack.PushInt(-1);
						break;
					case OpCodeValues.Ldc_I8:
						Stack.PushLong(ReadLong());
						break;
					case OpCodeValues.Ldc_R4:
						Stack.PushFloat(ReadFloat());
						break;
					case OpCodeValues.Ldc_R8:
						Stack.PushDouble(ReadDouble());
						break;
					#endregion

					#region Comparisons
					case OpCodeValues.Ceq:
						Stack.PushInt(Stack.PopULong() == Stack.PopULong() ? 1 : 0);
						break;
					case OpCodeValues.Clt:
						// Flip the sign because of the order the parameters
						// are pushed onto the stack.
						Stack.PushInt(Stack.PopLong() > Stack.PopLong() ? 1 : 0);
						break;
					case OpCodeValues.Cgt:
						Stack.PushInt(Stack.PopLong() < Stack.PopLong() ? 1 : 0);
						break;
					case OpCodeValues.Clt_Un:
						Stack.PushInt(Stack.PopULong() > Stack.PopULong() ? 1 : 0);
						break;
					case OpCodeValues.Cgt_Un:
						Stack.PushInt(Stack.PopULong() < Stack.PopULong() ? 1 : 0);
						break;
					#endregion

					#region Branch
					case OpCodeValues.Br:
						ip += ReadInt() + 4;
						break;
					case OpCodeValues.Br_S:
						ip += (sbyte)il[ip] + 1;
						break;
					case OpCodeValues.Brtrue_S:
						if (Stack.PopULong() != 0)
						{
							ip += (sbyte)il[ip];
						}
						ip++;
						break;
					case OpCodeValues.Brfalse_S:
						if (Stack.PopULong() == 0)
						{
							ip += (sbyte)il[ip];
						}
						ip++;
						break;

					case OpCodeValues.Blt_S:
						var target = (sbyte)il[ip++];
						if (Stack.PopULong() > Stack.PopULong())
						{
							ip += target;
						}
						break;
					//case OpCodeValues.Brtrue:
					//	if (Stack.PopULong() != 0)
					//	{
					//		ip += ReadInt();
					//		break;
					//	}
					//	ip += 4;
					//	break;
					//case OpCodeValues.Brfalse:
					//	if (Stack.PopULong() == 0)
					//	{
					//		ip += ReadInt();
					//		break;
					//	}
					//	ip += 4;
					//	break;
					//case OpCodeValues.Beq:
					//	if (Stack.PopULong() == Stack.PopULong())
					//	{
					//		ip += ReadInt();
					//		break;
					//	}
					//	ip += 4;
					//	break;
					case OpCodeValues.Beq_S:
						if (Stack.PopULong() == Stack.PopULong())
						{
							ip += (sbyte)il[ip];
						}
						ip++;
						break;
					#endregion

					#region Operators
					case OpCodeValues.Add:
						if (Stack.Peek().Type == VariableType.Float)
						{
							// For correctness I should check if both arguments are float,
							// however that will make it slower and the check isn't really
							// needed because the code being run is generated by the C#
							// compiler, so it is safe to assume the IL is valid.
							Stack.PushFloat(Stack.PopFloat() + Stack.PopFloat());
							break;
						}
						else if (Stack.Peek().Type == VariableType.Double)
						{
							Stack.PushDouble(Stack.PopDouble() + Stack.PopDouble());
							break;
						}
						Stack.PushLong(Stack.PopLong() + Stack.PopLong());
						break;
					case OpCodeValues.Sub:
						if (Stack.Peek().Type == VariableType.Float)
						{
							Stack.PushFloat(-Stack.PopFloat() + Stack.PopFloat());
							break;
						}
						else if (Stack.Peek().Type == VariableType.Double)
						{
							Stack.PushDouble(-Stack.PopDouble() + Stack.PopDouble());
							break;
						}
						Stack.PushLong(-Stack.PopLong() + Stack.PopLong());
						break;
					case OpCodeValues.Mul:
						if (Stack.Peek().Type == VariableType.Float)
						{
							Stack.PushFloat(Stack.PopFloat() * Stack.PopFloat());
							break;
						}
						else if (Stack.Peek().Type == VariableType.Double)
						{
							Stack.PushDouble(Stack.PopDouble() * Stack.PopDouble());
							break;
						}
						Stack.PushLong(Stack.PopLong() * Stack.PopLong());
						break;
					case OpCodeValues.Div:
						if (Stack.Peek().Type == VariableType.Float)
						{
							var divisorf = Stack.PopFloat();
							var dividendf = Stack.PopFloat();
							Stack.PushFloat(dividendf / divisorf);
							break;
						}
						else if (Stack.Peek().Type == VariableType.Double)
						{
							var divisord = Stack.PopDouble();
							var dividendd = Stack.PopDouble();
							Stack.PushDouble(dividendd / divisord);
							break;
						}
						var divisor = Stack.PopLong();
						var dividend = Stack.PopLong();
						Stack.PushLong(dividend / divisor);
						break;
					case OpCodeValues.Rem:
						divisor = Stack.PopLong();
						dividend = Stack.PopLong();
						Stack.PushLong(dividend % divisor);
						break;
					case OpCodeValues.Neg:
						if (Stack.Peek().Type == VariableType.Float)
						{
							Stack.PushFloat(-Stack.PopFloat());
							break;
						}
						else if (Stack.Peek().Type == VariableType.Double)
						{
							Stack.PushDouble(-Stack.PopDouble());
							break;
						}
						Stack.PushLong(-Stack.PopLong());
						break;
					#endregion

					#region Bitwise operators
					case OpCodeValues.And:
						Stack.PushLong(Stack.PopLong() & Stack.PopLong());
						break;
					case OpCodeValues.Or:
						Stack.PushLong(Stack.PopLong() | Stack.PopLong());
						break;
					case OpCodeValues.Xor:
						Stack.PushLong(Stack.PopLong() ^ Stack.PopLong());
						break;
					case OpCodeValues.Not:
						Stack.PushLong(~Stack.PopLong());
						break;
					case OpCodeValues.Shl:
						int shift = Stack.PopInt();
						long x = Stack.PopLong();
						Stack.PushLong(x << shift);
						break;
					case OpCodeValues.Shr:
						shift = Stack.PopInt();
						x = Stack.PopLong();
						Stack.PushLong(x >> shift);
						break;
					#endregion

					#region Cast
					case OpCodeValues.Conv_I:
					case OpCodeValues.Conv_I4:
						var v = Stack.Pop();
						if (v.Type == VariableType.Float)
						{
							Stack.PushInt((int)BitConverter.Int32BitsToSingle((int)v.Value));
						}
						else if (v.Type == VariableType.Double)
						{
							Stack.PushInt((int)BitConverter.Int64BitsToDouble((long)v.Value));
						}
						else
						{
							Stack.PushInt((int)v.Value);
						}
						break;
					case OpCodeValues.Conv_I1:
						v = Stack.Pop();
						if (v.Type == VariableType.Float)
						{
							Stack.PushInt((sbyte)BitConverter.Int32BitsToSingle((int)v.Value));
						}
						else if (v.Type == VariableType.Double)
						{
							Stack.PushInt((sbyte)BitConverter.Int64BitsToDouble((long)v.Value));
						}
						else
						{
							Stack.PushInt((sbyte)v.Value);
						}
						break;
					case OpCodeValues.Conv_I2:
						v = Stack.Pop();
						if (v.Type == VariableType.Float)
						{
							Stack.PushInt((short)BitConverter.Int32BitsToSingle((int)v.Value));
						}
						else if (v.Type == VariableType.Double)
						{
							Stack.PushInt((short)BitConverter.Int64BitsToDouble((long)v.Value));
						}
						else
						{
							Stack.PushInt((short)v.Value);
						}
						break;
					case OpCodeValues.Conv_I8:
						v = Stack.Pop();
						if (v.Type == VariableType.Float)
						{
							Stack.PushLong((long)BitConverter.Int32BitsToSingle((int)v.Value));
						}
						else if (v.Type == VariableType.Double)
						{
							Stack.PushLong((long)BitConverter.Int64BitsToDouble((long)v.Value));
						}
						else
						{
							Stack.PushLong((long)v.Value);
						}
						break;
					case OpCodeValues.Conv_U:
					case OpCodeValues.Conv_U4:
						v = Stack.Pop();
						if (v.Type == VariableType.Float)
						{
							Stack.PushULong((uint)BitConverter.Int32BitsToSingle((int)v.Value));
						}
						else if (v.Type == VariableType.Double)
						{
							Stack.PushULong((uint)BitConverter.Int64BitsToDouble((long)v.Value));
						}
						else
						{
							Stack.PushULong((uint)v.Value);
						}
						break;
					case OpCodeValues.Conv_U1:
						v = Stack.Pop();
						if (v.Type == VariableType.Float)
						{
							Stack.PushULong((byte)BitConverter.Int32BitsToSingle((int)v.Value));
						}
						else if (v.Type == VariableType.Double)
						{
							Stack.PushULong((byte)BitConverter.Int64BitsToDouble((long)v.Value));
						}
						else
						{
							Stack.PushULong((byte)v.Value);
						}
						break;
					case OpCodeValues.Conv_U2:
						v = Stack.Pop();
						if (v.Type == VariableType.Float)
						{
							Stack.PushULong((ushort)BitConverter.Int32BitsToSingle((int)v.Value));
						}
						else if (v.Type == VariableType.Double)
						{
							Stack.PushULong((ushort)BitConverter.Int64BitsToDouble((long)v.Value));
						}
						else
						{
							Stack.PushULong((ushort)v.Value);
						}
						break;
					case OpCodeValues.Conv_U8:
						v = Stack.Pop();
						if (v.Type == VariableType.Float)
						{
							Stack.PushULong((ulong)BitConverter.Int32BitsToSingle((int)v.Value));
						}
						else if (v.Type == VariableType.Double)
						{
							Stack.PushULong((ulong)BitConverter.Int64BitsToDouble((long)v.Value));
						}
						else
						{
							Stack.PushULong((ulong)v.Value);
						}
						break;

					case OpCodeValues.Conv_R_Un:
						// There is no Conv_R8_Un instruction, so the compiler generates Conv_R_Un
						// and Conv_R8 to convert from unsigned int to double, which loses precision.
						// To get around this issue we detect a cast from uint to double by checking
						// if the next instruction after a Conv_R_Un is Conv_R8 and then skip the
						// cast to float.
						if ((OpCodeValues)il[ip] == OpCodeValues.Conv_R8)
						{
							goto case OpCodeValues.Conv_R8;
						}
						else
						{
							goto case OpCodeValues.Conv_R4;
						}
					case OpCodeValues.Conv_R4:
						v = Stack.Pop();
						if (v.Type == VariableType.Int)
						{
							Stack.PushFloat((float)(long)v.Value);
						}
						else if (v.Type == VariableType.UInt)
						{
							Stack.PushFloat((float)v.Value);
						}
						else if (v.Type == VariableType.Float)
						{
							Stack.Push(v);
						}
						else if (v.Type == VariableType.Double)
						{
							Stack.PushFloat((float)BitConverter.Int64BitsToDouble((long)v.Value));
						}
						else
						{
							throw new NotSupportedException();
						}
						break;
					case OpCodeValues.Conv_R8:
						v = Stack.Pop();
						if (v.Type == VariableType.Int)
						{
							Stack.PushDouble((double)(long)v.Value);
						}
						else if (v.Type == VariableType.UInt)
						{
							Stack.PushDouble((double)v.Value);
						}
						else if (v.Type == VariableType.Float)
						{
							Stack.PushDouble((double)BitConverter.Int32BitsToSingle((int)v.Value));
						}
						else if (v.Type == VariableType.Double)
						{
							Stack.Push(v);
						}
						else
						{
							throw new NotSupportedException();
						}
						break;

					case OpCodeValues.Castclass:
						var type = method.Module.ResolveType(ReadInt());
						var ob = Stack.Pop();
						//if (ob.Type != VariableType.Object)
						//{
						//	throw new InvalidCastException(type, Heap.Objects[(int)ob.Value].GetType());
						//}

						var real_o = Heap[(int)ob.Value];
						if (!type.IsAssignableFrom(real_o.GetType()))
						{
							throw new InvalidCastException(type, real_o.GetType());
						}

						Stack.Push(new Variable((ulong)Heap.Add(Convert.ChangeType(real_o, type)), VariableType.Object));
						break;
					#endregion

					#region Switch
					case OpCodeValues.Switch:
						int n = ReadInt();
						var value = Stack.PopInt();
						if (value < n && value >= 0)
						{
							var jumpTable = MemoryMarshal.Cast<byte, int>(il.AsSpan(ip, n * sizeof(int)));
							ip += n * sizeof(int) + jumpTable[value];
						}
						else
						{
							ip += n * sizeof(int);
						}
						break;
					#endregion

					#region Load string
					case OpCodeValues.Ldstr:
						var strToken = ReadInt();
						var str = method.Module.ResolveString(strToken);
						Stack.PushObject(Heap.Add(str));
						break;
					#endregion

					#region Box
					case OpCodeValues.Box:
						var boxType = method.Module.ResolveType(ReadInt());
						var val = Stack.Pop().Value;
						object o;
						if (boxType == typeof(sbyte))
						{
							o = (sbyte)val;
						}
						else if (boxType == typeof(byte))
						{
							o = (byte)val;
						}
						else if (boxType == typeof(short))
						{
							o = (short)val;
						}
						else if (boxType == typeof(ushort))
						{
							o = (ushort)val;
						}
						else if (boxType == typeof(int))
						{
							o = (int)val;
						}
						else if (boxType == typeof(uint))
						{
							o = (uint)val;
						}
						else if (boxType == typeof(long))
						{
							o = (long)val;
						}
						else if (boxType == typeof(ulong))
						{
							o = (ulong)val;
						}
						else if (boxType == typeof(float))
						{
							o = BitConverter.Int32BitsToSingle((int)val);
						}
						else if (boxType == typeof(double))
						{
							o = BitConverter.Int64BitsToDouble((long)val);
						}
						else if (boxType == typeof(bool))
						{
							o = val == 1 ? true : false;
						}
						else
						{
							throw new NotSupportedException();
						}
						Stack.Push(new Variable((ulong)Heap.Add(o), VariableType.Object));
						break;
					#endregion

					#region Unbox
					case OpCodeValues.Unbox:
					case OpCodeValues.Unbox_Any:
						o = Heap[Stack.PopInt()];
						ReadInt();
						if (o is sbyte)
						{
							Stack.PushLong((sbyte)o);
						}
						else if (o is byte)
						{
							Stack.PushULong((byte)o);
						}
						else if (o is short)
						{
							Stack.PushLong((short)o);
						}
						else if (o is ushort)
						{
							Stack.PushULong((ushort)o);
						}
						else if (o is int)
						{
							Stack.PushLong((int)o);
						}
						else if (o is uint)
						{
							Stack.PushULong((uint)o);
						}
						else if (o is long)
						{
							Stack.PushLong((long)o);
						}
						else if (o is ulong)
						{
							Stack.PushULong((ulong)o);
						}
						else if (o is float)
						{
							Stack.PushFloat((float)o);
						}
						else if (o is double)
						{
							Stack.PushDouble((double)o);
						}
						break;
					#endregion

					#region Arrays
					case OpCodeValues.Newarr:
						var etype = method.Module.ResolveType(ReadInt());
						var length = Stack.PopInt();
						var array = Array.CreateInstance(etype, length);
						Stack.Push(new Variable((ulong)Heap.Add(array), VariableType.Object));
						break;

					case OpCodeValues.Ldlen:
						array = (Array)Heap[Stack.PopInt()];
						Stack.PushInt(array.Length);
						break;

					case OpCodeValues.Ldelem_I:
					case OpCodeValues.Ldelem_I4:
						var index = Stack.PopInt();
						var arr = (int[])Heap[Stack.PopInt()];
						Stack.PushInt(arr[index]);
						break;

					case OpCodeValues.Stelem_I:
					case OpCodeValues.Stelem_I4:
						var intVal = Stack.PopInt();
						index = Stack.PopInt();
						arr = (int[])Heap[Stack.PopInt()];
						arr[index] = intVal;
						break;

					case OpCodeValues.Ldelem_R4:
						index = Stack.PopInt();
						var fltArr = (float[])Heap[Stack.PopInt()];
						Stack.PushFloat(fltArr[index]);
						break;

					case OpCodeValues.Stelem_R4:
						var floatVal = Stack.PopFloat();
						index = Stack.PopInt();
						fltArr = (float[])Heap[Stack.PopInt()];
						fltArr[index] = floatVal;
						break;

					case OpCodeValues.Ldelem_Ref:
						index = Stack.PopInt();
						var oarr = (object[])Heap[Stack.PopInt()];
						Stack.Push(new Variable((ulong)Heap.Add(oarr[index]), VariableType.Object));
						break;
					case OpCodeValues.Stelem_Ref:
						var refVal = Heap[Stack.PopInt()];
						index = Stack.PopInt();
						oarr = (object[])Heap[Stack.PopInt()];
						oarr[index] = refVal;
						break;

					case OpCodeValues.Callvirt:
						var virt = method.Module.ResolveMethod(ReadInt());
						if (virt == strLength)
						{
							Stack.PushInt(((string)Heap[Stack.PopInt()]).Length);
						}
						else if (virt == indexStr)
						{
							var charIndex = Stack.PopInt();
							var s = (string)Heap[Stack.PopInt()];
							Stack.PushInt(s[charIndex]);
						}
						else
						{
							goto default;
						}
						break;
					#endregion

					#region Is instance
					case OpCodeValues.Isinst:
						var obj = Heap[Stack.PopInt()];
						var t = method.Module.ResolveType(ReadInt());
						Stack.PushInt(t.IsInstanceOfType(obj) ? 1 : 0);
						break;
					#endregion

					#region Load static field
					case OpCodeValues.Ldsfld:
						var fld = method.Module.ResolveField(ReadInt());
						var fldVal = fld.GetValue(null);
						if (fldVal is sbyte)
						{
							Stack.PushLong((sbyte)fldVal);
						}
						else if (fldVal is byte)
						{
							Stack.PushULong((byte)fldVal);
						}
						else if (fldVal is short)
						{
							Stack.PushLong((short)fldVal);
						}
						else if (fldVal is ushort)
						{
							Stack.PushULong((ushort)fldVal);
						}
						else if (fldVal is int)
						{
							Stack.PushLong((int)fldVal);
						}
						else if (fldVal is uint)
						{
							Stack.PushULong((uint)fldVal);
						}
						else if (fldVal is long)
						{
							Stack.PushLong((long)fldVal);
						}
						else if (fldVal is ulong)
						{
							Stack.PushULong((ulong)fldVal);
						}
						else if (fldVal is float)
						{
							Stack.PushFloat((float)fldVal);
						}
						else if (fldVal is double)
						{
							Stack.PushDouble((double)fldVal);
						}
						else if (fldVal is object)
						{
							Stack.Push(new Variable((ulong)Heap.Add(fldVal), VariableType.Object));
						}
						break;
					#endregion

					#region Call
					case OpCodeValues.Call:
						var metadataToken = ReadInt();
						var m = (MethodInfo)method.Module.ResolveMethod(metadataToken);
						if (m == initArray)
						{
							InitArray();
							break;
						}

						// I'm not writing a whole CLR, so if the method is in a different module
						// let the real CLR execute it.
						if (m.Module == method.Module)
						{
							var nargs = m.GetParameters().Length;
							var margs = new Variable[nargs];
							for (int i = 0; i < nargs; i++)
							{
								margs[i] = Stack.Pop();
							}
							var ec = new ExecutionContext(m, margs, VM);
							ec.Execute();
						}
						else
						{
							var p = m.GetParameters();
							var args = new object[p.Length];
							for (int i = args.Length - 1; i >= 0; i--)
							{
								var arg = Stack.Pop();
								var argT = p[i].ParameterType;
								if (argT == typeof(sbyte))
								{
									args[i] = (sbyte)arg.Value;
								}
								else if (argT == typeof(byte))
								{
									args[i] = (byte)arg.Value;
								}
								else if (argT == typeof(short))
								{
									args[i] = (short)arg.Value;
								}
								else if (argT == typeof(ushort))
								{
									args[i] = (ushort)arg.Value;
								}
								else if (argT == typeof(int))
								{
									args[i] = (int)arg.Value;
								}
								else if (argT == typeof(uint))
								{
									args[i] = (uint)arg.Value;
								}
								else if (argT == typeof(long))
								{
									args[i] = (long)arg.Value;
								}
								else if (argT == typeof(ulong))
								{
									args[i] = arg.Value;
								}
								else if (argT == typeof(float))
								{
									args[i] = BitConverter.Int32BitsToSingle((int)arg.Value);
								}
								else if (argT == typeof(double))
								{
									args[i] = BitConverter.Int64BitsToDouble((long)arg.Value);
								}
								else if (argT == typeof(bool))
								{
									args[i] = arg.Value == 1 ? true : false;
								}
								else if (argT == typeof(char))
								{
									args[i] = (char)arg.Value;
								}
								else if (!argT.IsValueType)
								{
									args[i] = Heap[(int)arg.Value];
								}
								else
								{
									throw new NotSupportedException();
								}
							}
							var ret = m.Invoke(null, args);
							var retT = m.ReturnType;
							if (retT != typeof(void))
							{
								if (retT == typeof(sbyte))
								{
									Stack.Push(new Variable((sbyte)ret));
								}
								else if (retT == typeof(byte))
								{
									Stack.Push(new Variable((byte)ret));
								}
								else if (retT == typeof(short))
								{
									Stack.Push(new Variable((short)ret));
								}
								else if (retT == typeof(ushort))
								{
									Stack.Push(new Variable((ushort)ret));
								}
								else if (retT == typeof(int))
								{
									Stack.Push(new Variable((int)ret));
								}
								else if (retT == typeof(uint))
								{
									Stack.Push(new Variable((uint)ret));
								}
								else if (retT == typeof(long))
								{
									Stack.Push(new Variable((long)ret));
								}
								else if (retT == typeof(ulong))
								{
									Stack.Push(new Variable((ulong)ret));
								}
								else if (retT == typeof(float))
								{
									Stack.PushFloat((float)ret);
								}
								else if (retT == typeof(double))
								{
									Stack.PushDouble((double)ret);
								}
								else if (retT == typeof(bool))
								{
									Stack.Push(new Variable((bool)ret ? 1 : 0));
								}
								else if (retT == typeof(char))
								{
									Stack.PushInt((char)ret);
								}
								else if (!retT.IsValueType)
								{
									Stack.Push(new Variable((ulong)Heap.Add(ret), VariableType.Object));
								}
								else
								{
									throw new NotSupportedException();
								}
							}
						}
						break;
					#endregion

					#region Return
					case OpCodeValues.Ret:
						// I feel like there may be a better way to do this than
						// switching on the return type and boxing the return value
						var retType = method.ReturnType;
						if (retType == typeof(void))
						{
							return null;
						}
						else
						{
							var ret = Stack.Peek();
							if (retType == typeof(sbyte))
							{
								return (sbyte)ret.Value;
							}
							else if (retType == typeof(byte))
							{
								return (byte)ret.Value;
							}
							else if (retType == typeof(short))
							{
								return (short)ret.Value;
							}
							else if (retType == typeof(ushort))
							{
								return (ushort)ret.Value;
							}
							else if (retType == typeof(int))
							{
								return (int)ret.Value;
							}
							else if (retType == typeof(uint))
							{
								return (uint)ret.Value;
							}
							else if (retType == typeof(long))
							{
								return (long)ret.Value;
							}
							else if (retType == typeof(ulong))
							{
								return ret.Value;
							}
							else if (retType == typeof(float))
							{
								return BitConverter.Int32BitsToSingle((int)ret.Value);
							}
							else if (retType == typeof(double))
							{
								return BitConverter.Int64BitsToDouble((long)ret.Value);
							}
							else if (retType == typeof(bool))
							{
								return ret.Value != 0;
							}
							else if (retType == typeof(char))
							{
								return (char)ret.Value;
							}
							else if (!retType.IsValueType)
							{
								return Heap[(int)ret.Value];
							}
							else
							{
								throw new NotImplementedException();
							}
						}
					#endregion

					default:
						throw new UnsupportedInstructionException(opcode);
				}
			}
		}

		private void InitArray()
		{
			var initData = method.Module.ResolveField(Stack.PopInt()).GetValue(null);
			var gch = GCHandle.Alloc(initData, GCHandleType.Pinned);
			var addr = gch.AddrOfPinnedObject();
			ReadOnlySpan<byte> data;
			unsafe
			{
				data = new ReadOnlySpan<byte>(addr.ToPointer(), Marshal.SizeOf(initData.GetType()));
			}
			var arr = Heap[Stack.PopInt()];
			Span<byte> arrData;
			if (arr is sbyte[] sb)
			{
				arrData = MemoryMarshal.AsBytes<sbyte>(sb);
			}
			else if (arr is byte[] b)
			{
				arrData = b;
			}
			else if (arr is short[] s)
			{
				arrData = MemoryMarshal.AsBytes<short>(s);
			}
			else if (arr is ushort[] us)
			{
				arrData = MemoryMarshal.AsBytes<ushort>(us);
			}
			else if (arr is int[] i)
			{
				arrData = MemoryMarshal.AsBytes<int>(i);
			}
			else if (arr is uint[] ui)
			{
				arrData = MemoryMarshal.AsBytes<uint>(ui);
			}
			else if (arr is long[] l)
			{
				arrData = MemoryMarshal.AsBytes<long>(l);
			}
			else if (arr is ulong[] ul)
			{
				arrData = MemoryMarshal.AsBytes<ulong>(ul);
			}
			else if (arr is float[] f)
			{
				arrData = MemoryMarshal.AsBytes<float>(f);
			}
			else if (arr is double[] d)
			{
				arrData = MemoryMarshal.AsBytes<double>(d);
			}
			else if (arr is bool[] bo)
			{
				arrData = MemoryMarshal.AsBytes<bool>(bo);
			}
			else
			{
				throw new NotSupportedException();
			}
			data.CopyTo(arrData);
			gch.Free();
		}

		public T Execute<T>()
		{
			return (T)Execute();
		}

		internal string Disassemble()
		{
			// TODO This is wrong and should be fixed

			StringBuilder sb = new StringBuilder();

			while (ip != il.Length)
			{
				var op = ReadOpCode();
				sb.AppendLine(op.ToString());
			}

			return sb.ToString();
		}

		private OpCodeValues ReadOpCode()
		{
			OpCodeValues opcode = (OpCodeValues)il[ip++];
			if (opcode > OpCodeValues.Prefix7)
			{
				opcode = (OpCodeValues)(il[ip++] | ((ushort)opcode << 8));
			}

			return opcode;
		}

		private short ReadShort()
		{
			short i = BitConverter.ToInt16(il, ip);
			ip += sizeof(short);
			return i;
		}

		private ushort ReadUShort()
		{
			ushort i = BitConverter.ToUInt16(il, ip);
			ip += sizeof(ushort);
			return i;
		}

		private int ReadInt()
		{
			int i = BitConverter.ToInt32(il, ip);
			ip += sizeof(int);
			return i;
		}

		private long ReadLong()
		{
			long i = BitConverter.ToInt64(il, ip);
			ip += sizeof(long);
			return i;
		}

		private float ReadFloat()
		{
			float f = BitConverter.ToSingle(il, ip);
			ip += sizeof(float);
			return f;
		}

		private double ReadDouble()
		{
			double d = BitConverter.ToDouble(il, ip);
			ip += sizeof(double);
			return d;
		}
	}
}
