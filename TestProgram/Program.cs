using microclr;
using System;
using System.Reflection;

namespace TestProgram
{
	class Program
	{
		public float Method()
		{
			int x = 100;
			float f = 3.14f;
			return f;
		}

		static void Main(string[] args)
		{
			var clr = new MicroClr();

			var method = typeof(Program).GetMethod(nameof(Method));
			//var method = typeof(Program).GetMethod(nameof(Main), BindingFlags.NonPublic | BindingFlags.Static);
			var ret = clr.Execute<float>(method);
			Console.WriteLine(ret);
		}
	}
}
