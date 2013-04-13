using UnityEngine;
using System.Collections;

public class GiveHealthOnTouch : MonoBehaviour {
	
	//When player enters into, heal all creatures
	void OnTriggerEnter(Collider other){
		PlayerCreatureHolder creatureHolder = other.GetComponent<PlayerCreatureHolder>();
		
		for(int i = 0; i<creatureHolder.myCreatures.Length; i++){
			creatureHolder.myCreatures[i].healCreature();
		}
		
	}
}
