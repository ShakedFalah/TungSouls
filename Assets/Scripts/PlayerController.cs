using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerController : MonoBehaviour
{
    [Header("Lanes Setup")]
    [SerializeField] private float laneSwitchSpeed = 10f; // moving left or right speed
    [SerializeField] private float laneSpacing = 1.5f;
    [SerializeField] private float middleLane = 0f;
    // -1.5 is left, 0 is center, 1.5 is right
    
    private float currentLane;
    
    [Header("Power-Up statuses")]
    public float invincibleDuration = 0f;
    public float multiplierDuration = 0f;
    public float magnetDuration = 0f;
    public float scoreMultiplier = 1f;
    public MagnetSettings magnetSettings;
    public InvincibilitySettings invincibilitySettings;
    public MultiplierSettings multiplierSettings;
    
    [Header("Jump Setup")]    
    [SerializeField] private float jumpPower = 4f;
    [SerializeField] private float gravity = 2f;
    
    public float[] lanesPositions => new float[] {-laneSpacing, middleLane, laneSpacing}; // using Lambda, I can skip making a function that gets the lanesPositions, making it in one line
    
    private Rigidbody rb;
    private bool isGrounded;
    
    void Start()
    {
        currentLane = transform.position.x;
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // stopping the player from spinning 
    }
    
    void Update()
    {
        float targerXPosition = currentLane; // where the player should be (same lane)
        
        Vector3 targetPosition = new Vector3(targerXPosition, transform.position.y, transform.position.z); // keeps the same y and z
        
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * laneSwitchSpeed); // makes the player slide to from thier current position to the target position 
        
        if(!isGrounded && rb.linearVelocity.y  < 0)
        {
            rb.AddForce(Vector3.down * gravity * Time.deltaTime, ForceMode.VelocityChange);
        }

        LowerTimer(ref magnetDuration);
        LowerTimer(ref invincibleDuration);

        if (multiplierDuration > 0)
        {
            multiplierDuration -= Time.deltaTime;
        } else
        {
            scoreMultiplier = 1f;
        }
    }

    private void LowerTimer(ref float timer)
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
    }

    public void MoveLeft()
    {
        if (currentLane > -laneSpacing)
        {
            currentLane = currentLane - laneSpacing;
        }
    }

    public void MoveRight()
    {
        if (currentLane < laneSpacing)
        {
            currentLane = currentLane + laneSpacing;
        }
    }

    public void Jump()
    {
        if (isGrounded)
        {
            // resets velocity Y to 0 so all jumps are equal 
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
            rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    private void OnCollisionEnter(Collision collision) // reset isGrounded if the player touches the ground
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
    
    // Invincibility 
    public void StartInvinciblity(float duration) // start invincibility coroutine
    {
        invincibleDuration = duration;
    }
    
    // Multiplayer
    public void StartMultiplier(float duration)
    {
        multiplierDuration = duration;
        scoreMultiplier *= multiplierSettings.multiplierValue;
    }
    
    // Magnet
    public void StartMagnet(float duration)
    {
        magnetDuration = duration;
    }    
}
