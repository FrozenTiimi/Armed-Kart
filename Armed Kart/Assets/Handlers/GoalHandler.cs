using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class GoalHandler : MonoBehaviour 
{
	public int NumberOfLaps;

	private Dictionary<string, int> LapsDone = new Dictionary<string, int>();
	private Dictionary<string, System.Diagnostics.Stopwatch> Timers = new Dictionary<string, System.Diagnostics.Stopwatch>();

	private void OnTriggerExit(Collider other) 
	{
		if (other.transform.parent.tag.ToLower ().Contains ("player")) 
		{
			var otherName = other.transform.parent.name;
			var hasGoneThroughAllCheckpoints = true;

			if (Timers.ContainsKey(otherName))
			{
				var timer = Timers[otherName];

				if (timer.IsRunning)
				{
					timer.Stop ();

					// Increment lap amount
					if (!LapsDone.ContainsKey (otherName))
						LapsDone.Add(otherName, 0);

					LapsDone[otherName]++;

					for (var i = 0; i < transform.childCount; i++)
					{
						if (!transform.GetChild (i).GetComponent<CheckpointHandler>().GetHasTriggeredCheckpoint(otherName))
						{
							hasGoneThroughAllCheckpoints = false;
							break;
						}
					}

					if (!hasGoneThroughAllCheckpoints)
					{
						Debug.Log(string.Format ("Player {0} has not gone through all the checkpoints!\nWhat a cheater!", otherName));
					}
					else
					{
						for (var i = 0; i < transform.childCount; i++)
						{
							transform.GetChild (i).GetComponent<CheckpointHandler>().RemovePlayerFromList(otherName);
						}

						Debug.Log ("Lap Finished! Time for " + otherName + ": " + GetCurrentElapsedLapTime(otherName) + " seconds");
						
						timer.Reset ();

						if (LapsDone[otherName] >= NumberOfLaps - 1)
						{
							other.transform.parent.GetComponent<CarEngine>().FinishRace ();
						}
						else
						{
							timer.Start ();
							
							Debug.Log ("New lap started for player " + otherName + "!");
						}
					}
				}
				else
				{
					for (var i = 0; i < transform.childCount; i++)
					{
						if (!transform.GetChild (i).GetComponent<CheckpointHandler>().GetHasTriggeredCheckpoint(otherName))
						{
							hasGoneThroughAllCheckpoints = false;
							break;
						}
					}

					if (!hasGoneThroughAllCheckpoints)
					{
						Debug.Log(string.Format ("Player {0} has not gone through all the checkpoints!\nWhat a cheater!", otherName));
					}
					else
					{
						timer.Reset ();
						timer.Start();
						
						Debug.Log ("New lap started for player " + otherName + "!");
					}
				}
			}
			else
			{
				Timers.Add (otherName, new System.Diagnostics.Stopwatch());

				Timers[otherName].Start ();

				Debug.Log ("First lap started for player " + otherName + "!");
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
