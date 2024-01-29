using DGD203_2;
using System;

public class Combat
{
    #region REFERENCES

    private Game _theGame;
    public Player Player { get; private set; }

    public List<Enemy> Enemies { get; private set; }

	private Location _location;

    #endregion

    #region VARIABLES

    private const int maxNumberOfEnemies = 5;

    private bool _isOngoing;


    private string _playerInput;

    #endregion

    #region CONSTRUCTOR

    public Combat(Game game, Location location)
	{
		_theGame = game;
		Player = game.Player;

		_isOngoing = false;

		_location = location;

		Random rand = new Random();
		int numberOfEnemies = rand.Next(1, maxNumberOfEnemies + 1);

		Enemies = new List<Enemy>();
		for (int i = 0; i < numberOfEnemies; i++)
		{
			Enemy nextEnemy = new Bandit();
			Enemies.Add(nextEnemy);
		}
	}

    #endregion


    #region METHODS

    #region Initialization & Loop

    public void StartCombat()
	{
        _isOngoing = true;

		while (_isOngoing)
		{
			GetInput();
			ProcessInput();

			if (!_isOngoing) break;

			ProcessEnemyActions();
			CheckPlayerPulse();
		}
	}

    private void GetInput()
    {
        Console.WriteLine($"There {(Enemies.Count == 1 ? "is" : "are")} {Enemies.Count} bandit{(Enemies.Count == 1 ? "" : "s")} in front of you. What do you want to do?");
        for (int i = 0; i < Enemies.Count; i++)
        {
            Console.WriteLine($"[{i + 1}]: Attack bandit {i + 1}");
        }
        Console.WriteLine($"[{Enemies.Count + 1}]: Try to flee (50% chance)");
        Console.WriteLine($"[{Enemies.Count + 2}]: Heal yourself");

        _playerInput = Console.ReadLine();  // Assign user input to _playerInput
    }




    private void ProcessInput()
	{
        if (_playerInput == "" || _playerInput == null)
        {
            Console.WriteLine("You can't just stand still, you are in a fight!");
            return;
        }

		ProcessChoice(_playerInput);
    }


	private void ProcessChoice(string choice)
	{
		if (Int32.TryParse(choice, out int value)) // When the command is an integer
		{
			if (value > Enemies.Count + 2)
			{
				Console.WriteLine("That is not a valid choice");
			} else
			{
				if (value == Enemies.Count + 1) 
				{
					TryToFlee();
				}
				else if (value == Enemies.Count + 2) 
				{
					HealPlayer();
				}
				else
				{
					HitEnemy(value);
				}
			}
		}
		else // When the command is not an integer
		{
			Console.WriteLine("You don't make any sense. Quit babbling, they are going to kill you!");
		}
	}

	private void CheckPlayerPulse()
	{
		if (Player.MaxHealth <= 0)
		{
			EndCombat();
		}
	}

    private void EndCombat()
    {
        _isOngoing = false;
		_location.CombatHappened();
    }

    #endregion

    #region Combat

    private void TryToFlee()
	{
		Random rand = new Random();
		double randomNumber = rand.NextDouble();

		if (randomNumber >= 0.5f)
		{
            for (int i = 1; i > 0; i--)
            {
                Thread.Sleep(100); // Sleep for 1 second (1000 milliseconds)
            }
            Console.WriteLine("You fled! You are a coward maybe, but a living one!");
            Console.WriteLine("[---------------------------------------------------------]");
            EndCombat();
            Console.WriteLine("You can continue your journey");
        } else
		{
			Console.WriteLine("You cannot flee because a bandit is in your way");
            Console.WriteLine("[---------------------------------------------------------]");
        }

    }

    private void HitEnemy(int index)
	{
		int enemyIndex = index - 1;
		int playerDamage = Player.Damage();

		Enemies[enemyIndex].TakeDamage(playerDamage);
		Console.WriteLine($"The bandit you attacked takes {playerDamage} damage!");
        
		for (int i = 3; i > 0; i--)
        {
            Thread.Sleep(100); // Sleep for 1 second (1000 milliseconds)
        }

        if (Enemies[enemyIndex].Health <= 0)
		{
			Console.WriteLine("This bandit is gone!");
			Enemies.RemoveAt(enemyIndex);
		}

    }
    private void HealPlayer()
    {
        int playerHeal = Player.Heal();

        Player.RestoreHealth(playerHeal);
        Console.WriteLine($"You healed yourself.Your current HP is {Player.CurrentHealth}!");

        for (int i = 3; i > 0; i--)
        {
            Thread.Sleep(100); // Sleep for 1 second (1000 milliseconds)
        }

        if (Player.CurrentHealth == Player.MaxHealth)
        {
            Console.WriteLine("You are fully healed!");
           
        }
        Console.WriteLine("[---------------------------------------------------------]");
    }





    private void ProcessEnemyActions()
	{
		if (Enemies.Count == 0)
		{
            for (Double i = 3.0; i > 0; i--)
            {
                Thread.Sleep(100); // Sleep for 1 second (1000 milliseconds)
            }
            Console.WriteLine("You defeated all your enemies!");
			EndCombat();
            Console.WriteLine("[---------------------------------------------------------]");
            Console.WriteLine("You can continue your journey");
        }


        for (int i = 0; i < Enemies.Count; i++)
		{
			int banditDamage = Enemies[i].Damage;
			Player.TakeDamage(banditDamage);
		}
	}



	#endregion

	#endregion

}
