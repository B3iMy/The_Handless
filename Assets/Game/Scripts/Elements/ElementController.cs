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
    public Transform skillPoint;

    public float attackCooldown = 0.5f;
    public float skillCooldown = 2;  

    protected virtual void OnAttackJoystickDown(PointerEventData eventData) { }

    protected virtual void OnAttackJoystickUp(PointerEventData eventData) { }

    protected virtual void OnAttackJoystickDrag(PointerEventData eventData) { }

    protected virtual void Ability2Canvas(Vector3 worldPosition) { }


    protected virtual void PerformNormalAttack() { }


    protected virtual void OnSkillButtonDown(BaseEventData eventData) { }
    
    protected virtual void OnSkillButtonUp(BaseEventData eventData) { }
    protected virtual void OnSkillButtonDrag(BaseEventData eventData) { }
    protected virtual void ActivateSkill(Vector3 position) { }
   
}
