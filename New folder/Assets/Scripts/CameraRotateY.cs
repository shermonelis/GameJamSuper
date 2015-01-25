using UnityEngine;
using System.Collections;

public class CameraRotateY : MonoBehaviour {
	ControlCharacter charController;
	void Start () {
		charController = gameObject.GetComponentInParent<ControlCharacter>();
	}
	
	void Update () {
		Rotate();
	}
	private void Rotate()
	{
		if(charController.m_Stunned)
			return;		
		transform.eulerAngles = new Vector3(transform.eulerAngles.x + -Input.GetAxis("Mouse Y") * 3 *charController.m_MouseSensitivity, transform.eulerAngles.y, transform.eulerAngles.z);
	}
}
