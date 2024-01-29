using System;
using static System.Net.Mime.MediaTypeNames;

public class Player
{
	private const int playerMaxHealth = 150;

	private const int playerDefaultMinDamage = 5;
	private const int playerDefaultMaxDamage = 15;

    private HashSet<Item> processedItems = new HashSet<Item>();

    public string Name { get; private set; }
	public int MaxHealth { get; private set; }
	public int CurrentHealth { get; set; }

	public Inventory Inventory { get; private set; }

    public Player(string name, List<Item> inventoryItems)
	{
		Name = name;
		MaxHealth = playerMaxHealth;
        CurrentHealth = MaxHealth;

        Inventory = new Inventory();

		for (int i = 0; i < inventoryItems.Count; i++)
		{
			Inventory.AddItem(inventoryItems[i]);
		}
	}

    public void TakeItem(Item item)
    {
        bool isNewItem = Inventory.AddItem(item);

        if (isNewItem && processedItems.Add(item))
        {
            if (item == Item.Belt)
            {
                MaxHealth += 20;
                Console.WriteLine("You equipped the Belt of the Champion, you gained 20 HP and 5 Attack Power");
            }

            if (item == Item.Horn)
            {
                MaxHealth += 50;
                Console.WriteLine("You equipped the Horn of White Hart, you gained 50 HP");
            }

            if (item == Item.Sword)
            {
                Console.WriteLine("You now have a Valyrian Sword in your hands, your Attack Power is increased by 10");
            }
        }
    }


    public void DropItem(Item item)
	{
		// This will be implemented in the future!
	}

	public void CheckInventory()
	{
		for (int i = 0; i < Inventory.Items.Count; i++)
		{
			Console.WriteLine($"You have {Inventory.Items[i]}");
		}
	}

    public int Damage()
	{
		Random damageRandom = new Random();
		int damage = damageRandom.Next(playerDefaultMinDamage, playerDefaultMaxDamage + 1);

        if (Inventory.Items.Contains(Item.Sword))
        {

            damage += 10;
        }

        if (Inventory.Items.Contains(Item.Belt))
        {

            damage += 5;
        }

        return damage;
	}

    public void TakeDamage(int amount)
    {
        CurrentHealth -= amount; // Change this line
        if (CurrentHealth < 0) CurrentHealth = 0;

        Console.WriteLine($"You took {amount} damage. You have {CurrentHealth} health left");

        if (CurrentHealth <= 0)
        {
            Console.WriteLine("GIT GUD");
        }
    }
    public int Heal()
    {
		int healAmount = 30;

        return healAmount;
    }

    public void RestoreHealth(int amount)
    {
        CurrentHealth = Math.Min(MaxHealth, CurrentHealth + amount);
    }

}
