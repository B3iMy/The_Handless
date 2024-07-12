using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FireElementController : ElementController
{

    //public Button attackButton;
    public Button skillButton;
    public Transform normalAttackPoint;

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
    [SerializeField] protected float fireRate = 0.5f;
    protected float nextFireTime;

    protected void Start()
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
                    GameObject attack = Instantiate(currentElement.normalAttackPrefab, normalAttackPoint.position, attackRotation);
                    Projectile projectile = attack.GetComponent<Projectile>();


                    if (projectile != null)
                    {
                        // Thiết lập thông tin người chơi
                        projectile.owner = this.gameObject;

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

        // Tính toán điểm mới dựa trên hướng và khoảng cách đã giới hạn
        Vector2 newHitpoint = (Vector2)playerTransform.position + direction * distance;

        // Cập nhật vị trí của abilityCanvas
        abilityCanvas.transform.position = new Vector3(newHitpoint.x, newHitpoint.y, abilityCanvas.transform.position.z);

        // Tính toán góc quay dựa trên hướng
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Lấy hướng của scale của người chơi
        float playerScaleDirection = Mathf.Sign(playerTransform.localScale.x);

        // Nếu hướng của scale của người chơi là âm (quay sang trái), thì thêm 180 độ
        if (playerScaleDirection < 0)
        {
            angle += 180f;
        }

        // Tạo một quaternion từ góc quay đã tính toán
        Quaternion rotation = Quaternion.Euler(0, 0, angle);

        // Áp dụng quay cho abilityCanvas
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
        }
    }


    protected override void OnAttackJoystickDown(PointerEventData eventData)
    {
        isDragging = true;
    }
    protected override void OnAttackJoystickUp(PointerEventData eventData)
    {
        Debug.Log("joystick Up!!!");
        isDragging = false;
        if (attackDirection.magnitude > 0 && Time.time - lastAttackTime > attackCooldown)
        {
            PerformNormalAttack();
        }
        attackDirection = Vector2.zero; // Đặt lại hướng tấn công khi thả joystick
    }
    protected override void OnAttackJoystickDrag(PointerEventData eventData)
    {
        Debug.Log("joy stick drag!!!");
        //joystickDirection = joystick.Direction;
        if (joystickDirection.magnitude >= 0.5f)
        {
            attackDirection = joystickDirection; // Lưu hướng kéo khi kéo joystick

            //PerformNormalAttack();
        }
    }
    protected override void OnSkillButtonDown(BaseEventData eventData)
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

    protected override void OnSkillButtonUp(BaseEventData eventData)
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

    protected override void OnSkillButtonDrag(BaseEventData eventData)
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
    private void AddEventTriggerListener(EventTrigger trigger, EventTriggerType eventType, System.Action<BaseEventData> action)
    {
        EventTrigger.Entry entry = new EventTrigger.Entry { eventID = eventType };
        entry.callback.AddListener((eventData) => action(eventData));
        trigger.triggers.Add(entry);
    }
}

  

