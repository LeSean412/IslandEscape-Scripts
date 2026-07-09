using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    public float health = 100f;
    public float attackDamage = 10f;
    public float attackRange = 1.5f;
    public Transform playerTarget;
    public float detectionRange = 10f;
    public float movementSpeed = 3f;
    public bool isDead = false;

    void Update()
    {
        if (isDead) return;

        // 🌟 FORCE THE JAGUAR TO ALWAYS UPDATE TO THE CURRENT ACTIVE SIBLING:
        if (GameManager.Instance != null && GameManager.Instance.activePlayer != null)
        {
            playerTarget = GameManager.Instance.activePlayer.transform;
        }

        // If we have a valid target, calculate hunting distance
        if (playerTarget != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, playerTarget.position);

            if (distanceToPlayer < detectionRange)
            {
                ChasePlayer();
            }
        }
    }
    void ChasePlayer()
    {
        Vector3 direction = (playerTarget.position - transform.position);
        direction.y = 0; // Lock to horizontal plane rotation pathing

        transform.position += direction.normalized * movementSpeed * Time.deltaTime;

        if (direction.magnitude > 0.1f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 5f * Time.deltaTime);
        }

        if (Vector3.Distance(transform.position, playerTarget.position) < attackRange)
        {
            AttackPlayer();
        }
    }

    void AttackPlayer()
    {
        Debug.Log(gameObject.name + " is Attacking the player!");
        PlayerHealth pH = playerTarget.GetComponent<PlayerHealth>();
        if (pH != null)
        {
            pH.TakeDamage(attackDamage * Time.deltaTime);
        }
    }

    public void TakeDamage(float dmg)
    {
        health -= dmg;
        if (health <= 0 && !isDead)
        {
            isDead = true;
            Debug.Log(gameObject.name + " was Defeated!");
            Destroy(gameObject, 1.5f);
        }
    }
}