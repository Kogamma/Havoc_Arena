using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using XInputDotNetPure;

public class PlayerController : MonoBehaviour 
{

	Rigidbody rb;

	public float acceleration;	// The acceleration of the forward/backwards movement
    public float retardation;
	public float rotationRate;	// How fast the vehicle will turn
    float orgRotationRate; // The original rate of rotation

    float maxVelocityX = 30f;
    float maxVelocityY = 15f;
    float maxVelocityZ = 30f;

	public float turnRotationAngle;		// Used for rotating the vehicle to make a fake turning effect
	public float turnRotationSpeed;	// How many seconds the fake turning effect will take place under

    public int playerNr = 1;

	float rotationVelocity;	// Used for reference in the fake turn effect

    public GameObject bulletObj;
    public float ShootDelay = 2f;
    float timer = 0f;

    bool canHonk = true;
    bool canBarrelRoll;

    GamePadState state;
    GamePadState prevState;
    public PlayerIndex controllerIndex;

	void Start () 
    {
		rb = GetComponent<Rigidbody> ();

        orgRotationRate = rotationRate;

        if (playerNr == 1)
            controllerIndex = PlayerIndex.One;
        else if (playerNr == 2)
            controllerIndex = PlayerIndex.Two;
        else if (playerNr == 3)
            controllerIndex = PlayerIndex.Three;
        else if (playerNr == 4)
            controllerIndex = PlayerIndex.Four;
	}

    void Update()
    {
        if (state.Buttons.A == ButtonState.Pressed && timer >= ShootDelay)
            Shoot();
        else if (timer <= ShootDelay)
            timer += Time.deltaTime;

        if (state.Buttons.Y == ButtonState.Pressed && canHonk)
        {
            GameObject.Find("GameManager").GetComponent<SoundManager>().currentAudioClip = GameObject.Find("GameManager").GetComponent<SoundManager>().hornSound;
            GameObject.Find("GameManager").GetComponent<SoundManager>().PlayCurrentAudioClip();

            canHonk = false;
        }
        else if(state.Buttons.Y == ButtonState.Released)
            canHonk = true;
            
    }


	void FixedUpdate()
	{
        state = GamePad.GetState((PlayerIndex)playerNr - 1);

        if(gameObject.GetComponent<TakeDamage>().isWinner && state.Buttons.Start == ButtonState.Pressed)
            Application.LoadLevel("Match Results");


        // Calculates the forward movement using the acceleration
        Vector3 forwardForce = -transform.forward * acceleration * state.Triggers.Right;

        // Applies time and gravitation to the calculation
        forwardForce = forwardForce * Time.deltaTime * rb.mass;

        Vector3 backwardForce = transform.forward * retardation * state.Triggers.Left;
        backwardForce = backwardForce * Time.deltaTime * rb.mass;

        if(gameObject.GetComponent<Hover>().aboveHoverHeight)
        {
            forwardForce /= 2;
            backwardForce /= 2;
            rotationRate = orgRotationRate / 5;
            maxVelocityY = 100f;
        }
        else
        {
            rotationRate = orgRotationRate;
            maxVelocityY = 15f;
        }

        if(gameObject.GetComponent<Hover>().stopMoving)
        {
            forwardForce = Vector3.zero;
            backwardForce = Vector3.zero;
        }

        // Adds the force to the vehicle
        rb.AddForce(forwardForce);
        rb.AddForce(backwardForce);
        if (name == "Player 1" && forwardForce != Vector3.zero)
            print("Forward Force: "  + forwardForce.normalized);
        if (name == "Player 1" && backwardForce != Vector3.zero)
            print("Backwards Force: " + backwardForce.normalized);

        rb.velocity = new Vector3(Mathf.Clamp(rb.velocity.x, -maxVelocityX, maxVelocityX),
            Mathf.Clamp(rb.velocity.y, -maxVelocityY, maxVelocityY),
            Mathf.Clamp(rb.velocity.z, -maxVelocityZ, maxVelocityZ));

        // Calculates the rotation the same way as the forward movement
        Vector3 turnTorque = Vector3.up * rotationRate * state.ThumbSticks.Left.X;

        // Applies time and gravitation to the calculation
        turnTorque = turnTorque * Time.deltaTime * rb.mass;
        // Adds torque to the vehicle
        rb.AddTorque(turnTorque);

        Vector3 newRotation = transform.eulerAngles;	// Gets the rotation from the vehicle

        // Changes the z rotation with x amount of the degrees that the turnRotationAngle variable is and does it during the time of the turnRotationSeekSpeed variable
        newRotation.z = Mathf.SmoothDampAngle(newRotation.z, state.ThumbSticks.Left.X * turnRotationAngle, ref rotationVelocity, turnRotationSpeed);

        // Applies the new rotation
        transform.eulerAngles = newRotation;
        
        prevState = state;
	}

    void Shoot()
    {
        if (timer >= ShootDelay)
        {
            timer = 0f;

            for (int i = 0; i < 2; i++)
            {
                GameObject bullet = Instantiate(bulletObj, transform.FindChild("BulletSpawner" + i).position, transform.rotation) as GameObject;
                bullet.GetComponent<BulletScript>().dir = transform.forward;
                bullet.GetComponent<BulletScript>().owner = gameObject;

                GameObject.Find("GameManager").GetComponent<SoundManager>().currentAudioClip = GameObject.Find("GameManager").GetComponent<SoundManager>().lazerSound;
                GameObject.Find("GameManager").GetComponent<SoundManager>().PlayCurrentAudioClip();
            }    
        }
    }
}

