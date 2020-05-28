using System;

namespace microclr
{
	class MicroClrStack
	{
		ulong[] Stack = new ulong[1024];
		int StackPointer = 0;

		public void PushULong(ulong i)
		{
			Stack[StackPointer++] = i;
		}

		public ulong PopULong()
		{
			return Stack[--StackPointer];
		}

		public void PushLong(long i)
		{
			Stack[StackPointer++] = (ulong)i;
		}

		public long PopLong()
		{
			return (long)Stack[--StackPointer];
		}

		public void PushInt(int i)
		{
			Stack[StackPointer++] = (ulong)i;
		}

		public int PopInt()
		{
			return (int)Stack[--StackPointer];
		}

		public void PushFloat(float f)
		{
			Stack[StackPointer++] = (ulong)BitConverter.SingleToInt32Bits(f);
		}

		public float PopFloat()
		{
			return BitConverter.Int32BitsToSingle((int)Stack[--StackPointer]);
		}

		public void PushDouble(double d)
		{
			Stack[StackPointer++] = (ulong)BitConverter.DoubleToInt64Bits(d);
		}

		public double PopDouble()
		{
			return BitConverter.Int64BitsToDouble((long)Stack[--StackPointer]);
		}
	}
}
