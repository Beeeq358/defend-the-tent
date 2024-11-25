using System.Collections;
using UnityEngine;

public class TrapScript : BaseObject
{
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

    protected override void OnCollisionEnter(Collision collision)
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
        bossRB.isKinematic = true;

        switch (type) 
        {
            case TrapType.Landmine:
                bossRB.AddExplosionForce(2f, transform.position, 10f, 2f, ForceMode.Impulse);
                break;
            case TrapType.Glue:
                StartCoroutine(GlueCoroutine(bossRB));
                break;
            case TrapType.Banana:
                playerMovement.IsStunned(3);
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
}
