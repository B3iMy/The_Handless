using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DamageTracker))]
public class DamageTrackerEditor : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		DamageTracker damageTracker = (DamageTracker)target;

		EditorGUILayout.Space();
		EditorGUILayout.LabelField("Damage Tracker", EditorStyles.boldLabel);

		foreach (var entry in damageTracker.PlayerDamageDict)
		{
			EditorGUILayout.LabelField($"{entry.Key}: {entry.Value} damage");
		}

		// Ensure the inspector updates immediately
		Repaint();
	}
}
