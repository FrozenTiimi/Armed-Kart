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
		base.MaxSpeed = 250; // 250 km/h
		base.CarWeight = 1; // 1 ton
	}
}
