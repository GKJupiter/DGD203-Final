using System;

public class Inventory
{
	public List<Item> Items {  get; private set; }

	public Inventory()
	{
		Items = new List<Item>();
	}

	public bool AddItem(Item item)
	{
		if (Items.Contains(item))
		{
			return false;
		}

        Items.Add(item);
		return true;
	}

	public void RemoveItem(Item item)
	{
		if (!Items.Contains(item))
		{
			return;
        }

		Items.Remove(item);
    }
}

public enum Item
{
	Belt,
	Horn,
	Sword
}
