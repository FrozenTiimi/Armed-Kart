using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// This is the base-entity handler for all entities. 
/// Please add this to all entities
/// </summary>
public class BasicEntityHandler : MonoBehaviour 
{

	/// <summary>
	/// Gets or sets the charactercontroller (player)
	/// </summary>
	/// <value>The player.</value>
	private Rigidbody Player { get; set; }

	private Vector3 StartingPosition { get; set; }

	/// <summary>
	/// The GRAVITY constant
	/// </summary>
	const float GRAVITY = 55f;
	const float GRAVITY_ADD = 0.5f;

	private float CurrentGravity = 0f;
	private float VerticalSpeed = 0f;

	private bool IsGrounded { get; set; }

	// Use this for initialization
	private void Start () 
	{
		this.IsGrounded = false;
		this.StartingPosition = transform.position;
	}
	
	// Update is called once per frame
	private void Update () 
	{
		this.Player = GetComponentInChildren<Rigidbody> ();

		var carWeight = 2f; // default;

		if (GetComponent <PlayerHandler>() != null) // checks if PlayerHandler is not found
		{
			this.Player.mass = carWeight = GetComponent<PlayerHandler> ().PlayerCar.GetCarWeight ();
			carWeight += 1f;
		} 

		// IMPROVED GRAVITY
		/*
		if (this.Player.isGrounded)
		{
			if (CurrentGravity > 0)
				CurrentGravity = 0;
			
			this.VerticalSpeed = -(this.Player.stepOffset * carWeight) / Time.deltaTime;
		}
		else
		{
			if (CurrentGravity <= GRAVITY - GRAVITY_ADD)
				CurrentGravity += GRAVITY_ADD;
			
			this.VerticalSpeed -= (CurrentGravity * carWeight) * Time.deltaTime;
		}
		*/

		//this.Player.Move (new Vector3 (0, this.VerticalSpeed, 0));

		this.IsGrounded = this.DetectOnGround ();

		if (transform.position.y < -100) // H4X0R DETECTED !!!!!!!!!
			transform.position = this.StartingPosition; // Go back to your corner of shame
	}

	private bool DetectOnGround()
	{
		Debug.DrawRay (this.Player.position, Vector3.down, Color.green);

		var wheelRays = new List<bool> ();

		for (var i = 0; i < this.Player.gameObject.transform.childCount; i++) 
		{
			var child = this.Player.gameObject.transform.GetChild(i);
			if (child.name.Contains("wheel"))
			{
				//Debug.Log (this.Player.gameObject.transform.GetChild(i).name);
				Debug.DrawRay (child.position, Vector3.down, Color.red);

				var raycastRay = new Ray (this.Player.position, Vector3.down);
				var raycastHitInfo = new RaycastHit();

				wheelRays.Add (!Physics.Raycast(raycastRay, out raycastHitInfo));

				if (Physics.Raycast(raycastRay, out raycastHitInfo))
				{
					Debug.Log (raycastHitInfo.collider.name);
					Debug.Log (raycastHitInfo.distance);
				}
			}
		}

		var raycastRay2 = new Ray (this.Player.position, Vector3.down);
		var raycastHitInfo2 = new RaycastHit();

		wheelRays.Add (!Physics.Raycast (raycastRay2, out raycastHitInfo2));
		
		//Debug.Log (wheelRays.Contains (true));
		
		return wheelRays.Contains(true);
	}

	private void LateUpdate()
	{/*
		this.Player.transform.Rotate(new Vector3(
			this.Player.transform.rotation.x, this.Player.transform.rotation.y / 2, this.Player.transform.rotation.z
			));
	*/
	}

	private void OnCollisionEnter(Collision other)
	{
		//if (other.collider.name == "Level")
		//	this.IsGrounded = true;
	}

	private void OnCollisionStay(Collision other)
	{
		if (other.collider.name != "Level")
			Debug.Log ("Collision !!!! " + other.collider.name);
	}

	private void OnCollisionExit(Collision other)
	{
		//if (other.collider.name == "Level")
		//	this.IsGrounded = false;
	}

	public bool GetIsGrounded()
	{
		return this.IsGrounded;
	}
}
