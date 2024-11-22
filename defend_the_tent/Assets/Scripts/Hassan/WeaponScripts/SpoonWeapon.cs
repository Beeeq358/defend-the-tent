using UnityEngine;

public class SpoonWeapon : BaseWeapon
{
    private PlayerInteract playerInteract;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        attackTimer += Time.deltaTime;
        if (objectParent != null)
        { 
            playerInteract = (PlayerInteract)objectParent;
            playerInteract.OnPlayerAttack.AddListener(() => this.Attack(weaponSO.weaponDamage));
        }
    }

    protected override void Attack(int weaponDamage)
    {
        if (attackTimer > weaponSO.attackSpeed)
        {
            Debug.Log("Attack: " + weaponDamage);
            // Perform a raycast or detect collision to determine if the "Boss" was hit
            RaycastHit hit;

            // Assume the attack originates from the player's position
            Vector3 attackOrigin = transform.position;
            Vector3 attackDirection = transform.forward;

            float attackRange = weaponSO.weaponRange;

            // Perform a raycast
            if (Physics.Raycast(attackOrigin, attackDirection, out hit, attackRange))
            {
                // Check if the hit object has the tag "Boss"
                if (hit.collider.CompareTag("Boss"))
                {
                    // Apply damage to the Boss
                    PlayerHealth boss = hit.collider.GetComponent<PlayerHealth>();
                    if (boss != null)
                    {
                        healthPoints--;
                        boss.TakeDamage(weaponDamage);
                    }
                }
            }
        }
       
    }
}
