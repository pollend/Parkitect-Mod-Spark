using System;


public class DecoratorTag : System.Attribute
{
	public readonly string name;
	public string Name{
		get{ return name; }
		set{ name = value; }
	}
	public DecoratorTag ()
	{
	}
	public DecoratorTag (string name)
	{
		this.name = name;
	}
}


