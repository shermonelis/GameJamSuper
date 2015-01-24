using UnityEngine;
using System.Collections;
using System.Net;
using TNet;

public class SpawnCharacter : MonoBehaviour
{
	public static bool isFirstPlayer; //seting master player

	public void CallCreatePlayer()
	{
		TNManager.Create(GetComponent<TNManager>().objects[1], false);
	}


}
