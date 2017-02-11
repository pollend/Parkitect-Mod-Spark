using System;

[ParkitectObjectTag("Wall")]
[Serializable]
public class WallParkitectObject : ParkitectObj
{
	public WallParkitectObject ()
	{
	}

	public override Type[] SupportedDecorators ()
	{
		return new Type[] {
			typeof(BaseDecorator),
			typeof(CategoryDecorator),
			typeof(ColorDecorator),
			typeof(BoundingBoxDecorator)
		};
	}
}

