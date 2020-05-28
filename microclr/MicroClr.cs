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
			var ctx = new ExecutionContext(method, Stack);
			return ctx.Execute<T>();
		}
	}
}
