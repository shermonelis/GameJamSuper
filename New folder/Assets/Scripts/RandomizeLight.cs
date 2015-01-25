using UnityEngine;
using System.Collections;

public class RandomizeLight : MonoBehaviour {


	Vector3 target = Vector3.zero;
	float intencity = 4.5f;

	void Start()
	{
		StartCoroutine(Cycle());
	}

	// Update is called once per frame
	void Update () 
	{
		GetComponent<Light>().intensity = Mathf.MoveTowards(GetComponent<Light>().intensity, intencity, Time.deltaTime); 
		transform.localPosition = Vector3.MoveTowards(transform.localPosition, new Vector3(target.x, transform.localPosition.y, target.z), Time.deltaTime); 
	}

	IEnumerator Cycle()
	{
		while(true)
		{
			intencity = Random.Range(2, 5.5f);
			target = Random.insideUnitSphere * 2;
			yield return new WaitForSeconds(0.7f);
		}
	}
}
