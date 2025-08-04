using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Scaling Settings")]
    public float normalSize = 1.0f;
    public float smallSize = 0.6f;
    public float scaleSpeed = 3.0f;
    
    [Header("Input Controls")]
    public KeyCode shrinkKey = KeyCode.S;
    public KeyCode normalKey = KeyCode.N;
    
    private Vector3 targetScale;
    private bool isSmall = false;
    
    void Start()
    {
        // Initialize at normal size
        targetScale = new Vector3(normalSize, normalSize, normalSize);
        transform.localScale = targetScale;
    }
    
    void Update()
    {
        // Handle keyboard input
        HandleInput();
        
        // Smooth scaling animation
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, scaleSpeed * Time.deltaTime);
    }
    
    void HandleInput()
    {
        if (Input.GetKeyDown(shrinkKey))
        {
            ShrinkPlayer();
        }
        
        if (Input.GetKeyDown(normalKey))
        {
            RestorePlayer();
        }
    }
    
    public void ShrinkPlayer()
    {
        targetScale = new Vector3(smallSize, smallSize, smallSize);
        isSmall = true;
        Debug.Log("Player shrinking to small size");
    }
    
    public void RestorePlayer()
    {
        targetScale = new Vector3(normalSize, normalSize, normalSize);
        isSmall = false;
        Debug.Log("Player returning to normal size");
    }
    
    // Public method for external control (ESP32 BLE later)
    public void SetPlayerSize(bool makeSmall)
    {
        if (makeSmall)
            ShrinkPlayer();
        else
            RestorePlayer();
    }
}
