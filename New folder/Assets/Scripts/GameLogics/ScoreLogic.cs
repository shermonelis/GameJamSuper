using UnityEngine;
using System.Collections;
using TNet;

public class ScoreLogic : TNBehaviour 
{
	public Obelisk m_Red;
	public Obelisk m_Blue;

	public GUISkin m_Skin;

	void Start()
	{
		m_Blue = GameObject.Find("Obelisk_Blue").GetComponent<Obelisk>();
		m_Red = GameObject.Find("Obelisk_Red").GetComponent<Obelisk>();
	}


	void OnGUI()
	{
		GUI.skin = m_Skin;

		GUI.Box(new Rect(Screen.width/2 - 400, 0, 800, 108), "", "Top");

		GUI.color = Color.yellow;

		GUI.Label(new Rect(Screen.width/2 - 50, 15, 100, 50), m_Red.m_TaskNumber + "-" + m_Blue.m_TaskNumber);

		
		if(m_Blue.m_TaskNumber >= 4)
			GUI.Box(new Rect(Screen.width/2 - 640, Screen.height/2 - 360, 1280, 720), "", "BlueWins");

		if(m_Red.m_TaskNumber >= 4)
			GUI.Box(new Rect(Screen.width/2 - 640, Screen.height/2 - 360, 1280, 720), "", "RedWins");
	}
}
