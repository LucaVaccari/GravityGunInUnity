using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    private Rigidbody rb;

    [SerializeField] private Transform cam;
    [SerializeField] private float speed = 10;

    //[Range(50, 150)]
    [SerializeField] private float sens = 120;
    private Vector2 inputMove;
    private Vector2 inputLook;

    [SerializeField] private float jumpForce = 50;
    [SerializeField] private LayerMask whatIsGround;
    private bool haveToJump = false, grounded = true;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Application.targetFrameRate = 60;
    }

    private void Update()
    {
        CalcInput();

        if (Input.GetButton("Exit"))
            Application.Quit();
    }
    private void FixedUpdate()
    {
        grounded = Physics.Raycast(transform.position + Vector3.up * .1f, Vector3.down, .2f, whatIsGround);

        Move();
        Look();
        Jump();
    }

    private void CalcInput()
    {
        inputMove.x = Input.GetAxis("Horizontal");
        inputMove.y = Input.GetAxis("Vertical");
        inputMove.Normalize();

        inputLook.x = Input.GetAxis("Mouse X");
        inputLook.y += -Input.GetAxis("Mouse Y");

        if (Input.GetButtonDown("Jump"))
            haveToJump = true;
    }

    private void Move()
    {
        Vector3 velocity = transform.forward * inputMove.y + transform.right * inputMove.x;
        if (!grounded)
        {
            rb.MovePosition(transform.position + velocity * Time.deltaTime * speed);
            rb.velocity = new Vector3(rb.velocity.x / 1.1f, rb.velocity.y, rb.velocity.z / 1.1f);
        }
        else
            rb.velocity = velocity * Time.deltaTime * speed * 100;
    }
    private void Look()
    {
        transform.Rotate(0, inputLook.x * sens * Time.deltaTime / 2, 0);
        float yRot = inputLook.y * sens * Time.deltaTime;
        yRot = Mathf.Clamp(yRot, -85, 85);
        cam.transform.eulerAngles = new Vector3(yRot, cam.transform.eulerAngles.y, cam.transform.eulerAngles.z);
    }
    private void Jump()
    {
        if (haveToJump)
        {
            haveToJump = false;
            if (grounded)
            {
                //jump
                rb.AddForce(Vector3.up * jumpForce * 5, ForceMode.VelocityChange);
            }
        }
    }
}
