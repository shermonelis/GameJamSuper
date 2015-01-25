﻿using UnityEngine;
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
	public Transform m_Bottom;

	public Vector3 f_newPosition;
	public Quaternion f_newRotation;

	public bool m_MyObject = true;
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
			//enabled = false;
			m_MyObject = false;
		}
	}
	void Start(){
		f_newRotation=transform.rotation;
		f_newPosition = transform.position;
		m_tnManager = GameObject.Find("Network").GetComponent<TNManager>();

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

	private void CallPickDropObject()
	{
		tno.Send("PickDropObject", Target.All);
	}

	//
	//
	//
	[RFC] private void PickDropObject()
	{
		if(m_PickedObject !=null){
			//m_PickedObject.GetComponent<Rigidbody>().isKinematic = false;
			m_PickedObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
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
			//m_PickedObject.GetComponent<Rigidbody>().isKinematic = true;

			m_PickedObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
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
	}
	
}
