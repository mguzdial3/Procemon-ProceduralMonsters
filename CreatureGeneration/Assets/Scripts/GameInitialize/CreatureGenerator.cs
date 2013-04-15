using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

//Generates all the creatures, and passes them up to the CreatureHolder
//THIS IS THE ONE FOR NATHAN TO MESS WITH
public class CreatureGenerator {
	CreatureImageGenerator imageGenerator;
	
	
	//Presenlty just does it randomly, but that needs to be changes
	public CreatureInfo[] generateCreatures(){
		imageGenerator = new CreatureImageGenerator();
		
		CreatureInfo[] creatures = new CreatureInfo[60];
		
		string[] nameBits = {"un", "za", "muh", "ga","go","gi","da","do","di","du","fa","fo","fu","la","lo","lu","ma","mu","mi","naom","nurm",
		"nor","dude","bro","va","vo","vive","mon","bro","wes","gorn","florp","bag","dern","qui","ze","xon", "ley", "durn", "yui"};
		
		
		string[] fightBits = {"punch", "munch", "slam", "bam", "wham", "jam", "schism", "tackle", "strike", "pound", "slam", "jab", "bite", "slice",
			"blast", "kick", "charge", "chomp", "thrust", "blitz", "barrage", "onslaught", "foray", "skirmish", "encroach"};
		
		string[] boostBits = {"boost", "help", "raise", "expand", "extend", "increase", "aid", "succor", "lift", "cure", "heal"};
		string[] negativeBits = {"lower", "decrease", "abation", "cut", "shave", "down", "lessen", "harm", "ruin", "vandalize"};
		
		//public const int LIGHTNING= 0, GROUND =1, WATER =2, FIRE = 3, GRASS =4, AIR=5, NORMAL=6;
		string[] groundBits = {"ground", "rock", "earth", "dirt", "mud","grass", "leaf", "vine", "thorn", "branch"};
		string[] waterBits = {"water", "aqua", "rain", "tsunami", "shower"};
		string[] fireBits = {"fire", "flame", "fiery", "magma", "burn", "lightning", "thunder", "electric", "volt", "magnetic"};
		string[] airBits = {"air", "wind", "storm", "sky", "zephyr", "gust"};
		string[] normalBits = {"strong", "cunning", "killer", "death", "average"};
		
		string[][] elementalBits = {normalBits, fireBits, waterBits, groundBits, airBits};
		
		//Load the creatures from the last run
		List<SerializableCreature> oldCreatures = null;
		try
	    {
			using (Stream stream = File.Open("data.bin", FileMode.Open))
			{
		    	BinaryFormatter bin = new BinaryFormatter();
		    	oldCreatures = (List<SerializableCreature>)bin.Deserialize(stream);
			}
	    }
	    catch (IOException)
	    {
			Debug.LogError("Error loading creatures");
	    }
		
		for(int i = 0; i<creatures.Length; i++){
			
			
			//Make creature Name
			int nameLength= Random.Range(2,4);
			int nameIndex = 0;
			
			
			string creatureName = "";
			while(nameIndex<nameLength){
				creatureName += nameBits[Random.Range(0, nameBits.Length)];
				nameIndex++;
			}
			
			
			//Set up the GA problem
			StatsProblem problem = new StatsProblem(oldCreatures);
			GeneticAlgorithm ga = new GeneticAlgorithm(problem);
			
			//Run the GA
			Chromosome result = ga.evaluateProblem(50, 1000, true, 0.8, 0.015, 20);
			List<Feature> featureSet = result.chromosome;
			int attack = 0, speed = 0, defense = 0, special = 0, type = 0, hp = 0;
			float accuracy = 0;
			foreach(Feature f in result.chromosome)
			{
				//Debug.Log (f.label + ": " + f.value);
				if (f.label.Equals("Attack"))
					attack = (int) f.value;
				else if (f.label.Equals("Speed"))
					speed = (int) f.value;
				else if (f.label.Equals("Defense"))
					defense = (int) f.value;
				else if (f.label.Equals("Special"))
					special = (int) f.value;
				else if (f.label.Equals("Accuracy"))
					accuracy = (int) f.value;
				else if (f.label.Equals("HP"))
					hp = (int) f.value;
				else if (f.label.Equals("Type"))
					type = (int) f.value;
			}
			
		
			//All values sum to 10
			//int attack = Random.Range(1,8);
			//int speed = Random.Range(1, 9-attack);
			//int defense = Random.Range(1, 10-attack-speed);
			//int special = Random.Range(1, 11-attack-defense-speed);
			//Type of creature determined randomly
			//int type = Random.Range(0,5);
			//Randomly generate accuracy
			//float percentAccuracy = Random.Range(0.8f,1.0f);
			
			
			//Calculate hitpoints
			//int[] possibleHitPoints = {50, 55, 60, 65};
			//int hitpoints = possibleHitPoints[Random.Range(0, 4)];
			
			CreatureInfo ci = new CreatureInfo(i, creatureName, attack, speed, defense, special, type, accuracy / 10, hp, imageGenerator.MakeACreature(type), null);

			
			
			//Four Basic Creature Attacks
			CreatureAttack[] fourBasic = new CreatureAttack[4];
			
			for(int f = 0; f<fourBasic.Length; f++){
				fourBasic[f] = generateAttack(ci.type, fightBits,elementalBits,boostBits,negativeBits);		
			}
			
			//Three Level Creature Attacks
			//TODO Fill this out
			CreatureAttack[] threeLevelGained = new CreatureAttack[4];
			
			for(int f = 0; f<threeLevelGained.Length; f++){
				threeLevelGained[f] = generateLevelAttack(ci.type, fightBits,elementalBits,boostBits,negativeBits);
				
			}
			
			
			ci.setAttacks(fourBasic,threeLevelGained);
			
			creatures[i]=ci;
			
		}
		
		return creatures;
	}
	
	
	private CreatureAttack generateAttack(int element, string[] fightBits, string[][] elementalBits, string[] boostBits,string[] negativeBits ){
			//First, determine what the type of attack will be
				int attackType = Random.Range(-2,3);
				
				//Damage type
				if(attackType<=0){
					attackType=0;
					int fightIndex=0;
					int fightLength = Random.Range(1,3);
					
					
					//NAME OF ATTACK
					string basicAttack = "";
					
					while(fightIndex<fightLength){
						
						basicAttack+=fightBits[Random.Range(0, fightBits.Length)];
						
						fightIndex++;
					}
					
					basicAttack= (basicAttack[0]).ToString().ToUpper() + basicAttack.Substring(1);
					
					
					//(string _name, int _attackType, int _elementalType, int _power, float _accuracyOfAttack, int _maxTimes, int _statTarget){
					
					//Element type for all basic moves is normal
					int elementalType = 0;
					
					//Calculate Power
					int power = Random.Range(15,36);
					
					//Accuracy of attack
					float accuracy = Random.Range(0.98f,1.0f);
					
					//Number of times you can do this
					int[] possibleNumberOfTimes = {20,25,30, 40, 60};
					int numberOfTimes = possibleNumberOfTimes[Random.Range(0, possibleNumberOfTimes.Length)];
					
					//Stat to target is hitpoints
					int statsToTarget = 0;
					
					return new CreatureAttack(basicAttack,attackType,0,power,accuracy,numberOfTimes,statsToTarget);

					
					
				}
				//Boosts my stats type
				else if(attackType==1){
					//Let's pick a type 
					int statsWerePointingAt = Random.Range(1,6);
					
					string nameOfAttack = "";
					
					nameOfAttack += getStringGivenType(statsWerePointingAt);
					
					int descriptiveWord = Random.Range(0, boostBits.Length);
					
					nameOfAttack+= "-"+boostBits[descriptiveWord];
					
					//Camel case!
					nameOfAttack =(nameOfAttack[0]).ToString().ToUpper() + nameOfAttack.Substring(1);
					
					
					//The Amount of stats to boost
					int statBoostAmnt = 0;
					
					//If we're not looking at accuracy
					if(statsWerePointingAt !=5){
						statBoostAmnt = Random.Range(1,4);
					}
					else{//We're looking at accuracy, needs to be higher
						statBoostAmnt = Random.Range(3,7);
						
					}
					
					//Calculate accuracy
					float accuracyOfAttack = 1.0f;
					
					//Calculate number of times you can use the move
					int[] possibleNumberOfTimes = {10, 15,20,25,30, 40};
					
					int numberOfTimes = possibleNumberOfTimes[Random.Range(0, possibleNumberOfTimes.Length)];
					
					return new CreatureAttack(nameOfAttack,attackType,0,statBoostAmnt,accuracyOfAttack,numberOfTimes,statsWerePointingAt);
					
				}
				//Harms your stats type
				else if(attackType==2){
					//Let's pick a type 
					int statsWerePointingAt = Random.Range(1,6);
					
					string nameOfAttack = "";
					
					nameOfAttack += getStringGivenType(statsWerePointingAt);
					
					int descriptiveWord = Random.Range(0, negativeBits.Length);
					
					nameOfAttack+= "-"+negativeBits[descriptiveWord];
					
					//Camel case!
					nameOfAttack =(nameOfAttack[0]).ToString().ToUpper() + nameOfAttack.Substring(1);
					
					
					//The Amount of stats to hurt
					int statHurtAmnt = 0;
					
					//If we're not looking at accuracy
					if(statsWerePointingAt !=5){
						statHurtAmnt = Random.Range(1,4);
					}
					else{//We're looking at accuracy, needs to be higher
						statHurtAmnt = Random.Range(3,7);
						
					}
					
					
					//Calculate accuracy
					float accuracyOfAttack = 1.0f;
					
					//Calculate number of times you can use the move
					int[] possibleNumberOfTimes = {10, 15,20,25,30};
					
					int numberOfTimes = possibleNumberOfTimes[Random.Range(0, possibleNumberOfTimes.Length)];
					
					return new CreatureAttack(nameOfAttack,attackType,0,statHurtAmnt,accuracyOfAttack,numberOfTimes,(int)statsWerePointingAt);
				}	
			return null;
	}
	
	private CreatureAttack generateLevelAttack(int element, string[] fightBits, string[][] elementalBits, string[] boostBits,string[] negativeBits ){
			//First, determine what the type of attack will be
				int attackType = Random.Range(-2,3);
				
				//Damage type
				if(attackType<=0){
					attackType=0;
					int fightIndex=0;
					int fightLength = Random.Range(1,3);
					
					
					//NAME OF ATTACK
					string basicAttack = "";
					
					while(fightIndex<fightLength){
						
						basicAttack+=fightBits[Random.Range(0, fightBits.Length)];
						
						fightIndex++;
					}
					
					basicAttack= (basicAttack[0]).ToString().ToUpper() + basicAttack.Substring(1);
					
					
					
					//For level up moves, determine if we are using element or not
			
					
					bool usingElement = Random.value>0.5f;
					int elementalType = 0;
					if(usingElement){
						elementalType = element;
					}
					
					//Calculate Power
					int power = Random.Range(15,36);
					
					//Accuracy of attack
					float accuracy = Random.Range(0.98f,1.0f);
					
					//Number of times you can do this
					int[] possibleNumberOfTimes = {20,25,30, 40, 60};
			
					
			
					int numberOfTimes = possibleNumberOfTimes[Random.Range(0, possibleNumberOfTimes.Length)];
					
					if(usingElement){
						power += 5;
						numberOfTimes-=5;
					}
			
			
					//Stat to target is hitpoints
					int statsToTarget = 0;
					
					return new CreatureAttack(basicAttack,attackType,0,power,accuracy,numberOfTimes,statsToTarget);

					
					
				}
				//Boosts my stats type
				else if(attackType==1){
					//Let's pick a type 
					int statsWerePointingAt = Random.Range(1,6);
					
					string nameOfAttack = "";
					
					nameOfAttack += getStringGivenType(statsWerePointingAt);
					
					int descriptiveWord = Random.Range(0, boostBits.Length);
					
					nameOfAttack+= "-"+boostBits[descriptiveWord];
					
					//Camel case!
					nameOfAttack =(nameOfAttack[0]).ToString().ToUpper() + nameOfAttack.Substring(1);
					
					
					//The Amount of stats to boost
					int statBoostAmnt = 0;
					
					//If we're not looking at accuracy
					if(statsWerePointingAt !=5){
						statBoostAmnt = Random.Range(3,7);
					}
					else{//We're looking at accuracy, needs to be higher
						statBoostAmnt = Random.Range(5,10);
						
					}
					
					
					//Calculate accuracy
					float accuracyOfAttack = 1.0f;
					
					//Calculate number of times you can use the move
					int[] possibleNumberOfTimes = {10, 15,20,25,30, 40};
					
					int numberOfTimes = possibleNumberOfTimes[Random.Range(0, possibleNumberOfTimes.Length)];
					
					return new CreatureAttack(nameOfAttack,attackType,0,statBoostAmnt,accuracyOfAttack,numberOfTimes,statsWerePointingAt);
					
				}
				//Harms your stats type
				else if(attackType==2){
					//Let's pick a type 
					int statsWerePointingAt = Random.Range(1,6);
					
					string nameOfAttack = "";
					
					nameOfAttack += getStringGivenType(statsWerePointingAt);
					
					int descriptiveWord = Random.Range(0, negativeBits.Length);
					
					nameOfAttack+= "-"+negativeBits[descriptiveWord];
					
					//Camel case!
					nameOfAttack =(nameOfAttack[0]).ToString().ToUpper() + nameOfAttack.Substring(1);
					
					
					//The Amount of stats to hurt
					int statHurtAmnt = 0;
					
					//If we're not looking at accuracy
					if(statsWerePointingAt !=5){
						statHurtAmnt = Random.Range(3,7);
					}
					else{//We're looking at accuracy, needs to be higher
						statHurtAmnt = Random.Range(5,10);
						
					}
					
					
					//Calculate accuracy
					float accuracyOfAttack = 1.0f;
					
					//Calculate number of times you can use the move
					int[] possibleNumberOfTimes = {10, 15,20,25,30};
					
					int numberOfTimes = possibleNumberOfTimes[Random.Range(0, possibleNumberOfTimes.Length)];
					
					return new CreatureAttack(nameOfAttack,attackType,0,statHurtAmnt,accuracyOfAttack,numberOfTimes,(int)statsWerePointingAt);
				}	
			return null;
	}
	
	
	
	
	//Pass in an attribute type, get a string out of it
	public string getStringGivenType(int statType){
		CreatureAttack c = new CreatureAttack();
		
		return c.getStringForStatType(statType);
	}
	
	
}
