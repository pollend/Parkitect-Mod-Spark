using System;

[ParkitectObjectTag("Seating")]
[Serializable]
public class SeatingParkitectObject : ParkitectObj
{
    public SeatingParkitectObject ()
    {
    }

    public override Type[] SupportedDecorators ()
    {
        return new Type[]{
            typeof(BaseDecorator),
            typeof(SeatDecorator),
            typeof(ColorDecorator),
            typeof(BoundingBoxDecorator)
        };
    }
}


