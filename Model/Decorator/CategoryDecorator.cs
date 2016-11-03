using System;
using UnityEditor;

[Serializable]
public class CategoryDecorator : Decorator
{
	public string category;
	
	public CategoryDecorator ()
	{
	}

    public override void RenderInspectorGUI (ParkitectObj parkitectObj)
	{
		this.category = EditorGUILayout.TextField("Category: ", this.category);

        base.RenderInspectorGUI (parkitectObj);
	}
}

