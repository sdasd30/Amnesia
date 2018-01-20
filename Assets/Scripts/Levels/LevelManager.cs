using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Enum describing door locations
public enum DoorDir { LEFT, UP, RIGHT, DOWN };

public class LevelManager : MonoBehaviour
{
	private static LevelManager m_instance;
	//private static Dictionary<Level,List<SaveDat>> saveData;
	[TooltipAttribute("Number of rooms to generate. (Must be at least 1)")]
	[SerializeField] private int m_numRooms;

	[TooltipAttribute("Door locations clockwise starting from the left.")]
	[SerializeField] private Vector2[] m_doorLocs;

	[TooltipAttribute("Door GameObjects clockwise starting from the left.")]
	[SerializeField] private GameObject[] m_doorObjs;

	// Root of the level tree
	private Level m_root;

	// Current level of the player
	private Level m_curr;

	// List of valid [X, Y] locations in the grid
	private List<int[]> m_grid = new List<int[]>();

	// Array describing the directions of each door in terms of the world grid
	private List<int[]> m_doorDirs = new List<int[]> {
		new int[] { -1,   0 },
		new int[] {  0,   1 },
		new int[] {  1,   0 },
		new int[] {  0,  -1 },
	};

	// Calling convention: EnterDoor(Door.LOCATION);
	public event Action<DoorDir> EnterDoor;

	// Object references to randomized objects:
	public GameObject enemy;
	public GameObject bucket;
	public GameObject fountain;
	public GameObject crate;
	public GameObject money;

	private void Awake()
	{
		m_instance = this;
	}

	private void Start()
	{
		//saveData = new Dictionary<Level,List<SaveDat>> ();
		EnterDoor += OnEnterDoor;
		GenerateLayout(m_numRooms);
		UpdateMinimap();
		UpdateDoors();
		RandomizeRoom ();
	}

	private void UpdateMinimap()
	{
		var q = new Queue<Level>();
		var visited = new List<Level>();
		q.Enqueue(m_curr);
		var uim = UIManager.Get();
		uim.ClearMinimap();

		while (q.Count != 0)
		{
			var curr = q.Dequeue();
			visited.Add(curr);
			var mmPos = UIManager.Get().ClampCoords(m_curr.Position, curr.Position);
			foreach (var neighbor in curr.OrderedNeighbors)
				if (!visited.Contains(neighbor.level))
					q.Enqueue(neighbor.level);
			if (mmPos[0] < 0 || mmPos[0] >= uim.MinimapSize || mmPos[1] < 0 || mmPos[1] >= uim.MinimapSize)
				continue;
			uim.SetMinimapCellColor(mmPos, Enumerable.SequenceEqual(m_curr.Position, curr.Position), curr.Saved);
		}
	}

	private void UpdateDoors()
	{
		foreach (var door in m_doorObjs)
			door.SetActive (m_curr.HasNeighbor(door.GetComponent<Door> ().doorDirection));
	}

	private void RandomizeRoom() {
		SaveObj[] sObjs = FindObjectsOfType<SaveObj> ();
		foreach (SaveObj so in sObjs) {
			Destroy (so.gameObject);
		}
		int numObjs = (int)Math.Round(UnityEngine.Random.Range (1f, 10f));
		for (int i = 0; i < numObjs; i++) {
			float r = UnityEngine.Random.Range (0f, 100f);
			Vector3 spawnPos = new Vector3 (UnityEngine.Random.Range (-3.5f, 4f), UnityEngine.Random.Range (-1.5f, 3f), 0f);
			if (r < 35f) {
				Instantiate (crate, spawnPos, Quaternion.identity);
			} else if (r < 55f) {
				Instantiate (enemy, spawnPos, Quaternion.identity);
			} else if (r < 80f) {
				Instantiate (bucket, spawnPos, Quaternion.identity);
			} else if (r < 95f) {
				Instantiate (money, spawnPos, Quaternion.identity);
			} else {
				Instantiate (fountain, spawnPos, Quaternion.identity);
			}
		}
	}

	private List<int[]> GetRandomDirections()
	{
		return m_doorDirs.OrderBy(x => UnityEngine.Random.value).ToList();
	}

	// Check if a space is available in the world tree
	private bool IsAvailable(Level newLevel)
	{
		return !m_grid.Contains(newLevel.Position);
	}

	// Populates the world with the given number of rooms
	private void GenerateLayout(int numRooms)
	{
		var numGenerated = 1;
		var q = new Queue<Level>();
		var visited = new List<Level>();

		m_root = new Level(0, 0);
		m_curr = m_root;
		q.Enqueue(m_root);

		while (numGenerated < numRooms && q.Count != 0)
		{
			var curr = q.Dequeue();
			var rd = GetRandomDirections();
			var numNewNeighbors = UnityEngine.Random.Range(1, 3);
			var neighborsAdded = 0;
			visited.Add(curr);

			for (int i = 0; i < rd.Count && neighborsAdded < numNewNeighbors; i++)
			{
				var l = new Level(curr.X + rd[i][0], curr.Y + rd[i][1]);
				var nn = new Neighbor
				{
					level = l,
					door = (DoorDir) m_doorDirs.IndexOf(rd[i]),
				};
				nn.level.AddNeighbor(new Neighbor {
					level = curr,
					door = (DoorDir) ((m_doorDirs.IndexOf(rd[i]) + 2) % 4)
				});
				if (!IsAvailable(l)) continue;
				neighborsAdded++;
				m_grid.Add(l.Position);
				curr.AddNeighbor(nn);
			}

			foreach (var neighbor in curr.OrderedNeighbors)
				if (!visited.Contains(neighbor.level))
					q.Enqueue(neighbor.level);
			numGenerated += neighborsAdded;
		}
	}

	// Return the LevelManager singleton
	public static LevelManager Get()
	{
		return m_instance;
	}

	public void OnEnterDoor(DoorDir door)
	{
		m_curr = m_curr.GetNeighbor(door).level ?? m_curr;
		//m_curr.RandomizeContents();
		RandomizeRoom();
		UpdateDoors();
	}
	/*
	public void saveRoom() {
		SaveObj[] sObjs = FindObjectsOfType<SaveObj> ();
		List<SaveDat> svd = new List<SaveDat> ();
		foreach (SaveObj so in sObjs) {
			SaveDat sv = new SaveDat ();
			sv.className = so.saveObjectType;
			sv.lastPos = so.transform.transform.position;
			if (so.GetComponent<ContinuousHitbox> ()) {
				if (so.GetComponent<ContinuousHitbox> ().finite) {
					sv.depleted = true; //so.GetComponent<ContinuousHitbox> ().m_amountDamage = 100f;
				}
			}
			svd.Add (sv);
		}
		saveData.Add (m_curr, svd);
	}
	*/
}

public class SaveDat {
	public string className;
	public Vector3 lastPos;
	public bool depleted;
}