using UnityEngine;
using UnityEngine.Events;

public class ObjectiveScript : MonoBehaviour, IDamageable
{
    [SerializeField]
    private int objectiveHealth;

    [SerializeField]
    private GameObject healthBarPrefab;
    private GameObject myHealthBar;

    public UnityEvent OnObjectiveDestroyed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        myHealthBar = Instantiate(healthBarPrefab, transform.position, Camera.main.transform.rotation);
        myHealthBar.GetComponent<HealthBar>().LogOn(gameObject, objectiveHealth);
        myHealthBar.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Object") && !collision.gameObject.CompareTag("Scenery") && !collision.gameObject.CompareTag("Player"))
        {
            TakeDamage(1);
        }
    }

    public void TakeDamage(int damage)
    {
        objectiveHealth -= damage;
    }
    public void RestoreHealth(int health)
    {
        objectiveHealth += health;
    }
    public void Die()
    {
        if (objectiveHealth <= 0)
        {
            OnObjectiveDestroyed.Invoke();
            Destroy(gameObject);
        }
    }
    public bool IsDead()
    {
        return (objectiveHealth >= 0);
    }
}
