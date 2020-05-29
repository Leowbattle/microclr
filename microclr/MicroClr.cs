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

		public T Execute<T>(MethodInfo method, params object[] args) where T: unmanaged
		{
			if (method.ReturnType != typeof(T))
			{
				throw new IncorrectReturnTypeException(typeof(T), method.ReturnType);
			}

			var ctx = new ExecutionContext(method, args, Stack);
			return ctx.Execute<T>();
		}

		public object Execute(MethodInfo method, params object[] args)
		{
			var ctx = new ExecutionContext(method, args, Stack);
			return ctx.Execute();
		}

		public string Disassemble(MethodInfo method)
		{
			return new ExecutionContext(method, null, Stack).Disassemble();
		}
	}
}
