using UnityEngine;
using System.Collections;

//Holds all the creatures for a single playthrough system
public class CreatureHolder : MonoBehaviour {
	CreatureGenerator generator;
	//Holds all creatures, all at level one
	public CreatureInfo[] allCreatures;
	public PlayerCreatureHolder playerCreatureHolder;
	
	public int numShow = 0;
	
	// Use this for initialization
	void Start () {
		generator = new CreatureGenerator();
		
		allCreatures = generator.generateCreatures();
		
		//TESTING: Randomly through creatures at player for now
		CreatureInfo[] playerCreatures = {allCreatures[Random.Range(0,allCreatures.Length)].cloneCreature(), allCreatures[Random.Range(0,allCreatures.Length)].cloneCreature()};
		playerCreatureHolder.myCreatures = playerCreatures;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKey(KeyCode.RightArrow)){
			numShow++;
		}
	}
	
	/** JUST HAD IT FOR TESTING PURPOSES
	void OnGUI(){
		if(allCreatures!=null){
	 	GUI.DrawTexture(new Rect(0,0,100,100), allCreatures[numShow].image);
		}
	}
	*/
}
