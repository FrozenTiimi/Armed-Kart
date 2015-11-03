using UnityEngine;
using System.Collections;

/// <summary>
/// A test car for the development team
/// </summary>
public class TestCar : CarHandler 
{
	/// <summary>
	/// Initializes a new instance of the <see cref="TestCar"/> class.
	/// </summary>
	public TestCar()
	{
		base.MaxSpeed = 250f; // 250 km/h
		base.CarWeight = 1f; // 1 ton
		base.CarReverseSpeed = 20f;
		base.Acceleration = 8f;
		base.CarTurningSpeedFactor = 1.15f;

		base.SpeedFactors = new float[2] { 5f, 10f };
	}
}
