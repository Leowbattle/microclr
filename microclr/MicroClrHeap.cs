using System;
using System.Collections.Generic;
using System.Text;

namespace microclr
{
	class MicroClrHeap
	{
		List<object> Objects { get; } = new List<object>();

		public int Count => Objects.Count;

		public int Add(object o)
		{
			Objects.Add(o);
			return Objects.Count;
		}

		public object this[int index]
		{
			get => Objects[index - 1];
		}
	}
}
