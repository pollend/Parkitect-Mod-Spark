using System;
using UnityEngine;

public class PathTypeDecorator : Decorator
{
	public enum PathType { Normal, Queue, Employee }
	public PathType pathType;
	public Texture2D pathTexture;

	public PathTypeDecorator ()
	{
	}
}


