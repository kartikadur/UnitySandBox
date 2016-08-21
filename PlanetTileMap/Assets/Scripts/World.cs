using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class World : MonoBehaviour {

	public GameObject[] sprites;

	public static World instance = null;

	int _level;		// indicates what level the user is playing

	int _length;		// x - direction
	int _breadth;	// y - direction

	Dictionary<string, GameObject> _prototypeGameObjectMap;

	Tile[] tiles;

	void Awake() {
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}
		DontDestroyOnLoad (gameObject);
	}

	void Start() {

		//FIXME: temporary way to start game
		CreateWorld (1, 250, 250);
	}

	public void CreateWorld(int level, int length, int breadth) {

		_level = level;

		_length = length;
		_breadth = breadth;

		//Create Surface Prototypes
		CreateSurfacePrototypes ();

		//Create Surface Tiles
		tiles = new Tile[_length * _breadth];
		CreateTiles ();
	}

	private void CreateSurfacePrototypes() {
		//create dictionary
		_prototypeGameObjectMap = new Dictionary<string, GameObject>();

		foreach (GameObject sprite in sprites) {
			bool isWater = false;
			int movementCost = 1;
			if (sprite.name.Contains ("lake")) {
				isWater = true;
				movementCost = 0;
			} else if (sprite.name.Contains ("mountain")) {
				movementCost = 0;
			}
			Debug.Log ("Create Surface Prototypes : " + sprite.name);
			_prototypeGameObjectMap.Add(sprite.name, sprite);
		}
	}

	private void CreateTiles() {
		for (int x = 0; x < _length; x++) {
			for (int y = 0; y < _breadth; y++) {
				GameObject toInstantiate = _prototypeGameObjectMap ["Grass_0"]; 
				GameObject gameObject = (GameObject)Instantiate (toInstantiate,
					                        new Vector3 ((x - y) * 1f, (x + y) * 0.5f, 0f),
					                        Quaternion.identity
				                        );

				gameObject.name = "Tile_" + x + "_" + y;
				gameObject.transform.SetParent (instance.transform, true);
				gameObject.GetComponent<SpriteRenderer> ().sortingOrder = x * _length + y;

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
