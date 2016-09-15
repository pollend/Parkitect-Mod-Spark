using System;
using UnityEditor;

public class CategoryDecoratorView : IDecoratorView
{
	public CategoryDecoratorView ()
	{
	}

	public void Render (Decorator decorator,ParkitectObj parkitectObj)
	{
		CategoryDecorator categoryDecorator =  (CategoryDecorator)decorator;
		categoryDecorator.category = EditorGUILayout.TextField("Category: ", categoryDecorator.category);

	}

}


