using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Stats")]
    public float force;
    public float airControl;
    public float extraDrag;
    public float extraAngularDrag;
    public float wallGrabDrag;

    [Header("Boost")]
    public float maxStamina;
    public float staminaUseRate;
    public float boostMultiplier;

    public float currentStamina;
    public bool isBoosting;

    private float multiplier = 1f;

    private CharacterData data;
    private CharacterStatModifier stats;

    private void Start()
    {
        data = GetComponent<CharacterData>();
        stats = GetComponent<CharacterStatModifier>();
    }

    private void FixedUpdate()
    {
        if (data.isPlaying)
        {
            if (data.canMove)
            {
                Move(data.input.direction);
            }

            if (data.isWallGrab && data.wallDistance < 0.7f)
            {
                Vector2 velocity = data.playerVel.rb.velocity;
                if (data.input.direction.y >= 0)
                {
                    _ = data.input.direction.x;
                    _ = 0f;
                }

                data.playerVel.rb.velocity = velocity;
            }
            
            ApplyPhysicsModifiers();
        }
    }

    public void Move(Vector2 direction)
    {
        UpdateMultiplier();
        
        if (!data.isStunned)
        {
            direction.y = Mathf.Clamp(direction.y, -1f, 0f) * 2f;
            float num = (1f - stats.slow) * stats.movementSpeed * force * data.playerVel.rb.mass * 0.01f * multiplier;

            if (isBoosting)
            {
                num *= boostMultiplier;
            }
            
            data.playerVel.rb.AddForce(direction * num, ForceMode2D.Force);
        }
    }

    private void ApplyPhysicsModifiers()
    {
        data.playerVel.rb.velocity *= 1f - 0.001f * extraDrag * multiplier;
        data.playerVel.rb.angularVelocity *= 1f - 0.001f * extraAngularDrag * multiplier;
    }

    private void UpdateMultiplier()
    {
        multiplier = (data.isGrounded ? 1f : airControl);
    }
}