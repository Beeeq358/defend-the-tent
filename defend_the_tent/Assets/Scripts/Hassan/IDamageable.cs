using UnityEngine;

public interface IDamageable
{
    public void TakeDamage(int damage);
    public void RestoreHealth(int health);
    public void Die();
    public bool IsDead();
}
