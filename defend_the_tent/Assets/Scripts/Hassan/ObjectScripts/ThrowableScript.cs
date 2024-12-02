using UnityEngine;

public class ThrowableScript : BaseObject
{
    [SerializeField]
    private int objectDamage;

    public enum ThrowableTypes
    {
        Tomato,
        TNT
    }

    [SerializeField]
    private GameObject explosionVFX, explosionLight;

    [SerializeField]
    private ThrowableTypes type;

    protected override void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Boss"))
        {
            DealDamageToBoss(objectDamage, collision.gameObject);
        }
    }

    private void DealDamageToBoss(int damage, GameObject boss)
    {
        Rigidbody bossRB = boss.GetComponent<Rigidbody>();  
        PlayerHealth playerHealth = boss.transform.parent.GetComponent<PlayerHealth>();

        // Check if the colliding object is tagged as "Boss"
        switch (type)
        {
            case ThrowableTypes.Tomato:
                playerHealth.TakeDamage(damage);
                AudioManager.Instance.Play("Tomato");
                Destroy(gameObject);
                break;
            case ThrowableTypes.TNT:
                bossRB.AddExplosionForce(20f, transform.position, 10f, 2f, ForceMode.Impulse);
                playerHealth.TakeDamage(damage);
                Instantiate(explosionVFX, transform.position, Quaternion.identity);
                Instantiate(explosionLight, transform.position, Quaternion.identity);
                Destroy(gameObject);
                break;
        }
    }
}
