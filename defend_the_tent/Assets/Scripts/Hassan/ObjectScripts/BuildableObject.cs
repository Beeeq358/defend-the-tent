using UnityEngine;

public class BuildableObject : BaseObject, IBuildable
{
    public bool _isInteractive = true;

    [SerializeField]
    private GameObject healthBarPrefab;
    private GameObject myHealthBar;

    private void Start()
    {
        myHealthBar = Instantiate(healthBarPrefab, transform.position, Camera.main.transform.rotation);
        myHealthBar.GetComponent<HealthBar>().LogOn(gameObject, healthPoints);
    }
    // Update is called once per frame
    void Update()
    {
        base.Die();
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Scenery") || collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Object"))
        {

        }
        else
        {
            if (IsKinematic())
            {
                TakeDamage(1);
            }

        }
    }

    public bool IsKinematic()
    {
        return rb.isKinematic;
    }
    public void SetKinematic(bool isKinematic)
    {
        rb.isKinematic = isKinematic;
        _isInteractive = false;
        Debug.Log(IsKinematic());
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        myHealthBar.GetComponent<HealthBar>().UpdateHealth(healthPoints);
    }
    public override void RestoreHealth(int health)
    {
        base.RestoreHealth(health);
        myHealthBar.GetComponent<HealthBar>().UpdateHealth(healthPoints);
    }
}