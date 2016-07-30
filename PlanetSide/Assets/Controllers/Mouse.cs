using UnityEngine;
using System.Collections;

namespace Controllers {
	public class Mouse : MonoBehaviour {

		public GameObject cursorIndicatorPrefab;

		Vector3 dragStartPosition;
		Vector3 point;
		int startX, startY, endX, endY;
		int xPos, yPos;

		// Use this for initialization
		void Start () {
			cursorIndicatorPrefab = (GameObject)Instantiate (cursorIndicatorPrefab, new Vector3 (0, 0, 0), Quaternion.identity);
		}
		
		// Update is called once per frame
		void Update () {


			Vector3 currPosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			currPosition.z = 0;
			//Update the cursorIndicator position
			point = Views.Level.Instance.IsometricToCartesian(currPosition.x, currPosition.y);
			xPos = (int)point.x;
			yPos = (int)point.y;

			//FIXME: The Mouse should response only when within the tile boundaries
			if (xPos >= 0 && xPos < 20 && yPos >= 0 && yPos < 20) {
				cursorIndicatorPrefab.transform.position = Views.Level.Instance.CartesianToIsometric (xPos, yPos);
			}

			// Start Drag
			if (Input.GetMouseButtonDown (0)) {
				dragStartPosition = currPosition;
				//Vector3 point = Views.Level.Instance.IsometricToCartesian(currPosition.x, currPosition.y);
				//Debug.Log ("Mouse Controller : (" + currPosition.x + ", " + currPosition.y + ") to ("+ point.x + ", " + point.y + ")");
			}

			if (dragStartPosition.x <= currPosition.x) {
				startX = Mathf.FloorToInt (dragStartPosition.x);
				endX = Mathf.FloorToInt (currPosition.x);
			} else {
				endX = Mathf.FloorToInt (dragStartPosition.x);
				startX = Mathf.FloorToInt (currPosition.x);
			}

			if (dragStartPosition.x <= currPosition.x) {
				startY = Mathf.FloorToInt (dragStartPosition.y);
				endY = Mathf.FloorToInt (currPosition.y);
			} else {
				endY = Mathf.FloorToInt (dragStartPosition.y);
				startY = Mathf.FloorToInt (currPosition.y);
			}

			if (Input.GetMouseButton (0)) {
				for (int x = startX; x < endX; x++) {
					for (int y = startY; y < endY; y++) {
						//FIXME: do something to show that the user is dragging the mouse button here
					}
					
				}
			}

			if (Input.GetMouseButtonUp (0)) {
				for (int x = startX; x <= endX; x++) {
					for (int y = startY; y <= endY; y++) {
						//FIXME: get the world coordinates and then convert from isometric coordinates to cartesian coordinates
						point = Views.Level.Instance.IsometricToCartesian(currPosition.x, currPosition.y);
						xPos = (int)point.x;
						yPos = (int)point.y;
						Models.Surface surfaceModel = Views.Level.Instance.GetSurfaceAt (xPos, yPos);
						Debug.Log ("Mouse COntroller : surface Models : " + xPos + ", " + yPos + ", " + surfaceModel);
						if (surfaceModel != null) {
							
							surfaceModel.Terrain = Models.Surface.TerrainType.Mountain;
						}
					}

				}
			}
		}
	}

}