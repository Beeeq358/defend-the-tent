using UnityEngine;
using static WeaponSO;

public class BaseWeapon : MonoBehaviour, IDamageable, IChildObject
{
    [SerializeField]
    protected WeaponSO weaponSO;
    protected int healthPoints;

    protected IObjectParent objectParent;

    void Start()
    {
        healthPoints = weaponSO.weaponHealth;
    }


    void Update()
    {
        switch (weaponSO.weaponRangeType)
        {
            case WeaponRange.Short:
                // Logic for short range
                weaponSO.weaponRange = 2f;
                break;
            case WeaponRange.Medium:
                // Logic for medium range
                weaponSO.weaponRange = 4f;
                break;
            case WeaponRange.Long:
                // Logic for long range
                weaponSO.weaponRange = 6f;
                break;
            default:
                Debug.LogWarning("Unknown weapon range!");
                break;
        }

        Die();
    }

    protected virtual void Attack(int weaponDamage)
    {
        // Attacks to be decided by deriving classes
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Object") && !collision.gameObject.CompareTag("Scenery") && !collision.gameObject.CompareTag("Player"))
        {
            TakeDamage(1);
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
        parent.SetObject(this);

        // Debug the assignment
        this.objectParent = parent;
        Debug.Log($"Object parent set to: {this.objectParent}");
    }

    public void ClearObjectParent(Player parent)
    {
        if (this.objectParent != null)
        {
            Debug.Log("Calling ClearObject on objectParent.");
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
        return transform.parent.gameObject;
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
