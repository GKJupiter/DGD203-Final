using System.IO;
using System.Numerics;

namespace DGD203_2
{
    public class Game
    {
        #region VARIABLES

        #region Game Constants

        private const int _defaultMapWidth = 5;
        private const int _defaultMapHeight = 13;

        #endregion

        #region Game Variables

        #region Player Variables

        public Player Player { get; private set; }

        private string _playerName;
        private List<Item> _loadedItems;

        #endregion

        #region World Variables

        private Location[] _locations;

        #endregion

        private bool _gameRunning;
        private Map _gameMap;
        private string _playerInput;

        #endregion

        #endregion

        #region METHODS

        #region Initialization

        public void StartGame(Game gameInstanceReference)
        {
            // Generate game environment
            CreateNewMap();

            // Load game
            LoadGame();

            // Deal with player generation
            CreatePlayer();
            

            InitializeGameConditions();

            _gameRunning = true;
            StartGameLoop();
            
        }

        private void CreateNewMap()
        {
            _gameMap = new Map(this, _defaultMapWidth, _defaultMapHeight);
        }

        private void CreatePlayer()
        {
            if (_playerName == null)
            {
                GetPlayerName();
            }

            if (_loadedItems == null)
            {
                _loadedItems = new List<Item>();
            }

            // _playerName may be null. It would be a good idea to put a check here.
            Player = new Player(_playerName, _loadedItems);
        }

        private void GetPlayerName()
        {
            Console.WriteLine("Welcome to the most awesomest RPG game of all time!");
            Console.WriteLine("Would you be kind enough to provide us with your name in the next line?");
            _playerName = Console.ReadLine();

            if (_playerName == "no")
            {
                Console.WriteLine("Player name not entered, giving the name Bastard");
                _playerName = "Bastard";
            }
            else if (_playerName == "No")
            {
                Console.WriteLine("You sure you want your name to be No? If you are sure enough  please confirm your name with yes or no");
                string _nameCheck = Console.ReadLine();
                if (_nameCheck == "Yes")
                {
                    Console.WriteLine("I see it is a bit confusing but I'll get used to it, No");
                    _playerName = "No";
                }
                else if (_nameCheck == "no")
                {
                    Console.WriteLine("Player name not entered, giving the name Bastard");
                    _playerName = "Bastard";
                }
            }
            else if (_playerName == "")
            {
                Console.WriteLine("Player name not entered, giving the name Bastard");
                _playerName = "Bastard";
            }
            else
            {
                Console.WriteLine($"Pleased to meet you {_playerName}, we will have a great adventure together!");
            }
            for (int i = 5; i > 0; i--)
            {
                Thread.Sleep(100); // Sleep for 1 second (1000 milliseconds)
            }
        }

        private void InitializeGameConditions()
        {
            Console.WriteLine("Tip: You can use \"" + "help" + "\" command to see the list of command whenever you want");

            for (int i = 1; i > 0; i--)
            {
                Thread.Sleep(1000); // Sleep for 1 second (1000 milliseconds)
            }

            Console.WriteLine("Game initializing...");

            for (int i = 2; i > 0; i--)
            {
                Console.WriteLine($"{i}..");
                Thread.Sleep(1000); // Sleep for 1 second (1000 milliseconds)
            }

            _gameMap.CheckForLocation(_gameMap.GetCoordinates());
        }


        #endregion

        #region Game Loop

        private void StartGameLoop()
        {
            while (_gameRunning)
            {
                GetInput();
                ProcessInput();
                CheckPlayerPulse();
            }
        }

        private void GetInput()
        {
            _playerInput = Console.ReadLine();
            _playerInput = _playerInput.ToLower();
        }

        private void ProcessInput()
        {
            if (_playerInput == "" || _playerInput == null)
            {
                Console.WriteLine("Give me a command!");
                return;
            }

            switch (_playerInput)
            {
                case "n":
                    _gameMap.MovePlayer(0, 1);
                    break;
                case "s":
                    _gameMap.MovePlayer(0, -1);
                    break;
                case "e":
                    _gameMap.MovePlayer(1, 0);
                    break;
                case "w":
                    _gameMap.MovePlayer(-1, 0);
                    break;
                case "exit":
                    EndGame();
                    Console.WriteLine("Adventures never end");
                    break;
                case "save":
                    SaveGame();
                    Console.WriteLine("Game saved successfully");
                    break;
                case "load":
                    LoadGame();
                    Console.WriteLine("Game loaded successfully");
                    break;
                case "help":
                    Console.WriteLine(HelpMessage());
                    break;
                case "where":
                    _gameMap.CheckForLocation(_gameMap.GetCoordinates());
                    break;
                case "clear":
                    Console.Clear();
                    break;
                case "who":
                    Console.WriteLine($"Who who? What do you mean? Don't you really know your name? What kind of Hero are you? Well you have a great friend which is me. YOU ARE {Player.Name}, I won't forget your name even if you forget it.");
                    break;
                case "take":
                    _gameMap.TakeItem(Player, _gameMap.GetCoordinates());
                    break;
                case "inventory":
                    Player.CheckInventory();
                    break;
                default:
                    Console.WriteLine("Command not recognized. Please type 'help' for a list of available commands");
                    break;
            }
        }

        private void CheckPlayerPulse()
        {
            if (Player.MaxHealth <= 0)
            {
                EndGame();
            }
        }

        private void EndGame()
        {
            Console.WriteLine("We hope you enjoyed your adventure!");
            _gameRunning = false;
        }

        #endregion

        #region Save Management

        private void LoadGame()
        {
            string path = SaveFilePath();

            if (!File.Exists(path)) return;
            
            // Reading the file contents
            string[] saveContent = File.ReadAllLines(path);

            // Set the player name
            _playerName = saveContent[0];

            // Set player coordinates
            List<int> coords = saveContent[1].Split(',').Select(int.Parse).ToList();
            Vector2 coordArray = new Vector2(coords[0], coords[1]);

            // Set player inventory
            _loadedItems = new List<Item>();

            List<string> itemStrings = saveContent[2].Split(',').ToList();

            for (int i = 0; i < itemStrings.Count; i++)
            {
                if (Enum.TryParse(itemStrings[i], out Item result))
                {
                    Item item = result;
                    _loadedItems.Add(item);
                    _gameMap.RemoveItemFromLocation(item);
                }
            }

            _gameMap.SetCoordinates(coordArray);

        }

        private void SaveGame()
        {
            // Player Coordinates
            string xCoord = _gameMap.GetCoordinates()[0].ToString();
            string yCoord = _gameMap.GetCoordinates()[1].ToString();
            string playerCoords = $"{xCoord},{yCoord}";

            // Player inventory
            List<Item> items = Player.Inventory.Items;
            string playerItems = "";
            for (int i = 0; i < items.Count; i++)
            {
                playerItems += items[i].ToString();
                
                if(i != items.Count -1)
                {
                    playerItems += ",";
                }
            }

            string saveContent = $"{_playerName}{Environment.NewLine}{playerCoords}{Environment.NewLine}{playerItems}";

            string path = SaveFilePath();

            File.WriteAllText(path, saveContent);
        }

        private string SaveFilePath()
        {
            // Get the save file path
            string workingDirectory = Environment.CurrentDirectory;
            string projectDirectory = Directory.GetParent(workingDirectory).Parent.Parent.FullName;
            string path = projectDirectory + @"\save.txt";

            return path;
        }

        #endregion

        #region Miscellaneous

        private string HelpMessage()
        {
            return @"Here are the current commands:
N: Go north
S: Go south
W: Go west
E: Go east
where: Display your current location
who: If you forget your name, well... I will remember what kind of idiot you are
take: Pick up an item
inventory: Lets you take a look at your storage
clear: Erase all text on display
save: Save current game
load: Load saved game
exit: Exit the game";

        }

        #endregion

        #endregion
    }
}