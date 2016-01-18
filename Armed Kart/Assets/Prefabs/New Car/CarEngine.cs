using UnityEngine;
using System.Collections;
using System.Linq;

public class CarEngine : MonoBehaviour 
{
	public float moveSpeed;
	public float rotateSpeed;

	//Here we store the original movement/rotation speed that is set individually for every car before runtime.
	private float originalSpeed;

	private float boundY;
	private bool isRayTouchingGround;
	private Rigidbody carRB;

	// Use this for initialization
	void Start () 
	{
		carRB = GetComponent<Rigidbody> ();
		carRB.centerOfMass = new Vector3 (-1, 0, -2);
		originalSpeed = moveSpeed;
		boundY = GetComponent<MeshCollider> ().bounds.extents.y;
	}
	
	// Update is called once per frame
	void Update ()
	{
		var rotateMovement = rotateSpeed / (moveSpeed % rotateSpeed);

		RaycastHit hit;
		IsGrounded ();

		if (isRayTouchingGround)

			Debug.Log ("ON GROUND!!!" + carRB.position);
		else
			Debug.Log ("NOT ON GROUND!!!" + carRB.position);

		//Turning
		if (isRayTouchingGround) 
		{
			if (Input.GetKey ("a")) 
			{
				transform.Rotate (0, 0, -rotateSpeed * Time.fixedDeltaTime);
				if (moveSpeed == originalSpeed) {
					moveSpeed = (moveSpeed / 2);
				}
			}

			if (Input.GetKey ("d")) 
			{
				transform.Rotate (0, 0, rotateSpeed * Time.fixedDeltaTime);
				if (moveSpeed == originalSpeed) 
				{
					moveSpeed = (moveSpeed / 2);
				}

			} 
			else 
			{
				moveSpeed = originalSpeed;
			}
			
			if(Input.GetKey(KeyCode.Space))
			{
				carRB.velocity = carRB.velocity * 0.99f;
			}
		}		
	}

	void LateUpdate() {
		carRB.angularVelocity = carRB.angularVelocity * 0.01f;

		var kek = RealisticVelocity(moveSpeed) * Time.fixedDeltaTime;
		carRB.AddForce (transform.right * kek);
	}

	float RealisticVelocity(float speed) {
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
		var x = new Vector3(transform.position.x, transform.position.y - (transform.lossyScale.y / 2), transform.position.z);
		var y = Quaternion.AngleAxis(GetAngle (), transform.forward * -1) * transform.forward;
		var z = 10;

		RaycastHit hit;

		Physics.Raycast (x, y, out hit, z);
		Debug.DrawRay (x, y, Color.yellow);

		if (hit.collider.tag == "Ramp") 
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
