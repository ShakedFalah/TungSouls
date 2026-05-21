using UnityEngine;
using UnityEngine.Rendering;

public class PlayerController : MonoBehaviour
{
    [Header("Lanes Setup")]
    [SerializeField] private int laneSpacing = 1;
    [SerializeField] private float laneSwitchSpeed = 10f; // moving left or right speed

    // -1.5 is left, 0 is center, 1.5 is right
    private int CurrentLane = 0;
    
    [Header("Jump Setup")]    
    [SerializeField] private float jumpPower = 4f;
    [SerializeField] private float gravity = 2f;
    
    private Rigidbody rb;
    private bool isGrounded;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // stopping the player from spinning 
    }
    
    void Update()
    {
        float targerXPosition = CurrentLane * laneSpacing; // where the player should be (same lane)
        
        Vector3 targetPosition = new Vector3(targerXPosition, transform.position.y, transform.position.z); // keeps the same y and z
        
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * laneSwitchSpeed); // makes the player slide to from thier current position to the target position 
        
        if(!isGrounded && rb.linearVelocity.y  < 0)
        {
            rb.AddForce(Vector3.down * gravity * Time.deltaTime, ForceMode.VelocityChange);
        }
    }

    public void MoveLeft()
    {
        if (CurrentLane > -laneSpacing)
        {
            CurrentLane--;
        }
    }

    public void MoveRight()
    {
        if (CurrentLane < laneSpacing)
        {
            CurrentLane++;
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
}
