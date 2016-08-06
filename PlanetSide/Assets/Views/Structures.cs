using UnityEngine;
using System.Collections;

namespace Views {
	public class Structures {

		//TODO: For now this may not be needed, in the future this may be needed IDK
		//Views.Surfaces surfaceView;
//		Views.Levels levelView;

		GameObject gameObject;

		public GameObject GameObject {
			get {
				return gameObject;
			}
		}

		public Structures(Views.Levels level, GameObject gameObject) {

//			this.levelView = level;
			this.gameObject = gameObject;
		}

		public void SetPosition(float x, float y, float z = 0) {
			this.gameObject.transform.position = new Vector3 (x, y, z);
		}
	}

}