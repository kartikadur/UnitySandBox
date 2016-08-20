using UnityEngine;
using System.Collections;

public class Build : MonoBehaviour {

	Controllers.Mouse mouseController;
	Models.Levels levelModel;
	Models.Surfaces surfaceModel;
	Models.Structures structureModel;

	// Use this for initialization
	void Start () {

		mouseController = GameObject.FindObjectOfType<Controllers.Mouse> ();
		levelModel = Models.Levels.Instance;
	}


	/* surface terrain modifiers */
	/// <description>
	/// set the parameters that will allow the mouse control to change the terrain
	/// </description>
	public void SetPlain() {
		mouseController.isBuildModeActive = false;
		mouseController.terrainToApply = Models.Surfaces.TerrainType.Plain;
	}

	public void SetMountain() {
		mouseController.isBuildModeActive = false;
		mouseController.terrainToApply = Models.Surfaces.TerrainType.Mountain;
	}

	public void SetLake() {
		mouseController.isBuildModeActive = false;
		mouseController.terrainToApply = Models.Surfaces.TerrainType.Lake;
	}

	/* structure generation methods */
	/// <description>
	/// Build calls for establishing structures on surface
	/// Build[StructureType] will allow the user to create a structure of BuildingType on the surface
	/// </description>
	public void BuildWall() {
		mouseController.isBuildModeActive = true;
		mouseController.structureToBuild = Models.Structures.StructureType.Wall;
	}

	//Function call to set road building mode
	public void BuildRoad() {
		mouseController.isBuildModeActive = true;
		mouseController.structureToBuild = Models.Structures.StructureType.Road;
	}

	//Function call to set house building mode
	public void BuildHouse() {
		mouseController.isBuildModeActive = true;
		mouseController.structureToBuild = Models.Structures.StructureType.House;
	}
}
