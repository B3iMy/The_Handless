using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class WindControler : ElementController
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

        // Calculate direction and distance directly from playerTransform to the worldPosition
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
        if (currentElement != null && currentElement.skillPrefab != null)
        {
            GameObject skill = Instantiate(currentElement.skillPrefab, transform.position, transform.rotation);
            Projectile projectile = skill.GetComponent<Projectile>();
            if (projectile != null)
            {
                Vector2 direction = (position - transform.position).normalized;
                projectile.Initialize(direction, currentElement.skillSpeed, currentElement.skillDamage);
            }
            else
            {
                Debug.LogError("Skill prefab does not have a Projectile component");
            }
        }
        else
        {
            Debug.LogError("Skill prefab or currentElement is null");
        }
    }

}