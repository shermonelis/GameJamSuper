using UnityEngine;
using System.Collections;
using System.Net;
using TNet;

public class SpawnCharacter : MonoBehaviour
{
	public GameObject spots;
	public bool m_First = true;

	public void CallCreatePlayer()
	{
		TNManager.Create(GameObject.Find("Network").GetComponent<TNManager>().objects[1], false);
	}

	public void OnLevelWasLoaded()
	{

		if(TNManager.isConnected)
		{
			
			GameObject g = (GameObject)Instantiate(spots);
			g.name = spots.name;
			if(!m_First)
				CallCreatePlayer();
		}

		if(m_First)
		{
			m_First = false;
		}


	}
}
