using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Build : MonoBehaviour {

	public GameObject house_0;

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

	// Use this for initialization
	void Start () {
		world = World.instance;
		convert = new World.ConvertUnits ();
		mouse = GameObject.FindObjectOfType<Mouse> ();
		_showPlacement = new GameObject ();
		_showPlacement.AddComponent<SpriteRenderer> ();

		_cannotPlaceHere =  new Color(1.0f,0.25f,0.25f,0.8f);
		_canPlaceHere = new Color(0.25f,1.0f,0.25f,0.8f);

		CreateStructurePrototypes ();
	}
	
	// Update is called once per frame
	void Update () {

		ShowIfStructureIsPlaceableOnTile ();
	
	}

	/// <summary>
	/// Creates the structure prototypes.
	/// </summary>
	private void CreateStructurePrototypes() {
		_structurePrototypesMap = new Dictionary<string, Structure> ();
		_structureGameObjectMap = new Dictionary<string, GameObject> ();

		Structure structure = Structure.CreateStructure ("House", 2, 2);
		_structurePrototypesMap.Add (structure.GetName (), structure);
		//FIXME: temporary, do this in the sprite controller or something laters
		_structureGameObjectMap.Add (structure.GetName (), house_0);
	}

	/* --- Build Structures --- */
	//When isStructureModeActive is active user can build or destroy buildings
	public void SetBuildMode() {
		_isStructureModeActive = true;
	}

	public void ResetBuildMode() {
		_isStructureModeActive = false;
		_structureToBuild = "";
	}

	public bool GetBuildMode() {
		return _isStructureModeActive;
	}

	/// <summary>
	/// Shows if structure is placeable on tile.
	/// </summary>
	protected void ShowIfStructureIsPlaceableOnTile() {
		if (_isStructureModeActive == true && (_structureToBuild != "" || _structureToBuild != null) ) {
			Tile tile = mouse.GetTileUnderMouse ();

			if (tile == null) {
				//If there are no tiles under the mouse then do nothing
				return;
			}

			//Once we know the tile exists, then get its dimensions
			int xPos = tile.getX ();
			int yPos = tile.getY ();

			// Adjust sprite, sorting order, and position of the game object to show whether
			// user can build structure in current position
			_showPlacement.GetComponent<SpriteRenderer> ().sprite = _structureGameObjectMap [_structureToBuild].GetComponent<SpriteRenderer> ().sprite;
			_showPlacement.GetComponent<SpriteRenderer> ().sortingOrder = world._sortingOrderMax - Mathf.CeilToInt (Mathf.Sqrt (xPos * xPos + yPos * yPos));
			_showPlacement.transform.position = convert.fromCartesianToIsometricCoordinates (new Vector3 (xPos, yPos, 0.0f));
			//show the user whether they can place their selected structure
			if (world.CanPlaceOnTileAt (new Vector3 (xPos, yPos, 0.0f), 
				    new Vector3 (_structure.GetLength (), _structure.GetBreadth (), 0.0f)) == false) {

				//shows a red ghost of the structure to be placed here
				_showPlacement.GetComponent<SpriteRenderer> ().color = _cannotPlaceHere;

			} else {

				//shows a green ghost of the structure to be placed here
				_showPlacement.GetComponent<SpriteRenderer> ().color = _canPlaceHere;
			}

		}
	}

	public void BuildHouse() {
		SetBuildMode ();
		_structureToBuild = "House";
		_structure = _structurePrototypesMap [_structureToBuild];
		_showPlacement.name = _structureToBuild;
		_showPlacement.transform.SetParent (this.transform, true);

	}

	protected void BuildStructure(Structure structure, Tile tile) {

	}

	/* --- Destroy Structures? --- */
}
