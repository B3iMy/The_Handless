using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public abstract class ElementController : MonoBehaviour
{
    public Element currentElement;
    public Transform normalAttackPoint;
    public Transform skillPoint;

    public float attackCooldown = 0.5f;
    public float skillCooldown = 2f;
    

    //public Button attackButton;
    public Button skillButton;

    protected float lastAttackTime;
    protected float lastSkillTime;

    // Indicator initial field
    [SerializeField] protected Vector2 abilityPos;
    [SerializeField] protected Canvas abilityCanvas;
    [SerializeField] protected Image abilityCircleImg;
    [SerializeField] protected float maxAbilityDistance = 4f;
    [SerializeField] protected Transform playerTransform;

    [SerializeField] protected Vector3 position;
    protected RaycastHit2D hit;
    [SerializeField] protected bool isAiming = false;

    // Joystick Attack
    [SerializeField] protected FixedJoystick joystick;
    protected Vector2 joystickStartPos;
    protected Vector2 joystickDirection;
    protected Vector2 attackDirection;
    [SerializeField] protected bool isDragging = false;

    protected virtual void Start()
    {
        // Đặt lastAttackTime và lastSkillTime về giá trị sao cho người chơi có thể tấn công ngay lập tức
        lastAttackTime = -attackCooldown;
        lastSkillTime = -skillCooldown;
        //attackButton.onClick.AddListener(PerformNormalAttack);
        //Register event listener for joystick
        joystick.OnPointerDownEvent += OnAttackJoystickDown;
        joystick.OnPointerUpEvent += OnAttackJoystickUp;
        joystick.OnDragEvent += OnAttackJoystickDrag;

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
        joystick.gameObject.SetActive(true);
    }

    protected virtual void OnAttackJoystickDown(PointerEventData eventData)
    {
        //Debug.Log("joystick Down!!!");
        isDragging = true;
    }

    protected virtual void OnAttackJoystickUp(PointerEventData eventData)
    {
       // Debug.Log("joystick Up!!!");
        isDragging = false;
        if (attackDirection.magnitude > 0 && Time.time - lastAttackTime > attackCooldown)
        {
            PerformNormalAttack();
        }
        attackDirection = Vector2.zero; // Đặt lại hướng tấn công khi thả joystick
    }

    protected virtual void OnAttackJoystickDrag(PointerEventData eventData)
    {
        Debug.Log("joy stick drag!!!");
        //joystickDirection = joystick.Direction;
        if (joystickDirection.magnitude >= 0.5f)
        {
            attackDirection = joystickDirection; // Lưu hướng kéo khi kéo joystick

            //PerformNormalAttack();
        }
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
        if (isDragging)
        {
            joystickDirection = joystick.Direction;
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
        Debug.Log("joy stick up!!!");
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
        Debug.Log("joy stick is draging!!!");
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
  
}
