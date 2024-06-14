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
	protected override void UpdateSkillIndicator()
	{
		// Lấy vị trí của người chơi
		position = playerTranform.position;

		// Tạo một tia từ vị trí của người chơi về phía chuột
		Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Vector2 direction = mousePosition - position;
		ray = new Ray2D(position, direction);

		// Thực hiện raycast trong không gian 2D
		hit = Physics2D.Raycast(ray.origin, ray.direction);

		// Tính toán vị trí của skill indicator
		if (hit.collider != null)
		{
			// Nếu tia va chạm với một đối tượng, đặt skill indicator tại điểm va chạm
			abilityPos = hit.point;
		}
		else
		{
			// Nếu không có va chạm, đặt skill indicator tại điểm cuối của tia (ví dụ: khoảng cách tối đa)
			abilityPos = ray.origin + ray.direction * 10.0f; // 10.0f là khoảng cách tối đa
		}

		// Cập nhật vị trí của hình ảnh skill indicator trong không gian màn hình
		Vector2 screenPoint = Camera.main.WorldToScreenPoint(abilityPos);
		abilityCircleImg.transform.position = screenPoint;

		// Hiển thị skill indicator
		abilityCircleImg.enabled = true;
	}
    protected override void ExecuteSkillAtPosition(Vector2 position)
    {
        if (Time.time - lastSkillTime > skillCooldown)
        {
            if (currentElement != null && currentElement.skillPrefab != null)
            {
                GameObject skill = Instantiate(currentElement.skillPrefab, position, Quaternion.identity);

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

    protected override void Onenable()
    {
        skillButton.onClick.AddListener(OnSkillButtonDown);
    }
    protected override void onDisable()
    {
        skillButton.onClick.RemoveListener(OnSkillButtonDown);

    }

    protected override void OnSkillButtonDown()
    {
        isSkillButtonHeld = true;

    }

}
