using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

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

	



    protected override void Ability2Canvas(Vector3 worldPosition)
    {
        
        Vector2 direction = ((Vector2)worldPosition - (Vector2)playerTransform.position).normalized;
        float distance = Vector2.Distance(worldPosition, playerTransform.position);
        distance = Mathf.Min(distance, maxAbilityDistance);

        // Calculate the new hit point based on direction and clamped distance
        Vector2 newHitpoint = (Vector2)playerTransform.position + direction * distance;

        // Update the position of the abilityCanvas
        abilityCanvas.transform.position = new Vector3(newHitpoint.x, newHitpoint.y, abilityCanvas.transform.position.z);
    }
    protected override void ActivateSkill(Vector3 position)
    {
        if (currentElement.skillPrefab != null)
        {
            GameObject skill = Instantiate(currentElement.skillPrefab, position, Quaternion.identity);

            // Add FireWallUnit component to each child of the skill (FireWall)
            WindWallUnit[] units = skill.GetComponentsInChildren<WindWallUnit>();
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
