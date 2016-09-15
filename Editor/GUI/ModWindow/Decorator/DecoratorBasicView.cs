using System;
using UnityEditor;

public class DecoratorBasicView : IDecoratorView
{
	public DecoratorBasicView ()
	{
		
	}

	public void Render (Decorator decorator,ParkitectObj parkitectObj)
	{
		BaseDecorator baseDecorator = (BaseDecorator)decorator;
		baseDecorator.InGameName = EditorGUILayout.TextField("In Game name: ", baseDecorator.InGameName);
		baseDecorator.price = EditorGUILayout.FloatField("Price: ", baseDecorator.price);
	}

}


