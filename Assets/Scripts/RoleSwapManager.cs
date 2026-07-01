using UnityEngine;

public class RoleSwapManager : MonoBehaviour
{
    [Header("Assign in Inspector")]
    public GameObject playerObject;   
    public GameObject doorObject;     

    void Start()
    {
        SetupRoleSwap();
    }

    void SetupRoleSwap()
    {
        if (playerObject == null || doorObject == null)
        {
            Debug.LogError("[RoleSwapManager] Please assign playerObject and doorObject in the Inspector!");
            return;
        }

       
        PlayerController playerCtrl = playerObject.GetComponent<PlayerController>();
        if (playerCtrl != null)
        {
            playerCtrl.SetControlEnabled(false); 
        }

        
        Rigidbody2D playerRb = playerObject.GetComponent<Rigidbody2D>();
        if (playerRb != null)
        {
            playerRb.bodyType = RigidbodyType2D.Static;
        }

     
        PlayerController doorCtrl = doorObject.GetComponent<PlayerController>();
        if (doorCtrl != null)
        {
            doorCtrl.SetControlEnabled(true);
        }
        else
        {
            Debug.LogWarning("[RoleSwapManager] No PlayerController on doorObject. Add one and set isControlledByPlayer = false by default.");
        }

        
        Rigidbody2D doorRb = doorObject.GetComponent<Rigidbody2D>();
        if (doorRb == null)
        {
            Debug.LogWarning("[RoleSwapManager] doorObject has no Rigidbody2D. The door won't be able to move or jump.");
        }
    }
}