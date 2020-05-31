using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using microclr;
using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Benchmarks
{
	public class Benchmarks
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		static void Empty()
		{

		}

		public static void CallInLoop()
		{
			for (int i = 0; i < 1000; i++)
			{
				Empty();
			}
		}

		MicroClr mclr = new MicroClr();
		MethodInfo method = typeof(Benchmarks).GetMethod(nameof(CallInLoop));

		[Benchmark]
		public void BenchMicroClr()
		{
			mclr.Execute(method);
		}

		[Benchmark(Baseline = true)]
		public void BenchDotnet()
		{
			CallInLoop();
		}

		static void Main(string[] args)
		{
			new Benchmarks().BenchMicroClr();
			//BenchmarkRunner.Run<Benchmarks>();
		}
	}
}
