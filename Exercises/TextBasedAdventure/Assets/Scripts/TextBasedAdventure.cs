using UnityEngine;

public class TextBasedAdventure : MonoBehaviour
{
    [System.Serializable]
    public struct Room
    {
        public string Name;
        public TileType Type;
        public bool isVisited;
    }

    [System.Serializable]
    public struct RoomRow
    {
        public Room[] rooms;
    }

    public enum TileType
    {
        Invalid,
        Empty,
        Item,
        Enemy,
        Teleporter,
        Blockade,
        Exit,
    }

    private Room[,] dungeon =
    {
        {   new Room { Name = "Dark Cave",      Type = TileType.Empty      },
            new Room { Name = "Mossy Tunnel",   Type = TileType.Item       },
            new Room { Name = "Crystal Room",   Type = TileType.Teleporter },
            new Room { Name = "Wine Cellar",    Type = TileType.Enemy      }
        },

        {   new Room { Name = "Bone Chamber",   Type = TileType.Enemy      },
            new Room { Name = "Flooded Hall",   Type = TileType.Empty      },
            new Room { Name = "Iron Gate",      Type = TileType.Exit       },
            new Room { Name = "Sandy Beach",    Type = TileType.Enemy      }
        },

        {   new Room { Name = "Goblin Den",     Type = TileType.Empty      },
            new Room { Name = "Armory",         Type = TileType.Enemy      },
            new Room { Name = "Throne Room",    Type = TileType.Item       },
            new Room { Name = "Secret Passage", Type = TileType.Empty      }
        },

        {   new Room { Name = "Chasm",          Type = TileType.Blockade   },
            new Room { Name = "Hot Spring",     Type = TileType.Item       },
            new Room { Name = "Library",        Type = TileType.Teleporter },
            new Room { Name = "Temple",         Type = TileType.Enemy      }
        }
    };

    public string[,] tileDescriptions =
    {
        {   "No light enters here. Tread lightly.",
            "Life seems to find a way to live... even down here.",
            "The crystals emit a faint glow, as though they feel your presence.",
            "These barrels are all empty. Who drank all the wine?",
        },

        {   "Bones creek under your feet. An ominous aura fills the room.",
            "Stone pillars stretch far beneath the water.",
            "Many others have failed to reach here. Consider yourself lucky.",
            "The sand feels cold. Has it always been this way. Summer never seems to arrive.",
        },

        {   "Snarls echo along the walls. Deep scratches and bloodstains seem to mar the floor.",
            "Once a bastion of awe-inspiring weaponry and skilled craftsmanship, now rusted and abandoned.",
            "Here sat a merciless king. Beneath his greed was a but a lonely coward.",
            "This is how he escaped. Where does it lead?",
        },

        {   "Watch your step. Many have met their end here.",
            "These waters are known for their rejuvinating power.",
            "Wonderful knowledge is kept here, so are secrets.",
            "Long ago, this was a place of worship for many. Now it is the resting place of a forgotten.",
        },
    };

    private int playerRow = 0;
    private int playerCol = 0;
    private int playerHealth = 10;
    private int enemyDamage = 1;
    private int itemHealAmount = 2;

    private Rigidbody rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        OutputTileInformation();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        bool wasKeyPressed = HandleInput(out int newRow, out int newCol);
        if (!wasKeyPressed)
        {
            return;
        }
        SetPlayerPosition(newRow, newCol);
        OutputTileInformation();
    }

    private void OutputTileInformation()
    {
        Debug.Log("You are in: " + tileNames[playerRow, playerCol]);

        switch (tileTypes[playerRow, playerCol])
        {
            case TileType.Empty:
                Debug.Log("There is nothing here.");
                break;
            case TileType.Enemy:
                Debug.Log("Oooo a spooky ghost");
                EncounterEnemy();
                break;
            case TileType.Item:
                Debug.Log("You see a shiny object");
                ItemPickup();
                break;
            case TileType.Exit:
                Debug.Log("You see a way out");
                break;
            default:
                Debug.LogError("Invalid TileType");
                break;
        }
    }

    private void EncounterEnemy()
    {
        PlayerTakeDamage(enemyDamage);
    }

    private void ItemPickup()
    {
        PlayerHeal(itemHealAmount);
    }

    private void PlayerHeal(int heal)
    {
        playerHealth += heal;
        Debug.Log("You get healed. Your health is now " + playerHealth);
    }

    /// <summary>
    /// Decreases player's health by damage amount if the player is alive, otherwise ends game
    /// </summary>
    /// <param name="damage"></param>
    private void PlayerTakeDamage(int damage)
    {
        playerHealth -= damage;
        Debug.Log("You get hit. Your health is now " + playerHealth);

        if (playerHealth <= 0)
        {
            playerHealth = 0;
            Debug.Log("You are dead");
            Debug.Log("Please restart the game to try again");
            Debug.Break();
        }
    }

    /// <summary>
    /// Sets the player position to the tile corresponging to the matching teleporter in the dungeon
    /// </summary>
    private void TryTeleport()
    {
        bool isTeleporterCountValid = ValidateTeleporterCount();
        if (!isTeleporterCountValid)
        {
            return;
        }
        bool wasTeleporterPairFound = FindTeleporterPair(out int teleporterRow, out int teleporterCol);
        if (!wasTeleporterPairFound)
        {
            return;
        }
        bool wasTileAvailable = SetPlayerPosition(teleporterRow, teleporterCol);
        if (wasTileAvailable)
        {
            playerTile = dungeon[playerPosition.row, playerPosition.col];
            OutputTileInformation();
        }
    }

    /// <summary>
    /// Counts the number of rooms in the dungeon that have the same Teleporter label, then validates
    /// that this count is even, i.e. that each teleporter has a pair
    /// </summary>
    /// <returns>True if each room's Teleporter label has a match pair, false if not</returns>
    private bool ValidateTeleporterCount()
    {
        bool wasTeleporterCountValid = true;

        List<string> teleporterList = new List<string>();

        foreach (Room room in dungeon)
        {
            if (room.TeleporterLabel != null && !teleporterList.Contains(room.TeleporterLabel))
            {
                int labelCount = 0;
                foreach (Room label in dungeon)
                {
                    if (label.TeleporterLabel == room.TeleporterLabel)
                    {
                        labelCount++;
                    }
                }

                if (labelCount % 2 != 0) // not all labels have pairs
                {
                    return wasTeleporterCountValid = false;
                }

                teleporterList.Add(room.TeleporterLabel);
            }
        }

        if (teleporterList.Count == 0)
        {
            wasTeleporterCountValid = false;
        }

        return wasTeleporterCountValid;
    }

    /// <summary>
    /// Sets potential teleporter pair's position
    /// </summary>
    /// <param name="teleporterRow">teleporter pair's row position</param>
    /// <param name="teleporterCol">teleporter pair's column position</param>
    /// <returns>True if a corresponding teleporter pair was found, false if not</returns>
    private bool FindTeleporterPair(out int teleporterRow, out int teleporterCol)
    {
        bool hasFoundTeleporterPair = false;

        teleporterRow = playerPosition.row;
        teleporterCol = playerPosition.col;

        for (int row = 0; row < dungeon.GetLength(0); row++)
        {
            for (int col = 0; col < dungeon.GetLength(1); col++)
            {
                Room currentTile = playerTile;
                Room newTile = dungeon[row, col];
                if (newTile.TeleporterLabel == currentTile.TeleporterLabel && newTile.Name != currentTile.Name)
                {
                    teleporterRow = row;
                    teleporterCol = col;
                    hasFoundTeleporterPair = true;
                    break;
                }
            }
        }
        return hasFoundTeleporterPair;
    }

    /// <summary>
    /// Handles the player's teleport input
    /// </summary>
    /// <returns>True if Spacebar was pressed, false if not</returns>
    private bool WasLookPressed()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// Handles the player's teleport input
    /// </summary>
    /// <returns>True if the T key was pressed, false if not</returns>
    private bool WasTeleportActivated()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// Handles the player's movement input and sets potential new tile position to move into
    /// </summary>
    /// <param name="newRow">new row position</param>
    /// <param name="newCol">new column position</param>
    /// <returns>True if any move input was pressed, false if not</returns>
    private bool PlayerMovement(out int newRow, out int newCol)
    {
        bool hasMoved = true;
        newRow = playerPosition.row;
        newCol = playerPosition.col;

        if (Input.GetKeyDown(KeyCode.D))
        {
            Debug.Log("You pressed " + KeyCode.D);
            newCol++;
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("You pressed " + KeyCode.A);
            newCol--;
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            Debug.Log("You pressed " + KeyCode.W);
            newRow--;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log("You pressed " + KeyCode.S);
            newRow++;
        }
        else
        {
            hasMoved = false;
        }
        return hasMoved;
    }

    /// <summary>
    /// Sets the player position to a new row and column position based on the dungeon's bounds
    /// and the new tile's TileType, and let's the player know if the new tile position is inaccessible
    /// </summary>
    /// <param name="newRow"></param>
    /// <param name="newCol"></param>
    /// <returns>True if the new tile position was within the bounds & not a blockade, false if not</returns>
    private bool SetPlayerPosition(int newRow, int newCol)
    {
        bool wasTileAvailable = CheckIfNewPositionInBounds(newRow, newCol);
        if (wasTileAvailable)
        {
            bool wasNextTileBlockade = CheckIfNewTileIsBlockade(newRow, newCol);
            if (wasNextTileBlockade == false)
            {
                playerPosition.row = newRow;
                playerPosition.col = newCol;
            }
            else
            {
                wasTileAvailable = false;
                Debug.Log("Can't go that way");
            }
        }
        else
        {
            wasTileAvailable = false;
            Debug.Log("Can't go that way");
        }
        return wasTileAvailable;
    }

    /// <summary>
    /// Determine if the new row and column position are within the bounds of the tiles
    /// </summary>
    /// <param name="newRow"></param>
    /// <param name="newCol"></param>
    /// <returns>True if it is within the bounds, false if not</returns>
    private bool CheckIfNewPositionInBounds(int newRow, int newCol)
    {
        return (newRow >= 0 && newRow < dungeon.GetLength(0)) && (newCol >= 0 && newCol < dungeon.GetLength(1));
    }

    /// <summary>
    /// Determine if the potential new tile for player movement is a blockade
    /// </summary>
    /// <param name="newRow"></param>
    /// <param name="newCol"></param>
    /// <returns>True if tile is blockade, false if not</returns>
    private bool CheckIfNewTileIsBlockade(int newRow, int newCol)
    {
        bool isNextTileBlockade = false;

        Room nextTile = dungeon[newRow, newCol];
        if (nextTile.Type == TileType.Blockade)
        {
            isNextTileBlockade = true;
        }
        return isNextTileBlockade;
    }
}
