using System;
using System.Collections.Generic;

namespace SoftEngCWK
{
    [Serializable]
    internal class PlayerCreator
    {
        private readonly int errorCode;
        private string name;
        private int difficulty;
        private int health;
        private int attack;
        private int statPoints;
        private int statPointsLimit;
        private int points;
        private int id;
        private int elixirIncreaseHealth = 4;
        private Dictionary<string, int> inventory;
        private const int easyDifficulty = 1;
        private const int mediumDifficulty = 2;
        private const int hardDifficulty = 3;
        private const int hardDifficultyStatPoints = 5;
        private const int mediumDifficultyStatPoints = 10;
        private const int easyDifficultyStatPoints = 15;
        private bool isAlive = true;
        

        public PlayerCreator()
        {
            name = "Nameless";
            difficulty = 1;
            health = 3;
            attack = 3;
            this.inventory = new Dictionary<string, int>()
            {
                {"Gold Coins", 0},
                {"Health Elixir", 0},
                {"Sword", 0}
            };
        }

        public bool ValidNumberCheck(int queryNumber, int validNumber) //used by SellItems() to check if user input for selling items number is valid
        {
            bool isValidNumber = false;

            if (queryNumber <= validNumber && queryNumber > 0)
            {
                isValidNumber = true;
            }

            return isValidNumber;
        }

        public void SellItems()
        {
            string userSell;
            int userSellNumber;
            int sellHealthElixirMultiplier = 3;
            int sellSwordMultiplier = 20;

            foreach (var item in this.inventory)
            {
                userSell = "no";
                userSellNumber = 0;
                if (item.Key == "Gold Coins")
                {
                    Console.WriteLine("Tip: Gold Coins can't be sold!");
                }
                if (item.Value != 0 && item.Key != "Gold Coins")
                {
                    Console.WriteLine($"\nPlayer {this.GetName()}, do you wish to sell any of your {item.Key}? (type yes or no)");
                    userSell = Console.ReadLine();
                    if (userSell == "yes")
                    {
                        do
                        {
                            Console.WriteLine($"How much of your {item.Key} do you want to sell? You currently have: {this.inventory[item.Key]}");
                            userSellNumber = int.Parse(Console.ReadLine());

                        } while (ValidNumberCheck(userSellNumber, this.inventory[item.Key]) != true);

                        if (item.Key == "Health Elixir")
                        {
                            this.inventory["Gold Coins"] += (sellHealthElixirMultiplier * userSellNumber);
                        }
                        else if (item.Key == "Sword")
                        {
                            this.inventory["Gold Coins"] += (sellSwordMultiplier * userSellNumber);
                        }
                        Console.WriteLine($"\nAfter selling the {item.Key} you now have {this.inventory["Gold Coins"]} Gold Coins!");

                        this.inventory[item.Key] -= userSellNumber;
                    }
                }
            }
        }

        public Dictionary<string, int> GetInventoryContents()
        {
            return this.inventory;
        }

        public void AddItemToInventory(Dictionary<string, int> loot)
        {
            //the itemName is the key used to check inventory
            

            foreach (var item in loot)
            {
                if (this.inventory.ContainsKey(item.Key))
                {
                    this.inventory[item.Key] += item.Value;
                    Console.WriteLine($"User {this.GetName()} now has {this.inventory[item.Key]} of {item.Key}.");
                }
            }
        }

        public int GetPlayerGoldCoins()
        {
            int goldCoinNumber;
            goldCoinNumber = this.inventory["Gold Coins"];
            return goldCoinNumber;
        }

        public int GetPlayerHealthElixir()
        {
            int elixirNumber;
            elixirNumber = this.inventory["Health Elixir"];
            return elixirNumber;
        }

        public void UseHealthElixir(int playerUseElixirs)
        {
            int elixirNumber = GetPlayerHealthElixir();
            Console.WriteLine($"Player {this.name} has {elixirNumber} Health Elixirs, you can buy more if you reach a shop for gold coins.");
            if (elixirNumber == 0)
            {
                Console.WriteLine("You don't have any Health Elixirs.");
            }
            else if (elixirNumber < playerUseElixirs)
            {
                Console.WriteLine($"You don't have {playerUseElixirs} Health Elixirs, you have {elixirNumber}");
            }
            else if (playerUseElixirs <= elixirNumber && playerUseElixirs > 0)
            {
                int oldHealth = this.health;
                this.health += (playerUseElixirs * elixirIncreaseHealth);
                Console.WriteLine($"The player {this.name} had their health go up from {oldHealth} to {this.health}!");
                DeductPlayerHealthElixir(playerUseElixirs);
            }
        }

        public int DeductPlayerHealthElixir(int elixirCost)
        {
            this.inventory["Health Elixir"] -= elixirCost;
            return this.inventory["Health Elixir"];
        }

        public int DeductPlayerGoldCoins(int cost)
        {
            this.inventory["Gold Coins"] -= cost;
            return this.inventory["Gold Coins"];
        }

        public int GetAffordableItemCount(int itemSingleCost)
        {
            int itemCount;
            int playerGold = this.GetPlayerGoldCoins();
            itemCount = playerGold / itemSingleCost;
            return itemCount;
        }

        public bool CheckAlive()
        {
            return this.isAlive;
        }

        public void TakeDamage(int damage)
        {
            this.health -= damage;
            if (this.health <= 0)
            {
                this.isAlive = false;
            }
        }

        public void SetPlayerId(int id)
        {
            this.id = id;
        }

        public int GetPlayerId()
        {
            return this.id;
        }

        public void SetPlayerName(string name)
        {
            this.name = name;
        }

        public void SetDifficulty(int difficulty)
        {
            //uses difficulty from game config
            this.difficulty = difficulty;

            switch (difficulty)
            {
                case easyDifficulty:
                    this.statPoints = easyDifficultyStatPoints;
                    break;
                case mediumDifficulty:
                    this.statPoints = mediumDifficultyStatPoints;
                    break;
                case hardDifficulty:
                    this.statPoints = hardDifficultyStatPoints;
                    break;
                default:
                    Console.WriteLine("Invalid choice for difficulty.");
                    break;
            }
        }

        public int DistributeStatPointsHandler()
        {

            return errorCode;
        }

        public int DistributeStatPoints(int healthPoints, int attackPoints)
        {
            // Ensure the total points distributed do not exceed the limit
            int totalPoints = healthPoints + attackPoints;
            //int limit = (difficulty == easyDifficulty) ? easyDifficultyStatPoints : (difficulty == mediumDifficulty) ? mediumDifficultyStatPoints : hardDifficultyStatPoints;
            int limit;
            if (this.difficulty == 1)
            {
                this.statPointsLimit = easyDifficultyStatPoints;
            }
            else if (this.difficulty == 2)
            {
                this.statPointsLimit = mediumDifficultyStatPoints;
            }
            else if (this.difficulty == 3)
            {
                this.statPointsLimit = hardDifficultyStatPoints;
            }


            if (totalPoints == this.statPointsLimit)
            {
                this.health += healthPoints;
                this.attack += attackPoints;
                return 0;
            }
            else if (totalPoints > this.statPointsLimit)
            {
                Console.Clear();
                Console.WriteLine($"\nYou have exceeded the limit of {this.statPointsLimit} stat points for difficulty of {this.difficulty}. Please Try Again without exceeding the remaining stats number.");
                return 1;
            }
            else
            {
                Console.Clear();
                Console.WriteLine("\nStat points unable to be concluded error NUM 2.");
                return 2;
            }
        }

        public string GetName()
        {
            return name;
        }

        public int GetPoints()
        {
            return this.points;
        }

        public int GetDifficulty()
        {
            return this.difficulty;
        }

        public int GetHealth()
        {
            return health;
        }

        public int GetAttack()
        {
            return attack;
        }

        public void IncreasePoints(int increaseAmount)
        {
            this.points += increaseAmount;
        }

        public void SetStatPointsToDistribute()
        {
            if (this.difficulty == 1)
                this.statPoints = easyDifficultyStatPoints;
            else if (this.difficulty == 2)
                this.statPoints = mediumDifficultyStatPoints;
            else if (this.difficulty == 3)
                this.statPoints = hardDifficultyStatPoints;
        }

        public int GetStatPointsToDistribute()
        {
            return this.statPoints;
        }

    }
}
