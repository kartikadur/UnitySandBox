using UnityEngine;
using System.Collections;

namespace Util {
	public class Utility {

		/// <summary>
		/// Converts isometric point to cartesian point given offset
		/// </summary>
		/// <returns>Cartesian vector3</returns>
		/// <param name="point">Point.</param>
		/// <param name="offset">Offset.</param>
		public static Vector3 IsometricToCartesian(Vector3 point, Vector3 offset = default(Vector3)) {

			if (offset == default(Vector3)) {
				offset = new Vector3 (0.5f, 0.25f, 0f);
			}
			float xPos = (point.x - point.y) * offset.x;
			float yPos = (point.x + point.y - point.z) * offset.y;
			//FIXME: currently zPos is hardcoded but should be calculated later
			float zPos = 0f;
			return new Vector3 (xPos, yPos, zPos);
		}

		/// <summary>
		/// Converts cartesian point to isometric point given offset
		/// </summary>
		/// <returns>isometric vector3</returns>
		/// <param name="point">Point.</param>
		/// <param name="offset">Offset.</param>
		public static Vector3 CartesianToIsometric(Vector3 point, Vector3 offset = default(Vector3)) {

			if (offset == default(Vector3)) {
				offset = new Vector3 (0.5f, 0.25f, 0f);
			}
			int xPos = Mathf.CeilToInt (point.x / offset.x + point.y / offset.y) / 2;
			int yPos = Mathf.CeilToInt (point.y / offset.y - point.x / offset.x) / 2;
			int zPos = 0;
			return new Vector3 (xPos, yPos, zPos);
		}

		/// <summary>
		/// Swap the specified values
		/// </summary>
		/// <param name="a">The alpha component.</param>
		/// <param name="b">The beta component.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static void Swap<T> (ref T a, ref T b) {
			T temp = a;
			a = b;
			b = temp;
		}

		/// <summary>
		/// Return the sorting layer for surface or structure based on it's x and y coordinate
		/// </summary>
		/// <returns>The order layer number</returns>
		/// <param name="height">Height.</param>
		/// <param name="width">Width.</param>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		public static int SortingOrderLayer(int height, int width, int x, int y) {
			return (width * height) - (x * width + y);
		}

	}
}