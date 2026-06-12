/*
 * Assignment: Monster Brawl
 *
 * Objective:
 * Implement a battle simulation in the Start method. You are given five monsters, each with a name, attack stat, health stat and speed stat.
 *      Attack determines how much damage it does when it attacks. Health determines how much damage it can take before dying.
 *      Speed determines how often a monster attacks (1 means it attacks every turn, 2 means every 2 turns, 3 means every 3 turns, and so on)
 *
 *  Print the roster
 *      1. Loop through the monsters and print each one in this exact format:
 *      Goblin | ATK: 8 | HP: 30 | SPD: 1
 *
 *  Simulate every unique 1v1 fight
 *      1. Every monster should fight every other monster exactly once.
 *          Goblin vs Orc and Orc vs Goblin are the same fight � only one should occur.
 *      2. A monster should never fight itself.
 *      3. In each fight, both monsters attack simultaneously each turn.
 *      4. A monster only attacks on turns that are a multiple of its speed.
 *          E.g. a monster with speed 2 attacks on turns 2, 4, 6, etc. A monster with speed 3 attacks on turns 3, 6, 9, etc.
 *      5. The fight ends when one or both monsters reach 0 HP or below.
 *      6. Print each fight result in this exact format:
 *          Goblin vs Orc | Winner: Orc | Turns: 12 | Remaining HP: 24
 *      7. If both monsters die on the same turn, print:
 *          Goblin vs Orc | Draw | Turns: 8
 *  Instructions:
 *      Attach the script to any active GameObject in your Unity scene.
 *      Observe the results in the Console after hitting the Play button.
 */

using UnityEngine;

public class MonsterBrawl : MonoBehaviour
{
    void Start()
    {
        string[] monsterNames = { "Goblin", "Orc", "Troll", "Skeleton", "Ogre" };
        int[] attackStats = { 8, 20, 35, 12, 50 };
        int[] healthStats = { 30, 80, 200, 50, 250 };
        int[] speedStats = { 1, 2, 3, 1, 4 };

        // YOUR CODE GOES HERE
        Monster[] monsters = new Monster[monsterNames.Length]; // declares an array of the Monster class to hold the roster's monsters
        // loops through this monsters array to populate each monster's fields using the names & stats arrays
        for (int i = 0; i < monsters.Length; i++)
        {
            // break out of the loop if there is a missing stat for a monster
            if (i == attackStats.Length || i == healthStats.Length || i == speedStats.Length)
            {
                Debug.Log("Missing a stat for: " + monsterNames[i]);
                break;
            }

            monsters[i] = new Monster(); // instantiates the Monster object to allow access
            monsters[i].name = monsterNames[i]; // stores the corresponding monster's name
            monsters[i].atk = attackStats[i]; // stores that monster's atk
            monsters[i].hp = healthStats[i]; // stores that monster's health
            monsters[i].spd = speedStats[i]; // stores that monster's speed
        }

        PrintRoster(monsters); // Prints the Roster, passes in the array of Monsters

        FightSim(monsters);
    }

    // Loops through the monsters array and prints each one's info in the instructed format
    void PrintRoster(Monster[] monsters)
    {
        foreach (Monster monster in monsters)
        {
            Debug.Log(monster.name + " | ATK: " + monster.atk + " | HP: " + monster.hp + " | SPD: " + monster.spd);
        }
    }

    void FightSim(Monster[] monsters)
    {

    }

    // Declares a Monster class to hold all the monster info from the roster & facilitate fight sim
    public class Monster
    {
        public string name;
        public int atk;
        public int hp;
        public int spd;
    }
}