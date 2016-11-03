using System;

[ParkitectObjectTag("Deco")]
[Serializable]
public class DecoParkitectObject : ParkitectObj
{
	public DecoParkitectObject ()
	{
	}

	public override Type[] SupportedDecorators ()
	{
		return new Type[]{typeof(BaseDecorator) };
	}
}


