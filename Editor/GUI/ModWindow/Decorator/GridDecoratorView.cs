using System;
using AssemblyCSharp;
using UnityEditor;

public class GridDecoratorView : IDecoratorView
{
	
		public GridDecoratorView ()
		{
		}


	public void Render (Decorator decorator,ParkitectObj parkitectObj)
	{
		GridDecorator gridDecorator = (GridDecorator)decorator;
		gridDecorator.grid = EditorGUILayout.Toggle("GridSnap: ", gridDecorator.grid);
		gridDecorator.heightDelta = EditorGUILayout.FloatField("HeightDelta: ", gridDecorator.heightDelta);
		gridDecorator.snapCenter = EditorGUILayout.Toggle("SnapCenter: ", gridDecorator.snapCenter);
		gridDecorator.gridSubdivision = EditorGUILayout.FloatField("Grid Subdivision", gridDecorator.gridSubdivision);

	}

}

