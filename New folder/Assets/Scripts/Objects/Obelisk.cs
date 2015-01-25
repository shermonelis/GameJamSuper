using UnityEngine;
using System.Collections;
using TNet;
using System.Linq;

public class Obelisk : TNBehaviour {

	public Color[] m_Colors = new Color[4];
	public Transform[] m_Transforms = new Transform[4];
	public int m_TaskNumber = 0;
	public AudioClip insertCubeSound;

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
		if(col.CompareTag("Player"))
		{
			ControlCharacter c = col.gameObject.GetComponent<ControlCharacter>();

			if(c.m_PickedObject == null)
				return;

			c.m_PickedObject.tag = "PartTaken";
			Debug.Log("trigger enter");
			if(c.m_PickedObject.renderer.material.color == m_Colors[m_TaskNumber])
			{
				c.m_PickedObject.GetComponent<PickableObject>().SetState(2);
				c.m_PickedObject.GetComponent<PickableObject>().State = 2;

				Debug.Log("atitiko" + m_TaskNumber);
				//col.rigidbody.isKinematic = true;
				//col.rigidbody.constraints = RigidbodyConstraints.FreezeAll;
				c.m_PickedObject.GetComponent<PickableObject>().f_newPosition = m_Transforms[m_TaskNumber].position;
				//col.transform.parent = m_Transforms[m_TaskNumber];
				m_TaskNumber++;
				tno.Send("AddPoint", Target.Others);

				audio.PlayOneShot(insertCubeSound);
			}
		}
	}

	void OnTriggerExit(Collider col)
	{
		if(col.CompareTag("Player"))
		{
			ControlCharacter c = col.gameObject.GetComponent<ControlCharacter>();
			if(c.m_PickedObject == null)
				return;

			Debug.Log("trigger exit");
			c.m_PickedObject.tag = "Part";
			//int index = 0;
			//for(int i = 0; i < m_Colors.Length; i++)
			//{
				//if(col.renderer.material.color == m_Colors[i])
				//{
				//	index = i;
				//}
			//}
			//if(index < m_TaskNumber)
			//{
				if(m_TaskNumber > 0)
				{
					m_TaskNumber--;
					tno.Send("RemovePoint", Target.Others);
				}
			//}
		}
	}

	[RFC] private void RemovePoint()
	{
		m_TaskNumber--;
	}

	[RFC] private void AddPoint()
	{
		m_TaskNumber++;
	}
}
