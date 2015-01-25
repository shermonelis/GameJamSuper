using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TNet;

public class Lobby : TNBehaviour
{
	public static Lobby instance;


	public GUISkin m_skin;
	private int m_ReadyCount = 0;
	[HideInInspector]
	public int m_PlayerCount = 0;
	void Awake()
	{
		if(TNManager.isThisMyObject)
		{
			//PlayerInfo.instance.CallSetTeam(Random.Range(0,2));
			instance = this;
		}else
			enabled = false;
	}

	void OnGUI()
	{
		GUI.skin = m_skin;

		PlayerInfo[] pinfos = GameObject.FindObjectsOfType<PlayerInfo>();

		if(GUI.Button(new Rect(Screen.width/2 + 155/2f, Screen.height - 107, 281, 107), "", "SelectBlue"))
			SelectTeam(0);
		
		if(GUI.Button(new Rect(Screen.width/2 - 155/2f - 268, Screen.height - 114, 268, 114), "", "SelectRed"))
			SelectTeam(1);

		if(GUI.Button(new Rect(Screen.width/2 - 155/2f, Screen.height - 105, 155, 105), "", "Ready"))
			SetReady();

		GUI.Box(new Rect(Screen.width/2 - 617/2f, 100, 617, 246), "", "Players");

		m_ReadyCount = 0;
		int blueready = 0;
		int redready = 0;
		for(var p = 0; p < pinfos.Length; p++)
		{
//			switch(pinfos[p].m_Team)
//			{
//			case -1 : GUI.color = Color.white; break;
//			case 0 : GUI.color = Color.blue; break;
//			case 1 : GUI.color = Color.red; break;
//			}

//			if(pinfos[p].m_Team == -1)
//				if((pinfos.Length % 2) == 1)
//					pinfos[p].SetTeam(0);
//				else
//					pinfos[p].SetTeam(1);

			if(pinfos[p].m_Team == 0)
			{
				blueready++;
				if(pinfos[p].m_Ready)
				{
					//GUI.Box(new Rect(Screen.width/2 + 300, 100 + (50 * p), 30, 30), "", "BlueReady");
					m_ReadyCount++;

					GUI.color = Color.green;
				}
				else
					GUI.color = Color.white;
				GUI.Label(new Rect(Screen.width/2 + 60, 150 + (50 * (blueready-1)), 200, 30), pinfos[p].m_PlayerName, "Lobby");
			}
			else
			{
				redready++;
				if(pinfos[p].m_Ready)
				{
					//GUI.Box(new Rect(Screen.width/2 -230, 150 + (50 * p), 30, 30), "", "RedReady");
					m_ReadyCount++;

					//GUI.Box(new Rect(200, 150 + (30 * p), 100, 30), "Ready");
					GUI.color = Color.green;
				}
				else
					GUI.color = Color.white;
				GUI.Label(new Rect(Screen.width/2 - 150, 150 + (50 * (redready-1)), 200, 30), pinfos[p].m_PlayerName, "Lobby");
			}

			GUI.color = Color.white;
		}
		m_PlayerCount = pinfos.Length;
	}

	void FixedUpdate()
	{
		if(m_ReadyCount == m_PlayerCount && m_PlayerCount > 0)
		{
			GameObject.Find("Main Camera").SetActive(false);
			GameObject g = GameObject.Find("Network");
			Vector3 position = g.GetComponent<RandomSpawn>().GetRandomPosition(PlayerInfo.instance.m_Team);
			TNManager.Create(g.GetComponent<TNManager>().objects[0], position, Quaternion.identity , false);
			Destroy(this);
			Application.LoadLevelAdditive("Game");			
		}
	}

	public void SelectTeam(int team)
	{
		PlayerInfo.instance.CallSetTeam(team);
	}

	public void SetReady()
	{
		if(PlayerInfo.instance.m_Team != -1)
			PlayerInfo.instance.CallSetReady(!PlayerInfo.instance.isReady());
	}
	#region Helper Functions

	#endregion
}

