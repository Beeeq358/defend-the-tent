using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Player playerScript;
    private void Awake()
    {
        playerScript = GetComponent<Player>();
    }


}
