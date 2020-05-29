﻿using System;

namespace microclr
{
	class MicroClrStack
	{
		Variable[] Stack = new Variable[1024];
		int StackPointer = 0;

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
			Stack[StackPointer++] = new Variable(i, VariableType.UInt);
		}

		public ulong PopULong()
		{
			return Stack[--StackPointer].Value;
		}

		public void PushLong(long i)
		{
			Stack[StackPointer++] = new Variable((ulong)i, VariableType.Int);
		}

		public long PopLong()
		{
			return (long)Stack[--StackPointer].Value;
		}

		public void PushInt(int i)
		{
			Stack[StackPointer++] = new Variable((ulong)i, VariableType.Int);
		}

		public int PopInt()
		{
			return (int)Stack[--StackPointer].Value;
		}

		public void PushFloat(float f)
		{
			Stack[StackPointer++] = new Variable((ulong)BitConverter.SingleToInt32Bits(f), VariableType.Float);
		}

		public float PopFloat()
		{
			return BitConverter.Int32BitsToSingle((int)Stack[--StackPointer].Value);
		}

		public void PushDouble(double d)
		{
			Stack[StackPointer++] = new Variable((ulong)BitConverter.DoubleToInt64Bits(d), VariableType.Double);
		}

		public double PopDouble()
		{
			return BitConverter.Int64BitsToDouble((long)Stack[--StackPointer].Value);
		}
	}
}
