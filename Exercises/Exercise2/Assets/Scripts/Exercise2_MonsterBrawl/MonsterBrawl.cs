using UnityEngine;

public class MonsterBrawl : MonoBehaviour
{
    [System.Serializable]
    public struct Monster // Used to create the roster & facilitate fight sim
    {
        public string Name;
        public int AttackStat;
        public int HealthPoints;
        public int SpeedStat;
    }
    [SerializeField] private Monster[] roster; // roster filled in the inspector

    // Used to keep track of who wins each fight as well as fight duration (in turns),
    // matches will be displayed to console on conclusion
    public struct FightLog
    {
        public Monster FighterA;
        public Monster FighterB;
        public string Winner;
        public int WinnerFinalHP;
        public int TotalTurns;
    }

    void Start()
    {
        // -for me
        //string[] monsterNames = { "Goblin", "Orc", "Troll", "Skeleton", "Ogre"  };
        //   int[]  attackStats = {      8,      20,     35,        12,      50   };
        //   int[]  healthStats = {     30,      80,    200,        50,     250   };
        //   int[]   speedStats = {      1,       2,      3,         1,       4   };

        PrintRoster(roster);
        FightSim(roster);
    }

    // -for me
    // Loops through the roster array and prints each monster's info in the instructed format
    void PrintRoster(Monster[] roster)
    {
        foreach (Monster monster in roster)
        {
            Debug.Log(monster.Name + " | ATK: " + monster.AttackStat + " | HP: " + monster.HealthPoints + " | SPD: " + monster.SpeedStat);
        }
    }

    void FightSim(Monster[] roster)
    {
        FightLog log = new FightLog();
        for (int roundCount = 0; roundCount < (roster.Length - 1); roundCount++)
        {
            int match = 1;
            while (match < (roster.Length - roundCount))
            {
                log.FighterA = roster[roundCount];
                log.FighterB = roster[roundCount + match];

                DisplayResults(DeclareWinner(Combat(log)));

                log = new FightLog();
                match++;
            }
        }
    }

    FightLog Combat(FightLog log)
    {
        log.TotalTurns = 0; // taking turns from 1

        while (log.FighterA.HealthPoints > 0 && log.FighterB.HealthPoints > 0)
        {
            log.TotalTurns++;

            if (log.TotalTurns % log.FighterA.SpeedStat == 0)
            {
                log.FighterB.HealthPoints -= log.FighterA.AttackStat;
            }
            if (log.TotalTurns % log.FighterB.SpeedStat == 0)
            {
                log.FighterA.HealthPoints -= log.FighterB.AttackStat;
            }
        }
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

    //--------------summary----------------
    // method recieves the FightLog history previously updated by Combat()
    // to display a battle result message in the instructed format
    // corresponding to winning conditions
    //--------------summary----------------
    // parameter: --------- <Fightlog log>
    // local variables: --- <string resultFighters>,<string drawMessage>,<string winnerMessage>
    void DisplayResults(FightLog log)
    {
        string resultFighters = (log.FighterA.Name + " vs " + log.FighterB.Name);
        string drawMessage = (" | " + log.Winner + " | Turns: " + log.TotalTurns);
        string winnerMessage = (" | Winner: " + log.Winner + " | Turns: " + log.TotalTurns + " | Remaining HP: " + log.WinnerFinalHP);

        if (log.Winner == "Draw")
        {
            Debug.Log(resultFighters + drawMessage);
        }
        else
        {
            Debug.Log(resultFighters + winnerMessage);
        }
    }
}