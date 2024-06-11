using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MovementCharacter : MonoBehaviour
{
    public float moveSpead = 15.0f;
    private Rigidbody2D rb;
    [SerializeField] protected Joystick joystick;
    private float moveHorizontal;
    private float moveVertical;
    private Vector2 movement;
    public bool rightFacing = true;

    private Animator anim;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();  
        anim = GetComponent<Animator>();
    }

    
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        moveHorizontal = joystick.Horizontal;
        moveVertical = joystick.Vertical;
        
        movement = new Vector2 (moveHorizontal, moveVertical)*moveSpead*Time.deltaTime;
    
        //Di chuyen theo vector
        rb.MovePosition(rb.position + movement);
        if (moveHorizontal > 0 && !rightFacing)
        {
            flip();
        }
        if (moveHorizontal < 0 && rightFacing)
        {
            flip();
        }
        
    }

    void flip()
    {
        Vector3 currentscale = gameObject.transform.localScale;
        currentscale.x *= -1;
        gameObject.transform.localScale = currentscale;
        rightFacing = !rightFacing;
    }
}
