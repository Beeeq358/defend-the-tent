using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInteract : Player
{
    [Header("Config")]
    public float bossSlamCooldown;
    public int bossSlamDamage;

    private bool isSlamming;

    [SerializeField] private Collider slamHB, halfcircleHB;

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
        Debug.Log(isBoss);
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
            if (!isSlamming)
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

        // Update last interact direction only if the player is moving
        if (moveDir != Vector3.zero)
        {
            lastInteractDir = moveDir;
        }

        float interactDistance = 2f;
        // Adjust for desired box size (width, height, depth)
        Vector3 boxHalfExtents = new Vector3(1f, 1f, 1f); 

        // Use player's current forward direction or last interact direction
        Vector3 direction = moveDir != Vector3.zero ? moveDir.normalized : targetTransform.forward.normalized;

        Quaternion boxOrientation = Quaternion.LookRotation(direction, Vector3.up);

        // Visualize the BoxCast
        Debug.DrawRay(targetTransform.position, direction * interactDistance, Color.red, 0.1f);

        // Perform the BoxCast
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


    private IEnumerator BossFrontSlam()
    {
        isSlamming = true;
        //start animation
        yield return new WaitForSeconds(bossSlamCooldown);
        //start slam VFX
        List<Collider> playerColliders = slamHB.GetComponent<HitBox>().GetPlayerColliders();
        foreach (Collider collider in playerColliders)
        {
            collider.GetComponent<PlayerHealth>().TakeDamage(bossSlamDamage);
        }
        List<Collider> objectColliders = slamHB.GetComponent<HitBox>().GetObjectColliders();
        foreach (Collider collider in objectColliders)
        {
            collider.GetComponent<BaseObject>().TakeDamage(bossSlamDamage);
        }
        isSlamming = false;
    }
}
