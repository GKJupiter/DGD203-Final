using System;
using System.Numerics;
using System.ComponentModel;
using System.Diagnostics;
using DGD203_2;

public class Map
{
	private Game _theGame;

	private Vector2 _coordinates;

	private int[] _widthBoundaries;
	private int[] _heightBoundaries;

	private Location[] _locations;


	public Map(Game game, int width, int height)
	{
		_theGame = game;

		// Setting the width boundaries
		int widthBoundary = (width - 1) / 2;

        _widthBoundaries = new int[2];
        _widthBoundaries[0] = -widthBoundary;
		_widthBoundaries[1] = widthBoundary;

		// Setting the height boundaries
        int heightBoundary = (height - 1) / 2;

        _heightBoundaries = new int[2];
		_heightBoundaries[0] = -heightBoundary;
		_heightBoundaries[1] = heightBoundary;

		// Setting starting coordinates
        _coordinates = new Vector2(0,0);

		GenerateLocations();

		// Display result message
		Console.WriteLine($"Westeros has been created in {width}x{height} size");
    }

    #region Coordinates

    public Vector2 GetCoordinates()
	{
		return _coordinates;
	}

	public void SetCoordinates(Vector2 newCoordinates)
	{
		_coordinates = newCoordinates;
	}

	#endregion

	#region Movement

	public void MovePlayer(int x, int y)
	{
		int newXCoordinate = (int)_coordinates[0] + x;
        int newYCoordinate = (int)_coordinates[1] + y;

		if (!CanMoveTo(newXCoordinate, newYCoordinate)) 
		{
            Console.WriteLine("You've encountered the boundaries of your amazing world");
            return;
        }

		_coordinates[0] = newXCoordinate;
		_coordinates[1] = newYCoordinate;

		CheckForLocation(_coordinates);
    }

	private bool CanMoveTo(int x, int y)
	{
		return !(x < _widthBoundaries[0] || x > _widthBoundaries[1] || y < _heightBoundaries[0] || y > _heightBoundaries[1]);
	}

	#endregion

	#region Locations

	private void GenerateLocations()
	{
        _locations = new Location[14];

        Vector2 winterfellLocation = new Vector2(0, 5);
		List<Item> winterfellItems = new List<Item>();
		winterfellItems.Add(Item.Sword);
        Location winterfell = new Location("Winterfell", LocationType.City, winterfellLocation, winterfellItems);
        _locations[0] = winterfell;

        Vector2 theValeLocation = new Vector2(2, 3);
        Location theVale = new Location("The Vale of Arryn", LocationType.City, theValeLocation);
        _locations[1] = theVale;

        Vector2 ironIslesLocation = new Vector2(-2, -2);
        Location ironIsles = new Location("Iron Isles", LocationType.City, ironIslesLocation);
        _locations[2] = ironIsles;

        Vector2 riverrunLocation = new Vector2(1, 1);
        Location riverrun = new Location("Riverrun", LocationType.City, riverrunLocation);
        _locations[3] = riverrun;

        Vector2 casterlyRockLocation = new Vector2(-1, -1);
        Location casterlyRock = new Location("Casterly Rock", LocationType.City, riverrunLocation);
        _locations[4] = casterlyRock;

        Vector2 stormsEndLocation = new Vector2(2, -2);
        List<Item> stormsEndItems = new List<Item>();
        stormsEndItems.Add(Item.Horn);
        Location stormsEnd = new Location("Storm's End", LocationType.City, stormsEndLocation, stormsEndItems);
        _locations[5] = stormsEnd;

        Vector2 highgardenLocation = new Vector2(-1, -4);
        Location highgarden = new Location("HighGarden", LocationType.City, highgardenLocation);
        _locations[6] = highgarden;

        Vector2 dorneLocation = new Vector2(0, -6);
        List<Item> dorneItems = new List<Item>();
        dorneItems.Add(Item.Belt);
        Location dorne = new Location("Dorne", LocationType.City, dorneLocation, dorneItems);
        _locations[7] = dorne;

        Vector2 firstCombatLocation = new Vector2(-1, 1);
		Location firstCombat = new Location("First Combat", LocationType.Combat, firstCombatLocation);
		_locations[8] = firstCombat;

        Vector2 secondCombatLocation = new Vector2(-2, -3);
        Location secondCombat = new Location("Second Combat", LocationType.Combat, secondCombatLocation);
        _locations[9] = secondCombat;

        Vector2 thirdCombatLocation = new Vector2(2, -5);
        Location thirdCombat = new Location("Third Combat", LocationType.Combat, thirdCombatLocation);
        _locations[10] = thirdCombat;

        Vector2 fourthCombatLocation = new Vector2(-2, 5);
        Location fourthCombat = new Location("Fourth Combat", LocationType.Combat, fourthCombatLocation);
        _locations[11] = fourthCombat;

        Vector2 fifthCombatLocation = new Vector2(2, 6);
        Location FifthCombat = new Location("Fifth Combat", LocationType.Combat, fifthCombatLocation);
        _locations[12] = FifthCombat;

        Vector2 sixthCombatLocation = new Vector2(-2, -6);
        Location sixthCombat = new Location("Sixth Combat", LocationType.Combat, sixthCombatLocation);
        _locations[13] = sixthCombat;
    }

	public void CheckForLocation(Vector2 coordinates)
	{
        Console.WriteLine($"You are now standing on {_coordinates[0]},{_coordinates[1]}");

        if (IsOnLocation(_coordinates, out Location location))
        {
            if (location.Type == LocationType.Combat)
			{
				if (location.CombatAlreadyHappened) return;

				Console.WriteLine("Prepare to fight!");
				Combat combat = new Combat(_theGame, location);

				combat.StartCombat();

			} else
			{
				Console.WriteLine($"You are in {location.Name} {location.Type}");

				if (HasItem(location))
				{
					Console.WriteLine($"There is a {location.ItemsOnLocation[0]} here");
				}
			}
        }
    }

	private bool IsOnLocation(Vector2 coords, out Location foundLocation)
	{
		for (int i = 0; i < _locations.Length; i++)
		{
			if (_locations[i].Coordinates == coords)
			{
				foundLocation = _locations[i];
				return true;
			}
		}

		foundLocation = null;
		return false;
	}

	private bool HasItem(Location location)
	{
		return location.ItemsOnLocation.Count != 0;

		// ---- THE LONG FORM ----
		//if (location.ItemsOnLocation.Count == 0)
		//{
		//	return false;
		//} else
		//{
		//	return true;
		//}
	}

	public void TakeItem(Location location)
	{

	}

	public void TakeItem(Player player, Vector2 coordinates)
	{
		if (IsOnLocation(coordinates, out Location location))
		{
			if (HasItem(location))
			{
				Item itemOnLocation = location.ItemsOnLocation[0];

				player.TakeItem(itemOnLocation);
				location.RemoveItem(itemOnLocation);
                return;
			}
		}

		Console.WriteLine("There is nothing to take here!");
	}

	public void RemoveItemFromLocation(Item item)
	{
		for (int i = 0; i < _locations.Length; i++)
		{
			if (_locations[i].ItemsOnLocation.Contains(item))
			{
				_locations[i].RemoveItem(item);
			}
		}
	}

	#endregion
}