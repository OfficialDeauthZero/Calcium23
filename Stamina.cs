using System.Collections.Generic;
using UnityEngine;

public class VRPhysicsEngineWithStamina : MonoBehaviour
{
    // Avatar properties
    public float mass = 70f; // Base mass in kilograms
    public float baseSpeed = 5f; // Base movement speed
    public float baseStrength = 10f; // Base strength multiplier
    public float baseJumpHeight = 2f; // Base jump height in meters

    // Health properties
    public float health = 100f;
    public float maxHealth = 100f;

    // Stamina properties
    public float maxStamina = 20f; // 20 seconds of stamina
    private float currentStamina;
    private bool isMoving;
    private float staminaRecoveryRate = 5f; // Stamina recovery per second
    private float fatigueMultiplier = 0.7f; // 30% slower when out of stamina

    // Inventory properties
    public List<InventorySlot> inventory = new List<InventorySlot>();
    public float maxCarryWeight = 50f; // Maximum weight player can carry

    // Calculated properties
    private float speed;
    private float strength;
    private float jumpHeight;

    // Gravity and jump calculations
    private float gravity = -9.81f;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("No Rigidbody attached to the avatar.");
            return;
        }

        InitializeInventory();
        currentStamina = maxStamina; // Start with full stamina
        UpdatePhysicsProperties();
    }

    void InitializeInventory()
    {
        // Initialize 4 main slots and 2 holsters
        for (int i = 0; i < 4; i++) inventory.Add(new InventorySlot("Main Slot " + (i + 1)));
        for (int i = 0; i < 2; i++) inventory.Add(new InventorySlot("Holster " + (i + 1)));
    }

    void UpdatePhysicsProperties()
    {
        float inventoryWeight = CalculateInventoryWeight();
        float totalMass = mass + inventoryWeight;

        // Convert mass into speed, strength, and jump height
        speed = baseSpeed / (totalMass / 70f); // Adjust speed relative to base mass
        strength = baseStrength * (totalMass / 70f); // Strength increases with total mass
        jumpHeight = baseJumpHeight / Mathf.Sqrt(totalMass / 70f); // Jump height decreases with total mass

        // Adjust speed further based on health
        speed *= GetHealthSpeedMultiplier();

        // Adjust speed if stamina is depleted
        if (currentStamina <= 0f)
        {
            speed *= fatigueMultiplier; // Reduce speed when out of stamina
        }

        Debug.Log($"Physics Updated: Speed={speed}, Strength={strength}, JumpHeight={jumpHeight}, InventoryWeight={inventoryWeight}");
    }

    float CalculateInventoryWeight()
    {
        float weight = 0f;
        foreach (var slot in inventory)
        {
            if (slot.Item != null)
            {
                weight += slot.Item.Weight;
            }
        }
        return Mathf.Clamp(weight, 0, maxCarryWeight);
    }

    float GetHealthSpeedMultiplier()
    {
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
        HandleMovement();
        HandleStamina();

        // Simulate adding an item to inventory for testing
        if (Input.GetKeyDown(KeyCode.I))
        {
            AddItemToSlot(new InventoryItem("Test Item", 10f));
        }
    }

    void HandleMovement()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        Vector3 move = transform.right * moveX + transform.forward * moveZ;

        // Determine if the player is moving
        isMoving = move.magnitude > 0.1f;

        // Apply velocity
        rb.velocity = new Vector3(move.x * speed, rb.velocity.y, move.z * speed);

        // Jump Logic
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            Jump();
        }
    }

    void HandleStamina()
    {
        if (isMoving)
        {
            // Decrease stamina when moving
            currentStamina -= Time.deltaTime;
            currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);
        }
        else
        {
            // Recover stamina when not moving
            currentStamina += staminaRecoveryRate * Time.deltaTime;
            currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);
        }

        // Update physics properties if stamina changes
        UpdatePhysicsProperties();

        // Debugging stamina
        Debug.Log($"Stamina: {currentStamina}/{maxStamina}");
    }

    void Jump()
    {
        float jumpForce = Mathf.Sqrt(2 * -gravity * jumpHeight);
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 1.1f);
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

    public void AddItemToSlot(InventoryItem item)
    {
        foreach (var slot in inventory)
        {
            if (slot.Item == null)
            {
                slot.Item = item;
                UpdatePhysicsProperties();
                Debug.Log($"Added {item.Name} to {slot.Name}, Weight={item.Weight}");
                return;
            }
        }
        Debug.Log("No available inventory slots!");
    }
}

[System.Serializable]
public class InventorySlot
{
    public string Name;
    public InventoryItem Item;

    public InventorySlot(string name)
    {
        Name = name;
        Item = null;
    }
}

[System.Serializable]
public class InventoryItem
{
    public string Name;
    public float Weight;

    public InventoryItem(string name, float weight)
    {
        Name = name;
        Weight = weight;
    }
}
