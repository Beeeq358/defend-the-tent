using UnityEngine;

public class PlayerInteract : Player
{
    protected override void Update()
    {
        HandleInteractions();
        base.Update();
        if (inputBuilded)
        {
            if (inputBuilded)
            {
                if (selectedBaseObject != null && selectedBaseObject.objectSO.objectType == ObjectType.Buildable && !isBoss)
                {
                    if (selectedBaseObject is BuildableObject buildableObject)
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
        if (inputGrabStrength > 0)
        {
            Debug.Log("Interacted!");
            if (baseObject == null && !isBoss)
            {
                if (selectedBaseObject != null)
                {
                    if (selectedBaseObject is BuildableObject buildableObject)
                    {
                        if (!buildableObject._isInteractive)
                        {
                            Debug.LogWarning("This object is not interactive!");
                            return;
                        }
                    }
                    InteractGrab(this, selectedBaseObject);
                }
                else
                {
                    Debug.LogWarning("No Object selected!");
                }
            }

        }
        if (inputGrabStrength == 0 && baseObject != null && !isBoss)
        {
            Rigidbody tempRB = baseObject.rb;
            Debug.Log("Dropping Object");
            baseObject.ClearObjectParent(this);
            tempRB.isKinematic = false;
            tempRB.AddForce(recentGrabStrength * throwStrength * (targetTransform.forward + new Vector3(0, throwHeight, 0)), ForceMode.Impulse);
        }
    }

    public void InteractGrab(Player player, BaseObject baseObject)
    {
        baseObject.SetObjectParent(player);
    }

    public void InteractBuild(Player player, BuildableObject buildableObject)
    {
        StartCoroutine(playerMovement.StunTime(1f));
        buildableObject.SetKinematic(true);
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
        if (Physics.Raycast(targetTransform.position, lastInteractDir, out RaycastHit raycastHit, interactDistance, objectLayerMask))
        {
            if (raycastHit.transform.TryGetComponent(out BaseObject baseObject))
            {
                if (baseObject != selectedBaseObject)
                {
                    SetSelectedObject(baseObject);
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
