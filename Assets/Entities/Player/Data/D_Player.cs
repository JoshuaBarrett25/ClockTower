using UnityEngine;

[CreateAssetMenu(fileName = "newPlayerData", menuName = "Scriptable Objects/Entity Data/Player Data")]
public class D_Player : D_Entity
{
    [Header("Interact Stats")]
    public float interactDistance = 3.5f;
    
    
    
    [Header("Movement")]
    public float maxSpeed = 13f;
    public float groundAcceleration = 5f;
    public float groundDeceleration = 20f;
    public float airAcceleration = 5f;
    public float airDeceleration = 5f;

    [Header("Jump")]
    public float jumpHeight = 6.5f;
    public float jumpHeightFactor = 1.06f;
    public float timeTillApex = 0.35f;
    public float gravityOnRelease = 2f;
    public float maxFallSpeed = 28f;
    public int jumpsAllowed = 2;

    [Header("Jump Cut")]
    [Range(0.02f, 0.3f)] public float timeForUpwardsCancel = 0.027f;
    public float apexThreshold = 0.97f;
    public float apexHangTime = 0.075f;

    [Header("Jump Buffer")]
    public float jumpBufferTime = 0.15f;
    
    [Header("Coyote Time")]
    public float coyoteTime = 0.15f;
    
    [Header("Dash")]
    
    [Header("Collisions")]
    public LayerMask groundLayer;
    public float groundCheckDistance = 0.02f;
    public float headCheckDistance = 0.02f;
    public float headWidth = 0.75f;

    public void OnEnable()
    {
        CalculateGravity();
    }
    
    public float gravity { get; private set; }
    public float initialJumpVelocity { get; private set; }
    public float adjustedJumpHeight { get; private set; }
    
    public void CalculateGravity()
    {
        adjustedJumpHeight = jumpHeight * jumpHeightFactor; 
        gravity = -(2f * jumpHeight) / Mathf.Pow(timeTillApex, 2f);
        initialJumpVelocity = Mathf.Abs(gravity) * timeTillApex;
    }

}
