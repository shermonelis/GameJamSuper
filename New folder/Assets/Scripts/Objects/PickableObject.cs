using UnityEngine;
using System.Collections;

public class PickableObject : MonoBehaviour
{


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
}

