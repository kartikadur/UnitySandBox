﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class World : MonoBehaviour {

	public class ConvertUnits{

		Vector3 _offset;

		public ConvertUnits(Vector3 offset) {
			_offset = offset;
		}

		public Vector3 GetOffset() {
			return _offset;
		}

		/// <summary>
		/// Converts the cartesian coordinates to isometric coordinates.
		/// </summary>
		/// <returns>The isometric coordinates.</returns>
		/// <param name="coordinates">Coordinates.</param>
		public Vector3 fromCartesianToIsometricCoordinates(Vector3 coordinates) {
			float xPos = (coordinates.x - coordinates.y) * _offset.x;
			float yPos = (coordinates.x + coordinates.y - coordinates.z) * _offset.y;
			float zPos = 0.0f;

			return new Vector3 (xPos, yPos, zPos);
		}

		/// <summary>
		/// Converts the isometric coordinates to cartesian coordinates.
		/// </summary>
		/// <returns>The cartesian coordinates.</returns>
		/// <param name="coordinates">Coordinates.</param>
		public Vector3 fromIsometricToCartesianCoordinates(Vector3 coordinates) {
			float xPos = Mathf.CeilToInt (coordinates.y / _offset.y + coordinates.x / _offset.x) / 2;
			float yPos = Mathf.CeilToInt (coordinates.y / _offset.y - coordinates.x / _offset.x) / 2;
			float zPos = 0.0f;

			return new Vector3 (xPos, yPos, zPos);
		}
	}

	public GameObject[] sprites;

	public static World instance = null;
	public ConvertUnits convert;

	int _level;		// indicates what level the user is playing

	int _length;		// x - direction
	int _breadth;	// y - direction
	int _sortingOrderMax;

	Dictionary<string, GameObject> _prototypeGameObjectMap;

	Tile[] tiles;

	void Awake() {
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}
		DontDestroyOnLoad (gameObject);

		convert = new ConvertUnits (new Vector3 (0.5f, 0.25f, 0f));
	}

	void Start() {

		//FIXME: temporary way to start game
		CreateWorld (1, 10, 10);
		Camera.main.transform.position = new Vector3 (0f, _breadth * convert.GetOffset().y, -10f);
	}

	public void CreateWorld(int level, int length, int breadth) {

		_level = level;

		_length = length;
		_breadth = breadth;
		_sortingOrderMax = Mathf.CeilToInt (Mathf.Sqrt (_length * _length + _breadth * _breadth));

		//Create Surface Prototypes
		CreateSurfacePrototypes ();

		//Create Surface Tiles
		tiles = new Tile[_length * _breadth];
		CreateTiles ();
	}

	/// <summary>
	/// Gets the tile at coordinate.
	/// </summary>
	/// <returns>The <see cref="Tile"/>.</returns>
	/// <param name="coordinate">Coordinate.</param>
	public Tile GetTileAt(Vector3 coordinate) {
		if (coordinate.x < 0 || coordinate.x > _length - 1 || coordinate.y < 0 || coordinate.y > _breadth - 1) {
			return null;
		} else {
			return tiles [(int)coordinate.x * _length + (int)coordinate.y];
		}
	}

	private void CreateSurfacePrototypes() {
		//create dictionary
		_prototypeGameObjectMap = new Dictionary<string, GameObject>();

		foreach (GameObject sprite in sprites) {
			Debug.Log ("Create Surface Prototypes : " + sprite.name);
			_prototypeGameObjectMap.Add(sprite.name, sprite);
		}
	}

	private void CreateTiles() {
		for (int x = 0; x < _length; x++) {
			for (int y = 0; y < _breadth; y++) {
				GameObject toInstantiate = _prototypeGameObjectMap ["Grass_0"]; 
				GameObject gameObject = (GameObject)Instantiate (toInstantiate,
					                        convert.fromCartesianToIsometricCoordinates (new Vector3 (x, y, 0.0f)),
					                        Quaternion.identity
				                        );

				gameObject.name = "Tile_" + x + "_" + y;
				gameObject.transform.SetParent (instance.transform, true);
				gameObject.GetComponent<SpriteRenderer> ().sortingOrder = _sortingOrderMax - Mathf.CeilToInt (Mathf.Sqrt (x * x + y * y));

				Tile tile = new Tile (this, gameObject , x, y);
				tiles [x * _length + y] = tile;
//				break;
			}
//			break;
		}
	}

	private void CreateStructures() {

	}

}
