using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinProjectile : MonoBehaviour
{

    private bool isPlayerColliding = false;
    private bool isEnemyColliding = false;
    [SerializeField] protected GameObject player;
    [SerializeField] protected GameObject Enemy;

    private float damageInterval = 0.5f;
    private float nextDamageTime = 0f;
    public float speed = 0f;
    public float lifetime = 5f;
    [SerializeField] private float damage = 10f;


    private Vector2 direction;

    private Rigidbody2D rb; // Rigidbody của lốc
    private Collider2D damageTrigger; // Collider2D của DamageTrigger


    // Khởi tạo lốc với hướng di chuyển
    public void Initialize(Vector2 direction, float speed)
    {
        this.direction = direction.normalized;
        this.speed = speed;
        
     
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, lifetime); // Hủy lốc sau khi sống trong lifetime giây
    }

        
    private void FixedUpdate()
    {
        transform.Translate(speed * Time.deltaTime * (Vector3)direction, Space.World);
        //rb.velocity = direction * speed;

    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Kiểm tra xem va chạm với Player
        if (collision.gameObject.CompareTag("Enemy"))
        {
            isPlayerColliding = true;
            isEnemyColliding = true;
            Enemy = collision.gameObject;

            // Gây sát thương ngay lập tức
            ApplyDamageToPlayer();
            nextDamageTime = Time.time + damageInterval;
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {

        if (isEnemyColliding && Enemy != null)
        {
            if (Time.time >= nextDamageTime)
            {
                ApplyDamageToPlayer();
                nextDamageTime = Time.time + damageInterval;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // Kiểm tra xem va chạm với Player
        if (collision.gameObject.CompareTag("Player"))
        {
            isEnemyColliding = true;
            Enemy = collision.gameObject;

            // Gây sát thương ngay lập tức
            ApplyDamageToPlayer();
            nextDamageTime = Time.time + damageInterval;
        }
        if (collision.gameObject.CompareTag("FireBall"))
        {
            Debug.Log("Tornador va cham voi fireBall");
            
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        isEnemyColliding = true;
        isPlayerColliding= true;
        if(collision.CompareTag("Enemy")|| collision.CompareTag("Player")){
            // Gây damage cho Enemy
            EnemyBehaviour enemyBehaviour = collision.GetComponent<EnemyBehaviour>();
            PlayerBehaviour playerBehaviour = collision.GetComponent<PlayerBehaviour>();
            if (enemyBehaviour != null)
            {
                enemyBehaviour.TakeHit(damage,player);
                Debug.Log("Enemy took damage: " + damage);
            }
            else if (playerBehaviour != null)
            {
                playerBehaviour.TakeHit(damage);
            }
            else
            {
                Debug.LogError("No Enemy script found on collided object.");
            }

        

        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") || collision.CompareTag("Player"))
        {
            // Gây damage cho Enemy
            EnemyBehaviour enemyBehaviour = collision.GetComponent<EnemyBehaviour>();
            PlayerBehaviour playerBehaviour = collision.GetComponent<PlayerBehaviour>();
            if (enemyBehaviour != null && isEnemyColliding)
            {
                if (Time.time >= nextDamageTime)
                {
                    enemyBehaviour.TakeHit(damage,player);
                    nextDamageTime = Time.time + damageInterval;
                    Debug.Log("Enemy took damage: " + damage);
                }
            }
            else if (playerBehaviour != null && isPlayerColliding)
            {
                if (Time.time >= nextDamageTime)
                {
                    playerBehaviour.TakeHit(damage);
                    nextDamageTime = Time.time + damageInterval;
                }
            }
            else
            {
                Debug.LogError("No Enemy script found on collided object.");
            }
        }
    }

    // Xử lý va chạm với Projectile (đòn tấn công)

    private void ApplyDamageToPlayer()
    {
        // Thêm logic xử lý mất máu của người chơi ở đây
        Debug.Log("Player mất máu!");
        // Ví dụ: Gây sát thương cho người chơi
        PlayerBehaviour playerController = player.GetComponent<PlayerBehaviour>();
        if (playerController != null)
        {
            playerController.TakeHit(10); // Giả sử phương thức TakeDamage tồn tại trong PlayerController
        }
    }
}
