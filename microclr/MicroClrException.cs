using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace microclr
{
	public class MicroClrException : Exception
	{
	}

	public class UnsupportedInstructionException : MicroClrException
	{
		public OpCodeValues Instruction { get; }

		public UnsupportedInstructionException(OpCodeValues instruction)
		{
			Instruction = instruction;
		}

		public override string Message => $"Unsupported CLR instruction {Instruction}";
	}

	public class NonStaticMethodException : MicroClrException
	{
		public MethodInfo Method { get; }

		public NonStaticMethodException(MethodInfo method)
		{
			Method = method;
		}

		public override string Message => $"Tried to execute non-static method {Method}";
	}

	public class IncorrectReturnTypeException : MicroClrException
	{
		public Type Expected { get; }
		public Type Requested { get; }

		public IncorrectReturnTypeException(Type expected, Type requested)
		{
			Expected = expected;
			Requested = requested;
		}

		public override string Message => $"Requested return type {Requested} does not match {Expected}";
	}

	public class ParameterCountException : MicroClrException
	{
		public int Expected { get; }
		public int Given { get; }

		public ParameterCountException(int expected, int given)
		{
			Expected = expected;
			Given = given;
		}

		public override string Message => $"Incorrect number of parameters {Given}, expected {Expected}";
	}

	public class ParameterTypeException : MicroClrException
	{
		public int Index { get; }
		public Type Expected { get; }
		public Type Given { get; }

		public ParameterTypeException(int index, Type expected, Type given)
		{
			Index = index;
			Expected = expected;
			Given = given;
		}

		public override string Message => $"Incorrect parameter type {Given} for parameter {Index}, expected {Expected}";
	}

	public class InvalidCastException : Exception
	{
		public Type Tried { get; }
		public Type Actual { get; }

		public InvalidCastException(Type tried, Type actual)
		{
			Tried = tried;
			Actual = actual;
		}

		public override string Message => $"Cannot cast {Actual} to {Tried}";
	}
}
