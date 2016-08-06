using UnityEngine;
using System;
using System.Collections;

namespace Views {

	public class Surfaces {

		Views.Levels level;

		GameObject gameObject;

		public GameObject GameObject {
			get {
				return gameObject;
			}
		}

		public Surfaces(GameObject gameObject){
//			this.level = level;
			this.gameObject = gameObject;
		}

		public void SetPosition(float x, float y, float z = 0) {
			this.gameObject.transform.position = new Vector3 (x, y, z);
		}
	}

}