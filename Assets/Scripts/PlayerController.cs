using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float gravity = -9.81f;
    public float jumpHeight = 1.5f;
    public Transform cameraTransform;
    public float mouseSensitivity = 100f;

    private CharacterController controller;
    private Vector3 velocity;
    private float xRotation = 0f;
    private bool isGrounded;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor to the center of the screen
        Cursor.visible = false; // Optionally make the cursor invisible
    }

    void Update()
    {
        isGrounded = controller.isGrounded; // Check if the player is grounded

        // Mouse Look - controlling the camera's pitch (up/down) and yaw (left/right)
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Limit the up/down rotation

        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f); // Rotate camera up/down
        transform.Rotate(Vector3.up * mouseX); // Rotate player (character) left/right

        // Movement Input (Horizontal and Vertical axis)
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // Calculate movement direction in world space (relative to the camera's orientation)
        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * moveSpeed * Time.deltaTime); // Move the player

        // Apply Gravity - make sure gravity is applied even when grounded
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Ensures the player stays grounded (a small value to keep them on the ground)
        }

        // Jump Logic (if player presses the jump button and is grounded)
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity); // Apply a force upwards
        }

        // Apply gravity to velocity (this will pull the player down)
        velocity.y += gravity * Time.deltaTime;

        // Apply the updated velocity to the character controller
        controller.Move(velocity * Time.deltaTime);
    }
}
