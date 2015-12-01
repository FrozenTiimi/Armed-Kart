using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GameStart : MonoBehaviour 
{
	private System.Diagnostics.Stopwatch GameTimer = new System.Diagnostics.Stopwatch();

	private void Awake()
	{
		// REMOVE THE GOD DAMN FPS LIMIT
		//Application.targetFrameRate = 120;

		// Safety switch
		//GameObject.FindGameObjectWithTag ("extracam").GetComponent<Camera> ().enabled = true;
		GameObject.FindGameObjectWithTag ("Cam1").GetComponent<Camera> ().enabled = true;
		GameObject.FindGameObjectWithTag ("Cam2").GetComponent<Camera> ().enabled = false;
	}

	private void Start()
	{
		GameTimer.Start ();
	}

	private void OnDestroy()
	{
		GameTimer.Stop ();
		GameTimer.Reset ();
	}

	public int ElapsedGameTimeSec()
	{
		return GameTimer.Elapsed.Seconds;
	}

	public long ElapsedGameTimeMsec()
	{
		return GameTimer.ElapsedMilliseconds;
	}

}
