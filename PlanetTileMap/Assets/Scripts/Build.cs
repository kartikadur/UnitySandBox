using UnityEngine;
using System.Collections;

public class Build : MonoBehaviour {

	bool isStructureModeActive = false;

	World world;
	Mouse mouse;

	// Use this for initialization
	void Start () {
		world = World.instance;
		mouse = GameObject.FindObjectOfType<Mouse> ();
	}
	
	// Update is called once per frame
	void Update () {

		if (isStructureModeActive == true) {
			//building mode
			//do something
		}
		// else 
		// do nothing
	
	}

	/* --- Build Structures --- */
	//When isStructureModeActive is active user can build or destroy buildings
	public void SetBuildMode() {
		isStructureModeActive = true;
	}

	public void ResetBuildMode() {
		isStructureModeActive = false;
	}

	public bool GetBuildMode() {
		return isStructureModeActive;
	}

	public void BuildStructure(Structure structure, Tile tile) {

	}

	/* --- Destroy Structures? --- */
}
