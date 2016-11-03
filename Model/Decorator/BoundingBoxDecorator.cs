using System;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class BoundingBoxDecorator : Decorator
{
    public BoundingBoxDecorator ()
    {
        if (boundingBoxes == null)
            boundingBoxes = new List<BoundingBox> ();
    }

	[NonSerialized]
	private bool enableEditing = false;
    private bool snap = false;
    private Vector2 scrollPos2;
    public BoundingBox selected;

    public List<BoundingBox> boundingBoxes;


    public override void RenderInspectorGUI (ParkitectObj parkitectObj)
    {
        Event e = Event.current;

        scrollPos2 = EditorGUILayout.BeginScrollView(scrollPos2, "GroupBox", GUILayout.Height(100));
        for (int i = 0; i < this.boundingBoxes.Count; i++)
        {
            Color gui = GUI.color;
            if (this.boundingBoxes[i] == selected)
            { GUI.color = Color.red; }

            if (GUILayout.Button("BoudingBox" + (i + 1)))
            {
                if (e.button == 1)
                {
                    this.boundingBoxes.RemoveAt(i);
                    return;
                }

                if (selected == this.boundingBoxes[i])
                {
                    selected = null;
                    return;
                }
                selected = this.boundingBoxes[i];
            }
            GUI.color = gui;
        }
        EditorGUILayout.EndScrollView();

        if (GUILayout.Button("Add BoudingBox"))
        {
            this.boundingBoxes.Add(new BoundingBox());
        }
        string caption = "Enable Editing";
        if (enableEditing)
        {
            caption = "Disable Editing";
        }
        if (GUILayout.Button(caption))
        {
            enableEditing = !enableEditing;
        }
        if (enableEditing)
        {
            GUILayout.Label("Hold S - Snap to 0.25");
        }

        base.RenderInspectorGUI (parkitectObj);
    }

    public override void RenderSceneGUI (ParkitectObj parkitectObj)
    {
        drawBox(parkitectObj);

        if (!enableEditing)
        {
            return;
        }

       // EditorUtility.SetDirty(PMM);
        Tools.current = Tool.None;

        int controlID = GUIUtility.GetControlID(GetHashCode(), FocusType.Passive);
        switch (Event.current.type)
        {
        case EventType.layout:
            HandleUtility.AddDefaultControl(controlID);
            break;
        case EventType.keyDown:
            if (Event.current.keyCode == KeyCode.S)
            {
                snap = true;
            }
            break;
        case EventType.keyUp:
            if (Event.current.keyCode == KeyCode.S)
            {
                snap = false;
            }
            break;
        }

        base.RenderSceneGUI (parkitectObj);
    }

    private void drawBox(ParkitectObj parkitectObj)
    {
 
        foreach (BoundingBox box in this.boundingBoxes)
        {


            Vector3 diff = box.bounds.max - box.bounds.min;
            Vector3 diffX = new Vector3(diff.x, 0, 0);
            Vector3 diffY = new Vector3(0, diff.y, 0);
            Vector3 diffZ = new Vector3(0, 0, diff.z);

            Color fill = Color.white;
            fill.a = 0.025f;
            Color outer = Color.gray;
            if (enableEditing && box == selected)
            {
                fill = Color.magenta;
                fill.a = 0.1f;
                outer = Color.black;
            }

            // left
            drawPlane(box.bounds.min, box.bounds.min + diffZ, box.bounds.min + diffZ + diffY, box.bounds.min + diffY, fill, outer, parkitectObj);

            //back
            drawPlane(box.bounds.min, box.bounds.min + diffX, box.bounds.min + diffX + diffY, box.bounds.min + diffY, fill, outer, parkitectObj);

            //right
            drawPlane(box.bounds.max, box.bounds.max - diffY, box.bounds.max - diffY - diffZ, box.bounds.max - diffZ, fill, outer, parkitectObj);

            //forward
            drawPlane(box.bounds.max, box.bounds.max - diffY, box.bounds.max - diffY - diffX, box.bounds.max - diffX, fill, outer, parkitectObj);

            //up
            drawPlane(box.bounds.max, box.bounds.max - diffX, box.bounds.max - diffX - diffZ, box.bounds.max - diffZ, fill, outer, parkitectObj);

            //down
            drawPlane(box.bounds.min, box.bounds.min + diffX, box.bounds.min + diffX + diffZ, box.bounds.min + diffZ, fill, outer, parkitectObj);

            if (enableEditing && box == selected)
            {
                box.bounds.min = handleModifyValue(box.bounds.min, parkitectObj.gameObject.transform.InverseTransformPoint(Handles.PositionHandle(parkitectObj.gameObject.transform.TransformPoint(box.bounds.min), Quaternion.LookRotation(Vector3.left, Vector3.down))));
                box.bounds.max = handleModifyValue(box.bounds.max, parkitectObj.gameObject.transform.InverseTransformPoint(Handles.PositionHandle(parkitectObj.gameObject.transform.TransformPoint(box.bounds.max), Quaternion.LookRotation(Vector3.forward))));
                Handles.Label(parkitectObj.gameObject.transform.position + box.bounds.min, box.bounds.min.ToString("F2"));
                Handles.Label(parkitectObj.gameObject.transform.position +  box.bounds.max, box.bounds.max.ToString("F2"));
            }
        }
    }

    private Vector3 handleModifyValue(Vector3 value, Vector3 newValue)
    {
        if (snap && (newValue - value).magnitude > Mathf.Epsilon)
        {
            if (Mathf.Abs(newValue.x - value.x) > Mathf.Epsilon)
            {
                newValue.x = Mathf.Round(newValue.x * 4) / 4;
            }
            if (Mathf.Abs(newValue.y - value.y) > Mathf.Epsilon)
            {
                newValue.y = Mathf.Round(newValue.y * 4) / 4;
            }
            if (Mathf.Abs(newValue.z - value.z) > Mathf.Epsilon)
            {
                newValue.z = Mathf.Round(newValue.z * 4) / 4;
            }
        }
        return newValue;
    }

    private void drawPlane(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4, Color fill, Color outer, ParkitectObj PO)
    {
        Handles.DrawSolidRectangleWithOutline(new Vector3[] { PO.gameObject.transform.TransformPoint(p1), PO.gameObject.transform.TransformPoint(p2), PO.gameObject.transform.TransformPoint(p3), PO.gameObject.transform.TransformPoint(p4) }, fill, outer);
    }
}

