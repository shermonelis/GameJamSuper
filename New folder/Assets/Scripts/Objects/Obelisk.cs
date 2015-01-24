using UnityEngine;
using System.Collections;
using TNet;
using System.Linq;

public class Obelisk : TNBehaviour {

	public Color[] m_Colors = new Color[4];
	public Transform[] m_Transforms = new Transform[4];
	public int m_TaskNumber = 0;

	void Start()
	{
		m_Transforms = gameObject.transform.Find("Tasks").Cast<Transform>().ToArray();
		for(int i = 0; i < m_Transforms.Length; i++)
		{
			m_Transforms[i].renderer.material.color = m_Colors[i];
		}
	}


	void OnTriggerEnter(Collider col)
	{
		if(col.CompareTag("Part"))
		{
			if(col.renderer.material.color == m_Colors[m_TaskNumber])
			{
				Debug.Log("enter fit");
				col.rigidbody.isKinematic = true;
				col.transform.position = m_Transforms[m_TaskNumber].position;
				col.transform.parent = m_Transforms[m_TaskNumber];
				m_TaskNumber++;
				tno.Send("AddPoint", Target.Others);
			}
		}
	}

	void OnTriggerExit(Collider col)
	{
		if(col.CompareTag("Part"))
		{
			int index = 0;
			for(int i = 0; i < m_Colors.Length; i++)
			{
				if(col.renderer.material.color == m_Colors[i])
				{
					index = i;
				}
			}
			if(index < m_TaskNumber)
			{
				m_TaskNumber--;
				tno.Send("RemovePoint", Target.Others);
			}
		}
	}

	[RFC] private void RemovePoint()
	{
		Debug.Log("call");
		m_TaskNumber--;
	}

	[RFC] private void AddPoint()
	{
		m_TaskNumber++;
	}
}
