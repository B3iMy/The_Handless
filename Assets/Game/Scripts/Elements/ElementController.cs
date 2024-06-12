using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public class currentElementController : MonoBehaviour
{
	public Element currentElement;
	public Transform attackPoint;
	public float attackCooldown = 0.5f;
	public float skillCooldown = 2f;

	private float lastAttackTime;
	private float lastSkillTime;

	private void Update()
	{
		HandleInput();
	}

	private void HandleInput()
	{
		if (Input.GetKeyDown(KeyCode.Space) && Time.time - lastAttackTime > attackCooldown)
		{
			PerformNormalAttack();
			lastAttackTime = Time.time;
			
		}

		if (Input.GetKeyDown(KeyCode.W) && Time.time - lastSkillTime > skillCooldown)
		{

			Debug.Log("skill is showing");
			PerformSkill();
			lastSkillTime = Time.time;
		}
	}

	private void PerformNormalAttack()
	{
		if (currentElement != null && currentElement.normalAttackPrefab != null)
		{
			GameObject attack = Instantiate(currentElement.normalAttackPrefab, attackPoint.position, attackPoint.rotation);
			Projectile projectile = attack.GetComponent<Projectile>();
			if (projectile != null)
			{
				Vector2 direction = attackPoint.right;
				projectile.Initialize(direction, currentElement.normalAttackSpeed, currentElement.normalAttackDamage);
			}
		}
		else
		{
			Debug.LogError("Normal attack prefab or currentElement is null");
		}
	}

	private void PerformSkill()
	{
		if (currentElement != null && currentElement.skillPrefab != null)
		{
			GameObject skill = Instantiate(currentElement.skillPrefab, attackPoint.position, attackPoint.rotation);
			Destroy(skill, currentElement.skillDuration);
		}
		else
		{
			Debug.LogError("Skill prefab or element is null");
		}
	}
}
