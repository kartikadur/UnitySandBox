﻿using UnityEngine;
using System.Collections;

public class Tile {

	World _world;

	GameObject _surface;

	bool _canBuildHere = false;
	Structure _structure;


	int _x;
	int _y;

	public Tile(World world, GameObject surface, bool canBuildHere, int x, int y) {
		_world = world;
		_surface = surface;
		_canBuildHere = canBuildHere;

		_x = x;
		_y = y;
	}

	public World getWorld() {
		return _world;
	}

	public GameObject getSurface() {
		return _surface;
	}

	public int getX() {
		return _x;
	}

	public int getY() {
		return _y;
	}

	/// <summary>
	/// Gets the structure.
	/// </summary>
	/// <returns>The structure.</returns>
	public Structure GetStructure() {
		return _structure;
	}

	/// <summary>
	/// Determines whether this instance has structure.
	/// </summary>
	/// <returns><c>true</c> if this instance has structure; otherwise, <c>false</c>.</returns>
	public bool HasStructure() {
		return _structure != null;
	}

	/// <summary>
	/// Determines whether this instance can build here.
	/// </summary>
	/// <returns><c>true</c> if this instance can build here; otherwise, <c>false</c>.</returns>
	public bool CanBuildHere() {
		return (_canBuildHere == true && _structure == null);
	}


	/// <summary>
	/// Builds the structure.
	/// </summary>
	/// <returns><c>true</c>, if structure was built, <c>false</c> otherwise.</returns>
	/// <param name="structure">Structure.</param>
	public bool BuildStructure(Structure structure) {
		Debug.Log ("Tile --> Build Structure : Tile at (" + _x + ", " + _y + ")");
		if (CanBuildHere () == true) {
			_structure = structure;
			return true;
		}
		return false;
	}

	/// <summary>
	/// Destroies the structure.
	/// </summary>
	/// <returns><c>true</c>, if structure was destroyed, <c>false</c> otherwise.</returns>
	/// <param name="structure">Structure.</param>
	public bool DestroyStructure(Structure structure) {
		if (_structure == structure) {
			_structure = null;
			return true;
		}
		return false;
	}

}
