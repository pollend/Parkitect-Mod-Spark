using System;
using UnityEngine;

[ParkitectObjectTag("Path")]
[Serializable]
public class PathStyleParkitectObject : ParkitectObj
{
	public PathStyleParkitectObject ()
	{
	}

	public override Type[] SupportedDecorators ()
	{
		return new Type[]{
			typeof(BaseDecorator),
			typeof(CategoryDecorator),
			typeof(PathDecorator),
			typeof(BoundingBoxDecorator)
		};
	}
}

