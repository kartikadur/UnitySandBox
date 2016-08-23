using UnityEngine;
using System.Collections;

public class Structure {

	string _name;

	int _length;
	int _breadth;

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

	public static Structure CreateStructure(string name, int length, int breadth) {
		Structure prototype = new Structure ();
		prototype._name = name;
		prototype._length = length;
		prototype._breadth = breadth;

		return prototype;
	}

	public static Structure PlaceStructureOnTile(Structure prototype, Tile tile) {
		Structure structure = new Structure ();

		structure._name = prototype._name;
		structure._length = prototype._length;
		structure._breadth = prototype._breadth;

		if (tile.CanBuildHere () == false) {
			return null;
		} else {
			tile.BuildStructure (structure);
		}

		return structure;
	}

	public void SetGameObject(GameObject gameObject) {
		_gameObject = gameObject;
	}

	public void AddSurface(Tile tile) {
		_tile = tile;
	}


}

