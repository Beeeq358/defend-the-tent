using UnityEngine;

public class BuildableObject : BaseObject, IBuildable
{
    public bool _isInteractive = true;
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
        if (collision.gameObject.CompareTag("Scenery") || collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Object"))
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
        _isInteractive = false;
        Debug.Log(IsKinematic());
    }
}