using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class FireElementController : ElementController
{
    protected override void PerformNormalAttack()
    {
        if (attackDirection.magnitude > 0)
        {
            // Chuyển đổi hướng kéo sang tọa độ thế giới
            Vector3 attackDirectionWorld = new Vector3(attackDirection.x, attackDirection.y, 0).normalized;
            Vector3 attackDirectionWorld2 = Camera.main.WorldToScreenPoint(attackDirectionWorld);
            // Tính toán góc quay dựa trên hướng của đạn tấn công
            float angle = Mathf.Atan2(attackDirectionWorld2.y, attackDirectionWorld2.x) * Mathf.Rad2Deg;

            // Tạo Quaternion để đảm bảo đạn tấn công quay theo hướng bay
            Quaternion attackRotation = Quaternion.Euler(0, 0, angle);
            if (Time.time - lastAttackTime > attackCooldown)
            {
                if (currentElement != null && currentElement.normalAttackPrefab != null)
                {
                   
                    // Tạo đạn và định vị nó tại điểm tấn công với phép quay tương ứng
                    GameObject attack = Instantiate(currentElement.normalAttackPrefab, normalAttackPoint.position,attackRotation);
                    Projectile projectile = attack.GetComponent<Projectile>();

                    if (projectile != null)
                    {
                        // Áp dụng rotation cho đạn
                        projectile.Initialize(attackDirectionWorld, currentElement.normalAttackSpeed, currentElement.normalAttackDamage);
                        lastAttackTime = Time.time;
                    }
                    else
                    {
                        Debug.LogError("Projectile component not found on normalAttackPrefab");
                    }
                }
                else
                {
                    Debug.LogError("Normal attack prefab or currentElement is null");
                }

                // Đặt lại hướng tấn công
                attackDirection = Vector2.zero;

                Debug.Log("Performing attack in direction: " + attackDirection);
            }
            else
            {
                Debug.Log("Attack is on cooldown, no attack performed.");
            }

        }
        else
        {
            Debug.Log("Attack direction is zero, no attack performed.");
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
        // Calculate the rotation angle based on the direction
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Rotate the abilityCanvas according to the calculated angle
        Quaternion rotation = Quaternion.Euler(0, 0, angle);
        abilityCanvas.transform.rotation = rotation;
    }
    protected override void ActivateSkill(Vector3 position)
    {
        if (currentElement.skillPrefab != null)
        {
            // Instantiate the skill prefab at the specified position
            GameObject skill = Instantiate(currentElement.skillPrefab, position, Quaternion.identity);

            // Calculate direction from player to the skill's position
            Vector2 direction = ((Vector2)position - (Vector2)playerTransform.position).normalized;

            // Calculate rotation angle based on direction
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // Create a Quaternion to rotate the skill
            Quaternion skillRotation = Quaternion.Euler(0, 0, angle);

            // Apply rotation to the skill
            skill.transform.rotation = skillRotation;

            // Add FireWallUnit component to each child of the skill (FireWall)
            WindWallUnit[] units = skill.GetComponentsInChildren<WindWallUnit>();
            foreach (var unit in units)
            {
                unit.damage = currentElement.skillDamage;
            }

            // Destroy the skill after a certain duration
            Destroy(skill, currentElement.skillDuration);

            // Update the last skill activation time
            lastSkillTime = Time.time;
        }
        else
        {
            Debug.LogError("Skill prefab or element is null");
        }
    }

   

    
}
