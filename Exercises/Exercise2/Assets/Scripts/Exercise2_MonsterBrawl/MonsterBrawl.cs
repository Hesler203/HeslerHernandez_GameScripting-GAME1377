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

using Unity.VisualScripting;
using UnityEngine;

public class MonsterBrawl : MonoBehaviour
{
    // Used to create the roster & facilitate fight sim
    [System.Serializable]
    public struct Monster
    {
        public string Name;
        public int AttackStat;
        public int HealthPoints;
        public int SpeedStat;
    }
    // Will hold the roster
    [SerializeField] private Monster[] roster;

    // Used to keep track of who wins each fight as well as fight duration (in turns), match will be displayed to console on conclusion
    public struct FightLog
    {
        public Monster FighterA;
        public Monster FighterB;
        public string Winner;
        public int WinnerFinalHP;
        public int TotalTurns;
    }
    private FightLog fightLog;

    void Start()
    {
        // -for me
        //string[] monsterNames = { "Goblin", "Orc", "Troll", "Skeleton", "Ogre"  };
        //   int[]  attackStats = {      8,      20,     35,        12,      50   };
        //   int[]  healthStats = {     30,      80,    200,        50,     250   };
        //   int[]   speedStats = {      1,       2,      3,         1,       4   };

        PrintRoster(roster); // Prints the Roster, passes in the array of monsters

        FightSim(roster, fightLog);
    }

    // -for me
    // Loops through the roster array and prints each one's info in the instructed format
    void PrintRoster(Monster[] roster)
    {
        foreach (Monster monster in roster)
        {
            Debug.Log(monster.Name + " | ATK: " + monster.AttackStat + " | HP: " + monster.HealthPoints + " | SPD: " + monster.SpeedStat);
        }
    }

    /* Simulate every unique 1v1 fight
     *      1.Every monster should fight every other monster exactly once.
     *          Goblin vs Orc and Orc vs Goblin are the same fight � only one should occur.
     *      2. A monster should never fight itself.
     *      3. In each fight, both roster attack simultaneously each turn.
     *      4. A monster only attacks on turns that are a multiple of its speed.
     * E.g.a monster with speed 2 attacks on turns 2, 4, 6, etc.A monster with speed 3 attacks on turns 3, 6, 9, etc.
     *      5. The fight ends when one or both roster reach 0 HP or below.
     *      6. Print each fight result in this exact format:
     *          Goblin vs Orc | Winner: Orc | Turns: 12 | Remaining HP: 24
     *      7. If both roster die on the same turn, print:
     *          Goblin vs Orc | Draw | Turns: 8
     *  Instructions:
     *      Attach the script to any active GameObject in your Unity scene.
     * Observe the results in the Console after hitting the Play button.
     */

    void FightSim(Monster[] roster, FightLog log)
    {
        int roundCount = 0; // starts at round 0

        do
        {
            for (int i = roundCount; i < roster.Length; i++)
            {
                log.TotalTurns = 1; // taking turns from 1

                log.FighterA = roster[roundCount];
                log.FighterB = roster[i + 1];

                DisplayResults(DeclareWinner(Combat(log)));

                log = new FightLog();
            }
            roundCount++;
        } while (roundCount < (roster.Length - 2));
    }

    FightLog Combat(FightLog log)
    {
        do
        {
            if (log.TotalTurns % log.FighterA.SpeedStat == 0)
            {
                log.FighterB.HealthPoints -= log.FighterA.AttackStat;
            }
            if (log.TotalTurns % log.FighterB.SpeedStat == 0)
            {
                log.FighterA.HealthPoints -= log.FighterB.AttackStat;
            }
            log.TotalTurns++;
        } while (log.FighterA.HealthPoints > 0 || log.FighterB.HealthPoints > 0);
        return log;
    }

    FightLog DeclareWinner(FightLog log)
    {
        if (log.FighterA.HealthPoints == log.FighterB.HealthPoints)
        {
            log.Winner = "Draw";
        }
        else
        {
            if (log.FighterA.HealthPoints > log.FighterB.HealthPoints)
            {
                log.Winner = log.FighterA.Name;
                log.WinnerFinalHP = log.FighterA.HealthPoints;
            }
            else
            {
                log.Winner = log.FighterB.Name;
                log.WinnerFinalHP = log.FighterB.HealthPoints;
            }
        }
        return log;
    }

    void DisplayResults(FightLog log)
    {
        string resultMessage = (log.FighterA.Name + " vs " + log.FighterB.Name + " | " + log.Winner + " | Turns: " + log.TotalTurns);

        if (log.Winner == "Draw")
        {
            Debug.Log(resultMessage);
        }
        else
        {
            Debug.Log(resultMessage + " Remaining HP: " + log.WinnerFinalHP);
        }
    }
}