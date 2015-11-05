﻿using UnityEngine;
using System.Threading;
using System.Collections;

public class GUIHandler : MonoBehaviour 
{
	/// <summary>
	/// The minicam to attach to this handler
	/// </summary>
	public RenderTexture Minicam;

	private PlayerHandler pHandler { get { return transform.parent.GetComponent<PlayerHandler>(); } }

	private bool ShowDebugGUI { get; set; }
	private float CurrentFPS { get; set; }
	private long CurrentVertices { get; set; }

	private void Start()
	{
		this.CurrentVertices = -1;
	}

	/// <summary>
	/// Updates the GUI Handler
	/// </summary>
	private void Update() 
	{
		if (Input.GetKeyDown (KeyCode.F1)) 
			this.ShowDebugGUI = !this.ShowDebugGUI;

		// Calculate FPS
		this.CurrentFPS = 1.0f / Time.deltaTime;
	}
	
	private void OnGUI()
	{
		var gS = new GUIStyle ();
		gS.normal.textColor = Color.red;

		GUI.Label (new Rect (0, 0, 200, 200), "Armed Kart Pre-Alpha", gS);

		if (ShowDebugGUI) 
		{
			// The FPS Counter
			GUI.Label (new Rect (0, 16, 200, 200), "FPS: " + this.CurrentFPS.ToString(), gS);
			GUI.Label (new Rect (0, 32, 200, 200), "Vertices in current terrain: " + ((this.CurrentVertices < 0) ? "Calculating" : this.CurrentVertices.ToString()), gS);
			GUI.Label (new Rect (0, 48, 200, 200), "Car Speed: " + pHandler.CurrentVelocity, gS);
			GUI.Label (new Rect (0, 64, 200, 200), "Is Colliding: " + pHandler.IsCarColliding().ToString(), gS);
			GUI.Label (new Rect (0, 80, 200, 200), "\tColliding Angle: " + pHandler.GetCollidingAngle().ToString(), gS);
		}

		// Draw the minimap
		GUI.DrawTexture (new Rect (Screen.width - 200, Screen.height - 200, 175, 175), Minicam);
	}
}
