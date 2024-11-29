using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInteract : Player
{
    [Header("Config")]
    public float bossSlamCooldown;
    public float bossSwipeCooldown;
    public float bossShockCooldown;
    public int bossSlamDamage;
    public int bossSwipeDamage;
    public int bossShockDamage;

    private bool isSlamming, isSwiping, isShockwave;

    [SerializeField] private Collider slamHB, halfcircleHB, shockwaveHB;
    [SerializeField] private Transform shockwaveVisual;
    [SerializeField] private GameObject explosionParticle, shockwaveParticle, slamParticle;

    public UnityEvent<Transform> OnPlayerGrab;
    public UnityEvent OnPlayerStopGrab;

    private void Start()
    {
        base.PlayerStart();
    }

    private void Update()
    {
        if (!isBoss)
        {
            HandleInteractions();
        }
        base.PlayerUpdate();
        if (input.inputAttack1)
        {
            InteractStandardAttack();
        }
        if (isBoss)
        {
            if (input.inputAttack2 && !isSlamming && !isSwiping && !isShockwave)
            {
                StartCoroutine(BossHalfSwipe());
            }
            if (input.inputAttack3 && !isSlamming && !isSwiping && !isShockwave)
            {
                StartCoroutine(BossShockWave());
            }
        }


        if (input.inputBuilded)
        {
            if (input.inputBuilded)
            {
                if (selectedChildObject is BaseObject && childObject == null)
                {
                    BaseObject baseObject = (BaseObject)selectedChildObject;
                    if (selectedChildObject != null && baseObject.objectSO.objectType == ObjectType.Buildable && !isBoss)
                    {
                        if (selectedChildObject is BuildableObject buildableObject  && buildableObject.isInteractive == true)
                        {
                            InteractBuild(this, buildableObject);
                        }
                        else
                        {
                            Debug.LogWarning("Selected object is not a BuildableObject!");
                        }
                    }
                }

            }
        }
        if (input.inputGrabStrength > 0)
        {
            if (childObject == null && !isBoss)
            {
                if (selectedChildObject != null)
                {
                    if (selectedChildObject is BuildableObject buildableObject)
                    {
                        if (!buildableObject.isInteractive)
                        {
                            Debug.LogWarning("This object is not interactive!");
                            return;
                        }
                    }
                    InteractGrab(this, selectedChildObject);
                }
                else
                {
                    Debug.LogWarning("No Object selected!");
                }
            }

        }
        if (input.inputGrabStrength == 0 && childObject != null && !isBoss)
        {
            OnPlayerStopGrab.Invoke();
            Rigidbody tempRB = childObject.GetGameObject().GetComponent<Rigidbody>();
            childObject.ClearObjectParent(this);
            tempRB.isKinematic = false;
            tempRB.AddForce(recentGrabStrength * throwStrength * (targetTransform.forward + new Vector3(0, throwHeight, 0)), ForceMode.Impulse);
        }
    }

    public void InteractGrab(Player player, IChildObject childObject)
    {
        GameObject objectToGrab = childObject.GetGameObject();
        OnPlayerGrab.Invoke(objectToGrab.transform);
        childObject.SetObjectParent(player);
    }

    public void InteractBuild(Player player, BuildableObject buildableObject)
    {
        float buildTime = 3f;
        StartCoroutine(playerMovement.StunTime(buildTime));
        StartCoroutine(Build(buildTime));
        buildableObject.SetKinematic(true);
    }

    private IEnumerator Build(float time)
    {
        Animator ani = GetComponent<PlayerInput>().animator;
        ani.SetBool("isBuilding", true);
        yield return new WaitForSeconds(time);
        ani.SetBool("isBuilding", false);
    }

    public void InteractStandardAttack()
    {
        if (isBoss)
        {
            if (!isSlamming && !isSwiping && !isShockwave)
            {
                Debug.Log("Started Coroutine bossfrontslam");
                StartCoroutine(BossFrontSlam());
            }
        }
        else
        {
            // This shouldn't happen
            //Debug.LogError("Player is neither a boss nor a player");
        }
    }

    private void HandleInteractions()
    {
        Vector2 inputVector = GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        if (moveDir != Vector3.zero)
        {
            lastInteractDir = moveDir;
        }

        float interactDistance = 2f;
        Vector3 boxHalfExtents = new Vector3(1f, 1f, 1f); 
        Vector3 direction = moveDir != Vector3.zero ? moveDir.normalized : targetTransform.forward.normalized;

        Quaternion boxOrientation = Quaternion.LookRotation(direction, Vector3.up);
        Debug.DrawRay(targetTransform.position, direction * interactDistance, Color.red, 0.1f);

        if (Physics.BoxCast(targetTransform.position, boxHalfExtents, direction, out RaycastHit boxCastHit, boxOrientation, interactDistance, objectLayerMask))
        {
            if (boxCastHit.transform.TryGetComponent(out IChildObject childObject))
            {
                if (childObject != selectedChildObject)
                {
                    if (childObject is BaseObject baseObject)
                    {
                        SetSelectedObject(baseObject);
                    }
                    else
                    {
                        SetSelectedObject(childObject);
                    }
                }
            }
            else
            {
                SetSelectedObject(null);
            }
        }
        else
        {
            SetSelectedObject(null);
        }
    }
    private void DamageColliders(int attackDamage, Collider attackHitbox)
    {
        // Handle player colliders
        List<Collider> playerColliders = attackHitbox.GetComponent<HitBox>().GetPlayerColliders();
        foreach (Collider collider in playerColliders)
        {
            var playerHealth = collider.GetComponentInParent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(attackDamage);
            }
            else
            {
                Debug.LogWarning("PlayerHealth component not found on collider parent.");
            }
        }

        // Handle object colliders
        List<Collider> objectColliders = attackHitbox.GetComponent<HitBox>().GetObjectColliders();
        foreach (Collider collider in objectColliders)
        {
            var damageable = collider.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(attackDamage);
            }
            else
            {
                Debug.LogWarning("IDamageable component not found on object collider.");
            }
        }

        // Handle specific objective collider (if needed)
        Collider objective = attackHitbox.GetComponent<HitBox>().GetObjectiveCollider();
        if (objective != null)
        {
            var objectiveScript = objective.GetComponent<ObjectiveScript>();
            if (objectiveScript != null)
            {
                objectiveScript.TakeDamage(attackDamage);
            }
            else
            {
                Debug.LogWarning("ObjectiveScript component not found on objective collider.");
            }
        }
    }


    private IEnumerator BossFrontSlam()
    {
        isSlamming = true;
        bossAnimator.SetTrigger("Slam");
        yield return new WaitForSeconds(bossSlamCooldown);
        GameObject explosion = Instantiate(slamParticle, bossTransform.position, Quaternion.identity);
        Destroy(explosion, 1.5f);
        DamageColliders(bossSlamDamage, slamHB);
        yield return new WaitForSeconds(2);
        isSlamming = false;
    }
    protected override void BaseClassExclusive()
    {

    }

    private IEnumerator BossHalfSwipe()
    {
        isSwiping = true;
        bossAnimator.SetTrigger("Swipe");
        yield return new WaitForSeconds(bossSwipeCooldown);
        //start slam VFX
        DamageColliders(bossSwipeDamage, halfcircleHB);
        yield return new WaitForSeconds(1);
        isSwiping = false;
    }

    private IEnumerator BossShockWave()
    {
        isShockwave = true;
        bossAnimator.SetTrigger("Shockwave");
        shockwaveVisual.gameObject.SetActive(true);
        float startRadius = 1;
        float endRadius = 2.5f;
        float duration = bossShockCooldown;
        float elapsedTime = 0;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            shockwaveHB.GetComponent<SphereCollider>().radius = Mathf.Lerp(startRadius, endRadius, t);
            Vector3 visualLerp = new Vector3(Mathf.Lerp(startRadius * 2, endRadius * 2, t), 0.04f, Mathf.Lerp(startRadius * 2, endRadius * 2, t));
            shockwaveVisual.transform.localScale = visualLerp;
            yield return null;
        }
        GameObject explosion = Instantiate(shockwaveParticle, bossTransform.position, Quaternion.identity);
        Destroy(explosion, 1.5f);
        DamageColliders(bossShockDamage, shockwaveHB);
        shockwaveVisual.gameObject.SetActive(false);
        yield return new WaitForSeconds(2);
        isShockwave = false;
    }
}
