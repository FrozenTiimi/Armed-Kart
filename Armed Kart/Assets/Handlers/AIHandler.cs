using UnityEngine;
using System.Collections;

public class AIHandler : MonoBehaviour 
{
	private CharacterController Player { get; set; }

	private Vector3 LastPosition { get; set; }
	private bool MustMove { get; set; }

	// Use this for initialization
	private void Start () 
	{
		this.MustMove = false;
	}
	
	// Update is called once per frame
	private void Update () 
	{
		//TODO: Make this happen! Currently, this is COMPLETELY RANDOM !!!
		this.Player = GetComponent<CharacterController> ();

		this.HandleAIRotation ();
		this.HandleAIMovement ();
	}

	private void LateUpdate()
	{
		if (LastPosition == transform.position) 
			this.MustMove = true;
		else if (MustMove)
			this.MustMove = false;

		this.LastPosition = transform.position;
	}

	private System.Random DoubleGuess = new System.Random();
	private System.Random IntGuess = new System.Random();

	private void HandleAIRotation()
	{
		float randFloat = (float)DoubleGuess.NextDouble () * 4;

		if (IntGuess.Next (0, 10) > 5) 
			randFloat *= -1;

		if (MustMove) 
			randFloat *= -1;

		transform.Rotate (new Vector3 (0, randFloat, 0));

		if (MustMove)
			transform.Rotate (new Vector3 (0, randFloat, 0));
	}

	private void HandleAIMovement()
	{
		var speed = new Vector3 (0, 0, 90);
		speed = transform.rotation * speed;

		this.Player.Move (speed * Time.deltaTime);
	}

	private void OnCollisionEnter(Collision other)
	{
	}

	private void OnCollisionStay(Collision other)
	{

	}
}
