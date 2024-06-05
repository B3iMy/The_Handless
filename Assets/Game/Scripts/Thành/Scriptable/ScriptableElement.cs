using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Element", menuName = "Element")]
public class Element : ScriptableObject
{
    public new string name;
    public string description;

    public int hp, atk, attack_speed, speed_movement, range_attack;

    public Sprite prefab_animation;

    public void Print()
    {
        Debug.Log(name + ": " + description);
    }
}
