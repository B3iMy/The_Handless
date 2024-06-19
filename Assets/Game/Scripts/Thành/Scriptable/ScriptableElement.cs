using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Element", menuName = "Element")]
public class Element : ScriptableObject
{
	public GameObject normalAttackPrefab;
	public float normalAttackSpeed = 10f;
	public float normalAttackDamage = 10f;

	public GameObject skillPrefab;
	public float skillDamage = 10f;
	public float skillDuration = 2f;
    public float skillSpeed = 10f;
}
