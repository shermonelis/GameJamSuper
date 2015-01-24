using UnityEngine;
using System.Collections;
using TNet;

public class SpawnCharacter : MonoBehaviour
{
	//
	//
	//
	public void CallCreatePlayer()
	{
		TNManager.Create(GetComponent<TNManager>().objects[1], false);
	}
}
