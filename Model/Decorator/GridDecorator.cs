using System;
using UnityEditor;


[Serializable]
public class GridDecorator : Decorator
{
	public bool snapCenter = true;
	public bool snap;
	public bool grid;
	public float heightDelta;
	public float gridSubdivision = 1;

	public GridDecorator ()
	{
	}

	#if UNITY_EDITOR
    public override void RenderInspectorGUI (ParkitectObj parkitectObj)
	{
		this.grid = EditorGUILayout.Toggle("GridSnap: ", this.grid);
		this.heightDelta = EditorGUILayout.FloatField("HeightDelta: ", this.heightDelta);
		this.snapCenter = EditorGUILayout.Toggle("SnapCenter: ", this.snapCenter);
		this.gridSubdivision = EditorGUILayout.FloatField("Grid Subdivision", this.gridSubdivision);

        base.RenderInspectorGUI (parkitectObj);
	}
	#endif
}


