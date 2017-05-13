using Assets.defs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameCoordinator : MonoBehaviour
{

    public Board Board;
    public GameObject PlayerPrefab;

    private Dictionary<PlayerColor, Player> players;

	void Start ()
	{
        InitializePlayers();
	}

    private void InitializePlayers()
    {
        players = new Dictionary<PlayerColor, Player>
        {
            { PlayerColor.Blue, InstantiatePlayer(PlayerColor.Blue) },
            { PlayerColor.Orange, InstantiatePlayer(PlayerColor.Orange) },
            { PlayerColor.Red, InstantiatePlayer(PlayerColor.Red) },
            { PlayerColor.White, InstantiatePlayer(PlayerColor.White) }
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
