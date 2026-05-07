using UnityEngine;
using UnityEngine.UI; // For UI Text (if needed)
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 4f;
    public float sprintSpeed = 8f;
    public float speedChangeRate = 10f; // Smooth fade speed
    private float currentMoveSpeed;

    public float groundDrag;
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    private bool readyToJump;

    public AudioSource footstepSource;
    public AudioClip footstepClip;
    public float basePitch = 1f;
    public float sprintPitchMultiplier = 2f;

    [Header("Stamina")]
    public float maxStamina = 100f;
    public float currentStamina;
    public float staminaDrainRate = 15f;
    public float staminaRegenRate = 10f;
    private bool isSprinting;
    public TextMeshProUGUI staminaText;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    private bool grounded;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public Transform orientation;

    private Rigidbody rb;
    private float horizontalInput;
    private float verticalInput;
    private Vector3 moveDirection;

private void Start()
{
    rb = GetComponent<Rigidbody>();
    rb.freezeRotation = true;
    readyToJump = true;
    currentMoveSpeed = walkSpeed;
    currentStamina = maxStamina;

    // Auto-find orientation if missing
    if (orientation == null)
    {
        orientation = new GameObject("Orientation").transform;
        orientation.SetParent(transform);
        orientation.localPosition = Vector3.zero;
        orientation.localRotation = Quaternion.identity;
    }

    if (footstepSource != null)
    {
        footstepSource.clip = footstepClip;
        footstepSource.loop = true;
    }
}


    private void Update()
    {

        MyInput();
        SpeedControl();
        GroundCheck();
        HandleSprint();
        HandleFootsteps();
        if (staminaText != null)
            staminaText.text = "STAMINA: " + Mathf.RoundToInt(currentStamina).ToString();
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKey(jumpKey) && readyToJump && grounded && currentStamina > 10)
        {
            Debug.Log($"Can Jump?");
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }


    }

private void HandleSprint()
{
    grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

    bool tryingToSprint = Input.GetKey(sprintKey) && grounded && (horizontalInput != 0 || verticalInput != 0);
    isSprinting = tryingToSprint && currentStamina > 0f;

    float targetSpeed = walkSpeed;

    if (isSprinting)
    {
        targetSpeed = sprintSpeed;
    }
    else if (tryingToSprint && currentStamina <= 0f)
    {
        targetSpeed = walkSpeed * 0.3f; // Slow player to 50% speed when exhausted
    }
    else
    {
        targetSpeed = walkSpeed;
    }

    currentMoveSpeed = Mathf.Lerp(currentMoveSpeed, targetSpeed, speedChangeRate * Time.deltaTime);

    if (isSprinting)
    {
        currentStamina -= staminaDrainRate * Time.deltaTime;
        currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);
    }
    else
    {
        currentStamina += staminaRegenRate * Time.deltaTime;
        currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);
    }

    if (staminaText != null)
    {
            staminaText.text = Mathf.RoundToInt(currentStamina).ToString();

        if (currentStamina <= 0f)
        {
            // Flash red when exhausted
            float t = Mathf.PingPong(Time.time * 3f, 1f); // Speed of flashing (5x faster)
            staminaText.color = Color.Lerp(Color.red, Color.white, t);
        }
        else
        {
            // Normal color when not exhausted
            staminaText.color = Color.white;
        }
    }

}


private void MovePlayer()
{
    Vector3 inputDirection;

    if (orientation != null)
    {
        inputDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
    }
    else
    {
        inputDirection = transform.forward * verticalInput + transform.right * horizontalInput;
    }

    inputDirection.Normalize();

    if (inputDirection.magnitude < 0.1f)
    {
        // No input? Stop XZ movement
        Vector3 stopVel = new Vector3(0f, rb.linearVelocity.y, 0f);
        rb.linearVelocity = stopVel;
        return;
    }

    // Apply force-based movement
    if (grounded)
    {
        rb.AddForce(inputDirection * currentMoveSpeed * 10f, ForceMode.Force);
    }
    else
    {
        rb.AddForce(inputDirection * currentMoveSpeed * 10f * airMultiplier, ForceMode.Force);
    }
}




private void SpeedControl()
{
Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

if (flatVel.magnitude > currentMoveSpeed)
{
    Vector3 limitedVel = flatVel.normalized * currentMoveSpeed;
    rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
}

}


private void Jump()
{
rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
    rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    currentStamina = currentStamina - 10;
}


    private void ResetJump()
    {
        readyToJump = true;
    }

private void GroundCheck()
{
    grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);
rb.linearDamping = grounded ? groundDrag : 0;
}
    private void HandleFootsteps()
    {
        // Use Rigidbody velocity to check movement on the ground
Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        bool isMoving = flatVel.magnitude > 0.1f && grounded;

        if (isMoving)
        {
            if (!footstepSource.isPlaying)
                footstepSource.Play();

            footstepSource.pitch = isSprinting ? basePitch * sprintPitchMultiplier : basePitch;
        }
        else
        {
            if (footstepSource.isPlaying)
                footstepSource.Stop();
        }
    }
}