﻿using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class AIHandler : MonoBehaviour 
{
	private CharacterController Player { get; set; }

	public CarTypes CarType;

	// Use this for initialization
	private void Start () 
	{
	}
	
	// Update is called once per frame
	private void Update () 
	{
		this.Player = GetComponent<CharacterController> ();

		this.HandleAIRotation ();
		this.HandleAIMovement ();
	}

	private void LateUpdate()
	{

	}

	private System.Random DoubleGuess = new System.Random();
	private System.Random IntGuess = new System.Random();

	private GameObject CurrentCheckpoint;
	private int CurrentCheckpointID;
	private GameObject[] CheckPoints;

	private void HandleAIRotation()
	{
		/*
		float randFloat = (float)DoubleGuess.NextDouble () * 4;

		if (IntGuess.Next (0, 10) > 5) 
			randFloat *= -1;

		if (MustMove) 
			randFloat *= -1;

		transform.Rotate (new Vector3 (0, randFloat, 0));

		if (MustMove)
			transform.Rotate (new Vector3 (0, randFloat, 0));
			*/

		var otherPlayer = GameObject.Find ("NewPlayer");
		var testTerrain = GameObject.Find ("TestTerrain");

		if (this.CheckPoints == null) 
		{
			this.CheckPoints = GameObject.FindGameObjectsWithTag ("checkpoint");
			this.CheckPoints = this.CheckPoints.ToList<GameObject>().OrderBy(g => int.Parse (g.name.Substring(10, g.name.Length - 10))).ToArray();
		}

		if (CurrentCheckpoint == null) 
		{
			this.CurrentCheckpointID = 0;
			CurrentCheckpoint = this.CheckPoints [this.CurrentCheckpointID];
			Debug.Log (this.CurrentCheckpoint.name);
		}

		transform.LookAt (this.CurrentCheckpoint.transform.position);
		//var model = transform.FindChild ("Model");
		//model.rotation = Quaternion.Euler (model.transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, model.transform.rotation.eulerAngles.z);

		Debug.DrawRay (transform.position, (this.CurrentCheckpoint.transform.position - transform.position), Color.blue);
	}

	private void HandleAIMovement()
	{
		var speed = new Vector3 (0, 0, 60);
		speed = transform.rotation * speed;

		this.Player.Move (speed * Time.deltaTime);

		if ((this.Player.transform.position.x > this.CurrentCheckpoint.transform.position.x - 5 && this.Player.transform.position.z > this.CurrentCheckpoint.transform.position.z - 5) &&
			(this.Player.transform.position.x < this.CurrentCheckpoint.transform.position.x + 5 && this.Player.transform.position.z < this.CurrentCheckpoint.transform.position.z + 5)) 
		{
			if (this.CurrentCheckpointID + 1 >= this.CheckPoints.Length)
				this.CurrentCheckpointID = 0;
			else
				this.CurrentCheckpointID++;

			this.CurrentCheckpoint = this.CheckPoints[this.CurrentCheckpointID];
		}
	}

	private void OnCollisionEnter(Collision other)
	{
	}

	private void OnCollisionStay(Collision other)
	{

	}
}
