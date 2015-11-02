using UnityEngine;
using System.Collections;

/// <summary>
/// Handles the player's movement, rotation, etc.
/// </summary>
public class PlayerHandler : MonoBehaviour 
{
	//TODO: This is currently ported for PC. PORT TO ANDROID LATER

	private CharacterController Player { get; set; }

	/// <summary>
	/// The type of the car.
	/// </summary>
	public CarTypes CarType;
	/// <summary>
	/// To-be-set in the Start() method. LOOK THERE.
	/// </summary>
	private CarHandler PlayerCar;

	/// <summary>
	/// The car's current velocity, limit is car type's maximum speed
	/// </summary>
	private float CurrentVelocity = 0;
	
	const float ROTATE_FACTOR = 1.15f;
	const float BRAKE_FACTOR = 1.02f; //TODO: OBSOLETE, REMOVE LATER?
	const float COLLISION_SPEED_FACTOR = 8f; //TODO: Fix this
	const float REALISM_FACTOR = 4f; // used to divide the velocity of the car on-screen
	const int SPEED_DROP_FACTOR = 10;
	const int BRAKE_SPEED_DROP_FACTOR = 5;

	private bool IsRotating { get; set; }
	private bool IsColliding { get; set; }
	
	/// <summary>
	/// Used to initialize the player
	/// </summary>
	private void Start () 
	{
		//TODO: Add all the car types. 
		// Initializes the car based on the type.
		switch (CarType) 
		{
			case 0:
			default:
				PlayerCar = new TestCar();
			break;
		}
	}
	
	/// <summary>
	/// Handles the player updates
	/// </summary>
	private void Update ()
	{
		this.Player = GetComponent<CharacterController> ();

		this.HandlePlayerRotation ();
		this.HandlePlayerMovement ();
	}

	private void LateUpdate()
	{

	}

	private Vector3 BackLeft
	{
		get { return transform.rotation * new Vector3 (transform.position.x - (transform.lossyScale.x / 2), transform.position.y, transform.position.z - (transform.lossyScale.z / 2)); }
	}

	private Vector3 BackRight
	{
		get { return transform.rotation * new Vector3 (transform.position.x + (transform.lossyScale.x / 2), transform.position.y, transform.position.z - (transform.lossyScale.z / 2)); }
	}

	private Vector3 FrontLeft
	{
		get { return transform.rotation * new Vector3 (transform.position.x - (transform.lossyScale.x / 2), transform.position.y, transform.position.z + (transform.lossyScale.z / 2)); }
	}

	private Vector3 FrontRight
	{
		get { return transform.rotation * new Vector3 (transform.position.x + (transform.lossyScale.x / 2), transform.position.y, transform.position.z + (transform.lossyScale.z / 2)); }
	}

	/// <summary>
	/// Handles the player rotation.
	/// </summary>
	private void HandlePlayerRotation()
	{
		//TODO: Make this rotation more realistic
		// Simple rotation
		if (Input.GetKey (KeyCode.A) || Input.GetKey (KeyCode.D)) 
		{
			IsRotating = true;

			var rot = Input.GetAxis("Horizontal");

			// 1 is full, -1 is full

			rot *= 500;
			rot -= (CurrentVelocity / 2);
			rot /= 500;

			Debug.Log (this.Player.isGrounded.ToString() + this.Player.transform.position.y.ToString());

			if (this.CurrentVelocity > 1 || this.CurrentVelocity < -1)
			{
				//if (this.Player.isGrounded)
					transform.Rotate(new Vector3(0, rot, 0));
				//else
					//transform.Rotate(new Vector3(0, rot, rot));
			}
		}
		else
			IsRotating = false;

		var lr = new RaycastHit();
		var rr = new RaycastHit();
		var lf = new RaycastHit();
		var rf = new RaycastHit();
		
		GetCubedRaycast (out lr, out rr, out lf, out rf);
		
		var upDir = GetCubedUpDirectoryNormalized (lr, rr, lf, rf);
		
		Debug.DrawRay(rr.point, Vector3.up);
		Debug.DrawRay(lr.point, Vector3.up);
		Debug.DrawRay(lf.point, Vector3.up);
		Debug.DrawRay(rf.point, Vector3.up);

		//Debug.Log (upDir);

		//if (upDir.z > 0)
		//	transform.up = upDir;
	}

	private float LastVelocity = 0;

	/// <summary>
	/// Handles the player movement.
	/// </summary>
	private void HandlePlayerMovement()
	{
		// The max speed. The player's speed should not get above this.
		var curMaxSpeed = PlayerCar.GetMaxSpeed ();
		var curMaxSpeedNoThrust = curMaxSpeed / 2;
		var speedFactors = PlayerCar.GetSpeedFactors ();
		var curMaxReverseSpeed = PlayerCar.GetCarReverseSpeed ();

		var speedFactor = 0f;

		/*if (Input.GetKey (KeyCode.S)) 
		{
			this.CurrentVelocity = LastVelocity / (BRAKE_FACTOR);
		} 
		else 
		{*/
		//TODO: My fiddly doos. You can remove these if you see fit.
		if (Input.GetKey (KeyCode.W) && Input.GetKey (KeyCode.LeftShift))
		{	
			if (this.CurrentVelocity > curMaxSpeed)
			{
				speedFactor = -10;
			}
			else if (this.CurrentVelocity < curMaxSpeed / 2)
			{
				speedFactor = speedFactors[1] * 2;
			}
			else if (this.CurrentVelocity < curMaxSpeed)
			{
				speedFactor = speedFactors[0] * 2;
			}
		}
		else if (Input.GetKey (KeyCode.W))
		{
			if (this.CurrentVelocity > curMaxSpeedNoThrust)
			{
				speedFactor = -10;
			}
			else if (this.CurrentVelocity < curMaxSpeedNoThrust / 2)
			{
				speedFactor = speedFactors[1];
			}
			else if (this.CurrentVelocity < curMaxSpeedNoThrust)
			{
				speedFactor = speedFactors[0];
			}
		}
		else
		{
			// This steps in after no key is pressed, so, this stops the car
			if (this.CurrentVelocity > 10)
			{
				speedFactor = -10;
			}
			else
			{
				if (!Input.GetKey (KeyCode.S))
					NullFloatValues (ref speedFactor, ref this.CurrentVelocity);
			}
		}

		// Handles the braking
		if (Input.GetKey (KeyCode.S)) 
		{
			speedFactor -= BRAKE_SPEED_DROP_FACTOR;

			if (this.CurrentVelocity < 0)
				speedFactor += this.CurrentVelocity;
		}

		if (this.CurrentVelocity < -curMaxReverseSpeed)
			speedFactor = 10;
		//}

		if (IsRotating)
			speedFactor /= ROTATE_FACTOR;

		/*
		if (IsColliding)
			speedFactor /= COLLISION_SPEED_FACTOR;
		*/

		this.CurrentVelocity += speedFactor;
		this.LastVelocity = CurrentVelocity;

		//Debug.Log (this.CurrentVelocity);

		//TODO: Make the character faster, make it "realistic", e.g. the car starts off slow, but gets faster and faster, 
		//		and nearing the end, gets faster by smaller amounts.
		//		like this: ___--¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯--_
		//		where _ = 0 - 10, - = 10 - 40, ¯ = 40 - 100, - = 100 - 120, _ = 120 - 130
		//		you should know this. BASICALLY, MAKE IT LIKE IN REAL LIFE

		this.CurrentVelocity /= (((2f * curMaxSpeed) - curMaxSpeed) / curMaxSpeed);

		var speed = new Vector3 (0, 0, (this.CurrentVelocity / REALISM_FACTOR) * -1);
		// We cut the velocity there ^ not to make the car too fast
		speed = transform.rotation * speed;
		speed = speed * Time.deltaTime;

		// This works because I removed the rigidbody component from the character. 
		// We do not need that, terrain handles collision fine already.
		this.Player.Move (speed);
	}

	private void OnCollisionStay(Collision other)
	{
	}

	private void OnCollisionEnter(Collision other)
	{
	}

	private void NullFloatValues(ref float first, ref float second)
	{
		first = 0;
		second = 0;
	}

	private void GetCubedRaycast(out RaycastHit bl, out RaycastHit br, out RaycastHit fl, out RaycastHit fr)
	{
		Physics.Raycast(BackLeft + Vector3.up, Vector3.down, out bl);
		Physics.Raycast(BackRight + Vector3.up, Vector3.down, out br);
		Physics.Raycast(FrontLeft + Vector3.up, Vector3.down, out fl);
		Physics.Raycast(FrontRight + Vector3.up, Vector3.down, out fr);
	}

	private Vector3 GetCubedUpDirectoryNormalized(RaycastHit bl, RaycastHit br, RaycastHit fl, RaycastHit fr)
	{
		return (Vector3.Cross(br.point - Vector3.up, bl.point - Vector3.up) +
		 			Vector3.Cross(bl.point - Vector3.up, fl.point - Vector3.up) +
		 			Vector3.Cross(fl.point - Vector3.up, fr.point - Vector3.up) +
		 			Vector3.Cross(fr.point - Vector3.up, br.point - Vector3.up)
		 		).normalized;
	}

	private void OnTriggerStay(Collider other)
	{

		//if (other.name != name)
			Debug.Log ("Nice meme!" + other.name);

	}
	
	private void OnControllerColliderHit(ControllerColliderHit hit)
	{
		/* */
	}
}
