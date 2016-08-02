using UnityEngine;
using System.Collections;

public static class Utility {

	//public static Vector3 DefaultOffset = new Vector3 (0.5f, 0.25f, 0f);


	/* Utility function to convert from Cartesian to Isometric coordinates */
	public static Vector3 ConvertCartesianToIsometric(Vector3 point, Vector3 offset = default(Vector3)) {
		if (offset == default(Vector3)) {
			offset = new Vector3 (0.5f, 0.25f, 0f);
		}
		float xPos = (point.x - point.y) * offset.x;
		float yPos = (point.x + point.y - point.z) * offset.y;
		//FIXME: hardcoded for now, but will fix eventually as needed
		float zPos = 0.0f;

		return new Vector3 (xPos, yPos, zPos);
	}

	/* Utility function to convert from isometric to cartesian coordinates */
	public static Vector3 ConvertIsometricToCartesian(Vector3 point, Vector3 offset = default(Vector3)) {

		if (offset == default(Vector3)) {
			offset = new Vector3 (0.5f, 0.25f, 0f);
		}
		int xPos = Mathf.CeilToInt(point.x / offset.x + point.y / offset.y) / 2;
		//clipping to corrent tile/coordinate

		int yPos = Mathf.CeilToInt(point.y / offset.y - point.x / offset.x) / 2;
		//FIXME: hardcoded for now, but will fix eventually as needed
		int zPos = 0;

		return new Vector3 (xPos, yPos, zPos);
	}

	public static void Swap<T> (ref T varA, ref T VarB) {
		T temp;
		temp = varA;
		varA = VarB;
		VarB = temp;
	}

	public static int SortingOrderNumber(int width, int height, int x, int y) {
		//(level.Width * level.Height) - (structureModel.SurfaceModel.X * level.Width + structureModel.SurfaceModel.Y)
		return (width * height) - (x * width + y);
	}
}
