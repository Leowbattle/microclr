using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace microclr
{
	public class MicroClr
	{
		MicroClrStack Stack = new MicroClrStack();

		public T Execute<T>(MethodInfo method) where T: unmanaged
		{
			if (method.ReturnType != typeof(T))
			{
				throw new IncorrectReturnTypeException(typeof(T), method.ReturnType);
			}

			var ctx = new ExecutionContext(method, Stack);
			return ctx.Execute<T>();
		}

		public void Execute(MethodInfo method)
		{
			var ctx = new ExecutionContext(method, Stack);
			ctx.Execute();
		}

		public string Disassemble(MethodInfo method)
		{
			return new ExecutionContext(method, Stack).Disassemble();
		}
	}
}
