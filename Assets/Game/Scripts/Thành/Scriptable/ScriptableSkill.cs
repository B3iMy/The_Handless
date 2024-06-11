using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

[CreateAssetMenu(fileName = "New Skill", menuName = "Skill")]
public class Skill : ScriptableObject
{
    public new string name;
    public string description;

    public int hp, atk, attack_speed, speed_movement, range_attack;

    public Sprite prefab_animation;
}
