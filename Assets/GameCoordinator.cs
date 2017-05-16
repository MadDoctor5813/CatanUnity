using Assets.defs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameCoordinator : MonoBehaviour
{

    public BoardView Board;
    public GameObject PlayerPrefab;

    private List<Player> players;
    private int turnIdx = 0;
    private Player currentPlayer = null;

	void Start ()
	{
        InitializePlayers();
        currentPlayer = players[turnIdx];
        currentPlayer.StartTurn();
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
        player.Coordinator = this;
        return player;
    }

    public void NextTurn()
    {
        currentPlayer.EndTurn();
        turnIdx = (turnIdx + 1) % players.Count;
        currentPlayer = players[turnIdx];
        currentPlayer.StartTurn();
    }

    void Update () 
	{
		
	}

}
