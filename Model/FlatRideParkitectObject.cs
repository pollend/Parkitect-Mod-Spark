using System;


[ParkitectObjectTag("FlatRide")]
[Serializable]
public class FlatRideParkitectObject : ParkitectObj
{
	public FlatRideParkitectObject ()
	{
	}

	public override Type[] SupportedDecorators ()
	{
		return new Type[] {
			typeof(BaseDecorator),
			typeof(FlatrideDecorator),
			typeof(BoundingBoxDecorator),
			typeof(AnimatorDecorator)
		};
	}

}

