using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace microclr
{
	struct ExecutionContext
	{
		MethodInfo method;
		byte[] il;
		int ip;

		MicroClrStack Stack;

		public ExecutionContext(MethodInfo method, MicroClrStack stack)
		{
			this.method = method;
			Stack = stack;

			il = method.GetMethodBody().GetILAsByteArray();
			ip = 0;
		}

		public T Execute<T>() where T: unmanaged
		{
			Span<ulong> locals = stackalloc ulong[method.GetMethodBody().LocalVariables.Count];

			while (true)
			{
				OpCodeValues opcode = ReadOpCode();

				//Console.WriteLine(opcode);

				// This page describes the behaviour of all the instructions.
				// https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes?view=netcore-3.1

				switch (opcode)
				{
					// Do nothing
					case OpCodeValues.Nop:
						break;

					// TODO Ldarg

					#region Store a value to a local variable
					case OpCodeValues.Stloc:
						locals[ReadUShort()] = Stack.PopULong();
						break;
					case OpCodeValues.Stloc_S:
						locals[il[ip++]] = Stack.PopULong();
						break;
					case OpCodeValues.Stloc_0:
						locals[0] = Stack.PopULong();
						break;
					case OpCodeValues.Stloc_1:
						locals[1] = Stack.PopULong();
						break;
					case OpCodeValues.Stloc_2:
						locals[2] = Stack.PopULong();
						break;
					case OpCodeValues.Stloc_3:
						locals[3] = Stack.PopULong();
						break;
					#endregion

					#region Push a local to the stack
					case OpCodeValues.Ldloc:
						Stack.PushULong(locals[ReadUShort()]);
						break;
					case OpCodeValues.Ldloc_0:
						Stack.PushULong(locals[0]);
						break;
					case OpCodeValues.Ldloc_1:
						Stack.PushULong(locals[1]);
						break;
					case OpCodeValues.Ldloc_2:
						Stack.PushULong(locals[2]);
						break;
					case OpCodeValues.Ldloc_3:
						Stack.PushULong(locals[3]);
						break;
					#endregion

					#region Push literal to the stack
					case OpCodeValues.Ldnull:
						Stack.PushULong(0);
						break;
					case OpCodeValues.Ldc_I4:
						Stack.PushInt(ReadInt());
						break;
					case OpCodeValues.Ldc_I4_S:
						Stack.PushInt(il[ip++]);
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

					#region Branch
					case OpCodeValues.Br:
						ip = ReadInt();
						break;
					case OpCodeValues.Br_S:
						ip += il[ip++];
						break;
					#endregion

					case OpCodeValues.Ret:
						var ret = Stack.PopULong();
						return MemoryMarshal.Cast<ulong, T>(MemoryMarshal.CreateSpan(ref ret, 1))[0];

					default:
						throw new UnsupportedInstructionException(opcode);
				}
			}
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

		private ushort ReadUShort()
		{
			ushort i = BitConverter.ToUInt16(il, ip);
			ip += 2;
			return i;
		}

		private int ReadInt()
		{
			int i = BitConverter.ToInt32(il, ip += 4);
			ip += 4;
			return i;
		}

		private long ReadLong()
		{
			long i = BitConverter.ToInt64(il, ip += 8);
			ip += 8;
			return i;
		}

		private float ReadFloat()
		{
			float f = BitConverter.ToSingle(il, ip);
			ip += 4;
			return f;
		}

		private double ReadDouble()
		{
			double d = BitConverter.ToDouble(il, ip += 4);
			ip += 4;
			return d;
		}
	}
}
