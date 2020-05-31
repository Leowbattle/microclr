using microclr;
using System;

namespace TestProgram
{
	class Program
	{
		public static void Fibonacci(int n)
		{
			string[] strings = new string[n];

			int a = 0;
			int b = 1;
			int c = 1;

			for (int i = 0; i < n; i++)
			{
				strings[i] = $"Fibonacci {i + 1} = {c}";
				c = a + b;
				a = b;
				b = c;
			}

			foreach (var str in strings)
			{
				Console.WriteLine(str);
			}
		}

		static void Main(string[] args)
		{
			var clr = new MicroClr();

			Fibonacci(10);

			var method = typeof(Program).GetMethod(nameof(Fibonacci));
			var ret = clr.Execute(method, 10);
			Console.WriteLine(ret);
		}
	}
}
