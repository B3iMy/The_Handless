using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FireElementController : ElementController
{
	protected override void PerformNormalAttack()
	{
		if (Time.time - lastAttackTime > attackCooldown)
		{
			if (currentElement != null && currentElement.normalAttackPrefab != null)
			{
				GameObject attack = Instantiate(currentElement.normalAttackPrefab, normalAttackPoint.position, normalAttackPoint.rotation);
				Projectile projectile = attack.GetComponent<Projectile>();
				if (projectile != null)
				{
					Vector2 direction = normalAttackPoint.right;
					projectile.Initialize(direction, currentElement.normalAttackSpeed, currentElement.normalAttackDamage);
					lastAttackTime = Time.time;
				}
			}
			else
			{
				Debug.LogError("Normal attack prefab or currentElement is null");
			}
		}
	}

	protected override void PerformSkill()
	{
		if (Time.time - lastSkillTime > skillCooldown)
		{
			if (currentElement != null && currentElement.skillPrefab != null)
			{
				GameObject skill = Instantiate(currentElement.skillPrefab, skillPoint.position, skillPoint.rotation);

				// Add FireWallUnit component to each child of the skill (FireWall)
				FireWallUnit[] units = skill.GetComponentsInChildren<FireWallUnit>();
				foreach (var unit in units)
				{
					
					unit.damage = currentElement.skillDamage;
				}

				Destroy(skill, currentElement.skillDuration);
				lastSkillTime = Time.time;
			}
			else
			{
				Debug.LogError("Skill prefab or element is null");
			}
		}
	}
}
