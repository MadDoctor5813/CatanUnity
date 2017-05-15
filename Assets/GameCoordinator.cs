using Assets.defs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameCoordinator : MonoBehaviour
{

    public Board Board;
    public GameObject PlayerPrefab;

    private List<Player> players;

	void Start ()
	{
        InitializePlayers();
	}

    private void InitializePlayers()
    {
        players = new List<Player>
        {
            { InstantiatePlayer(PlayerColor.Blue) },
            { InstantiatePlayer(PlayerColor.Orange) },
            { InstantiatePlayer(PlayerColor.Red) },
            { InstantiatePlayer(PlayerColor.White) }
        };
    }

    private Player InstantiatePlayer(PlayerColor color)
    {
        Player player = Instantiate(PlayerPrefab, this.transform).GetComponent<Player>();
        player.Color = color;
        player.Board = Board;
        return player;
    }

    void Update () 
	{
		
	}

}
