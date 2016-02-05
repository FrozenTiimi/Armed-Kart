using UnityEngine;
using System.Collections;
using System.Linq;

public class CarEngine : MonoBehaviour 
{
	//Movement mechanics
	public float moveSpeed; // Recommended is 2000+
	public float rotateSpeed; // Recommended is 30-60
	public float maxVelo; // Recommended is 100-200

	//Here we store the original movement/rotation speed that is set individually for every car before runtime.
	private float originalSpeed;

	//tools to check if grounded
	private bool isRayTouchingGround;
	private Rigidbody carRB;

	private bool LeftDown = false;
	private bool RightDown = false;

	private Vector3 LastPosition { get; set; }
	private Quaternion LastRotation { get; set; }

	private bool RaceFinished = false;

	// Use this for initialization
	void Start () 
	{
		carRB = GetComponent<Rigidbody> ();
		carRB.centerOfMass = new Vector3 (0, 0, -1);
		originalSpeed = moveSpeed;

		this.LastPosition = transform.position;
		this.LastRotation = transform.rotation;
	}
	
	// Update is called once per framef
	void Update ()
	{
		if (!this.RaceFinished) 
		{
			var rotateMovement = rotateSpeed / (moveSpeed % rotateSpeed);
			RaycastHit hit;
			IsGrounded ();
			
			//Turning
			if (isRayTouchingGround) 
			{
				if (Input.GetKey ("a")) 
				{
					transform.Rotate (0, -rotateSpeed * Time.fixedDeltaTime, 0);
					if (moveSpeed == originalSpeed) 
					{
						moveSpeed = (moveSpeed / 2);
					}
				}
				else if (this.LeftDown)
				{
					this.MoveLeft();
				}
				
				if (Input.GetKey ("d")) 
				{
					transform.Rotate (0, rotateSpeed * Time.fixedDeltaTime, 0);
					if (moveSpeed == originalSpeed) 
					{
						moveSpeed = (moveSpeed / 2);
					}
				} 
				else if (this.RightDown)
				{
					this.MoveRight();
				}
				
				else 
				{
					moveSpeed = originalSpeed;
				}
			}
		}
	}

	private void MoveLeft()
	{
		var rotateMovement = rotateSpeed / (moveSpeed % rotateSpeed);
		rotateMovement /= 2;

		transform.Rotate (0, rotateSpeed * Time.fixedDeltaTime, 0);
		if (moveSpeed == originalSpeed) 
		{
			moveSpeed = (moveSpeed / 2);
		}
	}

	private void MoveRight()
	{
		var rotateMovement = rotateSpeed / (moveSpeed % rotateSpeed);
		rotateMovement /= 2;

		transform.Rotate (0, -rotateSpeed * Time.fixedDeltaTime, 0);
		if (moveSpeed == originalSpeed) 
		{
			moveSpeed = (moveSpeed / 2);
		}
	}

	public void FinishRace()
	{
		this.RaceFinished = true;

		Invoke ("LoadMainMenu", 3.0f);
	}

	public void ChangeLeft(bool value)
	{
		this.LeftDown = value;
	}

	public void ChangeRight(bool value)
	{
		this.RightDown = value;
	}

	public void Reset() 
	{
		this.transform.position = this.LastPosition;
		this.transform.rotation = this.LastRotation;
		carRB.velocity = Vector3.zero;
	}

	public void UpdateLast()
	{
		this.LastPosition = transform.position;
		this.LastRotation = transform.rotation;
	}

	public bool HasFinishedRace()
	{
		return this.RaceFinished;
	}

	public Rigidbody GetRB()
	{
		return this.carRB;
	}

	private void LoadMainMenu()
	{
		Application.LoadLevel ("MainMenu");
	}

	void LateUpdate() 
	{
		if (!this.RaceFinished) 
		{
			var kek = RealisticVelocity(moveSpeed) * Time.deltaTime;
			if (isRayTouchingGround) 
			{			
				carRB.AddForce (transform.forward * moveSpeed * Time.fixedDeltaTime * kek);
				
				if(carRB.velocity.magnitude > maxVelo)
				{
					Vector3 tempVelo = carRB.velocity;
					tempVelo.Normalize();
					carRB.velocity = tempVelo * maxVelo;
				}
			}
		}
	}

	float RealisticVelocity(float speed) 
	{
		return speed * 6f;
	}

	float GetAngle() {
		Vector3 dir = new Vector3(-1, -1, 0);
		dir = transform.TransformDirection(dir);
		float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
		return angle;
	}

	bool IsGrounded() 
	{
		var x = new Vector3(transform.position.x, transform.position.y - (transform.lossyScale.y / 10), transform.position.z);
		var y = Quaternion.AngleAxis(GetAngle (), transform.right * -1) * transform.forward;
		var z = 10;

		RaycastHit hit;

		Physics.Raycast (x, y, out hit, z);
		Debug.DrawRay (x, y, Color.yellow);

		if (hit.collider != null && hit.collider.tag == "Ramp") 
		{
			isRayTouchingGround = true;
		} 
		else 
		{
			isRayTouchingGround = false;
			moveSpeed /= 1.25f;
		}
		return isRayTouchingGround;
	}
}
