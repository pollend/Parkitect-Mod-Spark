using System;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Rendering;

public class FlatRideAnimator : EditorWindow
{

	Vector2 scrollPosPhases;
	Vector2 scrollPosMotors;

	private bool _isPlaying;

	//For Adding events
	private Phase _curentPhase;

	private List<ReflectionProbe> _reflectionProbes = new List<ReflectionProbe>();

	// Add menu named "My Window" to the Window menu
	[MenuItem("Parkitect/Flat-Ride Animator", false, 2)]
	static void Init()
	{
		// Get existing open window or if none, make a new one:
		FlatRideAnimator window = (FlatRideAnimator) EditorWindow.GetWindow(typeof(FlatRideAnimator));
		window.Show();
		window.titleContent.text = "RideAnimator";
	}

	void OnEnable()
	{
		titleContent.image =
			(Texture2D) AssetDatabase.LoadAssetAtPath("Assets/Parkitect_ModSpark/Textures/AnimateIcon.png", typeof(Texture2D));

		EditorApplication.update += CallbackFunction;
	}

	public void repaintWindow()
	{
		Repaint();
	}


	private FlatRideParkitectObject GetActiveAnimator()
	{
		if (ModPayload.Instance.SelectedParkitectObject == null)
			return null;
		if (ModPayload.Instance.SelectedParkitectObject is FlatRideParkitectObject)
			return (FlatRideParkitectObject) ModPayload.Instance.SelectedParkitectObject;
		return null;
	}

	private Transform getSceneTransform()
	{
		var gameObjectref = ModPayload.Instance.SelectedParkitectObject.GetGameObjectRef(false);
		if (gameObjectref == null)
			return null;

		return gameObjectref.transform;
	}

	void AddEvent(object obj)
	{
		FlatRideParkitectObject animator = GetActiveAnimator();

		Type eventType = RideAnimationEvent.FindRideAnimationTypeByTag(obj.ToString());
		if (eventType != null)
		{
			_curentPhase.AddEvent((RideAnimationEvent) CreateInstance(eventType));
		}
		Type motorType = Motor.FindMotorTypeByTag(obj.ToString());
		if (motorType != null)
		{
			Motor motor = (Motor) CreateInstance(motorType);
			String tag = Motor.GetTagFromMotor(motorType);
			animator.AddMotor(motor);
			motor.Identifier = tag + "_" + animator.Motors.Count;
		}
	}

	void OnSelectionChange()
	{
		Repaint();
	}

	void OnGUI()
	{
		FlatRideParkitectObject animator = GetActiveAnimator();
		if (animator == null)
			return;
		Transform sceneTransform = getSceneTransform();
		if (sceneTransform == null)
			return;



		//if (ModManager && ModManager.asset != null && ModManager.asset.type == ParkitectObject.ObjType.FlatRide)
		{

			EditorUtility.SetDirty(animator);

			foreach (Phase p in animator.Phases)
			{
				foreach (RideAnimationEvent RAE in p.Events)
				{
					EditorUtility.SetDirty(RAE);
				}
			}
			foreach (Motor m in animator.Motors)
				EditorUtility.SetDirty(m);
			Event e = Event.current;

			GUILayout.BeginHorizontal(EditorStyles.toolbar);
			GUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.BeginVertical("grey_border", GUILayout.Width(300));
			GUILayout.BeginHorizontal(EditorStyles.toolbar);
			DrawToolStripEvents(animator, sceneTransform);
			GUILayout.EndHorizontal();
			scrollPosMotors = EditorGUILayout.BeginScrollView(scrollPosMotors, "grey_border");
			foreach (Motor m in animator.Motors)
			{


				EditorGUILayout.BeginVertical("ShurikenEffectBg");
				GUI.color = Color.Lerp(new Color(m.ColorIdentifier.r, m.ColorIdentifier.g, m.ColorIdentifier.b), new Color(1, 1, 1),
					.7f);
				if (GUILayout.Button(m.Identifier, "ShurikenModuleTitle"))
				{

					GUI.FocusControl("");
					m.ShowSettings = !m.ShowSettings;
					if (e.button == 1)
					{
						animator.RemoveMotor(m); //.Remove(m);
						DestroyImmediate(m);
						return;
					}


				}

				GUI.color = Color.white;

				if (m.ShowSettings)
					m.InspectorGUI(sceneTransform);
				EditorGUILayout.EndVertical();
			}
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndScrollView();
			if (GUILayout.Button("Add Motor"))
			{
				GenericMenu menu2 = new GenericMenu();
				menu2.AddItem(new GUIContent("Rotator"), false, AddEvent, "Rotator");
				menu2.AddItem(new GUIContent("RotateBetween"), false, AddEvent, "RotateBetween");
				menu2.AddItem(new GUIContent("Mover"), false, AddEvent, "Mover");
				menu2.AddItem(new GUIContent("MultiplyRotations"), false, AddEvent, "MultiplyRotations");
				menu2.AddItem(new GUIContent("PendulumRotator"), false, AddEvent, "PendulumRotator");
				menu2.ShowAsContext();
				e.Use();

			}
			GUILayout.Space(10);
			if (GUILayout.Button("Add Phase"))
			{
				// Now create the menu, add items and show it

				animator.AddPhase(CreateInstance<Phase>()); //.Add(new Phase());
			}
			EditorGUILayout.EndVertical();
			scrollPosPhases = EditorGUILayout.BeginScrollView(scrollPosPhases, "grey_border", GUILayout.ExpandWidth(true));
			EditorGUILayout.BeginHorizontal();
			//if (ModManager.asset.Animation != null)
			{
				for (int i = 0; i < animator.Phases.Count; i++) // Loop with for.
				{
					if (animator.Phases[i] == animator.CurrentPhase)
					{
						GUI.color = Color.Lerp(Color.green, new Color(1, 1, 1), .7f);
					}
					EditorGUILayout.BeginVertical("ProgressBarBack", GUILayout.MaxWidth(120), GUILayout.MinWidth(120),
						GUILayout.ExpandHeight(true));
					GUILayout.Button("Phase " + (i + 1), "ShurikenModuleTitle");

					if (GUILayout.Button(" Add Event", "OL Plus"))
					{
						GUI.FocusControl("");
						_curentPhase = animator.Phases[i];
						// Now create the menu, add items and show it
						GenericMenu menu = new GenericMenu();
						menu.AddItem(new GUIContent("WaitForSeconds"), false, AddEvent, "Wait");
						menu.AddItem(new GUIContent("Rotator/StartRotator"), false, AddEvent, "StartRotator");
						menu.AddItem(new GUIContent("Rotator/SpinRotator"), false, AddEvent, "SpinRotator");
						menu.AddItem(new GUIContent("Rotator/StopRotator"), false, AddEvent, "StopRotator");
						menu.AddItem(new GUIContent("RotateBetween/FromTo"), false, AddEvent, "From-ToRot");
						menu.AddItem(new GUIContent("RotateBetween/ToFrom"), false, AddEvent, "To-FromRot");
						menu.AddItem(new GUIContent("Mover/FromTo"), false, AddEvent, "From-ToMove");
						menu.AddItem(new GUIContent("Mover/ToFrom"), false, AddEvent, "To-FromMove");
						menu.AddItem(new GUIContent("MultiplyRotations/ApplyRotations"), false, AddEvent, "ApplyRotations");
						menu.AddItem(new GUIContent("PendulumRotator/ChangePendulum"), false, AddEvent, "ChangePendulum");
						menu.ShowAsContext();
						e.Use();
					}
					foreach (RideAnimationEvent rae in animator.Phases[i].Events)
					{

						EditorGUILayout.BeginVertical("ShurikenEffectBg");
						GUI.color = Color.Lerp(new Color(rae.ColorIdentifier.r, rae.ColorIdentifier.g, rae.ColorIdentifier.b),
							new Color(1, 1, 1), .7f);
						string Done = "";
						if (animator.Animating && rae.Done)
							Done = " ✓";
						if (GUILayout.Button(rae.EventName + Done, "ShurikenModuleTitle"))
						{
							GUI.FocusControl("");
							rae.ShowSettings = !rae.ShowSettings;
							if (e.button == 1)
							{
								animator.Phases[i].DeleteEvent(rae);
								return;
							}

						}
						GUI.color = Color.white;
						if (rae.ShowSettings)
							rae.RenderInspectorGUI(animator.Motors.ToArray());
						EditorGUILayout.EndVertical();
					}
					GUILayout.FlexibleSpace();



					EditorGUILayout.BeginHorizontal();
					if (i != 0)
					{

						if (GUILayout.Button("<<", "minibuttonleft"))
						{
							animator.Phases.Move(i, ListExtensions.MoveDirection.Up);
						}
					}
					else
					{
						GUILayout.Button("  ", "minibuttonleft");
					}
					if (GUILayout.Button("Clone", "minibuttonmid"))
					{

						animator.AddPhase(animator.Phases[i].ShallowCopy());
					}
					if (i != animator.Phases.Count - 1)
					{
						if (GUILayout.Button(">>", "minibuttonright"))
						{
							animator.Phases.Move(i, ListExtensions.MoveDirection.Down);
						}
					}
					else
					{
						GUILayout.Button("  ", "minibuttonright");
					}
					EditorGUILayout.EndHorizontal();
					if (GUILayout.Button(" Delete Phase", "OL Minus"))
					{
						animator.RemovePhase(animator.Phases[i]);
					}
					EditorGUILayout.EndVertical();
					GUI.color = Color.white;
				}
			}

			EditorGUILayout.EndHorizontal();
			EditorGUILayout.EndScrollView();
			EditorGUILayout.EndHorizontal();
			GUILayout.Label("© H-POPS - " + ModWindow.version, "PreToolbar");
		}
	}

	public enum MoveDirection
	{
		Up,
		Down
	}


	void OnFocus()
	{
		SceneView.onSceneGUIDelegate -= this.OnSceneGUI;
		// Add (or re-add) the delegate.
		SceneView.onSceneGUIDelegate += this.OnSceneGUI;

	}

	void ResetMotors(FlatRideParkitectObject animator, Transform root)
	{
		foreach (Motor m in animator.Motors)
		{
			m.Reset(root);
		}
		foreach (MultipleRotations R in animator.Motors.OfType<MultipleRotations>())
		{
			R.Reset(root);
		}
		foreach (Phase phase in animator.Phases)
		{
			phase.Exit();
		}
	}

	void DrawToolStripEvents(FlatRideParkitectObject animator, Transform root)
	{
		string button;
		if (_isPlaying)
		{
			button = "Stop";
			GUI.color = Color.red / 1.3f;
		}
		else
		{
			button = "Play";
			GUI.color = Color.green / 1.3f;
		}

		if (_isPlaying)
		{
			if (!animator.Animating)
			{
				ResetMotors(animator, root);
				animator.CurrentPhase = null;
			}
			_isPlaying = animator.Animating;
		}
		if (GUILayout.Button(button, EditorStyles.toolbarButton))
		{
			_reflectionProbes = FindObjectsOfType<ReflectionProbe>().ToList();
			_isPlaying = !_isPlaying;
			if (_isPlaying)
			{
				animator.Animate(root);
				_isPlaying = animator.Animating;

			}
			else
			{
				ResetMotors(animator, root);

				animator.CurrentPhase = null;
			}
		}
		GUI.color = Color.white;
		GUILayout.FlexibleSpace();

	}

	void OnDestroy()
	{
		SceneView.onSceneGUIDelegate -= this.OnSceneGUI;
	}

	void OnSceneGUI(SceneView sceneView)
	{
		FlatRideParkitectObject animator = GetActiveAnimator();
		if (animator == null)
			return;
		Transform sceneTransform = getSceneTransform();
		if (sceneTransform == null)
			return;
		Handles.zTest = CompareFunction.Always;

		// if (ModManager && ModManager.asset != null && ModManager.asset.type == ParkitectObject.ObjType.FlatRide)
		{

			foreach (Rotator R in animator.Motors.OfType<Rotator>().ToList())
			{

				if (R)
				{
					if (R.RotationAxis != Vector3.zero)
					{
						Transform transform = R.Axis.FindSceneRefrence(sceneTransform);
						if (transform != null)
						{

							Handles.color = new Color(R.ColorIdentifier.r, R.ColorIdentifier.g, R.ColorIdentifier.b, 1);
							Handles.CircleCap(0, transform.position, transform.rotation * Quaternion.LookRotation(R.RotationAxis), .3f);
							Handles.color = new Color(R.ColorIdentifier.r, R.ColorIdentifier.g, R.ColorIdentifier.b, 0.1f);
							Handles.DrawSolidDisc(transform.position, transform.rotation * R.RotationAxis, .3f);
						}

					}
				}
			}
			foreach (RotateBetween R in animator.Motors.OfType<RotateBetween>().ToList())
			{
				if (R)
				{
					Transform transform = R.Axis.FindSceneRefrence(sceneTransform);
					if (transform)
					{

						Color color = new Color(R.ColorIdentifier.r, R.ColorIdentifier.g, R.ColorIdentifier.b, .8f);
						float rayRange = 2.0f;
						Quaternion leftRayRotation = transform.localRotation;
						Quaternion rightRayRotation = Quaternion.Euler(transform.localEulerAngles + R.RotationAxis);
						if (!_isPlaying)
						{
							Vector3 leftRayDirection = leftRayRotation * transform.up;
							Vector3 rightRayDirection = rightRayRotation * transform.up;
							Debug.DrawRay(transform.position, leftRayDirection * rayRange, color);
							Debug.DrawRay(transform.position, rightRayDirection * rayRange, color);

						}
					}

				}
			}
			foreach (Mover R in animator.Motors.OfType<Mover>().ToList())
			{
				if (R != null)
				{
					Transform transform = R.Axis.FindSceneRefrence(sceneTransform);
					if (transform != null)
					{

						Handles.color = new Color(R.ColorIdentifier.r, R.ColorIdentifier.g, R.ColorIdentifier.b, 1);
						Handles.DrawLine(transform.position, transform.TransformPoint(R.ToPosition) - transform.localPosition);
						Handles.color = new Color(R.ColorIdentifier.r, R.ColorIdentifier.g, R.ColorIdentifier.b, 0.5f);
						Handles.SphereCap(0, transform.position, transform.rotation, .1f);
						Handles.SphereCap(0, transform.TransformPoint(R.ToPosition) - transform.localPosition, transform.rotation, .1f);
					}
				}
			}
			Handles.BeginGUI();


			Handles.EndGUI();
		}
	}

	private void CallbackFunction()
	{
		if (ModPayload.Instance.SelectedParkitectObject == null)
			return;
		FlatRideParkitectObject animator = GetActiveAnimator();
		if (animator == null)
			return;

		Transform sceneTransform = this.getSceneTransform();
		if (sceneTransform == null)
			return;

		if (_isPlaying)
		{
			foreach (ReflectionProbe RP in _reflectionProbes)
			{
				RP.RenderProbe();
			}
			repaintWindow();

			animator.Run(sceneTransform);
		}
	}

	void OnDisable()
	{
		EditorApplication.update -= CallbackFunction;
	}

}

public static class ListExtensions
{
    public static void Move<T>(this IList<T> list, int iIndexToMove,
        MoveDirection direction)
    {

        if (direction == MoveDirection.Up)
        {
            var old = list[iIndexToMove - 1];
            list[iIndexToMove - 1] = list[iIndexToMove];
            list[iIndexToMove] = old;
        }
        else
        {
            var old = list[iIndexToMove + 1];
            list[iIndexToMove + 1] = list[iIndexToMove];
            list[iIndexToMove] = old;
        }
    }
    public enum MoveDirection
    {
        Up,
        Down
    }
}

