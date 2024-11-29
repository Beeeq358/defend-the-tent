using System.Collections;
using UnityEditor.XR;
using UnityEngine;

public class Player : MonoBehaviour, IObjectParent
{
    [Header("Config")]
    protected float spawnRange = 3;
    protected float bossSpawnRange = 25;
    protected float throwStrength = 7;
    protected float throwHeight = 2;

    [SerializeField]
    protected IChildObject selectedChildObject;
    public bool isBoss;
    protected Vector3 lastInteractDir;
    protected IChildObject childObject;
    protected float recentGrabStrength;
    protected float frameCounter;
    protected Transform targetTransform;
    [SerializeField]
    protected LayerMask objectLayerMask;

    protected Transform playerObjectHoldPoint;
    protected Transform bossObjectHoldPoint;
    protected Transform normalTransform, bossTransform;
    [SerializeField]
    protected Rigidbody normalRB, bossRB;

    [SerializeField]
    protected GameObject normalPlayer, bossPlayer, playerHoldPoint, bossHoldPoint;

    protected PlayerMovement playerMovement;
    [SerializeField]
    protected GameManager gameManager;
    public Animator bossAnimator;

    [SerializeField] private GameObject followLightPrefab;
    private GameObject followLightChild;
    private FollowingLight fl;

    protected bool isFrame = true;
    protected PlayerInput input;

    private void Awake()
    {
        gameManager = FindFirstObjectByType<GameManager>();
        playerMovement = GetComponent<PlayerMovement>();
        input = GetComponent<PlayerInput>();
    }
    private void Start()
    {
        PlayerStart();
        isBoss = false;
        if (gameManager != null)
        {
            if (gameManager.gamePhase != GameManager.GamePhase.PreGame)
            {
                gameObject.SetActive(false);
            }
        }
        GameObject followLight = Instantiate(followLightPrefab, new Vector3(0, 27, 0), Quaternion.Euler(90, 0, 0));
        followLightChild = followLight;
        fl = followLightChild.GetComponent<FollowingLight>();
        fl.LogOn(normalPlayer);
    }

    public virtual void BecomeBoss()
    {
        isBoss = true;
        BaseClassExclusive();
    }

    protected virtual void BaseClassExclusive()
    {
        fl.BossLogOn(bossPlayer);
    }

    private void Update()
    {
        PlayerUpdate();
    }

    protected virtual void PlayerStart()
    {
        playerObjectHoldPoint = playerHoldPoint.transform;
        bossObjectHoldPoint = bossHoldPoint.transform;
        normalTransform = normalPlayer.transform;
        bossTransform = bossPlayer.transform;
        normalRB = normalPlayer.GetComponent<Rigidbody>();
        bossRB = bossPlayer.GetComponent<Rigidbody>();
        targetTransform = normalTransform;
        targetTransform.gameObject.SetActive(true);
        targetTransform.position = GetSpawnPosition(false);
    }

    private void HandleBaseObjectDestroyed(IChildObject destroyedObject)
    {
        Debug.Log($"Held object {destroyedObject.GetGameObject().name} was destroyed.");
        ClearObject();
    }

    protected virtual void PlayerUpdate()
    {   
        CalculateRecentGrabStrength();
    }

    protected virtual void CalculateRecentGrabStrength()
    {
        if (input.inputGrabStrength > 0 && input.inputGrabStrength >= recentGrabStrength)
        {
            recentGrabStrength = input.inputGrabStrength;
            frameCounter = 0;
        }
        frameCounter++;
        if (frameCounter > 30)
        {
            recentGrabStrength = 0;
        }
    }

    //returns a random spawn position in the middle of the arena
    protected virtual Vector3 GetSpawnPosition(bool isBoss)
    {
        if (isBoss)
        {
            Vector3 spawnPos;
            /*
            spawnPos = new Vector3(Random.Range(-bossSpawnRange, bossSpawnRange), 2, Random.Range(-bossSpawnRange, bossSpawnRange));
            while (Vector3.Distance(spawnPos, Vector3.zero) < 19)
            {
                spawnPos = new Vector3(Random.Range(-bossSpawnRange, bossSpawnRange), 2, Random.Range(-bossSpawnRange, bossSpawnRange));
            }
            */

            spawnPos = new Vector3(-30, 1, 0);
            return spawnPos;
        }
        else
            return new Vector3(Random.Range(-spawnRange, spawnRange), 1, Random.Range(-spawnRange, spawnRange));
    }

    public Vector2 GetMovementVectorNormalized()
    {
        return input.inputMovement.normalized;
    }

    protected void SetSelectedObject(IChildObject childObject)
    {
        if (selectedChildObject is BaseObject previouslySelectedBaseObject)
        {
            // Deselect the currently selected object
            previouslySelectedBaseObject.SetSelectedVisual(false);
        }

        // Update the selected object
        this.selectedChildObject = childObject;

        if (childObject is BaseObject newSelectedBaseObject)
        {
            if (newSelectedBaseObject.rb.isKinematic == true)
            {
                // do nothing
            }
            // Select the new object
            newSelectedBaseObject.SetSelectedVisual(true);
        }
    }


    public Transform GetObjectFollowTransform()
    {
        return playerObjectHoldPoint;
    }

    public void SetObject(IChildObject newChildObject)
    {
        // Unsubscribe from the previous object's OnDestroyed event, if any
        if (childObject != null)
        {
            childObject.OnDestroyed -= HandleBaseObjectDestroyed;
        }

        // Set the new object and subscribe to its OnDestroyed event
        childObject = newChildObject;

        if (childObject != null)
        {
            childObject.OnDestroyed += HandleBaseObjectDestroyed;
        }
    }

    public IChildObject GetObject()
    {
        return childObject;
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }

    public void ClearObject()
    {
        childObject = null;
    }
    public bool HasObject()
    {
        return childObject != null;
    }
}
