using UnityEngine;
using System.Collections;

/// <summary>
/// Car handler.
/// Tells a lot.
/// </summary>
public abstract class CarHandler
{
	//Maximum base speed.
	protected float MaxSpeed;

	/// <summary>
	/// The car weight.
	/// TODO: IMPLEMENT THIS
	/// </summary>
	protected float CarWeight;

	/// <summary>
	/// The car reverse speed.
	/// How fast the car reverses.
	/// TODO: IMPLEMENT THIS
	/// </summary>
	protected float CarReverseSpeed;

	/// <summary>
	/// The acceleration.
	/// The lower this is, the faster the car will go
	/// </summary>
	protected float Acceleration;

	/// <summary>
	/// The car turning speed factor.
	/// Lower this to make speed lower when turning
	/// </summary>
	protected float CarTurningSpeedFactor;

	/// <summary>
	/// The speed factors.
	/// The first parameter is when you have W pressed down
	/// The second parameter is when you have Shift + W pressed down
	/// </summary>
	protected float[] SpeedFactors = new float[] { 0f, 0f };

	/// <summary>
	/// Gets the car's max speed
	/// </summary>
	/// <returns>The max speed.</returns>
	public float GetMaxSpeed()
	{
		return MaxSpeed;
	}
	
	/// Gets the car weight and returns it.
	public float GetCarWeight()
	{
		return CarWeight;
	}

	//Gets the speed factor and returns it.
	public float[] GetSpeedFactors()
	{
		return SpeedFactors;
	}

	/// <summary>
	/// Gets the car reverse speed.
	/// </summary>
	/// <returns>The car reverse speed.</returns>
	public float GetCarReverseSpeed()
	{
		return CarReverseSpeed;
	}

	/// <summary>
	/// Gets the car acceleration.
	/// </summary>
	/// <returns>The car acceleration.</returns>
	public float GetCarAcceleration()
	{
		return Acceleration;
	}

	/// <summary>
	/// Gets the car turning speed factor.
	/// </summary>
	/// <returns>The car turning speed factor.</returns>
	public float GetCarTurningSpeedFactor()
	{
		return CarTurningSpeedFactor;
	}
}

/// <summary>
/// All the different car types to be added.
/// </summary>
public enum CarTypes
{
	Test = 0
}
