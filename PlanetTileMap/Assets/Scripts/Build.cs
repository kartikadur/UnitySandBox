using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class Build : MonoBehaviour {

	public GameObject house;
	public GameObject[] road;

	bool _isStructureModeActive = false;
	string _structureToBuild;

	World world;
	World.ConvertUnits convert;
	Mouse mouse;
	GameObject _showPlacement;
	Structure _structure;

	Color _cannotPlaceHere;
	Color _canPlaceHere;

	Dictionary<string, Structure> _structurePrototypesMap;
	Dictionary<string, GameObject> _structureGameObjectMap;
	Dictionary<string, Structure> _structuresPlacedMap;

	//to keep list of tiles on which draggable structures can be placed
	List<Tile> _tileList;
	bool _isDragMode = false;

	// Use this for initialization
	void Start () {
		world = World.instance;
		convert = new World.ConvertUnits ();
		mouse = GameObject.FindObjectOfType<Mouse> ();
		_showPlacement = new GameObject ();
		_showPlacement.AddComponent<SpriteRenderer> ();
		_showPlacement.SetActive (false);


		_tileList = new List<Tile> ();

		_cannotPlaceHere =  new Color(1.0f,0.25f,0.25f,0.8f);
		_canPlaceHere = new Color(0.25f,1.0f,0.25f,0.8f);

		CreateStructurePrototypes ();
	}
	
	// Update is called once per frame
	void Update () {

		//Do nothing in the world if the pointer is on a menu system.
		if (EventSystem.current.IsPointerOverGameObject ()) {
			return;
		}

		CheckForCancelCommand ();

		ShowIfStructureIsPlaceableOnTile ();


		//Show structure placement before mouse click

		//if mouse button (0) clicked 
		// record start tile
		// isdraggable = true
		LeftMouseButtonPressed();

		// while dragging
		// record position on each new game object and place it in a queue or array
		WhileDragging();

		//when mouse button (0) released
		//record end tile
		//isdraggable false
		// if draggable object then create structures for all gameobjects on screen and place permanent fictures in world
		// if single structure call build structure
		LeftMouseButtonReleased();

	
	}

	/// <summary>
	/// Creates the structure prototypes.
	/// Structure will grow massively when all structures are created, may export to another class?
	/// </summary>
	private void CreateStructurePrototypes() {
		Structure structure;

		_structurePrototypesMap = new Dictionary<string, Structure> ();
		_structureGameObjectMap = new Dictionary<string, GameObject> ();

		structure = Structure.CreateStructure ("House_", 2, 2, false);
		_structurePrototypesMap.Add (structure.GetName (), structure);
		//FIXME: temporary, do this in the sprite controller or something laters
		_structureGameObjectMap.Add (structure.GetName (), house);

		structure = Structure.CreateStructure("Road_", 1, 1, true);

		_structurePrototypesMap.Add (structure.GetName (), structure);
		//FIXME: temporary, do this in the sprite controller or something laters
//		_structureGameObjectMap.Add (structure.GetName () + "_", road[structure.GetName () + "_"]);
		_structureGameObjectMap.Add(structure.GetName(), road[0]);
		_structureGameObjectMap.Add(structure.GetName() + "N", road[1]);
		_structureGameObjectMap.Add(structure.GetName() + "E", road[2]);
		_structureGameObjectMap.Add(structure.GetName() + "S", road[3]);
		_structureGameObjectMap.Add(structure.GetName() + "W", road[4]);
		_structureGameObjectMap.Add(structure.GetName() + "NE", road[5]);
		_structureGameObjectMap.Add(structure.GetName() + "NS", road[6]);
		_structureGameObjectMap.Add(structure.GetName() + "ES", road[7]);
		_structureGameObjectMap.Add(structure.GetName() + "EW", road[8]);
		_structureGameObjectMap.Add(structure.GetName() + "SW", road[9]);
		_structureGameObjectMap.Add(structure.GetName() + "NW", road[10]);
		_structureGameObjectMap.Add(structure.GetName() + "NES", road[11]);
		_structureGameObjectMap.Add(structure.GetName() + "ESW", road[12]);
		_structureGameObjectMap.Add(structure.GetName() + "NSW", road[13]);
		_structureGameObjectMap.Add(structure.GetName() + "NEW", road[14]);
		_structureGameObjectMap.Add(structure.GetName() + "NESW", road[15]);
	}

	/* --- HELPER FUNCTIONS -- */
	/// <summary>
	/// Checks for cancel command.
	/// </summary>
	public void CheckForCancelCommand() {
		if (Input.GetMouseButton (1)) {
			ResetBuildMode ();
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
		
	protected void LeftMouseButtonPressed() {
		if (Input.GetMouseButtonDown (0) == true && _structure.isLinkedToNeighbor () == true) {
			Tile tile = GetTileUnderMouse ();
			if (tile != null && _tileList.Contains(tile) == false) {
				_tileList.Add (tile);
			}
			ShowIfStructureIsPlaceableOnTile (tile);
			_isDragMode = true;
		}
	}

	protected void LeftMouseButtonReleased() {
		if (Input.GetMouseButtonUp (0) == true ) {
			Tile tile = GetTileUnderMouse ();
			ShowIfStructureIsPlaceableOnTile (tile);
			if (_isDragMode == true) {
				if (tile != null && _tileList.Contains (tile) == false) {
					_tileList.Add (tile);
				}
				_isDragMode = false;
				BuildStructureOnMultipleTiles (_tileList);
			} else {
				BuildStructureOnTile (tile);
			}	
		} 
	}

	/// <summary>
	/// While the user is dragging the structure across multiple tiles.
	/// </summary>
	protected void WhileDragging() {
		if (_isDragMode == true) {
			Tile tile = GetTileUnderMouse ();
			ShowIfStructureIsPlaceableOnTile (tile);
			if (tile != null && _tileList.Contains(tile) == false) {
				_tileList.Add (tile);
			}
		}
	}


	/* --- Build Structures --- */
	//When isStructureModeActive is active user can build or destroy buildings
	public void SetBuildMode() {
		_isStructureModeActive = true;
	}

	public void ResetBuildMode() {
		_isStructureModeActive = false;
		_structureToBuild = "";
		_showPlacement.SetActive (false);
	}

	public bool GetBuildMode() {
		return _isStructureModeActive;
	}

	protected void ShowIfStructureIsPlaceableOnTile(Tile tile) {

		//if the tile is empty, or no structure is selected or not in build mode then exit
		if (tile == null || _isStructureModeActive == false || _structureToBuild == null || _structureToBuild == "") {
			return;

		}

		//Once we know the tile exists, then get its dimensions
		int xPos = tile.getX ();
		int yPos = tile.getY ();

		// Adjust sprite, sorting order, and position of the game object to show whether
		// user can build structure in current position
		_showPlacement.GetComponent<SpriteRenderer> ().sprite = _structureGameObjectMap [_structureToBuild].GetComponent<SpriteRenderer> ().sprite;
		_showPlacement.GetComponent<SpriteRenderer> ().sortingOrder = world._sortingOrderMax + 1;
		_showPlacement.transform.position = convert.fromCartesianToIsometricCoordinates (new Vector3 (xPos, yPos, 0.0f));
		//show the user whether they can place their selected structure
		if (world.CanPlaceStructureOnTile(_structure,tile) == false) {

			//shows a red ghost of the structure to be placed here
			_showPlacement.GetComponent<SpriteRenderer> ().color = _cannotPlaceHere;

		} else {

			//shows a green ghost of the structure to be placed here
			_showPlacement.GetComponent<SpriteRenderer> ().color = _canPlaceHere;
		}



	}
	/// <summary>
	/// Shows if structure is placeable on tile.
	/// </summary>
	protected void ShowIfStructureIsPlaceableOnTile() {
		if (_isStructureModeActive == true && (_structureToBuild != "" || _structureToBuild != null) ) {
			Tile tile = GetTileUnderMouse ();

			if (tile == null) {
				//If there are no tiles under the mouse then do nothing
				return;
			}
			ShowIfStructureIsPlaceableOnTile (tile);
		}
	}

	/// <summary>
	/// Builds the house.
	/// </summary>
	public void BuildHouse() {
		SetBuildMode ();
		_structureToBuild = "House_";
		prepareStructureToBuild ();
	}

	/// <summary>
	/// Builds the road.
	/// </summary>
	public void BuildRoad() {
		SetBuildMode ();
		_structureToBuild = "Road_";
		prepareStructureToBuild ();
	}

	/// <summary>
	/// Prepares the structure to build.
	/// </summary>
	protected void prepareStructureToBuild() {
		_structure = _structurePrototypesMap [_structureToBuild];
		_showPlacement.name = _structureToBuild;
		_showPlacement.transform.SetParent (this.transform, true);
		_showPlacement.SetActive (true);
	}

	/// <summary>
	/// Builds the structure on tile. Or basically shows the gameobject at the correct position
	/// </summary>
	/// <param name="tile">Tile.</param>
	protected void BuildStructureOnTile(Tile tile) {
	
		if (world.CanPlaceStructureOnTile (_structure, tile) == false) {
			return;
		}

		int xPos = tile.getX ();
		int yPos = tile.getY ();
		string links = "";

		Structure builtStructure = world.PlaceStructureOnTile (_structure, tile);
		GameObject gameObject = new GameObject ();
		gameObject.name = "Structure_" + _structureToBuild + "_" + xPos + "_" + yPos;
		gameObject.transform.SetParent (this.transform, true);
		SpriteRenderer sr = gameObject.AddComponent<SpriteRenderer> ();

		if (builtStructure.isLinkedToNeighbor () == true) {
			links = world.GetStructureNeighbors (builtStructure);
			builtStructure.RegisterGameObjectChangeCallbacks ((structure) => {
				string links_ = world.GetStructureNeighbors(structure);
				Debug.Log("Structure Callback from Build : neighbor links : " + links_);
				structure.GetGameObject().GetComponent<SpriteRenderer>().sprite = _structureGameObjectMap [structure.GetName() + links_].GetComponent<SpriteRenderer> ().sprite;
			});

		}

		sr.sprite = _structureGameObjectMap [_structureToBuild + links].GetComponent<SpriteRenderer> ().sprite;
		sr.sortingOrder = world._sortingOrderMax - Mathf.CeilToInt (Mathf.Sqrt (xPos * xPos + yPos * yPos));
		sr.transform.position = convert.fromCartesianToIsometricCoordinates (new Vector3 (xPos, yPos, 0.0f));
		builtStructure.SetGameObject (gameObject);
	}

	/// <summary>
	/// Builds the structure on multiple tiles.
	/// </summary>
	/// <param name="tileList">Tile list.</param>
	protected void BuildStructureOnMultipleTiles(List<Tile> tileList) {
		
		foreach (Tile tile in tileList) {
//			Debug.Log ("Build --> build structure on Multiple tiles. building on tile (" + tile.getX () + ", " + tile.getY () + ")");
			// build structure on tile
			BuildStructureOnTile (tile);
		}

		//empty tilelist regardless of how many structures were built
		_tileList.Clear();
	}

	/* --- Destroy Structures? --- */
}
