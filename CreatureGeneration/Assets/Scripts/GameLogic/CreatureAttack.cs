using UnityEngine;
using System.Collections;

//Information Holder for Creature's Attacks Info
public class CreatureAttack{
	//name of attack
	public string name;
	
	//Different attack types
	public int attackType = 0;
	public const int DAMAGE=0, BOOSTSTATS=1, HARMSTATS=2;
	
	
	
	
	//Base power for attack (used for damage for damageTypes and the amount to harm stats for stat types)
	public int power = 0;
	
	//Used for the power of a secondary attack (for damage and stats types)
	//public int power2 = 0;
	
	
	
	//percent accuracy (0.0f-1.0f)
	public float accuracyOfAttack = 1.0f;
	
	//Percent change of making a secondary attack for Damage and stats types ()
	//public float chanceOfSecondary = 0.0f;
	
	//The number of times this attack can be used
	public int timesUsed = 0;
	public int maxTimes = 30;
	
	//The element type of this attack
	public int elementalType;
	
	//For Stat-type attacks, defines which stat we target (attack hitpoints by default)
	public const int HITPOINTS=0, ATTACK=1, DEFENSE=2, SPEED= 3, SPECIAL =4, ACCURACY = 5;
	public int statTarget = 0;
	
	
	//Constructor to create a new attack (without a secondary attack)
	public CreatureAttack(string _name, int _attackType, int _elementalType, int _power, float _accuracyOfAttack, int _maxTimes, int _statTarget){
	
		name = _name;
		attackType=_attackType;
		elementalType=_elementalType;
		power=_power;
		accuracyOfAttack=_accuracyOfAttack;
		maxTimes =_maxTimes;
		statTarget=_statTarget;
	}
	//Constructor with secondary attack
	/**
	public CreatureAttack(string _name, int _attackType, int _elementalType, int _power, float _accuracyOfAttack, int _maxTimes, int _statTarget, int _power2, float _changeOfSecondary)
	:this(_name, _attackType,_elementalType,_power,_accuracyOfAttack,_maxTimes,_statTarget){
		
		power2 = _power2;
		chanceOfSecondary = _changeOfSecondary;
		
	}
	*/
	
}
