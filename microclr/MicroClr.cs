using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace microclr
{
	public class MicroClr
	{
		internal MicroClrStack Stack = new MicroClrStack();
		internal MicroClrHeap Heap = new MicroClrHeap();

		public T Execute<T>(MethodInfo method, params object[] args)
		{
			if (method.ReturnType != typeof(T))
			{
				throw new IncorrectReturnTypeException(typeof(T), method.ReturnType);
			}

			var ctx = new ExecutionContext(method, args, this);
			return ctx.Execute<T>();
		}

		public object Execute(MethodInfo method, params object[] args)
		{
			var ctx = new ExecutionContext(method, args, this);
			return ctx.Execute();
		}

		public string Disassemble(MethodInfo method)
		{
			return new ExecutionContext(method, (object[])null, this).Disassemble();
		}
	}
}
