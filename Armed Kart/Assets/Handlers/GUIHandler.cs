using UnityEngine;
using System.Threading;
using System.Collections;

using ArmedKart.Utilities;

public class GUIHandler : MonoBehaviour 
{
	/// <summary>
	/// The minicam to attach to this handler
	/// </summary>
	public RenderTexture Minicam;

	private PlayerHandler pHandler { get { return transform.parent.GetComponent<PlayerHandler>(); } }

	private bool ShowDebugGUI { get; set; }
	private bool RaceFinishedScreenShown { get; set; }
	private bool StopShowingRaceFinishedTexts { get; set; }

	private float CurrentFPS { get; set; }
	private long CurrentVertices { get; set; }

	private void Start()
	{
		this.RaceFinishedScreenShown = false;
		this.StopShowingRaceFinishedTexts = false;

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
			//GUI.Label (new Rect (0, 64, 200, 200), "Is Colliding: " + pHandler.IsCarColliding().ToString(), gS);
			//GUI.Label (new Rect (0, 80, 200, 200), "\tColliding Angle: " + pHandler.GetCollidingAngle().ToString(), gS);
		}

		var raceFinishedStyle = new GUIStyle (gS);
		raceFinishedStyle.fontSize = 65;
		raceFinishedStyle.fontStyle = FontStyle.Bold;
		raceFinishedStyle.normal.textColor = Color.green;

		var gameTimeStyle = new GUIStyle(gS);
		gameTimeStyle.fontSize = 42;
		
		var timeStyle = new GUIStyle(gS);
		timeStyle.fontSize = 30;

		var lapStyle = new GUIStyle(gS);
		lapStyle.fontSize = 20;

		var playerHandler = transform.parent.GetComponent<PlayerHandler> ();
		var goalHandler = GameObject.FindGameObjectWithTag ("Goal").GetComponent<GoalHandler> ();
		var gameStartHandler = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameStart> ();

		var gameTimeMsecs = gameStartHandler.ElapsedGameTimeMsec ();
		var fGameTimeMsecs = gameTimeMsecs / 1000f;

		var msecs = goalHandler.GetCurrentElapsedLapTimeMsec (transform.parent.name);
		var fMsecs = (msecs != -1 ? (msecs / 1000f) : 0.00);

		var lapsDone = goalHandler.GetPlayerLapsDone (transform.parent.name);
		lapsDone = (lapsDone < 0 ? 0 : lapsDone);

		var gameTimeFloat = float.Parse (fGameTimeMsecs.ToString ("F2"));
		var gameTimeMinutes = 0;

		if (gameTimeFloat > 60)
		{
			gameTimeMinutes = Mathf.FloorToInt(gameTimeFloat / 60);
			gameTimeFloat = gameTimeFloat % 60;
		}

		var gameTimeText = ((gameTimeMinutes > 0) ? gameTimeMinutes.ToString() + "m " : "") + gameTimeFloat.ToString("F2") + "s";
		var timeText = fMsecs.ToString ("F2");
		var lapText = "Current lap: " + lapsDone;
		var raceFinishedText = "Race Finished!";

		if (!StopShowingRaceFinishedTexts) 
		{
			GUI.Label (new Rect (CenterScreenX (gameTimeText, gameTimeStyle), 10, GetTextWidth (gameTimeText, gameTimeStyle), 
			                     gameTimeStyle.fontSize), gameTimeText, gameTimeStyle);
			GUI.Label (new Rect (CenterScreenX(timeText, timeStyle), 10 + gameTimeStyle.fontSize + 5, GetTextWidth(timeText, timeStyle), 
			                     timeStyle.fontSize), timeText, timeStyle);
			GUI.Label (new Rect (CenterScreenX (lapText, lapStyle), 10 + gameTimeStyle.fontSize + timeStyle.fontSize + 5 + 5, 
			                     GetTextWidth (lapText, lapStyle), lapStyle.fontSize), lapText, lapStyle);
		}

		if (playerHandler.GetHasFinishedRace ()) 
		{
			if (!StopShowingRaceFinishedTexts)
			{
				GUI.Label (new Rect (CenterScreenX (raceFinishedText, raceFinishedStyle), CenterScreenY (raceFinishedText, raceFinishedStyle),
				                     GetTextWidth (raceFinishedText, raceFinishedStyle), raceFinishedStyle.fontSize),
				           raceFinishedText, raceFinishedStyle);
			}
			
			if (!RaceFinishedScreenShown)
			{
				this.RaceFinishedScreenShown = true;

				Invoke ("ShowRaceFinishedScreen", 2);
			}
		}

		// Draw the minimap
		GUI.DrawTexture (new Rect (Screen.width - 200, Screen.height - 200, 175, 175), Minicam);
	}

	private void ShowRaceFinishedScreen()
	{
		this.StopShowingRaceFinishedTexts = true;

		var rCanvas = GameObject.FindGameObjectWithTag("RaceCanvas").GetComponent<Canvas>();
		rCanvas.enabled = true;
	}

	private float CenterScreenX(string text, GUIStyle style)
	{
		return (Screen.width / 2) - (GetTextWidth(text, style) / 2);
	}

	private float CenterScreenY(string text, GUIStyle style)
	{
		return (Screen.height / 2) - (GetTextHeight (text, style) / 2);
	}

	private float GetTextWidth(string text, GUIStyle style)
	{
		return style.CalcSize(new GUIContent(text)).x;
	}

	private float GetTextHeight(string text, GUIStyle style)
	{
		return style.CalcSize(new GUIContent(text)).y;
	}
}
