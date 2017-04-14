using Assets.defs;
using Assets.util;
using MonsterLove.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerStates
{
    Idle,
    PlacingSettlement,
    PlacingCity,
}

public class Player : MonoBehaviour
{
    public Board Board;

    private PrefabContainer prefabContainer;

    private StateMachine<PlayerStates> stateMachine;

    private Dictionary<UnitTypes, GameObject> ghostUnitPrefabs;

    private bool intersectionUpdated = false;
    private GhostUnit ghostUnit;
    private HexCorner lastIntersection = null;

	void Start ()
	{
        prefabContainer = GameObject.Find("PrefabContainer").GetComponent<PrefabContainer>();
        stateMachine = StateMachine<PlayerStates>.Initialize(this, PlayerStates.Idle);
        ghostUnitPrefabs = new Dictionary<UnitTypes, GameObject>();
        foreach (var ghost in prefabContainer.GetWithPrefix("ghost"))
        {
            ghostUnitPrefabs.Add(ghost.GetComponent<GhostUnit>().Type, ghost);
        }
	}
	
	public void Idle_Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            stateMachine.ChangeState(PlayerStates.PlacingSettlement);
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            stateMachine.ChangeState(PlayerStates.PlacingCity);
        }
    }

    private void UpdateLastIntersection()
    {
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(mouseRay, out hit))
        {
            HexCorner current = HexCorner.GetNearestCorner(hit.point);
            if (lastIntersection != null)
            {
                Board.SetUnitVisible(lastIntersection, true);
            }
            Board.SetUnitVisible(current, false);
            if (current != lastIntersection)
            {
                if (ghostUnit != null)
                {
                    ghostUnit.transform.position = HexCorner.ToLocalCoords(current);
                }
            }
            lastIntersection = current;
        }
    }

    public void PlacingSettlement_Enter()
    {
        ghostUnit = Instantiate(ghostUnitPrefabs[UnitTypes.Settlement], Board.transform).GetComponent<GhostUnit>();
    }

    public void PlacingSettlement_Update()
    {
        UpdateLastIntersection();
        if (Input.GetMouseButtonDown(0) == true)
        {
            if (Board.IsValidSettlement(lastIntersection))
            {
                GameObject unit = ghostUnit.Place();             
                Board.AddUnit(lastIntersection, unit.GetComponent<Unit>());
                stateMachine.ChangeState(PlayerStates.Idle);
            }
        }
    }

    public void PlacingCity_Enter()
    {
        ghostUnit = Instantiate(ghostUnitPrefabs[UnitTypes.City], Board.transform).GetComponent<GhostUnit>();
    }

    public void PlacingCity_Update()
    {
        UpdateLastIntersection();
        if (Input.GetMouseButtonDown(0) == true)
        {
            if (Board.IsValidCity(lastIntersection))
            {
                GameObject unit = ghostUnit.Place();
                Board.AddUnit(lastIntersection, unit.GetComponent<Unit>());
                stateMachine.ChangeState(PlayerStates.Idle);
            }
        }
    }
}
