using microclr;
using System;

namespace TestProgram
{
	class Program
	{
		public static void DoFibonacci(int n)
		{
			int a = 0;
			int b = 1;
			int c = 1;

			for (int i = 0; i < n; i++)
			{
				Console.WriteLine($"Fibonacci {i + 1} = {c}");
				c = a + b;
				a = b;
				b = c;
			}
		}

		static void Main(string[] args)
		{
			var clr = new MicroClr();

			var method = typeof(Program).GetMethod(nameof(DoFibonacci));
			var ret = clr.Execute(method, 10);
			Console.WriteLine(ret);
		}
	}
}
