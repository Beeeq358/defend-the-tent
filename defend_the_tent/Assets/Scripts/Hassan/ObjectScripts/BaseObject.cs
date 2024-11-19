using UnityEngine;
using System.Collections;

public class BaseObject : MonoBehaviour, IDamageable
{
    public ObjectSO objectSO;

    private IObjectParent objectParent;

    public Rigidbody rb;

    public int healthPoints;

    [SerializeField]
    private GameObject regularVisual;
    [SerializeField]
    private GameObject selectedVisual;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (objectParent != null)
        {
            rb.useGravity = false;
            rb.isKinematic = true;
        }
        else
        {
            rb.useGravity = true;
            rb.isKinematic = false;
        }

        Die();
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

    public void SetSelectedVisual(bool isActive)
    {
        if (selectedVisual != null)
        {
            selectedVisual.SetActive(isActive);
        }

        if (regularVisual != null)
        {
            regularVisual.SetActive(!isActive);
        }
    }


    public static BaseObject SpawnObject(ObjectSO objectSO, IObjectParent objectParent)
    {
        Transform ObjectTransform = Instantiate(objectSO.objectPrefab);

        // Grab the script attached to the baseObject
        BaseObject baseObject = ObjectTransform.GetComponent<BaseObject>();

        baseObject.SetObjectParent(objectParent);

        return baseObject;
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Scenery"))
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
}