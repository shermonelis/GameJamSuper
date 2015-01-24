using UnityEngine;
using System.Collections;
using TNet;

public class PlayerInfo : MonoBehaviour
{
	//instance
	public static PlayerInfo instance;

	//parameters
	public int m_Team = -1;
	public string m_PlayerName = "Guest";
	public bool m_Ready = false;


	//
	// Init
	//
	void Awake()
	{
		if(TNManager.isThisMyObject)
		{
			instance = this;
		}else
			enabled = false;
	}

	//
	//
	//
	void Start()
	{
		GameObject.Find("Lobby").GetComponent<Lobby>().AddPlayer(PlayerInfo.instance);
	}


	//
	// Set Team range [0, 1] 
	//
	[RFC] public void SetTeam(int team)
	{
		m_Team = team;
	}

	//
	// Set Name
	//
	[RFC] public void SetName(string pname)
	{
		m_PlayerName = pname;
	}

	//
	// Set ready
	//
	[RFC] public void SetReady(bool ready)
	{
		m_Ready = ready;
	}
}

