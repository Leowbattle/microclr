using System;
using System.Collections.Generic;
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
}
