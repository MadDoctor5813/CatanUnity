using Assets.defs;
using Assets.util;
using MonsterLove.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerStates
{
    Idle,
    PlacingSettlement
}

public class Player : MonoBehaviour
{
    public Board Board;

    private PrefabContainer prefabContainer;

    private StateMachine<PlayerStates> stateMachine;

    private Dictionary<UnitTypes, GameObject> ghostUnitPrefabs;

    private GhostUnit ghostUnit;
    private HexIntersection lastIntersection = null;

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
    }

    public void PlacingSettlement_Enter()
    {
        ghostUnit = Instantiate(ghostUnitPrefabs[UnitTypes.Settlement], Board.transform).GetComponent<GhostUnit>();
    }

    public void PlacingSettlement_Update()
    {
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(mouseRay, out hit))
        {
            HexIntersection current = HexIntersection.GetNearestIntersection(hit.point);
            if (current != lastIntersection)
            {
                ghostUnit.transform.position = HexIntersection.ToLocalCoords(current);
            }
            lastIntersection = current;
            if (Input.GetMouseButtonDown(0) == true)
            {
                GameObject unit = ghostUnit.Place();
                Board.AddUnit(current, unit.GetComponent<Unit>());
                stateMachine.ChangeState(PlayerStates.Idle);
            }
        }
    }
}
