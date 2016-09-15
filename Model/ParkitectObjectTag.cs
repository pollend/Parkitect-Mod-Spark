using System;

public class ParkitectObjectTag : System.Attribute 
{
	public readonly string name;
	public string Name{
		get{ return name; }
		set{ name = value; }
	}
	public ParkitectObjectTag ()
	{
	}
	public ParkitectObjectTag (string name)
	{
		this.name = name;
	}
}


