using UnityEngine;
using System.Collections;
using TNet;

public class ControlCharacter : TNBehaviour {

	// instance
	public static ControlCharacter instance;

	//parameters
	public TNObject m_NetObject;
	public float m_Speed = 5;
	public float m_MouseSensitivity = 0.5f;
	public float m_JumpingSpeed = 7f;
	public Vector3 m_LastPosition;
	public GameObject m_UsableObject;
	public GameObject m_PickedObject;
	public float m_StunDistance = 1.2f;
	public bool m_Stunned;
	public float m_StunDuration = 2f;
	float m_StunTimer;
	TNManager m_tnManager;

	public Material m_Blue;
	public Material m_Red;

	public Transform m_Bottom;
	public Transform m_PickSpot;

	public Vector3 f_newPosition;
	public Quaternion f_newRotation;

	public bool m_MyObject = true;
	public AudioClip[] jumpAudio;
	public AudioClip gotPunched;
	public AudioClip punchMissed;
	public AudioClip pickCubeSound;
	public AudioClip stealCubeSound;

	//
	// Init
	//
	void Awake()
	{
		if(TNManager.isThisMyObject)
		{

			instance = this;
			m_NetObject = GetComponent<TNObject>();
			m_Bottom = transform.Find("Bottom");
			m_PickSpot = transform.FindChild("PickedObjectPivot");
		}else{
			gameObject.transform.Find("Camera").GetComponent<Camera>().enabled = false;
			gameObject.transform.Find("Camera").GetComponent<AudioListener>().enabled = false;
			//enabled = false;
			m_MyObject = false;
		}
	}
	void Start(){
		f_newRotation=transform.rotation;
		f_newPosition = transform.position;
		m_tnManager = GameObject.Find("Network").GetComponent<TNManager>();

		if(m_MyObject)
		{
			if(PlayerInfo.instance.m_Team == 0)
			{
				Debug.Log("Team blue");
				//gameObject.transform.Find("group1/FagotukasFBX:Mesh").renderer.material = m_Blue;
				tno.Send("SetBlueMaterial", Target.AllSaved);
			}else
			{
				Debug.Log("Team red");
				//gameObject.transform.Find("group1/FagotukasFBX:Mesh").renderer.material = m_Red;
				tno.Send("SetRedMaterial", Target.AllSaved);
			}
		}

	}
	//
	// Set pickable objet. called from trigger
	//
	public void SetPickableObject(GameObject g)
	{
		m_UsableObject = g;
	}

	//
	// Remove pickable objet. called from trigger
	//
	public void RemovePickableObject()
	{
		m_UsableObject = null;
	}

	//
	// Process
	//
	void Update () 
	{

		ProcessPosition();
		ProcessStuns();
		ProcessPickedObject();

		if(!m_MyObject)
			return;


		Inputs();

		Rotate();
		Animate();

	}

	//
	// Character movement
	//
	private void Inputs()
	{
		if(m_Stunned)
			return;

		if(Input.GetKey(KeyCode.W))
			f_newPosition += transform.forward * m_Speed * Time.deltaTime;
		
		if(Input.GetKey(KeyCode.S))
			f_newPosition += transform.forward * -m_Speed * Time.deltaTime;
		
		if(Input.GetKey(KeyCode.D))
			f_newPosition += transform.right * m_Speed * Time.deltaTime;
		
		if(Input.GetKey(KeyCode.A))
			f_newPosition += transform.right * -m_Speed * Time.deltaTime;

		if(Input.GetKeyDown(KeyCode.Space) && IsGrounded())
			Jump ();

		if(Input.GetKeyDown(KeyCode.E))
			CallPickDropObject();

		if(Input.GetMouseButtonDown(0))
			Hit();
	}

	private void ProcessPosition()
	{
		transform.position = Vector3.MoveTowards(transform.position, new Vector3(f_newPosition.x, transform.position.y, f_newPosition.z), Time.deltaTime * m_Speed);
		transform.rotation = Quaternion.RotateTowards(transform.rotation, f_newRotation, Time.deltaTime * 300);
	}

	private void ProcessPickedObject()
	{
		if(m_PickedObject != null)
		{
			PickableObject po = m_PickedObject.GetComponent<PickableObject>();
			if(po.State == 1 && po.TeamOwner == PlayerInfo.instance.m_Team)
			{
				m_PickedObject.GetComponent<PickableObject>().f_newPosition = m_PickSpot.position + new Vector3 (0,0,m_PickedObject.collider.bounds.size.x);
			}else
				m_PickedObject = null;
		}
	}

	//
	// Look around
	//
	private void Rotate()
	{
		if(m_Stunned)
			return;

		f_newRotation.eulerAngles = new Vector3(f_newRotation.eulerAngles.x, f_newRotation.eulerAngles.y + Input.GetAxis("Mouse X") * 5 *m_MouseSensitivity, f_newRotation.eulerAngles.z);
			//= Vector3.MoveTowards (transform.eulerAngles, f_newRotation, Time.deltaTime *500 );
	}

	//
	// Get stun
	//
	public void CallApplyStun()
	{
		tno.Send("ApplyStun", Target.All);
	}

	//
	//
	//
	public void ProcessStuns()
	{
		if(m_Stunned)
		{
			m_StunTimer -= Time.deltaTime;
			if(m_StunTimer <= 0)
				m_Stunned = false;
		}
	}

	public void Hit()
	{
		audio.PlayOneShot(punchMissed);

		Ray r = new Ray(transform.position, transform.forward);
		RaycastHit hit;
		if(Physics.Raycast(r, out hit, m_StunDistance))	{
			if(hit.collider.CompareTag("Player")){
				hit.collider.GetComponent<ControlCharacter>().CallApplyStun();
				Vector3 pos = hit.collider.transform.position;
				pos.y += 1f;
				GameObject effect = GameObject.Instantiate(m_tnManager.objects[3], pos ,Quaternion.identity) as GameObject;
				effect.transform.rotation = Quaternion.LookRotation(Vector3.up, Vector3.forward);
				Destroy(effect, 1.5f);
			}
		}
	}

	//
	// Jump
	//
	private void Jump()
	{
		rigidbody.velocity = new Vector3 (0, m_JumpingSpeed, 0);
		if(m_PickedObject !=null){
			CallPickDropObject();
		}
		audio.PlayOneShot(jumpAudio[Mathf.FloorToInt(Random.Range(0, 1.9f))]);
	}

	private bool IsGrounded () 
	{
		Ray r = new Ray(m_Bottom.position, Vector3.down * 10);
		RaycastHit hit;
		if(Physics.Raycast(r, out hit))
		{
			Debug.DrawLine(m_Bottom.position, hit.point);
			if(Vector3.Distance(m_Bottom.position, hit.point) > 1f)
				return false;
		} 
		return true;
	}

	//
	// Process animation
	//
	private void Animate()
	{

		if(m_Stunned)
		{
			if(!animation.IsPlaying("Stun"))
			{
				animation.CrossFade("Stun");
				m_NetObject.Send("CallAnimation", Target.Others, "Stun");
			}
		}else if(!IsGrounded())
		{
			if(!animation.IsPlaying("Jump"))
			{
				animation.CrossFade("Jump");
				m_NetObject.Send("CallAnimation", Target.Others, "Jump");
			}

		}else if(m_LastPosition != f_newPosition)
		{
			if(m_PickSpot.transform.childCount > 0)
			{
				if(!animation.IsPlaying("Carry"))
				{
					animation.CrossFade("Carry");
					m_NetObject.Send("CallAnimation", Target.Others, "Carry");
				}
			}else
			{
				if(!animation.IsPlaying("Run"))
				{
					animation.CrossFade("Run");
					m_NetObject.Send("CallAnimation", Target.Others, "Run");
				}
			}
			m_LastPosition = f_newPosition;
		}else
		{
			if(!animation.IsPlaying("Idle"))
			{
				animation.CrossFade("Idle");
				m_NetObject.Send("CallAnimation", Target.Others, "Idle");
			}
		}
	}

	private void CallPickDropObject()
	{
		PickDropObject();
		//tno.Send("PickDropObject", Target.All);
	}

	//
	//
	//
	public void PickDropObject()
	{
//		if(m_PickedObject !=null)
//		{
//			//m_PickedObject.GetComponent<Rigidbody>().isKinematic = false;
//			//m_PickedObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
//			//m_PickedObject.transform.parent = null;
//			m_PickedObject = null;
//		}
		if(m_UsableObject != null){

			PickableObject po = m_UsableObject.GetComponent<PickableObject>();

			switch(po.State)
			{
			case 0:
				Debug.Log("state 0 dropped" );
				m_PickedObject = m_UsableObject;
				po.GetComponent<PickableObject>().CallSetState(1);
				po.CallSetTeam(PlayerInfo.instance.m_Team);
				audio.PlayOneShot(pickCubeSound);

				po.State = 1;
				po.TeamOwner = PlayerInfo.instance.m_Team;

				m_UsableObject = null;
			break;
			case 1:
				Debug.Log("state 1 mooving" );
				if(PlayerInfo.instance.m_Team != po.TeamOwner)
				{
					m_UsableObject.GetComponent<PickableObject>().CallSetState(0);
					po.State = 0;
				}
			break;
			case 2:
				Debug.Log("state 2 obelisk" );
				m_PickedObject = m_UsableObject;
				po.GetComponent<PickableObject>().CallSetState(1);
				po.CallSetTeam(PlayerInfo.instance.m_Team);
				po.State = 1;
				po.TeamOwner = PlayerInfo.instance.m_Team;
				m_UsableObject = null;

				audio.PlayOneShot(stealCubeSound);

			break;
			}
//
//			if(true)
//			{
//				m_UsableObject.GetComponent<PickableObject>().CallSetMooving(true);
//				m_UsableObject.GetComponent<PickableObject>().CallSetBusy(true);
//				m_PickedObject = m_UsableObject;
//				//m_UsableObject = null;
//				Debug.Log("not moovable");
//			}else if(m_UsableObject.GetComponent<PickableObject>().m_Busy)
//			{
//				m_UsableObject.GetComponent<PickableObject>().SetMooving(true);
//				m_UsableObject.GetComponent<PickableObject>().CallSetBusy(false);
//				//m_UsableObject = null;
//				Debug.Log("bussy");
//			}else
//			{
//				Debug.Log("pick");
//				m_PickedObject = m_UsableObject;
//				m_UsableObject.GetComponent<PickableObject>().CallSetBusy(true);
//				m_UsableObject = null;
//			}

			//Transform pivot = 
			//m_PickedObject.transform.parent = m_PickSpot;
				
			//m_PickedObject.transform.localRotation = Quaternion.identity;
			//m_PickedObject.GetComponent<Rigidbody>().isKinematic = true;

			//m_PickedObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
		}

	}

	//
	// network animation
	//
	[RFC] private void CallAnimation(string name)
	{
		animation.CrossFade(name);
	}

	//
	// network stun
	//
	[RFC] public void ApplyStun()
	{
		if(m_PickedObject !=null){
			CallPickDropObject();
		}
		m_Stunned = true;
		m_StunTimer = m_StunDuration;
		Vector3 pos = transform.position;
		pos.y += 1f;
		GameObject effect = GameObject.Instantiate(m_tnManager.objects[3], pos, Quaternion.identity) as GameObject;
		effect.transform.rotation = Quaternion.LookRotation(Vector3.up, Vector3.forward);
		Destroy(effect, m_StunDuration);

		audio.PlayOneShot(gotPunched);
	}

	[RFC] public void SetBlueMaterial()
	{
		gameObject.transform.Find("group1/FagotukasFBX:Mesh").renderer.material = m_Blue;
	}

	[RFC] public void SetRedMaterial()
	{
		gameObject.transform.Find("group1/FagotukasFBX:Mesh").renderer.material = m_Red;
	}
	
}
