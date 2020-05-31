using System;

namespace microclr
{
	class MicroClrStack
	{
		Variable[] Stack = new Variable[1024];
		int StackPointer = 0;

		public void Clear()
		{
			StackPointer = 0;
		}

		public Variable Peek()
		{
			return Stack[StackPointer - 1];
		}

		public void Push(Variable variable)
		{
			Stack[StackPointer++] = variable;
		}

		public Variable Pop()
		{
			return Stack[--StackPointer];
		}

		public void PushULong(ulong i)
		{
			Stack[StackPointer++] = new Variable(i);
		}

		public ulong PopULong()
		{
			return Stack[--StackPointer].Value;
		}

		public void PushLong(long i)
		{
			Stack[StackPointer++] = new Variable(i);
		}

		public long PopLong()
		{
			return (long)Stack[--StackPointer].Value;
		}

		public void PushInt(int i)
		{
			Stack[StackPointer++] = new Variable(i);
		}

		public int PopInt()
		{
			return (int)Stack[--StackPointer].Value;
		}

		public void PushFloat(float f)
		{
			Stack[StackPointer++] = new Variable(f);
		}

		public float PopFloat()
		{
			return BitConverter.Int32BitsToSingle((int)Stack[--StackPointer].Value);
		}

		public void PushDouble(double d)
		{
			Stack[StackPointer++] = new Variable(d);
		}

		public double PopDouble()
		{
			return BitConverter.Int64BitsToDouble((long)Stack[--StackPointer].Value);
		}

		public void PushObject(int o)
		{
			Stack[StackPointer++] = new Variable((ulong)o, VariableType.Object);
		}
	}
}
