using System;
using UnityEditor;

[Serializable]
public class CategoryDecorator : Decorator
{
	public string category;
	
	public CategoryDecorator ()
	{
	}

	public override void Render (ParkitectObj parkitectObj)
	{
		this.category = EditorGUILayout.TextField("Category: ", this.category);

		base.Render (parkitectObj);
	}
}

