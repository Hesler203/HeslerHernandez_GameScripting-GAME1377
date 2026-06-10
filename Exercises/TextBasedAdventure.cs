using UnityEngine;
using System.Collections;
using System.Diagnostics;
using System.Runtime.CompilerServices;

public class TextBasedAdventure : MonoBehaviour
{

  enum TileType
  {
    Invalid,
    Empty,
    Item,
    Enemy,
    Exit
  }

  string[] tileNames = { "Dark Cave", "Mossy Tunnel", "Crystal Room" };
  int playerPosition = 0;

  TileType[] tileTypes = { TileType.Empty, TileType.Item, TileType.Enemy };

  void Start()
  {

  }

  void Update()
  {
    int newIndex = playerPosition;
    if (Input.GetKeyDown(KeyCode.D))
    {
      newIndex++;
    }
    else if (Input.GetKeyDown(KeyCode.A))
    {
      newIndex--;
    }

    if (newIndex != playerPosition)
    {
      if (newIndex >= 0 && newIndex < tileNames.Length)
      {
        playerPosition = newIndex;
      }
      else
      {
        Debug.Log("Can't go that way!");
      }

      Debug.Log("You are in the " + tileNames[playerPosition]);
    }

    switch (tileTypes[playerPosition])
    {
      case TileType.Empty:
        Debug.Log("The room is empty.");
        break;
      case TileType.Item:
        Debug.Log("You found an item!");
        break;
      case TileType.Enemy:
        Debug.Log("An enemy attacks you!");
        break;
    }
  }

}