using UnityEngine;
using System.Collections;

public class SetMasterPlayer : MonoBehaviour {
	public bool isFirstPlayer; //seting master player

	void Start () {
		setIsPlayerFirst();
	}
	
	void Update () {
	}
	public void setIsPlayerFirst(){
		int pInfos = GameObject.FindObjectsOfType<PlayerInfo>().Length;

		if(pInfos==1){
			isFirstPlayer = true;
		} else
			isFirstPlayer = false;
	}
}
