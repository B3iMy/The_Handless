using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.UI;

public abstract class ElementController : MonoBehaviour
{
	public Element currentElement;
	public Transform normalAttackPoint;
	public Transform skillPoint;

	public float attackCooldown = 0.5f;
	public float skillCooldown = 2f;

	public Button attackButton;
	public Button skillButton;

	protected float lastAttackTime;
	protected float lastSkillTime;

	protected virtual void Start()
	{
		attackButton.onClick.AddListener(PerformNormalAttack);
		skillButton.onClick.AddListener(PerformSkill);
	}

	private void Update()
	{
	}

	protected abstract void PerformNormalAttack();

	protected abstract void PerformSkill();
}
