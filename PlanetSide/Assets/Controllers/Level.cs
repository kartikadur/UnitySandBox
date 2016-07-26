using UnityEngine;
using System.Collections;

namespace Controllers {
	public class Level : MonoBehaviour {

		//Parameters
		Models.Level level;
		public Sprite Plain;
		public Sprite Forest;
		public Sprite Lake;
		float yOffset = 0.25f;
		float xOffset = 0.5f;

		// Use this for initialization
		void Start () {
			level = new Models.Level ();

			for (int x = level.Width - 1; x >= 0; x--) {
				for (int y = 0; y < level.Height; y++) {
					
					Models.Surface surfaceModel = level.GetSurfaceAt (x, y);
					GameObject surfaceGO = new GameObject ();
					surfaceGO.name = "Surface-" + x + "-" + y;
					//Position tiles
					surfaceGO.transform.position = new Vector3 ((x + y) * xOffset, (y - x) * yOffset, 0);
					surfaceGO.transform.SetParent (this.transform, true);
					//Create Sprite Renderer element
					SpriteRenderer surfaceSprite = surfaceGO.AddComponent<SpriteRenderer> ();

					surfaceModel.RegisterSurfaceCallback ((surface) => {
						OnSurfaceChanged(surface, surfaceGO);
					});

				}
			}

			level.RandomizeSurfaces ();
		}
	
		// Update is called once per frame
		void Update () {
	
		}

		void OnSurfaceChanged(Models.Surface surface, GameObject surfaceGO) {
			//Set the terrain type based on the cheap way
			if (level.GetSurfaceAt (surface.X, surface.Y).Terrain == Models.Surface.TerrainType.Forest) {
				surfaceGO.GetComponent<SpriteRenderer> ().sprite = Forest;
			} else if (level.GetSurfaceAt (surface.X, surface.Y).Terrain == Models.Surface.TerrainType.Plain) {
				surfaceGO.GetComponent<SpriteRenderer> ().sprite = Plain;
			} else if (level.GetSurfaceAt (surface.X, surface.Y).Terrain == Models.Surface.TerrainType.Lake) {
				surfaceGO.GetComponent<SpriteRenderer> ().sprite = Lake;
			} else{
				Debug.LogError ("OnSurfaceChanged - Unrecognized Surface type");
			}

		}
			
	}
}