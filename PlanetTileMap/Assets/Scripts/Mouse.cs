using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class Mouse : MonoBehaviour {

	World world;
	Build buildController;

	Vector3 currentMousePosition;
	Vector3 lastMousePosition;
	Vector3 startPoint;
	Vector3 endPoint;

	bool _isDragMode = false;


	// Use this for initialization
	void Start () {
		world = World.instance;
		buildController = GameObject.FindObjectOfType<Build> ();
	}
	
	// Update is called once per frame
	void Update () {

		//Do nothing in the world if the pointer is on a menu system.
		if (EventSystem.current.IsPointerOverGameObject ()) {
			return;
		}

		CheckForCancelCommand ();

		//Tracks where mouse pointer starts this update
		currentMousePosition = GetMousePositionInWorld ();

		//check if user pressed left mouse button (or button(0))
		//	--> store start point
		// 	--> set is dragging to true to indicate drag mode enabled
		LeftMouseButtonPressed ();

		// here the user is definitely dragging so 
		//	--> if in build then call Build to show potential build structures
		WhileDragging();

		//check if user released left mouse button (or button(0))
		//	--> store end point
		//	--> if in build mode then call build to build structure
		//	--> set is dragging to false to indicate drag mode disabled
		LeftMouseButtonReleased();

		//Tracks where mouse pointer ends this update (for next update?)
		lastMousePosition = GetMousePositionInWorld ();
	}

	protected void LeftMouseButtonPressed() {
		if (Input.GetMouseButtonDown (0) == true) {
			startPoint = world.convert.fromIsometricToCartesianCoordinates (currentMousePosition);
			_isDragMode = true;

//			// Keep track of which tile is currently under the mouse
//			Tile tile = GetTileUnderMouse ();
//			if (tile != null) {
//				Debug.Log ("Mouse --> LeftMouseButtonReleased : tile coordinates (" + tile.getX () + ", " + tile.getY () + ")");
//			}
			Debug.Log ("Mouse --> LeftMouseButtonPressed : structure mode" + buildController.GetBuildMode ());
		}
	}

	protected void LeftMouseButtonReleased() {
		if (Input.GetMouseButtonUp (0) == true) {
			endPoint = world.convert.fromIsometricToCartesianCoordinates (currentMousePosition);
			_isDragMode = false;

			// Keep track of which tile is currently under the mouse
//			Tile tile = GetTileUnderMouse ();
//			if (tile != null) {
//				Debug.Log ("Mouse --> LeftMouseButtonReleased : tile coordinates (" + tile.getX () + ", " + tile.getY () + ")");
//			}
			Debug.Log ("Mouse --> LeftMouseButtonReleased : structure mode" + buildController.GetBuildMode ());
		}


	}

	protected void WhileDragging() {
		if (_isDragMode == true) {

			// Keep track of which tile is currently under the mouse
			Tile tile = GetTileUnderMouse ();
			if (tile != null) {
				Debug.Log ("Mouse --> While Dragging : tile coordinates (" + tile.getX () + ", " + tile.getY () + ")");
			}
		}
	}

	/// <summary>
	/// Checks for cancel command.
	/// </summary>
	public void CheckForCancelCommand() {
		if (Input.GetMouseButton (1)) {
			buildController.ResetBuildMode ();
		}
	}

	/// <summary>
	/// Gets the mouse position in world.
	/// </summary>
	/// <returns>The mouse position in world.</returns>
	public Vector3 GetMousePositionInWorld() {
		return Camera.main.ScreenToWorldPoint (Input.mousePosition);
	}

	/// <summary>
	/// Gets the tile under mouse.
	/// </summary>
	/// <returns>The tile under mouse.</returns>
	public Tile GetTileUnderMouse() {
		Vector3 point = world.convert.fromIsometricToCartesianCoordinates (GetMousePositionInWorld ());
		return world.GetTileAt (point);
	}

	/// <summary>
	/// Gets the structure under mouse.
	/// </summary>
	/// <returns>The structure under mouse.</returns>
	public Structure GetStructureUnderMouse() {
		//FIXME: temporary pull structure from tile
		return GetTileUnderMouse().GetStructure();
	}
}
