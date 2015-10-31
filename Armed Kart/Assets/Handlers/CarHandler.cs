using UnityEngine;
using System.Collections;

/// <summary>
/// Car handler.
/// Tells a lot.
/// </summary>
public abstract class CarHandler
{
	/// <summary>
	/// The maximum speed.
	/// </summary>
	protected float MaxSpeed;
	protected float CarWeight;
	protected float CarReverseSpeed;

	protected float[] SpeedFactors = new float[] { 0f, 0f };

	/// <summary>
	/// Gets the car's max speed
	/// </summary>
	/// <returns>The max speed.</returns>
	public float GetMaxSpeed()
	{
		return MaxSpeed;
	}

	/// <summary>
	/// Gets the car weight.
	/// </summary>
	/// <returns>The car weight.</returns>
	public float GetCarWeight()
	{
		return CarWeight;
	}

	public float[] GetSpeedFactors()
	{
		return SpeedFactors;
	}

	public float GetCarReverseSpeed()
	{
		return CarReverseSpeed;
	}
}

/// <summary>
/// All the different car types to be added.
/// </summary>
public enum CarTypes
{
	Test = 0
}
