using UnityEngine;
using System.Collections;

public class Tile {

	World _world;

	Surface _surface;

	int _x;
	int _y;
	int _z;

	public Tile(World world, Surface surface, int x, int y, int z =0) {
		_world = world;
		_surface = surface;

		_x = x;
		_y = y;
		_z = z;
	}

	public World getWorld() {
		return _world;
	}

	public Surface getSurface() {
		return _surface;
	}

	public int getX() {
		return _x;
	}

	public int getY() {
		return _y;
	}

	public int getZ() {
		return _z;
	}
}
