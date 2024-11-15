using UnityEngine;

public class BuildableObject : BaseObject, IBuildable
{
    // Update is called once per frame
    void Update()
    {
        base.Die();
    }

    protected override void TakeDamage(int damage)
    {
        if (IsKinematic())
        {
            base.TakeDamage(damage);
        }
    }


    protected override void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Scenery") || collision.gameObject.CompareTag("Player"))
        {

        }
        else
        {
            TakeDamage(1);
        }
    }

    public bool IsKinematic()
    {
        return rb.isKinematic;
    }
    public void SetKinematic(bool isKinematic)
    {
        rb.isKinematic = isKinematic;
        Debug.Log(IsKinematic());
    }
}
