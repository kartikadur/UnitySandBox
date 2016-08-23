using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class World : MonoBehaviour {

	public class ConvertUnits{

		Vector3 _offset;

		public ConvertUnits() {
			_offset = new Vector3 (0.5f, 0.25f, 0f);
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
	public int _sortingOrderMax;

	Dictionary<string, GameObject> _prototypeGameObjectMap;

	Tile[] tiles;

	void Awake() {
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}
		DontDestroyOnLoad (gameObject);

		convert = new ConvertUnits ();
	}

	void Start() {

		//FIXME: temporary way to start game
		CreateWorld (1, 10, 10);
		Camera.main.transform.position = new Vector3 (0f, _breadth * convert.GetOffset().y, -10f);
	}

	/// <summary>
	/// Gets the length.
	/// </summary>
	/// <returns>The length.</returns>
	public int GetLength() {
		return _length;
	}
	/// <summary>
	/// Gets the breadth.
	/// </summary>
	/// <returns>The breadth.</returns>
	public int GetBreadth() {
		return _breadth;
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

	/// <summary>
	/// Determines whether this instance can place on tile at the specified coordinate.
	/// </summary>
	/// <returns><c>true</c> if this instance can place on tile at the specified coordinate; otherwise, <c>false</c>.</returns>
	/// <param name="coordinate">Coordinate.</param>
	public bool CanPlaceOnTileAt(Vector3 coordinate, Vector3 dimesion) {
		int xPos = (int)coordinate.x;
		int yPos = (int)coordinate.y;
		int xLen = (int)dimesion.x;
		int yLen = (int)dimesion.y;

		//TODO: there is a problem if the user hovers over last tile row or column
		// To account for that the mouse positions will have to change somehow.
		if (xPos > _length - xLen || yPos > _breadth - yLen) {
			return false;
		}

		//TODO: fix for edge cases where coodinate is one less than boundary - structure dimension

		for (int x = xPos; x < xPos + xLen; x++) {
			for (int y = yPos; y < yPos + yLen; y++) {
				Tile tile = GetTileAt (new Vector3 (x, y, 0f));
				if (tile.CanBuildHere () == false) {
					return false;
				}
			}
		}

		return true;
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

				Tile tile = new Tile (this, gameObject, true, x, y);
				tiles [x * _length + y] = tile;
//				break;
			}
//			break;
		}
	}

}
