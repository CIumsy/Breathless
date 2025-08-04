using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Scaling Settings")]
    public float normalSize = 1.0f;
    public float smallSize  = 0.6f;
    public float scaleSpeed = 3.0f;

    [Header("Input Keys")]
    public KeyCode shrinkKey = KeyCode.S;
    public KeyCode normalKey = KeyCode.N;
    public KeyCode jumpKey   = KeyCode.Space;
    public KeyCode crouchKey = KeyCode.LeftControl;

    [Header("Animator")]
    public Animator animator;
    
    private Vector3 targetScale;
    private Vector3 lockPos;

    void Start()
    {
        if (animator == null) animator = GetComponent<Animator>();

        targetScale = Vector3.one * normalSize;
        transform.localScale = targetScale;
        lockPos = transform.position;
    }

    void Update()
    {
        ReadInput();
        SmoothScale();
        LockXZPosition();
    }

    void ReadInput()
    {
        if (Input.GetKeyDown(shrinkKey))  Shrink();
        if (Input.GetKeyDown(normalKey))  Grow();

        if (Input.GetKeyDown(jumpKey))    animator.SetTrigger("JumpTrigger");

        if (Input.GetKeyDown(crouchKey))  animator.SetTrigger("CrouchDownTrigger");
        if (Input.GetKeyUp  (crouchKey))  animator.SetTrigger("CrouchUpTrigger");
    }

    void Shrink()
    {
        targetScale = Vector3.one * smallSize;
        Debug.Log("Player shrinking to small size");
    }

    void Grow()
    {
        targetScale = Vector3.one * normalSize;
        Debug.Log("Player returning to normal size");
    }

    void SmoothScale()
    {
        transform.localScale = Vector3.Lerp(
            transform.localScale, 
            targetScale, 
            scaleSpeed * Time.deltaTime);
    }

    void LockXZPosition()
    {
        Vector3 p = transform.position;
        p.x = lockPos.x;
        p.z = lockPos.z;
        transform.position = p;
    }

    // Public methods for BLE integration (same names as before)
    public void ShrinkPlayer()
    {
        Shrink();
    }

    public void RestorePlayer()
    {
        Grow();
    }

    public void SetPlayerSize(bool makeSmall)
    {
        if (makeSmall) Shrink();
        else           Grow();
    }
}
