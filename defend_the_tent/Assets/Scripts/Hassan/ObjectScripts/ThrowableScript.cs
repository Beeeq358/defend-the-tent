using NUnit.Framework.Constraints;
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

    private readonly ThrowableTypes type;

    protected override void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Boss"))
        {
            DealDamagToBoss(objectDamage, collision.gameObject);
            base.OnCollisionEnter(collision);
        }
    }

    private void DealDamagToBoss(int damage, GameObject boss)
    {
        Rigidbody bossRB = boss.GetComponent<Rigidbody>();  
        PlayerHealth playerHealth = boss.transform.parent.GetComponent<PlayerHealth>();

        // Check if the colliding object is tagged as "Boss"
        switch (type)
        {
            case ThrowableTypes.Tomato:
                playerHealth.TakeDamage(damage);
                break;
            case ThrowableTypes.TNT:
                bossRB.AddExplosionForce(20f, transform.position, 10f, 2f, ForceMode.Impulse);
                playerHealth.TakeDamage(damage);
                break;
        }
    }
}
