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
        base.PlayerStart();
        targetTransform = normalPlayer.transform;
        maxHealth = healthPoints;
        myHealthBar = Instantiate(healthBarPrefab, targetTransform.position, Camera.main.transform.rotation);
        myHealthBar.GetComponent<HealthBar>().LogOn(targetTransform.gameObject, maxHealth);
        myHealthBar.SetActive(true);
    }
    void Update()
    {
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

        if (myHealthBar != null)
        {
            Destroy(myHealthBar);
        }

        myHealthBar = Instantiate(healthBarPrefab, targetTransform.position, Camera.main.transform.rotation);
        myHealthBar.GetComponent<HealthBar>().LogOn(targetTransform.gameObject, maxHealth);
        myHealthBar.SetActive(true);
        healthPoints = bossHealthPoints;
        maxHealth = healthPoints;
        targetTransform = bossPlayer.transform;
    }

    public void TakeDamage(int damage)
    {
        Debug.Log("Boss Damaged!");
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
}
