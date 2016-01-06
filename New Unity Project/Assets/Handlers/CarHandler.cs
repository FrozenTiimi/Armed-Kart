using UnityEngine;
using System.Collections;

public class CarHandler : MonoBehaviour 
{
	private Rigidbody Car { get; set; }

	public float Speed = 0f;
	public float Power = 0.01f;
	public float Friction = 0.95f;

	private bool Right = false;
	private bool Left = false;

	private void Start () 
	{
		this.Car = this.GetComponent<Rigidbody> ();

		this.Car.centerOfMass = new Vector3 (0f, 0f, 1.0f);
	}

	private void FixedUpdate() 
	{
		if (this.Right) 
			this.Speed += this.Power;
		else
			this.Speed -= this.Power;
	}

	private void Update () 
	{
		if (Input.GetKeyDown ("w"))
			this.Right = true;
		else if (Input.GetKeyUp ("w"))
			this.Right = false;

		if (Input.GetKeyDown ("s"))
			this.Left = true;
		else if (Input.GetKeyUp ("s"))
			this.Left = false;

		this.Speed *= Friction;

		transform.Translate (Vector3.right * this.Speed);
	}
}
