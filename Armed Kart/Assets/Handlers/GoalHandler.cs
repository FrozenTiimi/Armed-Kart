using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class GoalHandler : MonoBehaviour 
{
	[SerializeField] private int NumberOfLaps;

	private Dictionary<string, int> LapsDone = new Dictionary<string, int>();
	private Dictionary<string, System.Diagnostics.Stopwatch> Timers = new Dictionary<string, System.Diagnostics.Stopwatch>();

	/*#if UNITY_EDITOR
	private bool ShowLapWarning = true;

	public void ShowEditGUI() {
		NumberOfLaps = EditorGUILayout.IntField("Amount of laps: ", NumberOfLaps);

		if (NumberOfLaps < 1) 
		{
			NumberOfLaps = 3;
			EditorUtility.DisplayDialog ("Maximum number of laps too low!", "The maximum number of laps can't be negative, or 0!", "OK, reset to 3");
		} 
		else if (NumberOfLaps > 100 && ShowLapWarning) 
		{
			if (!EditorUtility.DisplayDialog ("You sure about this?", "Over 100 laps? Are you reeeeaaaallly sure?", "Yes, let me have fun! :-(", "No, reset to 3"))
				NumberOfLaps = 3;
			else
				ShowLapWarning = false;
		}
	}
	#endif*/
	
	private void OnTriggerEnter(Collider other) 
	{
		Debug.Log ("TRIGGERED");

		if (other.name.ToLower ().Contains ("player")) 
		{
			var hasGoneThroughAllCheckpoints = true;

			if (Timers.ContainsKey(other.name))
			{
				var timer = Timers[other.name];

				if (timer.IsRunning)
				{
					timer.Stop ();

					// Increment lap amount
					if (!LapsDone.ContainsKey (other.name))
						LapsDone.Add(other.name, 0);

					LapsDone[other.name]++;

					for (var i = 0; i < transform.childCount; i++)
					{
						if (!transform.GetChild (i).GetComponent<CheckpointHandler>().GetHasTriggeredCheckpoint(other.name))
						{
							hasGoneThroughAllCheckpoints = false;
							break;
						}
					}

					if (!hasGoneThroughAllCheckpoints)
					{
						Debug.Log(string.Format ("Player {0} has not gone through all the checkpoints!\nWhat a cheater!", other.name));
					}
					else
					{
						for (var i = 0; i < transform.childCount; i++)
						{
							transform.GetChild (i).GetComponent<CheckpointHandler>().RemovePlayerFromList(other.name);
						}

						Debug.Log ("Lap Finished! Time for " + other.name + ": " + GetCurrentElapsedLapTime(other.name) + " seconds");
						
						timer.Reset ();
						
						if (LapsDone[other.name] >= NumberOfLaps)
						{
							other.transform.GetComponent<PlayerHandler>().FinishRace();
						}
						else
						{
							timer.Start ();
							
							Debug.Log ("New lap started for player " + other.name + "!");
						}
					}
				}
				else
				{
					for (var i = 0; i < transform.childCount; i++)
					{
						if (!transform.GetChild (i).GetComponent<CheckpointHandler>().GetHasTriggeredCheckpoint(other.name))
						{
							hasGoneThroughAllCheckpoints = false;
							break;
						}
					}

					if (!hasGoneThroughAllCheckpoints)
					{
						Debug.Log(string.Format ("Player {0} has not gone through all the checkpoints!\nWhat a cheater!", other.name));
					}
					else
					{
						timer.Reset ();
						timer.Start();
						
						Debug.Log ("New lap started for player " + other.name + "!");
					}
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

	public int GetPlayerLapsDone(string playerName)
	{
		if (LapsDone.ContainsKey (playerName))
			return LapsDone [playerName];

		return -1;
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

	public long GetCurrentElapsedLapTimeMsec(string playerName)
	{
		if (Timers.ContainsKey (playerName))
			return Timers[playerName].ElapsedMilliseconds;
		
		return -1;
	}
}
