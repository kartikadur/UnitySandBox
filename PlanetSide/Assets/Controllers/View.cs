using UnityEngine;
using System.Collections;

public class View : MonoBehaviour {

	int mDelta = 10;
	float mSpeed = 10.0f;

	private Vector3 moveRight = Vector3.right;
	private Vector3 moveLeft = Vector3.left;
	private Vector3 moveUp = Vector3.up;
	private Vector3 moveDown = Vector3.down;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		//Basic camera movement for now
		//TODO: use camera.main.screenToWorldPoint to calculate screen movement
		if (Input.mousePosition.x >= Screen.width - mDelta) {
			transform.position += moveRight * Time.deltaTime * mSpeed;
		}
		if (Input.mousePosition.x <= mDelta) {
			transform.position += moveLeft * Time.deltaTime * mSpeed;
		}
		if (Input.mousePosition.y >= Screen.height - mDelta) {
			transform.position += moveUp * Time.deltaTime * mSpeed;
		}
		if (Input.mousePosition.y <= mDelta) {
			transform.position += moveDown * Time.deltaTime * mSpeed;
		}
	
	}
}
