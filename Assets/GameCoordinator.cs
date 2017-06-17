using Assets.defs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using MonsterLove.StateMachine;

public enum GameStates
{
    Setup,
    InProgress
}

public class GameCoordinator : MonoBehaviour
{

    public BoardView Board;
    public GameObject PlayerPrefab;


    public StateMachine<GameStates> GameState;

    private int turnsPassed = 0;

    private List<Player> players;
    private int turnIdx = 0;
    private Player currentPlayer = null;

	void Start ()
	{
        InitializePlayers();
        GameState = StateMachine<GameStates>.Initialize(this);
        GameState.ChangeState(GameStates.Setup);
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
        if (turnIdx == 0)
        {
            turnsPassed++;
        }
        currentPlayer = players[turnIdx];
        currentPlayer.StartTurn();
    }

    public void Setup_Update()
    {
        if (turnsPassed == 2)
        {
            GameState.ChangeState(GameStates.InProgress);
            turnsPassed = 0;
        }
    }

    void Update () 
	{
		
	}

}
