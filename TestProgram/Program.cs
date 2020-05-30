using microclr;
using System;
using System.Reflection;

namespace TestProgram
{
	class Program
	{
		public static int Read()
		{
			return Console.Read();
		}

		static void Main(string[] args)
		{
			var clr = new MicroClr();

			var method = typeof(Program).GetMethod(nameof(Read));
			var ret = clr.Execute<int>(method);
			Console.WriteLine(ret);
		}
	}
}
