using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

[CreateAssetMenu(fileName = "New Golem", menuName = "Golem")]
public class Golem : ScriptableObject
{
	public new string name;
	public string description;

	public int hp;
	public int atk;
	public float attack_speed;
	public float speed_movement;
	public float range_attack;
	public float whirlwind_duration; // Duration of whirlwind skill
	public float whirlwind_cooldown; // Cooldown of whirlwind skill
	public float whirlwindSpeed; // speed of whirlwind skill
	public Sprite prefab_animation;
}