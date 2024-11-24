using UnityEngine;

public class TrapScript : MonoBehaviour
{
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
        
    }
}
