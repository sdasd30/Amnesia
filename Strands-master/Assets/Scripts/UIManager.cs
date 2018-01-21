using UnityEngine;
using UnityEngine.UI;
using System;

public class UIManager : MonoBehaviour
{
	private static UIManager m_instance;

	[Header("Text")]
	[SerializeField] private Text m_health;
	[SerializeField] private Text m_money;
	[SerializeField] private Text m_powerUp;

	[Header("Game Objects")]
	[SerializeField] private GameObject m_player;
	[SerializeField] private GameObject m_minimap;
	[SerializeField] private GameObject m_minimapCell;

	[Header("Constant Data")]
	[Tooltip("Total number of cells in the minimap.")]
	[SerializeField] private int m_numMinimapCells;
	[Tooltip("Length and width of the square minmap.")]
	[SerializeField] private int m_minimapSize;
	[Tooltip("Coordinates [x, y] from bottom left indicating the player's current cell's minimap location.")]
	[SerializeField] private int[] m_minimapCenter;
	[Tooltip("Color of the player's current cell.")]
	[SerializeField] private Color m_playerCellColor;
	[Tooltip("Default color of a minimap cell.")]
	[SerializeField] private Color m_cellColor;
	[Tooltip("Color of a saved minimap cell.")]
	[SerializeField] private Color m_savedCellColor;

	private GameObject[,] m_minimapCells;

	public int MinimapSize
	{
		get
		{
			return m_minimapSize;
		}
	}

	private void Awake()
	{
		m_instance = this;
		// Populate minimap with cells
		m_minimapCells = new GameObject[4, 4];
		for (int i = 0; i < m_numMinimapCells; i++)
		{
			var mc = Instantiate(m_minimapCell);
			mc.transform.SetParent(m_minimap.transform);
			mc.transform.localScale = new Vector3(1, 1, 1);
			m_minimapCells[i / m_minimapSize, i % m_minimapSize] = mc;
		}
	}

	private void Update()
	{
		var a = m_player.GetComponent<Attackable>();
		var fc = m_player.GetComponent<FireController>();
		m_health.text = string.Format(Strings.HealthString, a.health, a.maxHealth);
		m_money.text = string.Format(Strings.MoneyString, a.money, a.money);
		m_powerUp.text = string.Format(Strings.PowerUpString, fc.powerUpTime, fc.maxPowerUpTime);
		m_powerUp.text = string.Format(Strings.PagesString, a.pagesUsed, a.maxPages);
	}

	// Returns the UIManager singleton
	public static UIManager Get()
	{
		return m_instance;
	}

	// Clamp coords to minimap space based on player's current location
	public int[] ClampCoords(int[] curr, int[] coords)
	{
		return new int[] {
			curr[1] + m_minimapCenter[0] - coords[1],
			coords[0] - curr[0] + m_minimapCenter[1]
		};
	}

	public void ClearMinimap()
	{
		foreach (var mc in m_minimapCells)
			mc.GetComponent<Image>().color = Color.clear;
	}

	public void SetMinimapCellColor(int[] pos, bool isCurrent, bool isSaved)
	{
		var newColor = isCurrent ? m_playerCellColor : isSaved ? m_savedCellColor : m_cellColor;
		m_minimapCells[pos[0], pos[1]].GetComponent<Image>().color = newColor;
	}
}
