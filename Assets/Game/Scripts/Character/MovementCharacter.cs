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
    // Start is called before the first frame update
    void Start()
    {
     rb = GetComponent<Rigidbody2D>();   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        // lay gia tri tu ban phim
        //moveHorizontal = Input.GetAxis("Horizontal");
        //moveVertical = Input.GetAxis("Vertical");

        //lay gia tri tu joystick
        moveHorizontal = joystick.Horizontal;
        moveVertical = joystick.Vertical;
        //dat gia tri cho vector
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
        //flip();
    }

    void flip()
    {
        Vector3 currentscale = gameObject.transform.localScale;
        currentscale.x *= -1;
        gameObject.transform.localScale = currentscale;
        rightFacing = !rightFacing;
        //if (moveHorizontal < 0)
        //{
        //    Vector3 currentScale = gameObject.transform.localScale;
        //    currentScale.x *= -1;
        //    gameObject.transform.localScale = currentScale;
        //}
        //else if (moveHorizontal > 0)
        //{
        //    gameObject.transform.localScale = new Vector3(1, 1, 1);
        //}
    }
}
