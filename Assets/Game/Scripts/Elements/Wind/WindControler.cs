using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public  class WindControler : ElementController
{
    protected float lastAttackTime;
    public Transform playerTransform; // Tham chiếu tới transform của người chơi
    public float maxAbilityDistance = 5f; // Khoảng cách tối đa của kỹ năng
    public Button skillButton; // Tham chiếu tới nút kỹ năng
    public Image skillIndicator; // Tham chiếu tới chỉ báo kỹ năng (Canvas/Image)
    public Canvas abilityCanvas; // Canvas cho kỹ năng
    private bool isAiming = false;
    private float lastSkillTime = 0f;
    private Vector3 skillIndicatorInitialPosition; // Lưu vị trí ban đầu của skill indicator
    private Vector2 skillDirection; // Hướng của kỹ năng
    protected void Start()
    {
        // Đặt lastAttackTime và lastSkillTime về giá trị sao cho người chơi có thể tấn công ngay lập tức
        lastAttackTime = -attackCooldown;
        lastSkillTime = -skillCooldown;
        // Add EventTrigger for skill button
        EventTrigger trigger = skillButton.GetComponent<EventTrigger>();
        if (trigger == null)
        {
            trigger = skillButton.gameObject.AddComponent<EventTrigger>();
        }
        AddEventTriggerListener(trigger, EventTriggerType.PointerDown, OnSkillButtonDown);
        AddEventTriggerListener(trigger, EventTriggerType.PointerUp, OnSkillButtonUp);
        AddEventTriggerListener(trigger, EventTriggerType.Drag, OnSkillButtonDrag);

        skillIndicator.enabled = false;
        abilityCanvas.enabled = false;

        // Lưu vị trí ban đầu của skill indicator khi khởi động
        skillIndicatorInitialPosition = skillIndicator.transform.localPosition;

    }

    private void Update()
    {
        if (isAiming)
        {
#if UNITY_EDITOR
            Vector3 mousePosition = Input.mousePosition;
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            worldPosition.z = 0; // Set z to 0 since chúng ta đang ở 2D
            UpdateSkillIndicator(worldPosition);
#else
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                Vector3 touchPosition = touch.position;
                Vector3 worldPosition = Camera.main.ScreenToWorldPoint(touchPosition);
                worldPosition.z = 0; // Set z to 0 since chúng ta đang ở 2D
                UpdateSkillIndicator(worldPosition);
            }
#endif
        }
    }

    protected override void OnSkillButtonDown(BaseEventData eventData)
    {
        Debug.Log("Skill Button Down!!!");
        if (Time.time - lastSkillTime >= skillCooldown)
        {
            abilityCanvas.enabled = true;
            skillIndicator.enabled = true; // Hiển thị chỉ báo kỹ năng
            isAiming = true;

            // Lưu vị trí ban đầu của skill indicator khi bắt đầu kéo
            skillIndicatorInitialPosition = skillIndicator.transform.localPosition;
        }
        else
        {
            Debug.Log("Skill is on cooldown.");
        }
    }

    protected override void OnSkillButtonUp(BaseEventData eventData)
    {
       
        Debug.Log("Skill Button Up!!!");
        if (isAiming)
        {
            ActivateSkill(playerTransform.position + (Vector3)skillDirection);
            isAiming = false;
            abilityCanvas.enabled = false;
            skillIndicator.enabled = false; // Ẩn chỉ báo kỹ năng
        }
    }

    protected override void OnSkillButtonDrag(BaseEventData eventData)
    {
        
        PointerEventData pointerData = eventData as PointerEventData;
        if (pointerData != null)
        {
            Debug.Log("Tornado skill is Draging");
            Vector3 pointerPosition = pointerData.position;
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(pointerPosition);
            worldPosition.z = 0; // Đảm bảo rằng chúng ta ở không gian 2D

            // Tính toán hướng từ vị trí của nhân vật (playerTransform.position) đến vị trí của chuột (worldPosition)
            skillDirection = ((Vector2)worldPosition - (Vector2)playerTransform.position).normalized;
            Debug.Log("SkillDirection: " + skillDirection);
            // Cập nhật chỉ báo kỹ năng dựa trên hướng tính toán
            UpdateSkillIndicator(playerTransform.position + (Vector3)skillDirection);
        }
    }

    private void UpdateSkillIndicator(Vector3 worldPosition)
    {
        
        Vector2 direction = ((Vector2)worldPosition - (Vector2)playerTransform.position).normalized;
        Vector2 newHitpoint = (Vector2)playerTransform.position + direction;

        abilityCanvas.transform.position = new Vector3(newHitpoint.x, newHitpoint.y, abilityCanvas.transform.position.z);
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(0, 0, angle);
        abilityCanvas.transform.rotation = rotation;

        // Cập nhật chỉ báo kỹ năng, nhưng giữ vị trí ban đầu của skill indicator
        skillIndicator.transform.position = playerTransform.position;
        skillIndicator.transform.rotation = rotation;
    }

    protected override void ActivateSkill(Vector3 position)
    {
        
        if (currentElement != null && currentElement.skillPrefab)
        {
            // Instantiate the skill prefab at the specified position
            GameObject skill = Instantiate(currentElement.skillPrefab, playerTransform.position, Quaternion.identity);
            Debug.Log("Skill instantiated: " + skill.name);

            // Debug: List all components attached to the instantiated skill object
            Component[] components = skill.GetComponents<Component>();
            foreach (Component component in components)
            {
                Debug.Log("Component on skill: " + component.GetType().Name);
            }

            // Initialize the WinProjectile component of the skill
            WinProjectile wind1 = skill.GetComponent<WinProjectile>();

            if (wind1 != null)
            {
                Debug.Log("WinProjectile component found. Initializing...");
                wind1.Initialize(skillDirection, currentElement.skillSpeed);
            }
            else
            {
                Debug.LogError("WinProjectile component not found on the instantiated skill prefab.");
            }

            // Destroy the skill after a certain duration
            Destroy(skill, currentElement.skillDuration);

            // Update the last skill activation time
            lastSkillTime = Time.time;
        }
        else
        {
            Debug.LogError("Skill prefab is null.");
        }
    }

    private void AddEventTriggerListener(EventTrigger trigger, EventTriggerType eventType, System.Action<BaseEventData> action)
    {
        EventTrigger.Entry entry = new EventTrigger.Entry { eventID = eventType };
        entry.callback.AddListener((eventData) => action(eventData));
        trigger.triggers.Add(entry);
    }
}