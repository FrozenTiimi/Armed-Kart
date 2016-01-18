using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public static class ApplicationModel 
{
	/// <summary>
	/// The player cars.
	/// 
	/// FIRST PARAMETER string IS THE PLAYER NAME
	/// SECOND PARAMETER string IS THE CAR MODEL (eg. car_model_Valtteri)
	/// </summary>
	private static Dictionary<string, string> PlayerCars = new Dictionary<string, string>();

	public static void AddCar(string playerName, string carMesh)
	{
		if (PlayerCars.ContainsKey (playerName))
			PlayerCars.Remove (playerName);

		PlayerCars.Add (playerName, carMesh);
	}

	public static string GetCarMesh(string playerName)
	{
		var val = string.Empty;

		if (!ApplicationModel.PlayerCars.TryGetValue (playerName, out val))
			return string.Empty;

	    return val;
	}
}