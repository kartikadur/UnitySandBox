using UnityEngine;
using System.Collections;

public class Mouse : MonoBehaviour {

	public GameObject cursorIndicator;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		Vector3 currPosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		currPosition.z = 0;
		//Update the cursorIndicator position
		cursorIndicator.transform.position = currPosition;
		
	}
}
