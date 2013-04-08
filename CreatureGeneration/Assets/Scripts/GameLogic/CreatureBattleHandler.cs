using UnityEngine;
using System.Collections;

//Detects when the player should enter a creature battle, and then runs through the battle
public class CreatureBattleHandler : MonoBehaviour {
	public PlayerMovement player;
	public PlayerCreatureHolder playerCreatureHolder;
	//Whether or not we're battling presently
	private bool battling;
	//Time that you've spent in a place where creatures can get you
	private int timeInGrass;
	public Texture2D creatureBattleBackground;
	CreatureHolder creatureHolder;
	
	CreatureInfo attackingCreature;
	
	void Start(){
		creatureHolder = gameObject.GetComponent<CreatureHolder>();
		playerCreatureHolder = player.GetComponent<PlayerCreatureHolder>();
	}
	
	
	// Update is called once per frame
	void Update () {
		//Find out what player is over
		if(!battling){
		RaycastHit hit;
		
			if(Physics.Raycast(player.transform.position, Vector3.forward, out hit, 10.0f)){
				if(hit.collider.tag=="ContainsCreatures"){
					int chanceOfBeingAttacked = Random.Range(0,1000-timeInGrass);
					timeInGrass++;
					if(chanceOfBeingAttacked ==0){
						Debug.Log("ATTACKED");
						battling= true;
						player.inMvmtMode=false;
						timeInGrass=0;
						attackingCreature = creatureHolder.allCreatures[Random.Range(0,creatureHolder.allCreatures.Length)].cloneCreature();
					}
				}
				else{
					timeInGrass=0;
				}
			}
		}
	}
	
	
	void OnGUI(){
		if(battling){
			GUI.DrawTexture(new Rect(0,0,Screen.width,Screen.height), creatureBattleBackground);
			
			GUI.DrawTexture(new Rect(0,0,100,100), playerCreatureHolder.myCreatures[0].image);
			GUI.DrawTexture(new Rect(Screen.width-100,0,100,100), attackingCreature.image);
			
			//Easy way to cheat back to reality (for now)
			if(GUI.Button(new Rect(Screen.width/2-50, Screen.height-100, 100,50), "Flee")){
				battling =false;
				player.inMvmtMode=true;
				attackingCreature=null;
			}
		}
	}
}
