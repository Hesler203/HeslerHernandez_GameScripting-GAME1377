using UnityEngine;

public class MonsterBrawl : MonoBehaviour
{
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

    public struct Monster // Used to create the roster & facilitate fight sim
    {
        public string Name;
        public int AttackStat;
        public int HealthPoints;
        public int SpeedStat;
    }
    private Monster[] roster = new Monster[]
    {
        new Monster(){Name = "Goblin", AttackStat = 8, HealthPoints = 30, SpeedStat = 1},
        new Monster(){Name = "Orc", AttackStat = 20, HealthPoints = 80, SpeedStat = 2},
        new Monster(){Name = "Troll", AttackStat = 35, HealthPoints = 200, SpeedStat = 3},
        new Monster(){Name = "Skeleton", AttackStat = 12, HealthPoints = 50, SpeedStat = 1},
        new Monster(){Name = "Ogre", AttackStat = 50, HealthPoints = 250, SpeedStat = 4},
    };
    private FightLog log = new FightLog();

    void Start()
    {
        PrintRoster();
        FightSim();
    }

    void PrintRoster()
    {
        foreach (Monster monster in roster)
        {
            Debug.Log(monster.Name + " | ATK: " + monster.AttackStat + " | HP: " + monster.HealthPoints + " | SPD: " + monster.SpeedStat);
        }
    }

    void FightSim()
    {
        // pointer to starting monster moves up with each round
        // conditional omits the last round since end monster has no one in front to fight
        for (int roundCount = 1; roundCount <= (roster.Length - 1); roundCount++)
        {
            int matchCount = 1;
            while (matchCount < (roster.Length - roundCount)) // number of matches shorten each round as pointer to starting monster moves up
            {
                log.FighterA = roster[roundCount];
                log.FighterB = roster[roundCount + matchCount]; // points to the monsters in front as matches continue

                Combat();
                DeclareWinner();
                DisplayResults();

                log = new FightLog();
                matchCount++;
            }
        }
    }

    void Combat()
    {
        log.TotalTurns = 0;

        while (log.FighterA.HealthPoints > 0 && log.FighterB.HealthPoints > 0)
        {
            ++log.TotalTurns; // taking turns from 1
            // attacks occur only whenever the the fighters' speed stats match the turn count
            if (log.TotalTurns % log.FighterA.SpeedStat == 0)
            {
                log.FighterB.HealthPoints -= log.FighterA.AttackStat;
            }
            if (log.TotalTurns % log.FighterB.SpeedStat == 0)
            {
                log.FighterA.HealthPoints -= log.FighterB.AttackStat;
            }
        }
    }

    void DeclareWinner()
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
    }

    //--------------summary----------------
    // method recieves the FightLog history previously updated by Combat()
    // to display a battle result message in the instructed format
    // corresponding to winning conditions
    //----------local variables------------
    // <string resultFighters> , <string drawMessage> , <string winnerMessage>
    // used to store the fight result messages 
    void DisplayResults()
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