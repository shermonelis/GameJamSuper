using UnityEngine;
using System.Collections;
using TNet;
public class PickableObject : TNBehaviour
{
	public Vector3 f_newPosition;
	public bool m_Moovable = true;
	public bool m_Busy;

	public int State = 0; //0-dropped, 1-moving, 2-obelisk
	public int TeamOwner = -1;

	void Start()
	{
		f_newPosition = transform.position;
	}

	private void OnTriggerEnter(Collider player)
	{
		if(player.CompareTag("Player"))
		{
			player.GetComponent<ControlCharacter>().SetPickableObject(gameObject);
		}
	}

	private void OnTriggerExit(Collider player)
	{
		if(player.CompareTag("Player"))
		{
			player.GetComponent<ControlCharacter>().RemovePickableObject();
		}
	}

	void Update()
	{
		transform.position = Vector3.MoveTowards(transform.position, new Vector3(f_newPosition.x, f_newPosition.y, f_newPosition.z), Time.deltaTime * 5);
	}

	public void CallSetState(int state) {tno.Send("SetState", Target.Others, state);}
	[RFC] public void SetState(int state)
	{
		State = State;
	}

	public void CallSetTeam(int team) {tno.Send("SetTeam", Target.Others, team);}
	[RFC] public void SetTeam(int team)
	{
		TeamOwner = team;
	}

//	public void CallSetMooving(bool mooving) { tno.Send("SetMooving", Target.All, mooving); }
//	[RFC] public void SetMooving(bool mooving)
//	{
//		m_Moovable = mooving;
//	}
//
//	
//	public void CallSetBusy(bool busy) { tno.Send("SetBusy", Target.All, busy); }
//	[RFC] public void SetBusy(bool busy)
//	{
//		m_Busy = busy;
//	}
}

