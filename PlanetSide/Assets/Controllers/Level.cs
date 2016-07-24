using UnityEngine;
using System.Collections;

namespace Controllers {
	public class Level : MonoBehaviour {

		//Parameters
		Models.Level level;
		public Sprite Plain;
		public Sprite Forest;

		// Use this for initialization
		void Start () {
			level = new Models.Level ();
			level.RandomizeSurfaces ();

			for (int x = 0; x < level.Width; x++) {
				for (int y = 0; y < level.Height; y++) {
					GameObject surfaceGO = new GameObject ();
					surfaceGO.name = "Plain-" + x + "-" + y;
					//Position tiles
					surfaceGO.transform.position = new Vector3 (x, y, 0);

					//Create Sprite Renderer element
					SpriteRenderer surfaceSprite = surfaceGO.AddComponent<SpriteRenderer> ();


					//Set the terrain type based on the cheap way
					if (level.GetSurfaceAt (x, y).Terrain == Models.Surface.TerrainType.Forest) {
						surfaceSprite.sprite = Forest;
					}
					if (level.GetSurfaceAt (x, y).Terrain == Models.Surface.TerrainType.Plain) {
						surfaceSprite.sprite = Plain;
					}

				}
			}
		}
	
		// Update is called once per frame
		void Update () {
	
		}
	}

}