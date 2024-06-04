using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") )
        {
            Debug.Log("player bi tan cong");
        }

    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log("player dang trong vung tan cong");
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("player thoat khoi vung tan cong");
    }
}
