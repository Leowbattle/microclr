using microclr;
using System;
using System.Reflection;

namespace TestProgram
{
	class Program
	{
		public double Method()
		{
			double d = 3.14;
			return d;
		}

		static void Main(string[] args)
		{
			var clr = new MicroClr();

			var method = typeof(Program).GetMethod(nameof(Method));
			//Console.WriteLine(clr.Disassemble(method));

			//var method = typeof(Program).GetMethod(nameof(Main), BindingFlags.NonPublic | BindingFlags.Static);
			var ret = clr.Execute<double>(method);
			Console.WriteLine(ret);
		}
	}
}
