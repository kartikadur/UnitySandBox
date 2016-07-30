using UnityEngine;
using System;
using System.Collections;


namespace Models {
	/// <summary>
	/// Models.Level tracks data related to the level as a whole and for surfaces, buildins, and resources as a set rather than individuals
	/// int height, int width : Dimensions of the level
	/// int ID or string name: level identifier in case there are multiple levels in a game.
	/// []/Dictionary surfaceModels : list of surfaces in the level (create a dictionary?)
	/// []/Dictionary structureModels : list of structures in the level (create a dictionary?)
	/// []/Dictionary resourceModels : list of resources in the level (create a dictionary?)
	/// </summary>

	//This class should essentially behave as a singleton to prevent mutiple copies of the same level or different levels.
	public class Levels {

		int width, height;

		//All these are single dimensional as the respective classes store their level coordinates
		//Also individual elements can be accessed using x*width + y
		Models.Surfaces[] surfaceModels;

		//These can be created on as needed basis
		Models.Structures[] structureModels;
		Models.Resources[] resourceModels;

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

		public Levels(int width = 10, int height = 10) {
			this.width = width;
			this.height = height;

			surfaceModels = new Models.Surfaces[Width * Height];

			for (int x = 0; x < width; x++) {
				for (int y = 0; y < height; y++) {
					surfaceModels [x * Width + y] = new Models.Surfaces (this, x, y);
					surfaceModels [x * Width + y].Terrain = randomizeTerrain ();
				}
			}
		}

		public Models.Surfaces GetSurfaceAt(int x, int y) {
			return surfaceModels [x * width + y];
		}

		private Models.Surfaces.TerrainType randomizeTerrain() {
			return (Models.Surfaces.TerrainType)UnityEngine.Random.Range (0, Enum.GetNames (typeof(Models.Surfaces.TerrainType)).Length);
		}
	}
}