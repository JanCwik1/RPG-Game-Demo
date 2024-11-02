using System;
using System.Collections.Generic;
using System.Linq;

namespace SoftEngCWK
{
    [Serializable]
    class Monster
    {
        private string name;
        private int health;
        private int attack;
        private Dictionary<string, int> lootTable;
        private int pointsForDeath;
        private bool isAlive=true;

        public Monster(string name, int health, int attack, int pointsForDeath)
        {
            this.name = name;
            this.health = health;
            this.attack = attack;
            this.pointsForDeath = pointsForDeath;
            this.lootTable = new Dictionary<string, int>
            {
                { "Gold Coins", 25 },
                { "Health Elixir", 5 },
                { "Sword", 3 },
            };

        }

        public void SetName(string name)
        {
            this.name = name;
        }

        public void SetHealth(int health)
        {
            this.health = health;
        }

        public void SetAttack(int attack)
        {
            this.attack = attack;
        }

        public void SetPointsForDeath(int pointsForDeath)
        {
            this.pointsForDeath = pointsForDeath;
        }

        public string GetName()
        {
            return this.name;
        }

        public int GetHealth()
        {
            return this.health;
        }

        public int GetAttack()
        {
            return this.attack;
        }

        public int GetPointsForDeath()
        {
            return this.pointsForDeath;
        }

        public int DealDamage()
        {
            return this.attack;
        }

        public Dictionary<string, int> AssignLoot()
        {
            Dictionary<string, int> playerLoot = new Dictionary<string, int>();
            Random rand = new Random();

            foreach (KeyValuePair<string, int> loot in this.lootTable)
            {
                if (rand.NextDouble() < (double)loot.Value / 100)
                {
                    playerLoot.Add(loot.Key, 1);
                }
            }

            if (this.health < 0)
            {
                if (playerLoot.ContainsKey("Gold Coins"))
                {
                    playerLoot["Gold Coins"] += this.pointsForDeath;
                }
                else
                {
                    playerLoot.Add("Gold Coins", this.pointsForDeath);
                }
            }

            return playerLoot;
        }

        public void TakeDamage(int damage)
        {
            this.health -= damage;
            if (this.health <= 0)
            {
                this.isAlive = false;
            }
        }

        public bool CheckAlive()
        {
            return this.isAlive;
        }

        public PlayerCreator ChooseRandomPlayer(List<PlayerCreator> players)
        {
            Random rand = new Random();
            List<PlayerCreator> alivePlayers = players.Where(p => p.CheckAlive()).ToList();
            if (alivePlayers.Count == 0)
            {
                return null; // No alive players
            }
            int index = rand.Next(alivePlayers.Count);
            return alivePlayers[index];
        }

    }
}
