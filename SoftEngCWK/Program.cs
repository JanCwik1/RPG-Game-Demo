using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

namespace SoftEngCWK
{
    [Serializable]
    internal class Program
    {
        private int levelTrackInt = 0; //get level number from save file and set level to that value
        private int userMenuChoice;
        private int currentSave;
        private List<string> loadNames;
        private string playerLoadNameCheck;
        private string gameConfigFileName = "gameConfig.txt";
        private static char configFileSeparatorSymbol = ':';
        private static int configID = 1;
        private string gameStateFileName = "";
        private int difficulty;
        private int playerNumber;
        private int hardDifficultyMulitplier;
        private int mediumDifficultyMultiplier;
        private int easyDifficultyMultiplier;
        private int initialPlayerHealth;
        private int initialPlayerAttack;
        private int playerStatsToUse;
        private List<PlayerCreator> players;
        private List<Level> levels;
        private Monster wolf = new Monster("Wolf", 5, 1, 10);
        private Monster goblin = new Monster("Goblin", 7, 3, 15);
        private Monster troll = new Monster("Troll", 10, 5, 25);

        public string GetValidPlayerInputString( string[] validStrings)
        {
            bool isValidEntry = false;
            string queryString = "";
            int positionInString = 0;
            do
            {
                try
                {
                    queryString = Console.ReadLine();
                    positionInString = Array.IndexOf(validStrings, queryString);
                    if (positionInString > -1)
                    {
                        isValidEntry = true;
                    }
                    else
                    {
                        Console.WriteLine($"The entry: {queryString} is not a valid input. Try again.\n");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"The entry: {queryString} is not a valid input. Try again.\n");
                }
            } while (!isValidEntry);
            return queryString;
        }

        public int ValidPlayerInputStringToInt(string playerInput)
        {
            int playerInputInt;
            bool validInt;
            if (validInt = int.TryParse(playerInput, out playerInputInt))
            {
                if (playerInputInt < 0)
                {
                    return 0;
                }
                return playerInputInt;
            }
            else
            {
                return 0;
            }
        }

        private void SaveGameToFile()
        {
            Random rand = new Random();
            int uniqueID = 0;
            uniqueID = rand.Next();
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create($"save_{this.players[0].GetName()}_SaveNumber_{this.currentSave}_ID_{uniqueID}.bin");

            bf.Serialize(file, this.gameConfigFileName);
            bf.Serialize(file, this.gameStateFileName);
            bf.Serialize(file, this.difficulty);
            bf.Serialize(file, this.playerNumber);
            bf.Serialize(file, this.hardDifficultyMulitplier);
            bf.Serialize(file, this.mediumDifficultyMultiplier);
            bf.Serialize(file, this.easyDifficultyMultiplier);
            bf.Serialize(file, this.initialPlayerHealth);
            bf.Serialize(file, this.initialPlayerAttack);
            bf.Serialize(file, this.playerStatsToUse);
            bf.Serialize(file, this.players);
            bf.Serialize(file, this.levels);
            bf.Serialize(file, this.wolf);
            bf.Serialize(file, this.goblin);
            bf.Serialize(file, this.troll);
            bf.Serialize(file, this.levelTrackInt);
            
            file.Close();
        }

        private void LoadGameFromFile(string saveToLoad)
        {
            try
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.OpenRead(saveToLoad);

                this.gameConfigFileName = (string)bf.Deserialize(file);
                this.gameStateFileName = (string)bf.Deserialize(file);
                this.difficulty = (int)bf.Deserialize(file);
                this.playerNumber = (int)bf.Deserialize(file);
                this.hardDifficultyMulitplier = (int)bf.Deserialize(file);
                this.mediumDifficultyMultiplier = (int)bf.Deserialize(file);
                this.easyDifficultyMultiplier = (int)bf.Deserialize(file);
                this.initialPlayerHealth = (int)bf.Deserialize(file);
                this.initialPlayerAttack = (int)bf.Deserialize(file);
                this.playerStatsToUse = (int)bf.Deserialize(file);
                this.players = (List<PlayerCreator>)bf.Deserialize(file);
                this.levels = (List<Level>)bf.Deserialize(file);
                this.wolf = (Monster)bf.Deserialize(file);
                this.goblin = (Monster)bf.Deserialize(file);
                this.troll = (Monster)bf.Deserialize(file);
                this.levelTrackInt = (int)bf.Deserialize(file);
            }
            catch
            {
                Console.WriteLine("Did not load a save.");
            }
            
        }
        
        private void AssignDifficulty()
        {
            bool valid = false;
            string difficultyStr;
            int difficulty;
            string[] validDifficulties = { "1", "2", "3" };

            Console.WriteLine("What difficulty do you wish to play:\n(enter the number corsponding to the difficulty)\n\n 1. Easy\n 2. Medium\n 3. Hard");
            difficultyStr = GetValidPlayerInputString(validDifficulties);
            difficulty = ValidPlayerInputStringToInt(difficultyStr);

            if (difficulty == 1)
            {
                Console.WriteLine("Please enjoy the Easy difficulty.");
                this.difficulty = difficulty;
            }
            else if (difficulty == 2)
            {
                Console.WriteLine("Please enjoy the Medium difficulty.");
                this.difficulty = difficulty;
            }
            else if (difficulty == 3)
            {
                Console.WriteLine($"Please enjoy the Hard difficulty.");
                this.difficulty = difficulty;
            }
        }

        private void AssignPlayerStatsToUse()
        {
            if (this.difficulty == 1)
            {
                this.playerStatsToUse *= this.easyDifficultyMultiplier;
            }
            else if (this.difficulty == 2)
            {
                this.playerStatsToUse *= this.mediumDifficultyMultiplier;
            }
            else if (this.difficulty == 3)
            {
                this.playerStatsToUse *= this.hardDifficultyMulitplier;
            }
        }

        private string CreateNewGameConfig() //create new game config including difficulty from weather, and player stats
        {
            //set difficulty
            //set number of players

            string configFileName;
            const int hardDifficultyMulitplier = 1;
            const int mediumDifficultyMultiplier = 2;
            const int easyDifficultyMultiplier = 3;
            const int initialPlayerHealth = 3;
            const int initialPlayerAttack = 3;
            const int playerStatsToUse = 5;
            string[] maxPlayers = { "1", "2", "3", "4"};
            List<PlayerCreator> players = new List<PlayerCreator>();

            this.AssignDifficulty();
            Console.WriteLine("How many players will be playing?");
            string playerNumStr = GetValidPlayerInputString(maxPlayers); 
            int playerNumber = ValidPlayerInputStringToInt(playerNumStr);
            Console.WriteLine("What do you want this config's name to be? (This must be a single unique word which hasn't been used already, and only contain alphanumeric characters): ");
            configFileName = (Console.ReadLine() + "Config.txt");

            using (StreamWriter writer = File.CreateText(configFileName))
            {
                writer.WriteLine($"Difficulty:{difficulty}");
                writer.WriteLine($"NumberOfPlayers:{playerNumber}");
                writer.WriteLine($"hardDifficultyMulitplier:{hardDifficultyMulitplier}");
                writer.WriteLine($"mediumDifficultyMultiplier:{mediumDifficultyMultiplier}");
                writer.WriteLine($"easyDifficultyMultiplier:{easyDifficultyMultiplier}");
                writer.WriteLine($"initialPlayerHealth:{initialPlayerHealth}");
                writer.WriteLine($"initialPlayerAttack:{initialPlayerAttack}");
                writer.WriteLine($"playerStatsToUse:{playerStatsToUse}");
            }
            return (configFileName);
        }

        private void CreatePlayers(int playerNumber, int playerStatsToUse, int initialPlayerHealth, int initialPlayerAttack)
        {
            int errorCode;
            int health, attack;
            int healthToAdd, attackToAdd;
            string[] validStats = { "health", "attack" };
            const string healthStat = "health";
            const string attackStat = "attack";
            this.players = new List<PlayerCreator>();
            PlayerCreator player;

            for (int i = 0; i< playerNumber; i++)
            {
                player = new PlayerCreator();

                Console.WriteLine($"What is the name of player {i + 1}: ");
                string playerName = Console.ReadLine();
                player.SetPlayerName(playerName);

                Console.WriteLine($"\nGame difficulty {this.difficulty}");
                player.SetDifficulty(this.difficulty);

                player.SetPlayerId(i + 1);
                player.SetStatPointsToDistribute();
                
                do
                {
                    health = this.initialPlayerHealth;
                    attack = this.initialPlayerAttack;
                    healthToAdd = 0;
                    attackToAdd = 0;

                    Console.WriteLine("When prompted enter a number coresponding to how many points you want into the specified player attribute category.");
                    Console.WriteLine("If you want to increase your Health stat, type: health"); // by three points
                    Console.WriteLine("If you want to increase your Attack stat, type: attack"); // by two points
                    for (int playerStatsRemaining = player.GetStatPointsToDistribute(); playerStatsRemaining > 0;)
                    {
                        Console.WriteLine("You have: " + playerStatsRemaining + " player stat points remaining. Your current stats are:\n Health: " + (health) + " \n Attack: " + (attack));
                        string targetStat = GetValidPlayerInputString(validStats);

                        Console.WriteLine($"How many of your stat points do you want to spend on {targetStat}");
                        int statValue = ValidPlayerInputStringToInt(Console.ReadLine());
                        switch (targetStat)
                        {
                            case healthStat:
                                health += statValue;
                                healthToAdd += statValue;
                                break;
                            case attackStat:
                                attack += statValue;
                                attackToAdd += statValue;
                                break;
                        }
                        playerStatsRemaining -= statValue;
                    }
                    errorCode = player.DistributeStatPoints(healthToAdd , attackToAdd ); //used to be -3 for both
                } while (errorCode != 0);

                Console.WriteLine($"Created player {i+1} of name {player.GetName()} with: {player.GetHealth()} Health and {player.GetAttack()} Attack.");
                this.players.Add(player);
            }
        }

        private List<Monster> CreateMonsters(int monsterNumber)//input is: number of monsters,
        {
            List<Monster> monsters = null; new List<Monster>();
            for (int i=0; i<= monsterNumber; i++)
            {
                monsters = new List<Monster>();
                if (monsterNumber == 0)
                {
                    monsters.Add(new Monster(wolf.GetName(), wolf.GetHealth(), wolf.GetAttack(), wolf.GetPointsForDeath()) );
                }
                else if (monsterNumber >= 3 && monsterNumber < 7)
                {
                    monsters.Add(new Monster(wolf.GetName(), wolf.GetHealth(), wolf.GetAttack(), wolf.GetPointsForDeath()));
                    monsters.Add(new Monster(goblin.GetName(), goblin.GetHealth(), goblin.GetAttack(), goblin.GetPointsForDeath()));
                }
                else if (monsterNumber >= 8)
                {
                    monsters.Add(new Monster(wolf.GetName(), wolf.GetHealth(), wolf.GetAttack(), wolf.GetPointsForDeath()));
                    monsters.Add(new Monster(goblin.GetName(), goblin.GetHealth(), goblin.GetAttack(), goblin.GetPointsForDeath()));
                    monsters.Add(new Monster(troll.GetName(), troll.GetHealth(), troll.GetAttack(), troll.GetPointsForDeath()));
                }
            }
            //monsters = wolf, goblin, troll
            return monsters;
        }

        private static void CreateDefaultConfig()
        {
            const int gameDifficulty = 2;
            const int playerNumber = 1;
            const int hardDifficultyMulitplier = 1;
            const int mediumDifficultyMultiplier = 2;
            const int easyDifficultyMultiplier = 3;
            const int initialPlayerHealth = 3;
            const int initialPlayerAttack = 3;
            const int playerStatsToUse = 5;


            using (StreamWriter writer = File.CreateText("DefaultConfigFile.txt"))
            {
                writer.WriteLine($"Difficulty:{gameDifficulty}");
                writer.WriteLine($"NumberOfPlayers:{playerNumber}");
                writer.WriteLine($"hardDifficultyMulitplier:{hardDifficultyMulitplier}");
                writer.WriteLine($"mediumDifficultyMultiplier:{mediumDifficultyMultiplier}");
                writer.WriteLine($"easyDifficultyMultiplier:{easyDifficultyMultiplier}");
                writer.WriteLine($"initialPlayerHealth:{initialPlayerHealth}");
                writer.WriteLine($"initialPlayerAttack:{initialPlayerAttack}");
                writer.WriteLine($"playerStatsToUse:{playerStatsToUse}");
            }
        }

        private void LoadExistingConfig(string configFileName)
        {
            const string defaultConfigFileName = "DefaultConfigFile.txt";
            string[] useDefaultConfigChoices= { "yes", "no"};

            for (int attempts = 0; attempts <= 5; attempts++)
            {
                if (!File.Exists(configFileName))
                {
                    Console.WriteLine($"Unfortunately the config file {configFileName} could not be found.");
                    Console.WriteLine("Do you wish to use the default config file instead? (type yes or no)");
                    string useDefaultConfigFile = GetValidPlayerInputString(useDefaultConfigChoices);

                    if (useDefaultConfigFile == "yes" || configFileName == defaultConfigFileName)
                    {
                        CreateDefaultConfig();
                        configFileName = defaultConfigFileName;
                    }
                    else
                    {
                        Console.WriteLine("What is the name of the config file you wish to use?");
                        configFileName = Console.ReadLine();
                    }
                }
                else
                {
                    Console.WriteLine($"Loaded the config file {configFileName}.");
                    break;
                }
            }

            try
            {
                using (StreamReader gameConfigFile = new StreamReader(configFileName))
                {
                    this.difficulty = int.Parse(configValueFromConfigLine(gameConfigFile.ReadLine()));
                    this.playerNumber = int.Parse(configValueFromConfigLine(gameConfigFile.ReadLine()));
                    this.hardDifficultyMulitplier = int.Parse(configValueFromConfigLine(gameConfigFile.ReadLine()));
                    this.mediumDifficultyMultiplier = int.Parse(configValueFromConfigLine(gameConfigFile.ReadLine()));
                    this.easyDifficultyMultiplier = int.Parse(configValueFromConfigLine(gameConfigFile.ReadLine()));
                    this.initialPlayerHealth = int.Parse(configValueFromConfigLine(gameConfigFile.ReadLine()));
                    this.initialPlayerAttack = int.Parse(configValueFromConfigLine(gameConfigFile.ReadLine()));
                    this.playerStatsToUse = int.Parse(configValueFromConfigLine(gameConfigFile.ReadLine()));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred, using default config.");
                CreateDefaultConfig();
                configFileName = defaultConfigFileName;
                StreamReader gameConfigFile = new StreamReader(configFileName);
            }
            this.AssignPlayerStatsToUse();

            Console.WriteLine("\nLoaded Config File.\n");
        }

        private void GenerateLevels()
        {
            int NumLevelsToGen = 0;
            if (this.difficulty == 1)
            {
                NumLevelsToGen = 4;
            }
            else if (this.difficulty == 2)
            {
                NumLevelsToGen = 6;
            }
            else if (this.difficulty == 3)
            {
                NumLevelsToGen = 8;
            }


            Console.WriteLine($"Loaded Difficulty:  {this.difficulty}");
            Console.WriteLine($"Levels to generate:  {NumLevelsToGen}");
            this.levels = new List<Level>();
            List<Monster> monsters = null;

            String[,] levelTypes =
            {
                {"Wilderness", "Hostile"},
                {"Shop", "Shop"},
                {"Dungeon", "Hostile" },
                {"Shop", "Shop"}
            }; //the order of the array determines what level is generated
            Level level;
            int levelsGenerated = 0;

            for (int levelNumber = 0; levelsGenerated < NumLevelsToGen; levelNumber++, levelsGenerated++)
            {

                if (levelNumber == (levelTypes.GetLength(0)))
                {
                    levelNumber = 0;
                }
                //Console.WriteLine($"\n123{levelNumber}\n {levelTypes.Length}456\n"); //used for testing number of levels generated

                monsters = null;
                level = new Level(levelTypes[levelNumber, 0], levelTypes[levelNumber, 1]);
                if (levelNumber == 0)
                {
                    monsters = CreateMonsters(levelsGenerated);
                    level.AddMonsters(monsters);
                }
                else if (levelNumber == 2)
                {
                    monsters = CreateMonsters(levelsGenerated*3);
                    level.AddMonsters(monsters);
                }

                this.levels.Add(level);
                Console.WriteLine($"Level {levelsGenerated + 1} generated");
            }
        }

        private bool CheckPlayersDead()
        {
            bool playersDead = false;
            // Check if all players are dead
            if (players.All(p => !p.CheckAlive()))
            {
                playersDead = true; // Break out of the loop if all players are dead
            }
            return playersDead;
        }

        private void StartNewGame()
        {
            string chosenConfigFileName;
            string[] validConfigChoices = { "1", "2", "3"};
            Console.WriteLine("\nStarting new game");
            Console.WriteLine("\nDo you want to (select numerical value associated with the choice)\n1. Create New Config\n2. Load Existing Config\n3. Use Default Config");

            string configChoiceStr = GetValidPlayerInputString(validConfigChoices);
            int configChoice = ValidPlayerInputStringToInt(configChoiceStr);
            switch (configChoice)
            {
                case 1:
                    string newConfigName = CreateNewGameConfig();
                    LoadExistingConfig(newConfigName);
                    break;
                case 2:
                    Console.WriteLine("Which config file name do you wish to choose (example: config2.txt): ");
                    chosenConfigFileName = Console.ReadLine();
                    LoadExistingConfig(chosenConfigFileName);//checks if the chosen config exists, if not makes the user type a valid one, or they choose to use default.
                    break;
                case 3:
                    chosenConfigFileName = "DefaultConfigFile.txt";
                    LoadExistingConfig(chosenConfigFileName);//checks if the default config exists, if not create it. Then use it to initialise the game.
                    break;
            }

            GenerateLevels();
            CreatePlayers(this.playerNumber, this.playerStatsToUse, this.initialPlayerHealth, this.initialPlayerAttack);
            GameLoop();
        }

        private string GameLoop()
        {
            int healthPotionPrice = 5;
            int itemsPlayerCanAfford = 0;
            int numItemsToBuyInt;
            int itemCost;
            int goldAfterBuy;
            string numItemsToBuyStr;
            string playerShopExitQuery;
            string healthPotionEffect = "regenerates 3 player hearts";
            string healthPotionKeyWord = "health";
            string healthPotionName = "Health Elixir";
            string correctPlayer = (this.playerNumber < 2) ? "player" : "players";
            string[] validBuySellAnswer = { "buy", "sell", "exit" }; //used to check if player input is valid in shop
            string[] validPlayGameChoices = { "Fight", "Item", "Quit" };
            string[] validBuyItemAnswer = { healthPotionKeyWord, "no"};
            string[] validExitAnswer = { "exit" };
            List<string> validMonsterFightChoice;
            Dictionary<string, int> itemsBought;
            
            string difficultyToString = null;
            switch (this.difficulty)
            {
                case 1:
                    difficultyToString = "Easy";
                    break;
                case 2:
                    difficultyToString = "Medium";
                    break;
                case 3:
                    difficultyToString = "Hard";
                    break;
            }
            string input = null;
            Console.WriteLine("\nYou are now playing the game Enjoy.");
            Console.WriteLine($"\nThere are {this.playerNumber} {correctPlayer}, on {difficultyToString} difficulty.");

            for (int level = this.levelTrackInt; level < this.levels.Count; level++)
            {
                this.levelTrackInt = level;
                Level currentLevel = this.levels[level];
                Console.WriteLine($"You enter the {level+1} level, it is the {currentLevel.GetName()}");

                if (currentLevel.GetCategory() == "Hostile")
                {
                    Console.WriteLine($"You encounter {currentLevel.GetNumberOfMonsters()} monsters.");
                    currentLevel.GetTotalMonstersDetail();
                    //player 1 to n attack monsters

                    List<Monster> CurrentLevelMonsters = currentLevel.GetMonsters();
                    do
                    {
                        validMonsterFightChoice = new List<string>();
                        int countMonster = 1;
                        foreach (Monster monster in CurrentLevelMonsters)
                        {
                            Console.WriteLine($"\n\n{countMonster}. {monster.GetName()} monster with {monster.GetHealth()} health.");
                            validMonsterFightChoice.Add((countMonster).ToString());
                            countMonster++;
                        }


                        foreach (PlayerCreator player in this.players)
                        {
                            if (player.CheckAlive() && CurrentLevelMonsters.Count > 0)
                            {
                                Console.WriteLine($"\nPlayer {player.GetPlayerId()}, with the name {player.GetName()}, what do you intend to do?");
                                Console.WriteLine("\nYou can Fight, Item, or Quit.");
                                input = GetValidPlayerInputString(validPlayGameChoices);
                                if (input == "Fight")
                                {
                                    Console.WriteLine("\nWhich monster do you wish to attack? (Type the number associated with the monster)");
                                    foreach (var num in validMonsterFightChoice)
                                    {
                                        Console.WriteLine($"Valid monster target: {num}");
                                    }
                                    string targetMonsterStr = GetValidPlayerInputString(validMonsterFightChoice.ToArray());
                                    int targetMonster = ValidPlayerInputStringToInt(targetMonsterStr);

                                    CurrentLevelMonsters[targetMonster - 1].TakeDamage(player.GetAttack());
                                    Console.WriteLine($"The {CurrentLevelMonsters[targetMonster - 1].GetName()} took {player.GetAttack()} damage and is on {CurrentLevelMonsters[targetMonster - 1].GetHealth()} health!");
                                    
                                    if (CurrentLevelMonsters[targetMonster - 1].CheckAlive() == false)
                                    {
                                        Console.WriteLine($"\nCongratulations! You killed a {CurrentLevelMonsters[targetMonster - 1].GetName()}");
                                        player.IncreasePoints(CurrentLevelMonsters[targetMonster - 1].GetPointsForDeath());

                                        Console.WriteLine($"Increased the points of player {player.GetName()} by {CurrentLevelMonsters[targetMonster - 1].GetPointsForDeath()}, bringing the points to {player.GetPoints()}");
                                        player.AddItemToInventory(CurrentLevelMonsters[targetMonster - 1].AssignLoot());
                                        CurrentLevelMonsters.RemoveAt(targetMonster - 1); // Remove the dead monster from the list
                                        validMonsterFightChoice.Remove((targetMonster-1).ToString());
                                    }
                                }
                                else if (input == "Item")
                                {
                                    int healthElixirUse = 0;
                                    Console.WriteLine($"You can only use Health Elixirs. Player {player.GetName()}, you have {player.GetPlayerHealthElixir()} Health Elixirs. How many do you wish to use?");
                                    healthElixirUse = ValidPlayerInputStringToInt(Console.ReadLine()); 
                                    player.UseHealthElixir(healthElixirUse);
                                }
                                else if (input == "Quit")
                                {
                                    return null;
                                }
                                this.SaveGameToFile();
                                this.currentSave += 1;
                            }   
                        }
                        
                        Random rand = new Random();
                        foreach (Monster monster in CurrentLevelMonsters) //
                        {
                            if (monster.CheckAlive())
                            {
                                PlayerCreator target = monster.ChooseRandomPlayer(players);
                                if (target != null)
                                {
                                    target.TakeDamage(monster.GetAttack());
                                    Console.WriteLine($"\n\n{monster.GetName()} monster attacks player {target.GetName()} dealing {monster.GetAttack()} damage, bringing down their health to {target.GetHealth()}!");
                                }
                            }
                        } //monsters do damage to the players choosing a random player as a target

                        if (CheckPlayersDead() == true)
                        {
                            break;
                        }
                    } while (CurrentLevelMonsters.Count > 0);

                    if (CheckPlayersDead() == true)
                    {
                        break;
                    }
                    Console.WriteLine("Well done you completed this level!, press any key to continue to the next level.\n");
                    Console.ReadLine();
                }
                else if (currentLevel.GetCategory() == "Shop")
                {
                    string playerBuyCheck = "";
                    string playerBuyItemCheck = "";
                    Console.WriteLine($"\nThere are no monsters, but you can buy and sell items here!");
                    foreach (PlayerCreator player in this.players)
                    {
                        if (player.CheckAlive())
                        {
                            do
                            {
                                Console.WriteLine($"\n{player.GetName()} do you want to buy or sell anything? (type buy or sell, you can leave the shop by typing: exit)");

                                playerBuyCheck = this.GetValidPlayerInputString(validBuySellAnswer);

                                if (playerBuyCheck == "buy")
                                {
                                    Console.WriteLine($"The shop currently sells Health Elixirs, 1 health potion costs {healthPotionPrice} gold coins and {healthPotionEffect}. To buy this item type the key word: {healthPotionKeyWord}");
                                    Console.WriteLine("If you want to buy an item type in the key word associated with the item, if you don't want to buy anything type: no");
                                    playerBuyItemCheck = GetValidPlayerInputString(validBuyItemAnswer);

                                    if (playerBuyItemCheck == "health")
                                    {
                                        //calculate how many player can afford
                                        itemsPlayerCanAfford = player.GetAffordableItemCount(healthPotionPrice);
                                        do
                                        {
                                            try
                                            {
                                                Console.WriteLine($"You can afford {itemsPlayerCanAfford} Health Potions, how many do you want to buy?");
                                                numItemsToBuyStr = Console.ReadLine();
                                                numItemsToBuyInt = int.Parse(numItemsToBuyStr);
                                            }
                                            catch (Exception e)
                                            {
                                                Console.WriteLine("Please enter a valid number, example: 10");
                                                numItemsToBuyStr = "falseInput";
                                                numItemsToBuyInt = 0;
                                            }
                                            
                                        } while (player.ValidNumberCheck(numItemsToBuyInt, itemsPlayerCanAfford) != true && itemsPlayerCanAfford > 0 && numItemsToBuyInt != 0);

                                        if (itemsPlayerCanAfford > 0)
                                        {
                                            //add items to player inventory
                                            //deduct player money based on cost of items
                                            itemsBought = new Dictionary<string, int>();
                                            itemsBought.Add(healthPotionName, numItemsToBuyInt);
                                            player.AddItemToInventory(itemsBought);
                                            itemCost = numItemsToBuyInt * healthPotionPrice;
                                            goldAfterBuy = player.DeductPlayerGoldCoins(itemCost);
                                            Console.WriteLine($"Thank you for your purchase, your remaining gold is: {goldAfterBuy}");
                                            itemsPlayerCanAfford = 0;
                                        }
                                        
                                    }
                                }
                                else if (playerBuyCheck == "sell")
                                {
                                    player.SellItems();
                                }

                                this.SaveGameToFile();
                                this.currentSave += 1;

                            } while (this.GetValidPlayerInputString(validExitAnswer)!= "exit");
                        }
                    }
                    //player 1 to n sell or buy items
                }
            }
            // Check if all players are dead
            if (players.All(p => !p.CheckAlive()))
            {
                Console.WriteLine("\nUnfortunately all players are dead.");// Break out of the loop if all players are dead
                Console.WriteLine("You lose.");
            }
            else
            {
                Console.WriteLine("\nWell done!");
                Console.WriteLine("You Win");
            }

            foreach (PlayerCreator player in this.players)
            {
                player.IncreasePoints(player.GetPlayerGoldCoins() * 10);
                Console.WriteLine($"\nThe score for player {player.GetName()} ended up being {player.GetPoints()}!");
                foreach (var item in player.GetInventoryContents())
                {
                    Console.WriteLine($"The player ended up with: {item.Value} of {item.Key}!");
                }
            }
            Console.ReadLine();
            return null;
        }

        private List<string> GetSaveNames()
        {
            string appDir;
            try
            {
                appDir = AppDomain.CurrentDomain.BaseDirectory;
                return (Directory.GetFiles(appDir, "save*")).ToList();
            }
            catch
            {
                return null;
            }
            
        }

        private void MainMenu()
        {
            Console.WriteLine("Welcome to the RPG Adventure!");
            Console.WriteLine("\n Please choose the numerical value corresponding to the option you wish to select.");
            Console.WriteLine("\nType 0 to exit the game.");
            Program game = new Program();
            string[] validMenuChoices = { "1", "2"};
            string menuChoice;

            do
            {
                Console.WriteLine("\n\n1. Start New Game\n2. Load Existing Game");
                menuChoice = GetValidPlayerInputString(validMenuChoices);
                this.userMenuChoice = ValidPlayerInputStringToInt(menuChoice);

                if (userMenuChoice == 1)
                {
                    game.StartNewGame();
                }
                else if (userMenuChoice == 2)
                {
                    try
                    {
                        loadNames = game.GetSaveNames();
                        foreach (var name in loadNames)
                        {
                            Console.WriteLine($"Enter {name} to load save.");
                        }
                        this.loadNames.Add("back");
                        Console.WriteLine("\nPlease provide the correct name for a save, if none are available then type 'back' to go to the menu.");
                        this.playerLoadNameCheck = Console.ReadLine();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("No valid saves, play the game first!");
                        break;
                    }

                    
                    if (this.playerLoadNameCheck.Length > 1)
                    {
                        game.LoadGameFromFile(this.playerLoadNameCheck);
                        game.GenerateLevels();
                        game.GameLoop();
                    }
                }

            } while (this.userMenuChoice != 0);
            Console.WriteLine("Thanks for playing.");
        }

        private static string configValueFromConfigLine(string line)
        {
            string[] gameConfigFields = line.Split(configFileSeparatorSymbol);
            return gameConfigFields[configID];
        }

        private static void Main(string[] args)
        {
            Program RPGGame = new Program();
            RPGGame.MainMenu();
        }
    }
}

