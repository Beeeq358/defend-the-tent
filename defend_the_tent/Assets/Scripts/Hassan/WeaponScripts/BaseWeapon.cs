using System;
using UnityEngine;
using static WeaponSO;

public class BaseWeapon : MonoBehaviour, IDamageable, IChildObject
{
    protected PlayerInteract playerInteract;

    public event Action<IChildObject> OnDestroyed;

    [SerializeField]
    private Rigidbody rb;   

    private void OnDestroy()
    {
       OnDestroyed?.Invoke(this);
    }

    [SerializeField]
    protected WeaponSO weaponSO;
    [SerializeField]
    protected int healthPoints;
    protected float attackTimer = 0f;

    protected IObjectParent objectParent;

    void Start()
    {
        healthPoints = weaponSO.weaponHealth;
    }


    protected void Update()
    {
        // Decrease the attack cooldown timer
        if (attackTimer > 0)
        {
            attackTimer -= Time.deltaTime;
        }

        // Update weapon range based on range type
        switch (weaponSO.weaponRangeType)
        {
            case WeaponRange.Short:
                weaponSO.weaponRange = 2f;
                break;
            case WeaponRange.Medium:
                weaponSO.weaponRange = 4f;
                break;
            case WeaponRange.Long:
                weaponSO.weaponRange = 6f;
                break;
            default:
                Debug.LogWarning("Unknown weapon range!");
                break;
        }

        Die();
    }

    protected virtual void Attack(int weaponDamage, GameObject boss)
    {
        PlayerHealth bosshealth = boss.transform.parent.GetComponent<PlayerHealth>();
        bosshealth.TakeDamage(weaponSO.weaponDamage);
    }

    private void OnCollisionEnter(Collision collision)
    {


        if (!collision.gameObject.CompareTag("Object") &&
            !collision.gameObject.CompareTag("Scenery") &&
            !collision.gameObject.CompareTag("Player"))
        {
            TakeDamage(1);
            Attack(weaponSO.weaponDamage, collision.gameObject);
        }
    }

    public void SetObjectParent(IObjectParent parent)
    {
        // Clear existing parent if present
        this.objectParent?.ClearObject();



        // Set new parent
        Transform parentTransform = parent.GetObjectFollowTransform();
        transform.parent = parentTransform;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        rb.isKinematic = true;
        rb.useGravity = false;
        parent.SetObject(this);

        // Debug the assignment
        this.objectParent = parent;
        Debug.Log($"Object parent set to: {this.objectParent}");
    }

    public void ClearObjectParent(Player parent)
    {
        if (this.objectParent != null)
        {
            playerInteract = null;
            rb.useGravity = true;
            rb.isKinematic = false;
            this.objectParent.ClearObject();
            transform.parent = null;
            this.objectParent = null;
        }
        else
        {
            Debug.LogWarning("objectParent is null!");
        }
    }

    public GameObject GetObjectParent()
    {
        if (this.objectParent != null)
        {
            return transform.parent.gameObject;
        }
        return null;
    }

    public GameObject GetGameObject()
    {
        return gameObject;
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
}
