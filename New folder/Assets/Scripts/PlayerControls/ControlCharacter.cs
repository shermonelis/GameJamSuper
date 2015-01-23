using UnityEngine;
using System.Collections;
using TNet;

public class ControlCharacter : TNBehaviour {


	public static ControlCharacter instance;


	void Awake()
	{
		if(TNManager.isThisMyObject)
			instance = this;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Input.GetKey(KeyCode.W))
			transform.position += transform.forward * 5 * Time.deltaTime;
	
		if(Input.GetKey(KeyCode.S))
			transform.position += transform.forward * -5 * Time.deltaTime;
	}
}
