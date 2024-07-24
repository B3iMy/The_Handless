using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
public class DamageTracker : MonoBehaviour
{
	public Dictionary<string, float> PlayerDamageDict { get; private set; } = new Dictionary<string, float>();

	private void Awake()
	{
		PlayerDamageDict = new Dictionary<string, float>();
	}

	public void AddDamage(string playerName, float damage)
	{
		if (PlayerDamageDict.ContainsKey(playerName))
		{
			PlayerDamageDict[playerName] += damage;
		}
		else
		{
			PlayerDamageDict[playerName] = damage;
		}

		// Mark the object as dirty to ensure the Inspector updates
		UnityEditor.EditorUtility.SetDirty(this);
	}
}
#endif