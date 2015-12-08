using UnityEngine;
using System.Collections;

namespace ArmedKart.Utilities
{
	public static class ExtensionUtils
	{
		/// <summary>
		/// Sets the variable to zero.
		/// Please use this only for strings, objects and numeric variable types.
		/// </summary>
		/// <param name="source">Source variable.</param>
		public static void SetToZero(this object source)
		{
			source = 0;
		}

		/// <summary>
		/// Determines if the variable is zero.
		/// </summary>
		/// <returns><c>true</c> if is zero; otherwise, <c>false</c>.</returns>
		/// <param name="source">Source variable.</param>
		public static bool IsZero(this object source)
		{
			return source == (object)0;
		}

		/// <summary>
		/// Round the specified source.
		/// </summary>
		/// <param name="source">Source.</param>
		public static float Round(this float source)
		{
			return Mathf.Round(source);
		}

		/// <summary>
		/// Rounds the specified source to four decimals.
		/// </summary>
		/// <returns>The four decimal-version of the source.</returns>
		/// <param name="source">Source.</param>
		public static float RoundFourDecimals(this float source)
		{
			return (float)System.Math.Round((decimal)source, 4);
		}

		/// <summary>
		/// Gets the value to radians
		/// </summary>
		/// <returns>The radians.</returns>
		/// <param name="degrees">Degrees.</param>
		public static float InRadians(this float degrees)
		{
			return degrees * Mathf.Deg2Rad;
		}

		/// <summary>
		/// Gets the value in degrees
		/// </summary>
		/// <returns>The degrees.</returns>
		/// <param name="radians">Radians.</param>
		public static float InDegrees(this float radians)
		{
			return radians * Mathf.Rad2Deg;
		}

		/// <summary>
		/// Multiply the specified value by 2.
		/// </summary>
		/// <param name="value">Value.</param>
		public static float Multiply(this float value)
		{
			return value.Multiply (2f);
		}

		/// <summary>
		/// Multiply the specified value by given by-parameter.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <param name="by">By.</param>
		public static float Multiply(this float value, float by)
		{
			return value * by;
		}

		/// <summary>
		/// Half the specified value.
		/// </summary>
		/// <param name="value">Value.</param>
		public static float Half(this float value)
		{
			return value / 2;
		}

		/// <summary>
		/// Quarter the specified value.
		/// </summary>
		/// <param name="value">Value.</param>
		public static float Quarter(this float value)
		{
			return value / 4;
		}
	}
}
