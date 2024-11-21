using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInteract : Player
{
    public UnityEvent OnPlayerAttack;

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
            Debug.Log("Attaceked");
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
                        if (selectedChildObject is BuildableObject buildableObject)
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
            Debug.Log("Interacted!");
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
            Rigidbody tempRB = childObject.GetGameObject().GetComponent<Rigidbody>();
            Debug.Log("Dropping Object");
            childObject.ClearObjectParent(this);
            tempRB.isKinematic = false;
            tempRB.AddForce(recentGrabStrength * throwStrength * (targetTransform.forward + new Vector3(0, throwHeight, 0)), ForceMode.Impulse);
        }
    }

    public void InteractGrab(Player player, IChildObject childObject)
    {
        childObject.SetObjectParent(player);
    }

    public void InteractBuild(Player player, BuildableObject buildableObject)
    {
        float buildTime = 1f;
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
            // Do boss logic
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
        Vector3 boxHalfExtents = new Vector3(1f, 1f, 1f); // Adjust for desired box size (width, height, depth)

        // Adjust the origin to be at the player's midsection, slightly below their center
        Vector3 origin = targetTransform.position;
        Vector3 direction = lastInteractDir.normalized;

        // Visualize the BoxCast
        Debug.DrawRay(origin, direction * interactDistance, Color.red, 0.1f);
        Debug.DrawLine(origin - targetTransform.right * boxHalfExtents.x,
                 origin + targetTransform.right * boxHalfExtents.x,
                 Color.blue, 0.1f);

        // Perform the BoxCast
        if (Physics.BoxCast(origin, boxHalfExtents, direction, out RaycastHit boxCastHit, Quaternion.identity, interactDistance, objectLayerMask))
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

}
