using Assets.defs;
using Assets.util;
using MonsterLove.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Assets.action;

public enum PlayerStates
{
    Idle,
    PlacingSettlement,
    PlacingCity,
    PlacingRoad,
}

public class Player : MonoBehaviour
{
    public BoardView Board;
    public PlayerColor Color;
    public GameCoordinator Coordinator;

    private bool isMyTurn = false;

    private PrefabContainer prefabContainer;

    private StateMachine<PlayerStates> stateMachine;

    private UnitTypes placingUnitType;

	void Start ()
	{
        prefabContainer = GameObject.Find("PrefabContainer").GetComponent<PrefabContainer>();
        stateMachine = StateMachine<PlayerStates>.Initialize(this, PlayerStates.Idle);
	}

    public void ChangeState(PlayerStates state, StateTransition transition = StateTransition.Safe)
    {
        stateMachine.ChangeState(state, transition);
    }

    public void StartTurn()
    {
        Debug.Log(string.Format("Starting turn for {0}", Color.ToString()));
        isMyTurn = true;
    }

    public void EndTurn()
    {
        isMyTurn = false;
    }
	
	public void Idle_Update()
    {
        if (isMyTurn)
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                ChangeState(PlayerStates.PlacingSettlement);
                placingUnitType = UnitTypes.Settlement;
            }
            else if (Input.GetKeyDown(KeyCode.C))
            {
                ChangeState(PlayerStates.PlacingCity);
                placingUnitType = UnitTypes.City;
            }
            else if (Input.GetKeyDown(KeyCode.R))
            {
                ChangeState(PlayerStates.PlacingRoad);
            }
        }
    }

    public void PlacingSettlement_Update()
    {
        Vector3? mousePos = RaycastMouse();
        if (mousePos.HasValue)
        {
            PlaceSettlementAction action = new PlaceSettlementAction(HexCorner.GetNearestCorner(mousePos.Value), Color);
            if (action.IsValid(Board))
            {
                Board.SetCurrentAction(action);
                if (Input.GetMouseButtonDown(0))
                {
                    Board.ApplyCurrentAction();
                    ChangeState(PlayerStates.Idle);
                }
            }
        }
    }

    public void PlacingCity_Update()
    {
        Vector3? mousePos = RaycastMouse();
        if (mousePos.HasValue)
        {
            PlaceCityAction action = new PlaceCityAction(HexCorner.GetNearestCorner(mousePos.Value), Color);
            if (action.IsValid(Board))
            {
                Board.SetCurrentAction(action);
                if (Input.GetMouseButtonDown(0))
                {
                    Board.ApplyCurrentAction();
                    ChangeState(PlayerStates.Idle);
                }
            }
        }
    }

    public void PlacingRoad_Update()
    {
        Vector3? mousePos = RaycastMouse();
        if (mousePos.HasValue)
        {
            PlaceRoadAction action = new PlaceRoadAction(HexEdge.GetNearestEdge(mousePos.Value), Color);
            if (action.IsValid(Board))
            {
                Board.SetCurrentAction(action);
                if (Input.GetMouseButtonDown(0))
                {
                    Board.ApplyCurrentAction();
                    ChangeState(PlayerStates.Idle);
                }
            }
        }
    }

    private Vector3? RaycastMouse()
    {
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(mouseRay, out hit))
        {
            return hit.point;
        }
        else
        {
            return null;
        }
    }
}
