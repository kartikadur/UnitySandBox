using UnityEngine;
using System.Collections;

public static class Utility {

	/* Utility function to convert from Cartesian to Isometric coordinates */
	public static Vector3 ConvertCartesianToIsometric(Vector3 point, float xOffset, float yOffset) {
		float xPos = (point.x + point.y) * xOffset;
		float yPos = (point.x - point.y) * yOffset;
		//FIXME: hardcoded for now, but will fix eventually as needed
		float zPos = 0.0f;

		return new Vector3 (xPos, yPos, zPos);
	}
}
