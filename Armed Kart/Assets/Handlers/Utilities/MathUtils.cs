using UnityEngine;
using System.Collections;

namespace ArmedKart.Utilities
{
	/// <summary>
	/// Contains all of Armed Kart's custom math utilities
	/// </summary>
	public static class MathUtils
	{
		/// <summary>
		/// Converts Degs to RAD.
		/// </summary>
		/// <returns>The radians.</returns>
		/// <param name="value">Given degree value.</param>
		public static float DegToRad(float value)
		{
			return value * Mathf.Deg2Rad;
		}

		/// <summary>
		/// Converts RAD to DEGS
		/// </summary>
		/// <returns>The degrees.</returns>
		/// <param name="value">Given radian value.</param>
		public static float RadToDeg(float value)
		{
			return value * Mathf.Rad2Deg;
		}

		/// <summary>
		/// Flips the specified value from for example, 1 to -1 and vice versa.
		/// LET THIS FUNCTION ALONE.
		/// </summary>
		/// <param name="val">Value.</param>
		public static float Flip(float val)
		{
			return val * -1;
		}

		/// <summary>
		/// Gets half of the given value
		/// </summary>
		/// <returns>Halved value.</returns>
		/// <param name="val">Value.</param>
		public static float HalfOf(float val)
		{
			return val / 2;
		}
	}
}
