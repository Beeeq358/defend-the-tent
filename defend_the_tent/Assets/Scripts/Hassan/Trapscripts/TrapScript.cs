using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapScript : MonoBehaviour, IChildObject
{
    public event Action<IChildObject> OnDestroyed;

    private void OnDestroy()
    {
        OnDestroyed?.Invoke(this);
    }

    protected IObjectParent objectParent;

    [SerializeField]
    private Rigidbody rb;

    [SerializeField]
    private GameObject explosionVFX, explosionLight;

    [SerializeField]
    private List<Collider> mainColliders = new List<Collider>();

    public enum TrapType
    {
        Landmine,
        Glue,
        Banana
    }

    public TrapType type;

    [SerializeField]
    private GameObject visual;

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
                if (objectParent == null)
                {
                    bossRB.mass = 4f;
                    bossRB.AddExplosionForce(20f, transform.position, 10f, 2f, ForceMode.Impulse);
                    boss.GetComponentInParent<PlayerHealth>().TakeDamage(15);
                    Instantiate(explosionVFX, transform.position, Quaternion.identity);
                    Instantiate(explosionLight, transform.position, Quaternion.identity);
                    Destroy(gameObject);
                }
                break;
            case TrapType.Glue:
                StartCoroutine(GlueCoroutine(bossRB));
                break;
            case TrapType.Banana:
                playerMovement.IsStunned(3, true);
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
        foreach (Collider collider in mainColliders)
        {
            collider.enabled = false;
        }
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
            foreach (Collider collider in mainColliders)
            {
                collider.enabled = true;
            }
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
}
