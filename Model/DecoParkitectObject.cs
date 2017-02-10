using System;

[ParkitectObjectTag("Deco")]
[Serializable]
public class DecoParkitectObject : ParkitectObj
{

	public override Type[] SupportedDecorators ()
	{
        return new Type[]{
            typeof(BaseDecorator),
            typeof(GridDecorator),
            typeof(CategoryDecorator),
            typeof(ColorDecorator),
            typeof(BoundingBoxDecorator)
        };
	}
}


