using UnityEngine;

public class PlayerHealth : Player, IDamageable
{
    [SerializeField]
    private int healthPoints;
    [SerializeField]
    private int bossHealthPoints;
    void Update()
    {
        if (isBoss)
        {
            healthPoints = bossHealthPoints;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Object") && !collision.gameObject.CompareTag("Scenery"))
        {
            TakeDamage(1);
        }
    }

    public void TakeDamage(int damage)
    {
        healthPoints -= damage;
    }
    public void RestoreHealth(int health)
    {
        healthPoints += health;
    }
    public void Die()
    {
        if (healthPoints <= 0)
        {
            GetComponent<PlayerMovement>().IsStunned(3);
        }
    }
    public bool IsDead()
    {
        return (healthPoints >= 0);
    }

    protected override void PlayerStart()
    {

    }
}
