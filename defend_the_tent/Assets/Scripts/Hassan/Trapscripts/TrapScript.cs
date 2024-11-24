using UnityEngine;

public class TrapScript : MonoBehaviour
{
    public enum TrapType
    {
        Landmine,
        Beartrap,
        Banana
    }

    public TrapType type;

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
        if (collision.gameObject.tag == "Boss")
        {
            TrapTriggered(collision.gameObject);

            Destroy(gameObject);
        }
    }

    private void TrapTriggered(GameObject boss)
    {
        Rigidbody bossRB = boss.GetComponent<Rigidbody>();
        bossRB.isKinematic = true;

        if (this.type == TrapType.Landmine)
        {
            bossRB.AddExplosionForce(2f, transform.position, 10f, 2f, ForceMode.Impulse);
        }
    }
}
