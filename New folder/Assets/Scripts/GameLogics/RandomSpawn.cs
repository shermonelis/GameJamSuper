using UnityEngine;
using System.Collections;

public class RandomSpawn : MonoBehaviour {

	public Vector3 GetRandomPosition(int team)
	{
		if(team == 0)
			return GameObject.Find("Spots/B" + Random.Range(1,6)).transform.position;
		else
			return GameObject.Find("Spots/R" + Random.Range(1,6)).transform.position;
	}
}
