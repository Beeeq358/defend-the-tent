using UnityEngine;
using System.Collections;
using UnityEngine.UIElements;

public class BaseObject : MonoBehaviour, IDamageable, IChildObject
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
        Debug.Log(rb.isKinematic);

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
            rb.useGravity = true;
            rb.isKinematic = false;
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
        if (!collision.gameObject.CompareTag("Scenery") && !collision.gameObject.CompareTag("Player") && !collision.gameObject.CompareTag("Object"))
        {
            TakeDamage(1);
        }
    }

    public virtual void TakeDamage(int damage)
    {
        healthPoints -= damage;

    }
    public virtual void RestoreHealth(int health)
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