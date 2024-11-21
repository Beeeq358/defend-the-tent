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
        HandleInteractions();
        base.PlayerUpdate();
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

    public void InteractAttack()
    {
        OnPlayerAttack.Invoke();

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
        Debug.DrawRay(targetTransform.position, lastInteractDir * interactDistance, Color.red, 0.1f);
        if (Physics.Raycast(targetTransform.position, lastInteractDir, out RaycastHit raycastHit, interactDistance, objectLayerMask))
        {
            if (raycastHit.transform.TryGetComponent(out IChildObject childObject))
            {
                Debug.Log($"Raycast hit: {raycastHit.transform.name}, but no IChildObject found!");
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
