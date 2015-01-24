using UnityEngine;
using System.Collections;
using System.Net;
using TNet;

public class SpawnCharacter : MonoBehaviour
{
	public void CallCreatePlayer()
	{
		TNManager.Create(GetComponent<TNManager>().objects[1], false);
	}


}
