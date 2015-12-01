using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GoalHandler))]
public class GoalEditor : Editor 
{
	public override void OnInspectorGUI()
	{
		(target as GoalHandler).ShowEditGUI ();
	}
}
