using UnityEngine;
using System.Collections;

public class CreatureTrainer : MonoBehaviour {
	public CreatureInfo[] creatures;
	public TrainerBattleHandler trainerBattlerHandler;
	
	private Vector3 goal, origPos;
	
	private int FACING_DOWN=0, FACING_RIGHT=1, FACING_LEFT=2, FACING_UP=3;
	public const int NORMAL = 0, FIRELIGHTNING =1, WATER = 2, GRASSGROUND = 3, AIR =4;
	public bool testSetup, sawPlayer;
	
	//For testing
	public int numOfCreatures = 2, level=2;
	public int facing = 0, element = 0;
	public float viewDistance = 48;
	
	private bool inBattle;
	
	void Update(){
		if(!inBattle){
			if(!testSetup){
				//WARNING, WE'LL WANT TO DO THIS SOMEWHERE ELSE THIS IS FOR TESTING
				setup(level,numOfCreatures,element,facing);
				
			}
			else if(testSetup && !sawPlayer){
				Vector3 getViewing = getView(facing);
				RaycastHit hit;
				if(Physics.Raycast(transform.position,getViewing,out hit, viewDistance)){
					if(hit.collider.tag=="Player"){
						PlayerMovement movement = hit.collider.gameObject.GetComponent<PlayerMovement>();
						movement.inMvmtMode=false;
						
						Vector3 toPlayer = movement.transform.position-transform.position;
						
						float magn = toPlayer.magnitude;
						
						toPlayer.Normalize();
						toPlayer*=(magn-10);
						
						goal = transform.position+toPlayer;
						
						sawPlayer=true;
					}
				}
			}
			else{
				//print("Getting to here repeatedly");
				if((goal-transform.position).magnitude>0.1f){
					Vector3 difference = goal-transform.position;
					
					transform.position+=difference.normalized*Time.deltaTime*50.0f;
					
				}
				else{
					inBattle = true;
					trainerBattlerHandler.startBattle(this);
				}
			}
		}
	}
	
	//Returns the view of this trainer given the player's facing
	private Vector3 getView(int _facing){
		Vector3 viewVector = new Vector3();
		
		if(_facing==FACING_DOWN){
			viewVector = new Vector3(0,-1,0);
		}
		else if(_facing==FACING_UP){
			viewVector = new Vector3(0,1,0);
		}
		else if(_facing==FACING_RIGHT){
			viewVector = new Vector3(1,0,0);
		}
		else if(_facing==FACING_LEFT){
			viewVector = new Vector3(-1,0,0);
		}
		
		return viewVector;
	}
	
	public void backToOriginalPos(){
		for(int i =0; i<creatures.Length; i++){
			creatures[i].hitPoints = creatures[i].maxHitPoints;
		}
		inBattle = false;
		transform.position=origPos;
		sawPlayer = false;
	}
	
	//Average level to generate creatures to, and the list of creatures in the creature holder
	//Element is the element to use
	public void setup(int level,int _numOfCreatures, int _element, int _facing){
		CreatureHolder creatureHolder = GameObject.Find("Admin").GetComponent<CreatureHolder>();
		trainerBattlerHandler = GameObject.Find("Admin").GetComponent<TrainerBattleHandler>();
		//Set up the originalPosition
		origPos = transform.position;
		
		testSetup=true;
		
		creatures = new CreatureInfo[numOfCreatures+1];
		int creaturesWeHave =0;
		
		
		while(creaturesWeHave<_numOfCreatures){
			
			CreatureInfo possCreature= creatureHolder.allCreatures[Random.Range(0,creatureHolder.allCreatures.Length)].cloneCreature();
			
			if(possCreature.type==_element){
				creatures[creaturesWeHave]=possCreature.cloneCreature();
				creatures[creaturesWeHave].levelTo(Random.Range(level-1,level+2));
				creaturesWeHave++;
			}
			
		
		}
		
		if(_facing==FACING_DOWN){
			transform.eulerAngles= new Vector3(90,90,-90);
		}
		else if(_facing==FACING_RIGHT){
			transform.eulerAngles= new Vector3(0,90,-90);
		}
		else if(_facing==FACING_LEFT){
			transform.eulerAngles= new Vector3(180,90,-90);
		}
		else if(_facing==FACING_UP){
			transform.eulerAngles= new Vector3(270,90,-90);
		}
		
	}
}
