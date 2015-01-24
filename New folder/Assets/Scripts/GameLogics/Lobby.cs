using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TNet;

public class Lobby : TNBehaviour
{
	public System.Collections.Generic.List<string> m_Players = new System.Collections.Generic.List<string>();

	public void AddPlayer(string p)
	{
		tno.Send("NetworkAddPlayer", Target.All, p);
	}

	[RFC] public void NetworkAddPlayer(string pname)
	{
		m_Players.Add(pname);
	}

	void OnGUI()
	{
		for(var p = 0; p < m_Players.Count; p++)
		{
			GUI.Box(new Rect(0, (30 * p), 200, 30), m_Players[p]);
		}



	}
}

