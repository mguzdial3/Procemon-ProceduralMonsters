using System;

[Serializable]
public class SerializableCreature
{
	public int speed;
	public int special;
	public int defense;
	public int type;
	public int attack;
	public int hp;
	public float score;
	
	public int bodySize;
	public int headSize;
    public int bodyType;
    public int headType;
    public int hasEyebrows;
	
	public SerializableCreature (int speed, int special, int defense, int type, int attack, int hp, float score)
	{
		this.speed = speed;
		this.special = special;
		this.defense = defense;
		this.type = type;
		this.attack = attack;
		this.hp = hp;
		this.score = score;
	}
	
    public SerializableCreature(int bodySize, int headSize, int bodyType, int headType, int hasEyebrows)
    {
        this.bodySize = bodySize;
		this.headSize = headSize;
		this.bodyType = bodyType;
		this.headType = headType;
		this.hasEyebrows = hasEyebrows;
    }
}


