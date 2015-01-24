using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TNet;

public class Lobby : TNBehaviour
{
	public static Lobby instance;

	private int m_ReadyCount = 0;
	[HideInInspector]
	public int m_PlayerCount = 0;
	void Awake()
	{
		if(TNManager.isThisMyObject)
		{
			instance = this;
		}else
			enabled = false;
	}
	void Start(){
		setIsPlayerFirst();
	}

	void OnGUI()
	{
		PlayerInfo[] pinfos = GameObject.FindObjectsOfType<PlayerInfo>();

		if(GUI.Button(new Rect(Screen.width/2 - 300, Screen.height/2, 200, 40), "Team Blue"))
			SelectTeam(0);
		
		if(GUI.Button(new Rect(Screen.width/2 + 100, Screen.height/2, 200, 40), "Team Red"))
			SelectTeam(1);

		if(GUI.Button(new Rect(Screen.width/2 - 100, Screen.height/2, 200, 40), "Set Ready"))
			SetReady();

		m_ReadyCount = 0;
		for(var p = 0; p < pinfos.Length; p++)
		{
			switch(pinfos[p].m_Team)
			{
			case -1 : GUI.color = Color.white; break;
			case 0 : GUI.color = Color.blue; break;
			case 1 : GUI.color = Color.red; break;
			}
			GUI.Box(new Rect(0, (30 * p), 200, 30), pinfos[p].m_PlayerName);
			if(pinfos[p].m_Ready)
			{
				GUI.color = Color.white;
				GUI.Box(new Rect(200, (30 * p), 100, 30), "Ready");
				m_ReadyCount++;
			}
		}
		m_PlayerCount = pinfos.Length;
	}

	void FixedUpdate()
	{
		if(m_ReadyCount == m_PlayerCount && m_PlayerCount > 0)
		{
			TNManager.Create(GameObject.Find("Network").GetComponent<TNManager>().objects[0], false);
			Destroy(this);
			Application.LoadLevel("Game");
			
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
	public void setIsPlayerFirst(){
		if(Lobby.instance.m_PlayerCount==1){
			SpawnCharacter.isFirstPlayer = true;
		} else
			SpawnCharacter.isFirstPlayer = false;
	}
	#endregion
}

