using System;

[ParkitectObjectTag("Shop")]
[Serializable]
public class ShopParkitectObject : ParkitectObj
{
	public ShopParkitectObject ()
	{
	}

	public override Type[] SupportedDecorators ()
	{
		return new Type[] {
			typeof(BaseDecorator),
			typeof(ShopDecorator)
		};
	}
}


