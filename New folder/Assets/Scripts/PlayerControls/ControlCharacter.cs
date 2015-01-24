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

	public Transform m_Bottom;

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
		Movement();

		Rotate();
		Animate();
	}

	//
	// Character movement
	//
	private void Movement()
	{
		if(Input.GetKey(KeyCode.W))
			transform.position += transform.forward * m_Speed * Time.deltaTime;
		
		if(Input.GetKey(KeyCode.S))
			transform.position += transform.forward * -m_Speed * Time.deltaTime;
		
		if(Input.GetKey(KeyCode.D))
			transform.position += transform.right * m_Speed * Time.deltaTime;
		
		if(Input.GetKey(KeyCode.A))
			transform.position += transform.right * -m_Speed * Time.deltaTime;

		if(Input.GetKeyDown(KeyCode.Space) && IsGrounded())
			Jump ();

		if(Input.GetKeyDown(KeyCode.E))
			PickDropObject();
	}

	//
	// Look around
	//
	private void Rotate()
	{
		transform.eulerAngles = new Vector3(0, Input.mousePosition.x*m_MouseSensitivity, 0);
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

		}else if(m_LastPosition != transform.position)
		{
			if(!animation.IsPlaying("run"))
			{
				animation.CrossFade("run");
				m_NetObject.Send("CallAnimation", Target.Others, "run");
			}
			m_LastPosition = transform.position;
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


}
