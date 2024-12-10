using UnityEngine;

public class VRPhysicsEngineWithHealth : MonoBehaviour
{
    // Avatar properties
    public float mass = 70f; // Mass in kilograms
    public float baseSpeed = 5f; // Base movement speed
    public float baseStrength = 10f; // Base strength multiplier
    public float baseJumpHeight = 2f; // Base jump height in meters

    // Health properties
    public float health = 100f; // Current health
    public float maxHealth = 100f; // Maximum health

    // Calculated properties
    private float speed;
    private float strength;
    private float jumpHeight;

    // Gravity and jump calculations
    private float gravity = -9.81f; // Gravity in m/s^2
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("No Rigidbody attached to the avatar.");
            return;
        }

        // Initialize physics properties
        UpdatePhysicsProperties();
    }

    void UpdatePhysicsProperties()
    {
        // Convert mass into speed, strength, and jump height
        speed = baseSpeed / (mass / 70f); // Adjust speed relative to a 70kg base
        strength = baseStrength * (mass / 70f); // Strength increases with mass
        jumpHeight = baseJumpHeight / Mathf.Sqrt(mass / 70f); // Jump height decreases with mass

        // Adjust speed based on health
        speed *= GetHealthSpeedMultiplier();

        Debug.Log($"Physics Updated: Speed={speed}, Strength={strength}, JumpHeight={jumpHeight}");
    }

    float GetHealthSpeedMultiplier()
    {
        // Health-based speed adjustment
        if (health >= 90f && health <= 100f)
        {
            return 1f; // Normal speed
        }
        else if (health >= 70f && health < 90f)
        {
            return 0.932451f; // 6.7548% slower
        }
        else if (health >= 45f && health < 70f)
        {
            return 0.8154347f; // 18.45653% slower
        }
        else if (health >= 1f && health < 45f)
        {
            return 0.5f; // Very slow
        }
        else
        {
            return 0f; // No movement if health is 0 or less
        }
    }

    void Update()
    {
        // Example VR Movement
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        rb.velocity = new Vector3(move.x * speed, rb.velocity.y, move.z * speed);

        // Jump Logic
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            Jump();
        }

        // Update physics properties if health changes
        if (Input.GetKeyDown(KeyCode.H)) // Simulate health change for testing
        {
            health -= 10f; // Decrease health by 10 for testing
            health = Mathf.Clamp(health, 0f, maxHealth); // Keep health within bounds
            UpdatePhysicsProperties();
        }
    }

    void Jump()
    {
        float jumpForce = Mathf.Sqrt(2 * -gravity * jumpHeight); // Using kinematics: v^2 = u^2 + 2as
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 1.1f); // Check if near ground
    }

    public void SetMass(float newMass)
    {
        mass = newMass;
        UpdatePhysicsProperties();
    }

    public void SetHealth(float newHealth)
    {
        health = Mathf.Clamp(newHealth, 0f, maxHealth);
        UpdatePhysicsProperties();
    }
}
