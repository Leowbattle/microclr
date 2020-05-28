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
}
