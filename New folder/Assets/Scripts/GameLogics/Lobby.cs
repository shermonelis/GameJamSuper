using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Lobby : MonoBehaviour
{
	public List<PlayerInfo> m_Players = new List<PlayerInfo>();

	public void AddPlayer(PlayerInfo p)
	{
		m_Players.Add(p);
	}

	void OnGUI()
	{
		for(var p = 0; p < m_Players.Count; p++)
		{
			GUI.Box(new Rect(0, (30 * p), 200, 30), m_Players[p].m_PlayerName);
		}
	}
}

