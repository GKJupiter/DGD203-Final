using System;

public class Bandit : Enemy
{

	private const int banditHealth = 15;

	private const int banditMinDamage = 3;
	private const int banditMaxDamage = 10;

	/*
	public int Damage
	{
		get
		{
			return goblinMaxDamage;
			//Random newRandom = new Random();
			//return newRandom.Next(goblinMinDamage, goblinMaxDamage + 1);
		} protected set
		{
			Damage = value;
		}
	}
	*/

	public Bandit()
	{
		Health = banditHealth;
		Damage = banditMaxDamage;
	}


}

