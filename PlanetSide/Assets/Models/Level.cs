﻿using UnityEngine;
using System.Collections;

namespace Models {

	public class Level {

		//Variables
		Surface[,] surfaces;
		int width;
		int height;

		//Accessors
		public int Width {
			get {
				return width;
			}
		}

		public int Height {
			get {
				return height;
			}
		}

		//Create empty surface tiles for the level.
		//Default width and height to set up to 100 thus allowing for 10000 surface tiles
		public Level (int width = 20, int height = 20) {
			this.width = width;
			this.height = height;

			//2D array of surface tiles in the level
			surfaces = new Surface[width, height];

			for (int x = 0; x < width; x++) {
				for (int y = 0; y < height; y++) {
					surfaces [x, y] = new Surface (this, x, y);
				}
			}

			Debug.Log ("World created");
		}

		public Surface GetSurfaceAt (int x, int y) {
			if (x > width || x < 0 || y > height || y < 0) {
				Debug.LogError ("Surface (" + x + ", " + y + ") is out of range");
				return null;
			}
			return surfaces [x, y];
		}

		public void RandomizeSurfaces() {
			for (int x = 0; x < width; x++) {
				for (int y = 0; y < height; y++) {
					int randTerrain = Random.Range (0, 2);
					if (randTerrain == 0) {
						surfaces [x, y].Terrain = Surface.TerrainType.Plain;
					} else if (randTerrain == 1) {
						surfaces [x, y].Terrain = Surface.TerrainType.Forest;
					} else {
						surfaces [x, y].Terrain = Surface.TerrainType.Mountain;
					}
				}
			}
		}
	}

}