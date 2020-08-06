using UnityEngine;

public class StrafeMovement : MonoBehaviour
{
    [SerializeField] private float accel = 200f;         // How fast the player accelerates on the ground
    [SerializeField] private float airAccel = 200f;      // How fast the player accelerates in the air
    [SerializeField] private float maxSpeed = 6.4f;      // Maximum player speed on the ground
    [SerializeField] private float maxAirSpeed = 0.6f;   // "Maximum" player speed in the air
    [SerializeField]  private float friction = 8f;        // How fast the player decelerates on the ground
    [SerializeField] private float jumpForce = 5f;       // How high the player jumps
    [SerializeField] private LayerMask groundLayers;

    [SerializeField] private GameObject camObj;


    //SLIDING
    [SerializeField] private Vector3 crouchScale = new Vector3(1, 0.5f, 1);
    [SerializeField] private float slideForce = 400;
    [SerializeField] private float slideFriction = 3f;

    public bool IsMoving {get {return input.x != 0 || input.y != 0;}}
	private bool isCrouching = false;
    private Vector3 playerScale;

	public bool onGround = false;

    private Rigidbody rb;
    private float lastJumpPress = -1f;
    private float jumpPressDuration = 0.1f;
    private Vector2 input = default;

    
    private void Start() {
        input = new Vector2();
        rb = GetComponent<Rigidbody>();
        playerScale = transform.localScale;
    }
    
	private void Update()
    {
        //print(new Vector3(GetComponent<Rigidbody>().velocity.x, 0f, GetComponent<Rigidbody>().velocity.z).magnitude);
        if (Input.GetButton("Jump"))
		{
			lastJumpPress = Time.time;
		}
        if (Input.GetKeyDown(KeyCode.LeftControl))
            StartCrouch();
        if (Input.GetKeyUp(KeyCode.LeftControl))
            StopCrouch();

        if (isCrouching)
            input = Vector2.zero;
        else
		    input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

	}

	private void FixedUpdate()
	{

        // Get player velocity
        Vector3 playerVelocity = rb.velocity;
        // Slow down if on ground
        playerVelocity = CalculateFriction(playerVelocity);
        // Add player input
        playerVelocity += CalculateMovement(input, playerVelocity);
        // Assign new velocity to player object
		rb.velocity = playerVelocity;
	}



    private void StartCrouch() {
        isCrouching = true;
        Managers.AudioManager.Play("Slide");
        transform.localScale = crouchScale;
        transform.position = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z);
        if (rb.velocity.magnitude > 0.5f) {
            if (onGround) {
                rb.AddForce(camObj.transform.forward * slideForce);
            }
        }
    }

    private void StopCrouch() {
        isCrouching = false;
        Managers.AudioManager.Stop("Slide");
        transform.localScale = playerScale;
        transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
    }

    /// <summary>
    /// Slows down the player if on ground
    /// </summary>
    /// <param name="currentVelocity">Velocity of the player</param>
    /// <returns>Modified velocity of the player</returns>
	private Vector3 CalculateFriction(Vector3 currentVelocity)
	{
        bool prevGround = onGround;
        onGround = CheckGround();
        if(prevGround != onGround) Managers.AudioManager.PlayOneShot("Footsteps");
        
		float speed = currentVelocity.magnitude;

        if(speed <= 0.5f)
        {
            Debug.Log("STOP SLIDE SOUND");
            Managers.AudioManager.Stop("Slide");
        }

        // If you are not on the ground, jumping, or not moving DONT APPLY FRICTION
        if (!onGround || Input.GetButton("Jump") || speed == 0f)
            return currentVelocity;

        float drop = speed * (isCrouching ? slideFriction : friction) * Time.deltaTime;
        
        return currentVelocity * (Mathf.Max(speed - drop, 0f) / speed);
    }
    
    /// <summary>
    /// Moves the player according to the input. (THIS IS WHERE THE STRAFING MECHANIC HAPPENS)
    /// </summary>
    /// <param name="input">Horizontal and vertical axis of the user input</param>
    /// <param name="velocity">Current velocity of the player</param>
    /// <returns>Additional velocity of the player</returns>
	private Vector3 CalculateMovement(Vector2 input, Vector3 velocity)
	{
        onGround = CheckGround();

        //Different acceleration values for ground and air
        float curAccel = accel;
        if (!onGround)
            curAccel = airAccel;

        //Ground speed
        float curMaxSpeed = maxSpeed;

        //Air speed
        if (!onGround)
            curMaxSpeed = maxAirSpeed;
        
        //Get rotation input and make it a vector
        Vector3 camRotation = new Vector3(0f, camObj.transform.rotation.eulerAngles.y, 0f);
        Vector3 inputVelocity = Quaternion.Euler(camRotation) *
                                new Vector3(input.x * curAccel, 0f, input.y * curAccel);

        //Ignore vertical component of rotated input
        Vector3 alignedInputVelocity = new Vector3(inputVelocity.x, 0f, inputVelocity.z) * Time.deltaTime;

        //Get current velocity
        Vector3 currentVelocity = new Vector3(velocity.x, 0f, velocity.z);

        //How close the current speed to max velocity is (1 = not moving, 0 = at/over max speed)
        float max = Mathf.Max(0f, 1 - (currentVelocity.magnitude / curMaxSpeed));

        //How perpendicular the input to the current velocity is (0 = 90°)
        float velocityDot = Vector3.Dot(currentVelocity, alignedInputVelocity);

        //Scale the input to the max speed
        Vector3 modifiedVelocity = alignedInputVelocity * max;

        //The more perpendicular the input is, the more the input velocity will be applied
        Vector3 correctVelocity = Vector3.Lerp(alignedInputVelocity, modifiedVelocity, velocityDot);

        //Apply jump
        correctVelocity += GetJumpVelocity(velocity.y);

        //Return
        return correctVelocity;
    }

    /// <summary>
    /// Calculates the velocity with which the player is accelerated up when jumping
    /// </summary>
    /// <param name="yVelocity">Current "up" velocity of the player (velocity.y)</param>
    /// <returns>Additional jump velocity for the player</returns>
	private Vector3 GetJumpVelocity(float yVelocity)
	{
		Vector3 jumpVelocity = Vector3.zero;

		if(!isCrouching && Time.time < lastJumpPress + jumpPressDuration && yVelocity < jumpForce && CheckGround())
		{
			lastJumpPress = -1f;
			jumpVelocity = new Vector3(0f, jumpForce - yVelocity, 0f);
		}

		return jumpVelocity;
	}
	
    /// <summary>
    /// Checks if the player is touching the ground. This is a quick hack to make it work, don't actually do it like this.
    /// </summary>
    /// <returns>True if the player touches the ground, false if not</returns>
	private bool CheckGround()
	{
        Ray ray = new Ray(transform.position, Vector3.down);
        bool result = Physics.Raycast(ray, GetComponent<Collider>().bounds.extents.y + 0.1f, groundLayers);
        return result;
	}
}