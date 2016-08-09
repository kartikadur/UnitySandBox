using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Controllers {

	public class Sprites : MonoBehaviour {


		/// <summary>
		/// Spritetypes: denotes all the different types of sprites available for use on screen
		/// the values in this enum will be the folder that contains these main types
		/// few examples are as noted below
		/// Surface Sprites:
		/// 	Plain, Lake, Mountain, Desert, Ocean, etc.
		/// 
		/// Building Sprites:
		/// 	Road, Walls, resource building types, settlement building types, storage building types, and creator building types
		/// 
		/// Resource Sprites:
		/// 	Humans and resources will be available here
		/// </summary>
		protected enum Spritetypes {};

		/// <summary>
		/// Sprite directions.
		/// This enum has values specific for the surface sprites that can form larger structures such as multiple mountain sprites can create a seamless mountain range
		/// this will incorporate sprites that show on which sides the sprite can connect with similar neighbors
		/// </summary>
		protected enum SpriteDirections {};

		//Create Callbacks that automatically add a sprite to the gameobject
		//when called from within the views.level file (to be named level controller later?)
		Dictionary<string, Sprite> namedSpriteMap;

		// Use this for initialization
		void Start () {

			//Load all the sprites. 
			namedSpriteMap = new Dictionary<string, Sprite> ();

		
		}
		
		// Update is called once per frame
		void Update () {
		
		}


	}
}