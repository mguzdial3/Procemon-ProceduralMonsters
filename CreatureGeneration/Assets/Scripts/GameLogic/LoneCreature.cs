using UnityEngine;
using System.Collections;

public class LoneCreature : MonoBehaviour {
	public CreatureBattleHandler battleHandler;
	private CreatureInfo me;
	private bool battlingPlayer;
	//The level you want the creature to be at
	public int creatureLevel = 6;
	//The ID for that creature
	public int ID = 61;
	//Purely for testing
	public Texture2D shouldDisplay;
	//Purely for testing
	public int attack, defense, speed, special;
	
	void Start(){
		CreatureGenerator cg = new CreatureGenerator();
		
		me = cg.generateBossCreature(Random.Range(0,5),creatureLevel,ID);
		shouldDisplay = me.image;
		
		attack= me.attack;
		defense = me.defense;
		speed = me.speed;
		special = me.special;
		
		renderer.material.SetTexture("_MainTex", me.image);
		if(battleHandler==null){
			battleHandler = GameObject.Find("Admin").GetComponent<CreatureBattleHandler>();
		}
	}
	
	
	
	
	void Update(){
		//print("Hit points of loner dude: "+me.hitPoints);
		if(me.hitPoints<=0 || me.captured){
			
			Destroy(gameObject);
		}
	}
	
	
	void OnTriggerEnter(Collider other){
		if(!battlingPlayer){
			battlingPlayer=true;
			battleHandler.startBattle(me);
		}
		
	}
}
