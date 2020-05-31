# microclr
An interpreter for a subset of dotnet IL implemented in C#

Demo program:

```csharp
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

			//Fibonacci(10);

			var method = typeof(Program).GetMethod(nameof(Fibonacci));
			clr.Execute(method, 10);
		}
	}
}
```

Output:
```
Fibonacci 1 = 1
Fibonacci 2 = 1
Fibonacci 3 = 2
Fibonacci 4 = 3
Fibonacci 5 = 5
Fibonacci 6 = 8
Fibonacci 7 = 13
Fibonacci 8 = 21
Fibonacci 9 = 34
Fibonacci 10 = 55
```
