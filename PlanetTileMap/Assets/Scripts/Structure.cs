using UnityEngine;
using System;
using System.Collections;

public class Structure {

	string _name;

	int _length;
	int _breadth;

	bool _linkedToNeighbor;

	Action<Structure> gameObjectChangedCallback;

	Tile _tile;
	Resource _resource;

	GameObject _gameObject;

	protected Structure() {
	}

	public string GetName() {
		return _name;
	}

	public int GetLength() {
		return _length;
	}

	public int GetBreadth() {
		return _breadth;
	}

	public bool isLinkedToNeighbor() {
		return _linkedToNeighbor;
	}


	public void SetGameObject(GameObject gameObject) {
		_gameObject = gameObject;
	}

	public GameObject GetGameObject() {
		return _gameObject;
	}

	public Tile GetTile() {
		return _tile;
	}

	public static Structure CreateStructure(string name, int length, int breadth, bool linkedToNeighbor) {
		Structure prototype = new Structure ();
		prototype._name = name;
		prototype._length = length;
		prototype._breadth = breadth;
		prototype._linkedToNeighbor = linkedToNeighbor;

		return prototype;
	}

	public static Structure PlaceStructureOnTile(Structure prototype, Tile tile) {
		Structure structure = new Structure ();

		structure._name = prototype._name;
		structure._length = prototype._length;
		structure._breadth = prototype._breadth;
		structure._linkedToNeighbor = prototype._linkedToNeighbor;

		if (tile.CanBuildHere () == false) {
			return null;
		} else {
			tile.BuildStructure (structure);
			structure._tile = tile;
		}

		if (structure.isLinkedToNeighbor() == true) {
			//North
			Tile northTile = tile.getWorld ().GetTileAtNorth(new Vector3(tile.getX(),tile.getY(),0));
			if (northTile != null && northTile.HasStructure() == true && northTile.GetStructure().GetName() == structure.GetName()) {
				//			Debug.Log ("World --> GetStructureNeighbors : North neighbor");
				northTile.GetStructure ().gameObjectChangedCallback (northTile.GetStructure ());
			}
			//East
			Tile eastTile = tile.getWorld ().GetTileAtEast(new Vector3(tile.getX(),tile.getY(),0));
			if (eastTile != null && eastTile.HasStructure() == true && eastTile.GetStructure().GetName() == structure.GetName()) {
				//			Debug.Log ("WOrld --> GetStructureNeighbors : East neighbor");
				eastTile.GetStructure ().gameObjectChangedCallback (eastTile.GetStructure ());
			}
			//South
			Tile southTile = tile.getWorld ().GetTileAtSouth(new Vector3(tile.getX(),tile.getY(),0));
			if (southTile != null && southTile.HasStructure() == true && southTile.GetStructure().GetName() == structure.GetName()) {
				//			Debug.Log ("WOrld --> GetStructureNeighbors : South neighbor");
				southTile.GetStructure ().gameObjectChangedCallback (southTile.GetStructure ());
			}
			//West
			Tile westTile = tile.getWorld ().GetTileAtWest(new Vector3(tile.getX(),tile.getY(),0));
			if (westTile != null && westTile.HasStructure() == true && westTile.GetStructure().GetName() == structure.GetName()) {
				//			Debug.Log ("WOrld --> GetStructureNeighbors : WEST neighbor");
				westTile.GetStructure ().gameObjectChangedCallback (westTile.GetStructure ());
			}

		}

		return structure;
	}

	/* Callback Registry */
	/// <summary>
	/// Registers the game object change callbacks.
	/// </summary>
	/// <param name="callback">Callback.</param>
	public void RegisterGameObjectChangeCallbacks(Action<Structure> callback) {
		gameObjectChangedCallback += callback;
	}
	/// <summary>
	/// Unregisters the game object change callbacks.
	/// </summary>
	/// <param name="callback">Callback.</param>
	public void UnregisterGameObjectChangeCallbacks(Action<Structure> callback) {
		gameObjectChangedCallback -= callback;
	}

}

