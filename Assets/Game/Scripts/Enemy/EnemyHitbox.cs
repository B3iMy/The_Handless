using UnityEngine;

public class EnemyHitbox : MonoBehaviour
{
    [SerializeField] private float radius = 1f;
    [SerializeField] private LayerMask playerLayer;

    public ScriptableEntity entity;
    private GolemBehaviour golemBehaviour;

    private Animator animator;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    private void Awake()
    {
        animator = GetComponentInParent<Animator>();
        golemBehaviour = GetComponentInParent<GolemBehaviour>();
    }

    private void Update()
    {
        if (golemBehaviour != null && golemBehaviour.isUsingSkill)
        {
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, radius, playerLayer);
            foreach (Collider2D enemy in hitEnemies)
            {
                PlayerBehaviour playerHit = enemy.GetComponent<PlayerBehaviour>();

                if (playerHit != null)
                {
                    playerHit.TakeHit(entity.atk);
                }
            }
        }
    }
}
