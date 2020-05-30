using System;
using System.Collections.Generic;
using System.Text;

namespace microclr
{
	class MicroClrHeap
	{
		internal List<object> Objects { get; } = new List<object>();

		public int Add(object o)
		{
			int id = Objects.Count;
			Objects.Add(o);
			return id;
		}
	}
}
