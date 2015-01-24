using UnityEngine;
using System.Collections;
using TNet;

public class PlayerInfo : MonoBehaviour
{
	//instance
	public static PlayerInfo instance;

	//parameters
	public int m_Team = -1;
	public string m_Name = "Guest";


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
		m_Name = pname;
	}
}

