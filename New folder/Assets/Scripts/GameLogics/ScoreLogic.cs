using UnityEngine;
using System.Collections;
using TNet;

public class ScoreLogic : TNBehaviour 
{
	public Obelisk m_Red;
	public Obelisk m_Blue;


	void Start()
	{
		m_Blue = GameObject.Find("Obelisk_Blue").GetComponent<Obelisk>();
		m_Red = GameObject.Find("Obelisk_Red").GetComponent<Obelisk>();
	}


	void OnGUI()
	{
		GUI.Box(new Rect(0,0,400,50), "Red team : " + m_Red.m_TaskNumber + " Blue team : " + m_Blue.m_TaskNumber);
	}
}
