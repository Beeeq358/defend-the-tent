using UnityEngine;

public class BuildableObject : BaseObject, IBuildable
{
    public bool _isInteractive = true;

    [SerializeField]
    private GameObject healthBarPrefab;
    private GameObject myHealthBar;

    [SerializeField]
    private GameObject buildingParticleSystem;

    private void Start()
    {
        myHealthBar = Instantiate(healthBarPrefab, transform.position, Camera.main.transform.rotation);
        myHealthBar.GetComponent<HealthBar>().LogOn(this.gameObject, healthPoints);
        myHealthBar.SetActive(false);
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
            if (!_isInteractive)
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
        GameObject buildingParticles = Instantiate(buildingParticleSystem, transform.position, Quaternion.identity);
        Destroy(buildingParticles, 3f);
        rb.isKinematic = isKinematic;
        _isInteractive = false;
        myHealthBar.SetActive(true);
    }

    public override void TakeDamage(int damage)
    {
        myHealthBar.GetComponent<HealthBar>().UpdateHealth(healthPoints);
        base.TakeDamage(damage);
    }
    public override void RestoreHealth(int health)
    {
        base.RestoreHealth(health);
        myHealthBar.GetComponent<HealthBar>().UpdateHealth(healthPoints);
    }
}