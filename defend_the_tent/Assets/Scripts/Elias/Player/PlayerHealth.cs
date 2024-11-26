using UnityEngine;

public class PlayerHealth : Player, IDamageable
{
    [SerializeField]
    private int healthPoints;
    [SerializeField]
    private int bossHealthPoints;

    [SerializeField] private GameObject healthBarPrefab;

    private GameObject myHealthBar;

    private int maxHealth;

    private void Start()
    {
        targetTransform = normalPlayer.transform;
        maxHealth = healthPoints;
        myHealthBar = Instantiate(healthBarPrefab, targetTransform.position, Camera.main.transform.rotation);
        myHealthBar.GetComponent<HealthBar>().LogOn(targetTransform.gameObject, maxHealth);
        myHealthBar.SetActive(true);
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
        myHealthBar.GetComponent<HealthBar>().UpdateHealth(healthPoints);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Object") && !collision.gameObject.CompareTag("Scenery"))
        {
            TakeDamage(1);
        }
    }

    public override void BecomeBoss()
    {
        base.BecomeBoss();
        targetTransform = bossPlayer.transform;
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
