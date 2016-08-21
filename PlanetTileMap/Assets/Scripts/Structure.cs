using UnityEngine;
using System.Collections;

public class Structure {

	int _length;
	int _breadth;

	Tile _tile;
	Resource _resource;

	protected Structure() {
	}

	public Structure CreateStructure(int length, int breadth) {
		Structure prototype = new Structure ();
		prototype._length = length;
		prototype._breadth = breadth;

		return prototype;
	}

	public void AddSurface(Tile tile) {
		_tile = tile;
	}


}

