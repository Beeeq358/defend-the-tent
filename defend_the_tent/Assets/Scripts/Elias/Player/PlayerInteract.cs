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
    [SerializeField] private GameObject explosionParticle;

    public UnityEvent OnPlayerAttack;
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
        if (input.inputAttack2 && isBoss && !isSlamming && !isSwiping && !isShockwave)
        {
            StartCoroutine(BossHalfSwipe());
        }
        if (input.inputAttack3 && isBoss && !isSlamming && !isSwiping && !isShockwave)
        {
            StartCoroutine(BossShockWave());
        }
        if (input.inputBuilded)
        {
            if (input.inputBuilded)
            {
                if (selectedChildObject is BaseObject)
                {
                    BaseObject baseObject = (BaseObject)selectedChildObject;
                    if (selectedChildObject != null && baseObject.objectSO.objectType == ObjectType.Buildable && !isBoss)
                    {
                        if (selectedChildObject is BuildableObject buildableObject  && buildableObject._isInteractive == true)
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
                        if (!buildableObject._isInteractive)
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
        if (!isBoss)
        {
            if (childObject is BaseWeapon)
            {
                OnPlayerAttack.Invoke();
            }
            else
            {
                Debug.LogWarning("Player does not currently hold a weapon");
            }
        }
        else if (isBoss)
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
            Debug.LogError("Player is neither a boss nor a player");
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
                Debug.Log($"BoxCast hit: {boxCastHit.transform.name}, checking for IChildObject.");
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
        List<Collider> playerColliders = attackHitbox.GetComponent<HitBox>().GetPlayerColliders();
        foreach (Collider collider in playerColliders)
        {
            collider.GetComponentInParent<PlayerHealth>().TakeDamage(attackDamage);
        }
        List<Collider> objectColliders = attackHitbox.GetComponent<HitBox>().GetObjectColliders();
        foreach (Collider collider in objectColliders)
        {
            collider.GetComponent<BaseObject>().TakeDamage(attackDamage);
        }
        Collider objective = attackHitbox.GetComponent<HitBox>().GetObjectiveCollider();
        if (objective != null)
        {
            objective.GetComponent<ObjectiveScript>().TakeDamage(attackDamage);
        }
    }

    private IEnumerator BossFrontSlam()
    {
        isSlamming = true;
        //start animation
        yield return new WaitForSeconds(bossSlamCooldown);
        //start slam VFX
        DamageColliders(bossSlamDamage, slamHB);
        isSlamming = false;
    }

    private IEnumerator BossHalfSwipe()
    {
        isSwiping = true;
        //start animation
        yield return new WaitForSeconds(bossSwipeCooldown);
        //start slam VFX
        DamageColliders(bossSwipeDamage, halfcircleHB);
        isSwiping = false;
    }

    private IEnumerator BossShockWave()
    {
        isShockwave = true;
        //start animation
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
        //impact VFX
        GameObject explosion = Instantiate(explosionParticle, bossTransform.position, Quaternion.identity);
        Destroy(explosion, 1.5f);
        DamageColliders(bossShockDamage, shockwaveHB);
        shockwaveVisual.gameObject.SetActive(false);
        isShockwave = false;
    }
}
