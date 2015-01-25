using UnityEngine;
using System.Collections;
using TNet;

public class PlayerInfo : TNBehaviour
{
	//instance
	public static PlayerInfo instance;

	//parameters
	public int m_Team = -1;
	public string m_PlayerName = "Guest";
	public bool m_Ready = false;
	public bool m_FirstPlayer = false;



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

	void Start()
	{

	}

	public bool isReady()
	{
		return m_Ready;
	}

	//
	// Set Team range [0, 1] 
	//
	public void CallSetTeam(int team) { tno.Send("SetTeam", Target.AllSaved, team); }
	[RFC] public void SetTeam(int team)
	{
		m_Team = team;
	}

	//
	// Set Name
	//
	public void CallSetName(string pname) { tno.Send("SetName", Target.AllSaved, pname); }
	[RFC] public void SetName(string pname)
	{
		m_PlayerName = pname;
	}

	//
	// Set ready
	//
	public void CallSetReady(bool ready) { tno.Send("SetReady", Target.AllSaved, ready); }
	[RFC] public void SetReady(bool ready)
	{
		m_Ready = ready;
	}

	//
	// Set first player
	//
	public void CallSetFirstPlayer(bool first) { tno.Send("SetFirstPlayer", Target.AllSaved, first); }
	[RFC] public void SetFirstPlayer(bool first)
	{
		m_FirstPlayer = first;
	}
}