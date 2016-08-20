using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Surface : MonoBehaviour {

	string _name;

	int _movementCost;

	bool _isWater;

	GameObject _sprite;

	public Surface (string name,
		int movementCost,
		bool isWater,
		GameObject sprite) {

		_name = name;

		_movementCost = movementCost;
		_isWater = isWater;

		_sprite = sprite;
	}

	public int getMovementCost() {
		return _movementCost;
	}

	public bool isWater() {
		return _isWater;
	}

	public string getName() {
		return _name;
	}

	public GameObject getSprite() {
		return _sprite;
	}
}
