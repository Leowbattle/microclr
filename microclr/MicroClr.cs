using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace microclr
{
	public class MicroClr
	{
		internal struct CachedMethod
		{
			public MethodInfo Method;
			public byte[] IL;

			public CachedMethod(MethodInfo method, byte[] iL)
			{
				Method = method;
				IL = iL;
			}
		}

		internal MicroClrStack Stack = new MicroClrStack();
		internal MicroClrHeap Heap = new MicroClrHeap();
		internal Dictionary<int, CachedMethod> MethodCache = new Dictionary<int, CachedMethod>();

		public T Execute<T>(MethodInfo method, params object[] args)
		{
			Stack.Clear();
			Heap.Clear();

			if (method.ReturnType != typeof(T))
			{
				throw new IncorrectReturnTypeException(typeof(T), method.ReturnType);
			}

			var ctx = new ExecutionContext(method, args, this);
			return ctx.Execute<T>();
		}

		public object Execute(MethodInfo method, params object[] args)
		{
			Stack.Clear();
			Heap.Clear();

			var ctx = new ExecutionContext(method, args, this);
			return ctx.Execute();
		}

		public string Disassemble(MethodInfo method)
		{
			return new ExecutionContext(method, (object[])null, this).Disassemble();
		}
	}
}
