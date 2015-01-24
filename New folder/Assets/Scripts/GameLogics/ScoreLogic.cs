using UnityEngine;
using System.Collections;
using TNet;

public class ScoreLogic : TNBehaviour 
{
	void OnTriggerEnter(Collider collider)
	{
		if(collider.CompareTag("Part"))
		{
			if(collider.transform.parent != null)
			{
				if(collider.transform.parent.tag == gameObject.tag)
					AddPoint();
				else
					RemovePoint();
			}
		}
	}

	public void AddPoint()
	{

	}

	public void RemovePoint()
	{

	}
}
