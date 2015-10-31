using UnityEngine;
using System.Collections;

public class AIHandler : BasicEntityHandler 
{
	private Vector3 LastPosition { get; set; }
	private bool MustMove { get; set; }

	// Use this for initialization
	private void Start () 
	{
		base.Start (); // always call these first

		this.MustMove = false;
	}
	
	// Update is called once per frame
	private void Update () 
	{
		base.Update (); // always call these first

		//TODO: Make this happen! Currently, this is COMPLETELY RANDOM !!!

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

		base.Player.Move (speed * Time.deltaTime);
	}
}
