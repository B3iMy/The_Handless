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
    [SerializeField] protected Image abilityCircleImg;
    [SerializeField] protected Transform playerTranform;

    protected Vector2 position;
    protected RaycastHit2D hit;
    protected Ray2D ray;
    protected bool isSkillButtonHeld = false;

    protected virtual void Start()
	{
		attackButton.onClick.AddListener(PerformNormalAttack);
		skillButton.onClick.AddListener(PerformSkill);
		abilityCircleImg.enabled = false;
		abilityCanvas.enabled = false;
	}

	private void Update()
	{
        if (isSkillButtonHeld)
        {
            UpdateSkillIndicator();

            // Kiểm tra khi người chơi nhả nút chuột
            if (Input.GetMouseButtonUp(0))
            {
                ExecuteSkillAtPosition(abilityPos);
                isSkillButtonHeld = false;
                abilityCircleImg.enabled = false;
            }
        }
    }

	protected abstract void PerformNormalAttack();

	protected abstract void PerformSkill();
	protected abstract void UpdateSkillIndicator();
	protected abstract void ExecuteSkillAtPosition(Vector2 position);

	protected abstract void onDisable();
	protected abstract void Onenable();
	protected abstract void OnSkillButtonDown();
}
