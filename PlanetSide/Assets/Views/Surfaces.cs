using UnityEngine;
using System;
using System.Collections;

namespace Views {

	public class Surfaces : Items {

		Views.Levels level;
		//Surfaces's screen positions.
		float x, y, z;

		Action<Views.Surfaces> callBackMethods;

		GameObject gameObject;

		public GameObject GameObject {
			get {
				return gameObject;
			}
		}

		public Surfaces(Views.Levels level, GameObject gameObject){
			this.level = level;
			this.gameObject = gameObject;
		}

		public void SetPosition(float x, float y, float z) {
			this.gameObject.transform.position = new Vector3 (x, y, z);
		}
	}

}