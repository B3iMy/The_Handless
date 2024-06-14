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

	// indicator initial field
	[SerializeField] protected Vector2 abilityPos;
    [SerializeField] protected Canvas abilityCanvas;
    [SerializeField] protected Image abilityImg;
    [SerializeField] protected Transform playerTranform;

		protected Vector2 position;
		private RaycastHit2D hit;
		private Ray2D ray;
 
    protected virtual void Start()
	{
		attackButton.onClick.AddListener(PerformNormalAttack);
		skillButton.onClick.AddListener(PerformSkill);
		abilityImg.GetComponent<Image>().enabled = false;
		abilityCanvas.enabled = false;
	}

	private void Update()
	{
		
	}

	protected abstract void PerformNormalAttack();

	protected abstract void PerformSkill();
}
