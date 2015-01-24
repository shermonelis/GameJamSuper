using UnityEngine;
using System.Collections;
using TNet;
using System.Linq;

public class Obelisk : TNBehaviour {

	public Color[] m_Colors = new Color[4];
	public int m_TaskNumber = 0;

	void Start()
	{
		Transform[] list = gameObject.transform.Find("Tasks").Cast<Transform>().ToArray();
		for(int i = 0; i < list.Length; i++)
		{
			list[i].renderer.material.color = m_Colors[i];
		}
	}


	void OnTriggerEnter(Collider col)
	{
		if(col.CompareTag("Part"))
		{
			if(col.renderer.material.color == m_Colors[m_TaskNumber])
				Debug.Log("Match Color");
			else
			{
				Debug.Log(col.renderer.material.color  + " " + m_Colors[m_TaskNumber]);
			}
		}

	}
}
