using UnityEngine;
using System.Collections;

public class Structure {

	string _name;

	int _length;
	int _breadth;

	Tile _tile;
	Resource _resource;

	GameObject building;

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
		Structure building = new Structure ();

		building._name = prototype._name;
		building._length = prototype._length;
		building._breadth = prototype._breadth;

		if (tile.CanBuildHere () == false) {
			return null;
		} else {
			tile.BuildStructure (building);
		}

		return building;
	}

	public void AddSurface(Tile tile) {
		_tile = tile;
	}


}

