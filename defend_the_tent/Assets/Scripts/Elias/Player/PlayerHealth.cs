using UnityEngine;

public class PlayerHealth : Player, IDamageable
{
    private int healthPoints;
    [SerializeField]
    private int bossHealthPoints;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        healthPoints = 1;
    }

    // Update is called once per frame
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
            Destroy(gameObject);
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
