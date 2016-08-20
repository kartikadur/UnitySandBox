using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class World : MonoBehaviour {

	public GameObject[] sprites;

	int _level;		// indicates what level the user is playing

	int _length;		// x - direction
	int _breadth;	// y - direction

	Dictionary<string, Surface> surfacePrototypesMap;

	Tile[] tiles;

	void Start() {
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
		surfacePrototypesMap = new Dictionary<string, Surface> ();

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
			Surface surface = new Surface (sprite.name, movementCost, isWater, sprite);
			surfacePrototypesMap.Add (sprite.name, surface);
		}
	}

	private void CreateTiles() {
		for (int x = 0; x < _length; x++) {
			for (int y = 0; y < _breadth; y++) {
				
				Tile tile = new Tile (this, surfacePrototypesMap ["Grass_0"], x, y);

				tiles [x * _length + y] = tile;
				Instantiate (tile.getSurface ().getSprite (), 
					new Vector3 ((x - y) * 1f, (x + y) * 0.5f, 0f), 
					Quaternion.identity);
			}
		}
	}

}
