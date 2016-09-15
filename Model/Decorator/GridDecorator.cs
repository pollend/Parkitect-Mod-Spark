using System;

namespace AssemblyCSharp
{
	public class GridDecorator : Decorator
	{
		public bool snapCenter = true;
		public bool snap;
		public bool grid;
		public float heightDelta;
		public float gridSubdivision = 1;

		public GridDecorator ()
		{
		}
	}
}

