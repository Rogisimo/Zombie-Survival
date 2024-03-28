using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Transform target; // The target that the enemy will move towards (usually the player)
    public float speed = 5f; // Movement speed of the enemy
    public float stoppingDistance = 2f; // Minimum distance to keep from the target
    public float attackInterval = 2f; // The interval between attacks
    private float timeSinceLastAttack = 0f; // Time passed since the last attack
    public float damage = 10f; // Damage this enemy can inflict
    public float health = 50f;

    private bool isDead = false;
    private bool isAttacking = false;

    private Rigidbody rb;
    private Animator anim;
    Player player;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        anim = GetComponent<Animator>();
        player = FindObjectOfType<Player>();
    }

    void Update()
    {
        if (!isDead)
        {
            timeSinceLastAttack += Time.deltaTime;

            bool isAttacking = anim.GetCurrentAnimatorStateInfo(0).IsName("Zombie attack");

            if (!isAttacking)
            {
                MoveTowardsTarget();
            }

            if (timeSinceLastAttack >= attackInterval && !isAttacking && Vector3.Distance(target.position, transform.position) <= stoppingDistance)
            {
                AttackPlayer();
                timeSinceLastAttack = 0f;
            }
        }
    }

    void MoveTowardsTarget()
    {
        if (target != null && !isAttacking) // Check if the enemy is not attacking
        {
            Vector3 direction = (target.position - transform.position).normalized;
            direction.y = 0; // This ensures that we only rotate around the Y axis

            // Ensure the enemy is always facing the target
            if (direction != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * speed);
            }

            if (Vector3.Distance(target.position, transform.position) <= stoppingDistance)
            {
                if (timeSinceLastAttack >= attackInterval)
                {
                    isAttacking = true; // Set the attacking flag to true
                    AttackPlayer(); // Perform the attack
                    timeSinceLastAttack = 0f; // Reset the time since last attack
                }
            }
            else
            {
                Vector3 movePosition = transform.position + direction * speed * Time.deltaTime;
                rb.MovePosition(movePosition);
            }
        }
    }

    void AttackPlayer()
    {
        anim.SetTrigger("Attack");
        // Implement the attack logic here. For now, just a debug log
        Debug.Log("Attacking the player with " + damage + " damage.");
        player.TakeDamage(damage);
        // Here you can also reduce the player's health or trigger attack animations/effects
        isAttacking = false; // Reset the attacking flag after the attack
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        Debug.Log("Enemy took " + amount + " damage.");

        if (health <= 0 && !isDead)
        {
            Death();
        }
    }

    void Death()
    {
        Debug.Log("Enemy died.");
        anim.SetBool("Dead", true);
        isDead = true;
        Destroy(gameObject, 5f);
    }
}
