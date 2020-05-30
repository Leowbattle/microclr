using System;
using System.Collections.Generic;
using System.Text;

namespace microclr
{
	public enum VariableType
	{
		Int,
		UInt,
		Float,
		Double,
		Object
	}

	public struct Variable
	{
		public ulong Value;
		public VariableType Type;

		public Variable(sbyte value) : this((ulong)value, VariableType.Int)
		{ }

		public Variable(byte value) : this(value, VariableType.UInt)
		{ }

		public Variable(short value) : this((ulong)value, VariableType.Int)
		{ }

		public Variable(ushort value) : this(value, VariableType.UInt)
		{ }

		public Variable(int value) : this((ulong)value, VariableType.Int)
		{ }

		public Variable(uint value) : this(value, VariableType.UInt)
		{ }

		public Variable(long value) : this((ulong)value, VariableType.Int)
		{ }

		public Variable(ulong value) : this(value, VariableType.UInt)
		{ }

		public Variable(float value) : this((ulong)BitConverter.SingleToInt32Bits(value), VariableType.Float)
		{ }

		public Variable(double value) : this((ulong)BitConverter.DoubleToInt64Bits(value), VariableType.Double)
		{ }

		public Variable(ulong value, VariableType type)
		{
			Value = value;
			Type = type;
		}

		public override string ToString()
		{
			return Type switch
			{
				VariableType.Int => ((long)Value).ToString(),
				VariableType.UInt => Value.ToString(),
				VariableType.Float => BitConverter.Int32BitsToSingle((int)Value).ToString(),
				VariableType.Double => BitConverter.Int64BitsToDouble((long)Value).ToString(),
				_ => Value.ToString()
			};
		}
	}
}
