using UnityEngine;
using System.Collections;
using TNet;

public class SpawnController : MonoBehaviour
{
	public void Start()
	{
		TNManager.Create(GameObject.Find("Network").GetComponent<TNManager>().objects[0], false);
	}
}

