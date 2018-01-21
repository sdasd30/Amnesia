using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public struct Neighbor
{
	public Level level;
	public DoorDir door;
}

public class Level
{
	// Position of this level in the world grid
	private int[] m_pos = new int[2];

	// List of neighbors accessible from this level
	private List<Neighbor> m_neighbors = new List<Neighbor>();

	public int[] Position
	{
		get
		{
			return m_pos;
		}
	}

	public bool Saved { get; set; }

	// Returns a list of neighbors in clockwise order from the left
	public List<Neighbor> OrderedNeighbors
	{
		get
		{
			return m_neighbors.OrderByDescending(x => (int)x.door).ToList();
		}
	}

	// Returns a list of neighbors in random order
	public List<Neighbor> RandomizedNeighbors
	{
		get
		{
			return m_neighbors.OrderBy(x => Random.value).ToList();
		}
	}

	// Adds a level to the list of neighbors
	public void AddNeighbor(Neighbor neighbor)
	{
		m_neighbors.Add(neighbor);
	}

	public Neighbor GetNeighbor(DoorDir door)
	{
		return m_neighbors.Find(x => x.door == door);
	}

	public bool HasNeighbor(DoorDir door)
	{
		return m_neighbors.Any(x => x.door == door);
	}

	// X position of this level in the world grid
	public int X
	{
		get { return m_pos[0]; }
		set { m_pos[0] = value; }
	}

	// Y position of this level in the world grid
	public int Y
	{
		get { return m_pos[1]; }
		set { m_pos[1] = value; }
	}

	// Initialize a level at pos (x, y) in the world grid
	public Level(int x, int y)
	{
		X = x;
		Y = y;
	}
}
