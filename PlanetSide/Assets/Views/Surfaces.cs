using UnityEngine;
using System.Collections;

namespace Views {

	public class Surfaces : Items {

		Views.Levels level;
		//Surfaces's screen positions.
		int x, y, z;

		GameObject gameObject;

		public GameObject GameObject {
			get {
				return gameObject;
			}
		}

		public Surfaces(Views.Levels level, int x, int y, GameObject gameObject){
			this.level = level;
			this.x = x;
			this.y = y;
			this.gameObject = gameObject;
		}

	}

}