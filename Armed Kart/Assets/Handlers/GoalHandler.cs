using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GoalHandler : MonoBehaviour 
{
	private Dictionary<string, System.Diagnostics.Stopwatch> Timers = new Dictionary<string, System.Diagnostics.Stopwatch>();

	private void OnTriggerEnter(Collider other) 
	{
		if (other.name.ToLower ().Contains ("player")) 
		{
			if (Timers.ContainsKey(other.name))
			{
				var timer = Timers[other.name];

				if (timer.IsRunning)
				{
					timer.Stop ();

					Debug.Log ("Lap Finished! Time for " + other.name + ": " + GetCurrentElapsedLapTime(other.name) + " seconds");

					timer.Reset ();
					timer.Start ();

					Debug.Log ("New lap started for player " + other.name + "!");
				}
				else
				{
					timer.Reset ();
					timer.Start();

					Debug.Log ("New lap started for player " + other.name + "!");
				}
			}
			else
			{
				Timers.Add (other.name, new System.Diagnostics.Stopwatch());

				Timers[other.name].Start ();

				Debug.Log ("First lap started for player " + other.name + "!");
			}
		}
	}

	/// <summary>
	/// Gets the current elapsed lap time for the specified player.
	/// </summary>
	/// <returns>The current elapsed lap time in seconds. If player not found, returns -1.</returns>
	public int GetCurrentElapsedLapTime(string playerName)
	{
		if (Timers.ContainsKey (playerName))
			return Timers[playerName].Elapsed.Seconds;

		return -1;
	}
}
