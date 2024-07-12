using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementSkill : MonoBehaviour
{
    [SerializeField] private float speed = 5f; // Tốc độ di chuyển

    // Update is called once per frame
    void Update()
    {
        // Di chuyển game object sang trái theo tốc độ đã đặt
        transform.Translate(Vector3.left * speed * Time.deltaTime);
    }

}
