<<<<<<< HEAD
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
=======
﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
>>>>>>> Khánh
using UnityEngine.UI;

public abstract class ElementController : MonoBehaviour
{
<<<<<<< HEAD
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
=======
    public Element currentElement;
    public Transform normalAttackPoint;
    public Transform skillPoint;

    public float attackCooldown = 0.5f;
    public float skillCooldown = 2f;
    

    public Button attackButton;
    public Button skillButton;

    protected float lastAttackTime;
    protected float lastSkillTime;

    // Indicator initial field
    [SerializeField] protected Vector2 abilityPos;
    [SerializeField] protected Canvas abilityCanvas;
    [SerializeField] protected Image abilityCircleImg;
    [SerializeField] protected float maxAbilityDistance = 7f;
    [SerializeField] protected Transform playerTransform;

    [SerializeField] protected Vector3 position;
    protected RaycastHit2D hit;
    [SerializeField] protected bool isAiming = false;

    protected virtual void Start()
    {
        attackButton.onClick.AddListener(PerformNormalAttack);

        // Add EventTrigger for skill button
        EventTrigger trigger = skillButton.GetComponent<EventTrigger>();
        if (trigger == null)
        {
            trigger = skillButton.AddComponent<EventTrigger>();
        }
        AddEventTriggerListener(trigger, EventTriggerType.PointerDown, OnSkillButtonDown);
        AddEventTriggerListener(trigger, EventTriggerType.PointerUp, OnSkillButtonUp);
        AddEventTriggerListener(trigger, EventTriggerType.Drag, OnSkillButtonDrag);

        abilityCircleImg.enabled = false;
        abilityCanvas.enabled = false;
    }

    private void Update()
    {
        if (isAiming)
        {
#if UNITY_EDITOR
            Vector3 mousePosition = Input.mousePosition;
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            worldPosition.z = 0; // Set z to 0 since we are in 2D
            Ability2Canvas(worldPosition);
#else
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                Vector3 touchPosition = touch.position;
                Vector3 worldPosition = Camera.main.ScreenToWorldPoint(touchPosition);
                worldPosition.z = 0; // Set z to 0 since we are in 2D
                Ability2Canvas(worldPosition);
            }
#endif
        }
    }

protected abstract void Ability2Canvas(Vector3 worldPosition);


protected abstract void PerformNormalAttack();
   

    protected virtual void OnSkillButtonDown(BaseEventData eventData)
    {
        Debug.Log("click!!!");
        if (Time.time - lastSkillTime > skillCooldown)
        {
            abilityCanvas.enabled = true;
            abilityCircleImg.enabled = true;
            Cursor.visible = false;
            isAiming = true;
        }
    }

    protected virtual void OnSkillButtonUp(BaseEventData eventData)
    {
        PointerEventData pointerData = eventData as PointerEventData;
        if (isAiming)
        {
            Vector3 pointerPosition = pointerData.position;
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(pointerPosition);
            worldPosition.z = 0; // Set z to 0 since we are in 2D
            ActivateSkill(worldPosition);
            isAiming = false;
            abilityCanvas.enabled = false;
            abilityCircleImg.enabled = false;
            Cursor.visible = true;
        }
    }

    protected virtual void OnSkillButtonDrag(BaseEventData eventData)
    {
        PointerEventData pointerData = eventData as PointerEventData;
        if (pointerData != null)
        {
            Vector3 pointerPosition = pointerData.position;
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(pointerPosition);
            worldPosition.z = 0; // Set z to 0 since we are in 2D
            Ability2Canvas(worldPosition);
        }
    }

    protected abstract void ActivateSkill(Vector3 position);

    private void AddEventTriggerListener(EventTrigger trigger, EventTriggerType eventType, System.Action<BaseEventData> action)
    {
        EventTrigger.Entry entry = new EventTrigger.Entry { eventID = eventType };
        entry.callback.AddListener((eventData) => action(eventData));
        trigger.triggers.Add(entry);
    }
>>>>>>> Khánh
}
