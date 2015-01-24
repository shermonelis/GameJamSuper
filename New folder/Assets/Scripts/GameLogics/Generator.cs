using UnityEngine;
using System.Collections;
using TNet;

public class Generator : TNBehaviour {
	[System.Serializable]
	public class ObjectsToSpawn	{
		public GameObject red;
		public GameObject green;		
		public GameObject blue;
	}
	public TNObject m_NetObject;	
	public ObjectsToSpawn m_objs;
	public int redCubesCount;
	public int greenCubesCount;
	public int blueCubesCount;
	public float m_distance;
	public Vector3[] CubeInstPoints;
	Transform spawner;
	LayerMask layer = 1 << 8;
	void Start () {
		m_NetObject = GetComponent<TNObject>();

		CubeInstPoints = new Vector3[redCubesCount+greenCubesCount+blueCubesCount];
		spawner = transform.FindChild("Spawner");
		//if I am fist then GenerateSpawnVectors
		if(SpawnCharacter.isFirstPlayer)
			GenerateSpawnVectors (redCubesCount+greenCubesCount+blueCubesCount);

		//else get values from the first guy;
		if(SpawnCharacter.isFirstPlayer)
			for(int i = 0; i < CubeInstPoints.Length; i++){
				m_NetObject.Send("GetPublicCubeValues", Target.Others, i, CubeInstPoints[i]);
			}
	}
	public void GenerateSpawnVectors(int count){
		for(int i = 0; i<count; i++){
			float rnd = Random.Range(0, 359);
			float dist = Random.Range(5, m_distance);
			spawner.localPosition = new Vector3(spawner.localPosition.x, spawner.localPosition.y, dist);
			transform.Rotate(0, rnd, 0);
			RaycastHit hit;

			Ray	ray = new Ray(spawner.position, Vector3.down);
			if (Physics.Raycast(ray, out hit, Mathf.Infinity, layer)){
				CubeInstPoints[i] = hit.point;
				/*createPoint.y += 3;
				GameObject newObj = Instantiate(obj, createPoint, Quaternion.identity) as GameObject;*/
			}
		}
	}
	[RFC]
	public void GetPublicCubeValues(int i, Vector3 vect){
			CubeInstPoints[i]= vect;
	}
}
