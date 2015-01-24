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
				AddPoint(col.gameObject);
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
				RemovePoint();
		}
	}

	private void RemovePoint()
	{
		m_TaskNumber--;
	}

	private void AddPoint(GameObject g)
	{
		g.transform.position = m_Transforms[m_TaskNumber].position;
		g.transform.parent = m_Transforms[m_TaskNumber];
		m_TaskNumber++;
	}
}
