using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class CheckpointHandler : MonoBehaviour 
{
	private List<string> CheckpointTriggered = new List<string> ();

	public void RemovePlayerFromList(string playerName)
	{
		CheckpointTriggered.Remove (playerName);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.transform.parent.name.ToLower ().Contains ("player")) 
		{
			if (!CheckpointTriggered.Contains (other.transform.parent.name))
				CheckpointTriggered.Add (other.transform.parent.name);
		}
	}

	/// <summary>
	/// Gets whether the player has triggered the checkpoint or not
	/// </summary>
	/// <returns><c>true</c>, if checkpoint was triggered by given player name, <c>false</c> otherwise.</returns>
	/// <param name="playerName">The player's name that is being checked.</param>
	public bool GetHasTriggeredCheckpoint(string playerName)
	{
		return CheckpointTriggered.Contains (playerName);
	}
}
