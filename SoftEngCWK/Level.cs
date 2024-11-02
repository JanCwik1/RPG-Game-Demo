using System;
using System.Collections.Generic;
using System.Linq;

namespace SoftEngCWK
{
    [Serializable]
    class Level
    {
        private string name;
        private string type;
        private int numOfMonsters;
        private List<Monster> monsters;
        private int totalAliveMonsters;

        public Level(string name, string type)
        {
            this.name = name;
            this.type = type;
        }

        public void AddMonsters(List<Monster> monsterList)
        {
            this.monsters = monsterList;
            this.numOfMonsters = monsterList.Count;
        }

        public string GetCategory()
        {
            return (this.type);
        }

        public int GetNumberOfMonsters()
        {
            return (this.numOfMonsters);
        }

        public string GetName()
        {
            return (this.name);
        }

        public void GetTotalMonstersDetail()
        {
            var monsterGroups = this.monsters.GroupBy(m => m.GetName());

            foreach (var group in monsterGroups)
            {
                Console.WriteLine("Name: {0}, Count: {1}", group.Key, group.Count());
            }
        }

        public List<Monster> GetMonsters()
        {
            return this.monsters;
        }

        public int GetTotalAliveMonsters()
        {
            int totalAlive = 0;
            foreach(Monster monster in this.monsters)
            {
                if (monster.GetHealth() <= 0)
                {
                    totalAlive += 1;
                }
            }
            return totalAlive;
        }

    }
}
