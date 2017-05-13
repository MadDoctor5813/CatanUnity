using Assets.defs;
using Assets.util;
using MonsterLove.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum PlayerStates
{
    Idle,
    PlacingUnit,
    PlacingRoad,
}

public class Player : MonoBehaviour
{
    public Board Board;
    public PlayerColor Color;

    private PrefabContainer prefabContainer;

    private StateMachine<PlayerStates> stateMachine;

    private UnitTypes placingUnitType;

	void Start ()
	{
        prefabContainer = GameObject.Find("PrefabContainer").GetComponent<PrefabContainer>();
        stateMachine = StateMachine<PlayerStates>.Initialize(this, PlayerStates.Idle);
	}
	
	public void Idle_Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            stateMachine.ChangeState(PlayerStates.PlacingUnit);
            placingUnitType = UnitTypes.Settlement;
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            stateMachine.ChangeState(PlayerStates.PlacingUnit);
            placingUnitType = UnitTypes.City;
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            stateMachine.ChangeState(PlayerStates.PlacingRoad);
        }
    }

    public void PlacingUnit_Update()
    {
        Vector3? mouseHit = RaycastMouse();
        if (mouseHit.HasValue)
        {
            HexCorner corner = HexCorner.GetNearestCorner(mouseHit.Value);
            Board.SetGhostUnit(corner, placingUnitType, Color);
            if (Input.GetMouseButtonDown(0))
            {
                if (Board.PlaceGhostUnit())
                {
                    stateMachine.ChangeState(PlayerStates.Idle);
                }
            }
        }
    }

    public void PlacingRoad_Update()
    {
        Vector3? mouseHit = RaycastMouse();
        if (mouseHit.HasValue)
        {
            HexEdge edge = HexEdge.GetNearestEdge(mouseHit.Value);
            Board.SetGhostRoad(edge, Color);
            if (Input.GetMouseButtonDown(0))
            {
                if (Board.PlaceGhostRoad())
                {
                    stateMachine.ChangeState(PlayerStates.Idle);
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
