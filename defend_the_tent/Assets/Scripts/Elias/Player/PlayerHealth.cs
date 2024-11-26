using UnityEngine;

public class PlayerHealth : Player, IDamageable
{
    [SerializeField]
    private int healthPoints;
    [SerializeField]
    private int bossHealthPoints;

    private int maxHealth;

    private void Start()
    {
        maxHealth = healthPoints;
    }
    void Update()
    {
        if (isBoss)
        {
            healthPoints = bossHealthPoints;
        }
        if (healthPoints <= 0)
        {
            Die();
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
        GetComponent<PlayerMovement>().IsStunned(3);
        healthPoints = maxHealth;
    }
    public bool IsDead()
    {
        return (healthPoints >= 0);
    }

    protected override void PlayerStart()
    {

    }
}
