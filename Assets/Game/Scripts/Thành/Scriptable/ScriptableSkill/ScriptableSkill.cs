using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

[CreateAssetMenu(fileName = "New Skill", menuName = "Skill")]
public class Skill : ScriptableObject
{
    public new string skillName; // Tên kỹ năng public
    public string description; // Miêu tả skill
    public SkillElement element; // Nguyên tố của skill
    public SkillType skillType; // Phân loại kỹ năng (ví dụ: tấn công, phòng thủ, hỗ trợ)
    Sprite skillIcon; // Biểu tượng kỹ năng
    public float skillManaCost; // Lượng mana tiêu hao khi sử dụng kỹ năng public GameObject skillPrefab; // Prefab hiệu ứng kỹ năng
    public float skillCooldown; // Thời gian hồi chiêu của kỹ năng
    public Sprite skillAnimation;
    public int hp, atk, attack_speed, speed_movement, range_attack;
    public enum SkillElement
    {

        Fire,
        Ice,
        Earth,
        Air,
    }
    public enum SkillType
    {

        Attack,
        Defense,
        Support
    }
}