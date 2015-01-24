using UnityEngine;
using System.Collections;

public class Generator : MonoBehaviour {
	[System.Serializable]
	public class ObjectsToSpawn	{
		public GameObject red;
		public GameObject green;		
		public GameObject blue;
	}
	public ObjectsToSpawn m_objs;
	public int redCubesCount;
	public int greenCubesCount;
	public int blueCubesCount;

	public float m_distance;

	LayerMask layer = 1 << 8;
	void Start () {
		SpawnObjs (m_objs.red, redCubesCount);
	}
	private void SpawnObjs(GameObject obj, int count){
		for(int i = 0; i<count; i++){
			float rnd = Random.Range(0, 359);
			float dist = Random.Range(5, m_distance);
			transform.Rotate(0, rnd, 0);
			RaycastHit hit;
			Ray ray = new Ray(Vector3.forward * dist, Vector3.down);
			if (Physics.Raycast(ray, out hit, Mathf.Infinity, layer)){
				Vector3 createPoint = hit.point;
				createPoint.y += 3;
				GameObject newObj = Instantiate(obj, createPoint, Quaternion.identity) as GameObject;
			}
		}
	}
}
