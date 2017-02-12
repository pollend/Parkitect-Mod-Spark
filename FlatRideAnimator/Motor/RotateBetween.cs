﻿using System;
using UnityEditor;
using UnityEngine;
[ExecuteInEditMode]
[Serializable]
public class RotateBetween : Motor
{
    [SerializeField]
    public Transform axis;
    [SerializeField]
    public Quaternion fromRotation;
    [SerializeField]
    public Vector3 rotationAxis = Vector3.up;
    [SerializeField]
    public Quaternion toRotation;
    [SerializeField]
    public Quaternion originalRotationValue;
    [SerializeField]
    public float duration = 1f;

    [SerializeField]
    private float currentPosition;

    [SerializeField]
    private float direction;
    public override string EventName
    {
        get
        {
            return "RotateBetween";
        }
    }
	public override void DrawGUI(Transform root)
    {

        Identifier = EditorGUILayout.TextField("Name ", Identifier);
        axis = (Transform)EditorGUILayout.ObjectField("axis", axis, typeof(Transform), true);
        rotationAxis = EditorGUILayout.Vector3Field("Rotate To", rotationAxis);
        duration = EditorGUILayout.FloatField("Time", duration);
		base.DrawGUI(root);
    }
	public override void Reset(Transform root)
    {
        if (axis)
            axis.localRotation = originalRotationValue;
        currentPosition = 0f;


		base.Reset(root);
    }
	public override void Enter(Transform root)
    {
        originalRotationValue = axis.localRotation;
        Initialize(axis, axis.localRotation,  Quaternion.Euler(axis.localEulerAngles +  rotationAxis), duration);
    }
    public void Initialize(Transform axis, Quaternion fromRotation, Quaternion toRotation, float duration)
    {
        this.axis = axis;
        this.fromRotation = fromRotation;
        this.toRotation = toRotation;
        this.duration = duration;
        axis.localRotation = Quaternion.Lerp(fromRotation, toRotation, 0);
    }

    public bool startFromTo()
    {
        if (this.direction != 1f)
        {
            this.direction = 1f;
            return true;
        }
        return false;
    }

    public bool startToFrom()
    {
        if (this.direction != -1f)
        {
            this.direction = -1f;
            return true;
        }
        return false;
    }

    public bool isStopped()
    {
        if (this.direction == 1f)
        {
            return this.currentPosition >= 0.99f;
        }
        return this.currentPosition <= 0.01f;
    }

    public void tick(float dt)
    {
        this.currentPosition += dt * this.direction * 1f / this.duration;
        this.currentPosition = Mathf.Clamp01(this.currentPosition);
        this.axis.localRotation = Quaternion.Lerp(this.fromRotation, this.toRotation, Mathfx.Hermite(0f, 1f, this.currentPosition));
    }
}
