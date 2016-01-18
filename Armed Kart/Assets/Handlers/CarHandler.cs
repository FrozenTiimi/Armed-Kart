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
	/// The car turning speed factor.
	/// Lower this to make speed lower when turning
	/// </summary>
	protected float CarTurningSpeedFactor;

	/// <summary>
	/// The car power.
	/// </summary>
	protected float CarPower;

	/// <summary>
	/// The car health.
	/// </summary>
	protected float CarHealth;

	protected string ModelName = string.Empty;

	public string ModelLocation 
	{
		get { return "Cars/" + ModelName; }
	}

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

	/// <summary>
	/// Gets the car reverse speed.
	/// </summary>
	/// <returns>The car reverse speed.</returns>
	public float GetCarReverseSpeed()
	{
		return CarReverseSpeed;
	}

	/// <summary>
	/// Gets the car turning speed factor.
	/// </summary>
	/// <returns>The car turning speed factor.</returns>
	public float GetCarTurningSpeedFactor()
	{
		return CarTurningSpeedFactor;
	}

	/// <summary>
	/// Gets the car power.
	/// </summary>
	/// <returns>The car power.</returns>
	public float GetCarPower()
	{
		return this.CarPower;
	}

	/// <summary>
	/// Gets the car health.
	/// </summary>
	/// <returns>The car health.</returns>
	public float GetCarHealth()
	{
		return this.CarHealth;
	}
}

/// <summary>
/// All the different car types to be added.
/// </summary>
public enum CarTypes
{
	Test = 0
}
