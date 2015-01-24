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

	public bool m_Stunned;
	public float m_StunTimer;

	public Transform m_Bottom;

	public Vector3 f_newPosition;
	public Vector3 f_newRotation;

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
		}else{
			gameObject.transform.Find("Camera").GetComponent<Camera>().enabled = false;
			enabled = false;
		}
	}
	void Start(){
		f_newRotation=transform.eulerAngles;
		f_newPosition = transform.position;
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
		ProcessStuns();
		Movement();

		Rotate();
		Animate();

	}

	//
	// Character movement
	//
	private void Movement()
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
			PickDropObject();

		if(Input.GetMouseButtonDown(0))
			Hit();

		transform.position = Vector3.Lerp(transform.position, f_newPosition, Time.deltaTime);
	}

	//
	// Look around
	//
	private void Rotate()
	{
		f_newRotation = new Vector3(0, Input.mousePosition.x*m_MouseSensitivity, 0);
		transform.eulerAngles = Vector3.Lerp( transform.eulerAngles, f_newRotation, Time.deltaTime);
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
			m_StunTimer -= Time.deltaTime;
		else
		{
			m_Stunned = false;
			m_StunTimer = 0;
		}
	}

	public void Hit()
	{
		Ray r = new Ray(transform.position, transform.forward * 5);
		RaycastHit hit;
		if(Physics.Raycast(r, out hit))
		{
			if(hit.collider.CompareTag("Player"))
				hit.collider.GetComponent<ControlCharacter>().CallApplyStun();
		}
	}

	//
	// Jump
	//
	private void Jump()
	{
		rigidbody.velocity = new Vector3 (0, m_JumpingSpeed, 0);
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
		if(!IsGrounded())
		{
			if(!animation.IsPlaying("jump_pose"))
			{
				animation.CrossFade("jump_pose");
				m_NetObject.Send("CallAnimation", Target.Others, "jump_pose");
			}

		}else if(m_LastPosition != f_newPosition)
		{
			if(!animation.IsPlaying("run"))
			{
				animation.CrossFade("run");
				m_NetObject.Send("CallAnimation", Target.Others, "run");
			}
			m_LastPosition = f_newPosition;
		}else
		{
			if(!animation.IsPlaying("idle"))
			{
				animation.CrossFade("idle");
				m_NetObject.Send("CallAnimation", Target.Others, "idle");
			}
		}
	}

	//
	//
	//
	private void PickDropObject()
	{
		if(m_PickedObject !=null){
			m_PickedObject.GetComponent<Rigidbody>().isKinematic = false;
			m_PickedObject.transform.parent = null;
			m_PickedObject = null;
		}
		if(m_UsableObject != null){
			m_PickedObject = m_UsableObject;
			m_UsableObject = null;
			Transform pivot = gameObject.transform.FindChild("PickedObjectPivot");
			m_PickedObject.transform.parent = pivot;
			m_PickedObject.transform.localPosition = new Vector3 (0,0,m_PickedObject.collider.bounds.size.x);
			m_PickedObject.transform.localRotation = Quaternion.identity;
			m_PickedObject.GetComponent<Rigidbody>().isKinematic = true;
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
		m_Stunned = true;
		m_StunTimer = 1.5f;
	}
	
}
