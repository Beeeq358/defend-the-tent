using System.Collections;
using UnityEngine;

public class TrapScript : MonoBehaviour, IChildObject
{
    protected IObjectParent objectParent;

    [SerializeField]
    private Rigidbody rb;

    [SerializeField]
    private GameObject explosionVFX;

    public enum TrapType
    {
        Landmine,
        Glue,
        Banana
    }

    public TrapType type;

    [SerializeField]
    private GameObject visual;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the colliding object is a boss
        if (collision.gameObject.CompareTag("Boss"))
        {
            TrapTriggered(collision.gameObject);

            visual.SetActive(false);
        }
    }

    private void TrapTriggered(GameObject boss)
    {
        Rigidbody bossRB = boss.GetComponent<Rigidbody>();
        PlayerMovement playerMovement = boss.transform.parent.GetComponent<PlayerMovement>();

        switch (type) 
        {
            case TrapType.Landmine:
                bossRB.AddExplosionForce(20f, transform.position, 10f, 2f, ForceMode.Impulse);
                boss.GetComponentInParent<PlayerHealth>().TakeDamage(15);
                boss.GetComponentInParent<PlayerMovement>().IsStunned(7);
                explosionVFX.SetActive(true);
                Destroy(gameObject, 1.3f);
                break;
            case TrapType.Glue:
                StartCoroutine(GlueCoroutine(bossRB));
                break;
            case TrapType.Banana:
                playerMovement.IsStunned(1);
                Destroy(gameObject);
                break;
            default:
                break;
        }
    }

    private IEnumerator GlueCoroutine(Rigidbody bossRB)
    {
        while (true)
        {
            bossRB.linearDamping = 20;
            yield return new WaitForSeconds(3f);
            Destroy(gameObject);
            break;
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
    }

    public void ClearObjectParent(Player parent)
    {
        if (this.objectParent != null)
        {
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
        return transform.parent.gameObject;
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }
}
