using Assets.board;
using Assets.defs;
using Assets.util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Assets.action;

public class BoardView : MonoBehaviour
{
    public GameCoordinator coordinator;

    public Board Board { get; set; }

    private IAction currentAction;

    PrefabContainer prefabContainer;
    private Dictionary<TileTypes, GameObject> tilePrefabs;
    private Dictionary<HexCoords, Tile> tileObjs;

    private BoxCollider boardCollider;

    private Dictionary<UnitTypes, GameObject> unitPrefabs;
    private GameObject roadPrefab;

	void Start ()
    {
        Board = new Board(coordinator);
        prefabContainer = GameObject.Find("PrefabContainer").GetComponent<PrefabContainer>();
        List<GameObject> tilePrefabsList = prefabContainer.GetWithPrefix("tile");
        tilePrefabs = new Dictionary<TileTypes, GameObject>();
        foreach (GameObject obj in tilePrefabsList)
        {
            tilePrefabs.Add(obj.GetComponent<Tile>().Type, obj);
        }
        tileObjs = new Dictionary<HexCoords, Tile>();
        unitPrefabs = new Dictionary<UnitTypes, GameObject>();
        foreach (var unit in prefabContainer.GetWithPrefix("unit"))
        {
            unitPrefabs.Add(unit.GetComponent<Unit>().Type, unit);
        }
        roadPrefab = prefabContainer.Get("road");
        boardCollider = GetComponent<BoxCollider>();
        Board.GenerateMap();
        GenerateTileObjs();
        GenerateCollider();
    }

    void Update ()
    {

	}

    public void SetCurrentAction(IAction action)
    {
        if (currentAction != null)
        {
            currentAction.Undo(this);
        }
        currentAction = action;
        currentAction.Display(this);
    }

    public void ApplyCurrentAction()
    {
        currentAction.Apply(this);
        currentAction = null;
    }

    public Unit InstantiateUnit(HexCorner location, PlayerColor color, UnitTypes type)
    {
        Unit unit = Instantiate(unitPrefabs[type], transform.TransformPoint(location.ToLocalCoords()), location.ToLocalRot(),
            transform).GetComponent<Unit>();
        unit.Type = type;
        unit.Color = color;
        return unit;
    }

    public Road InstantiateRoad(HexEdge edge, PlayerColor color)
    {
        Road road = Instantiate(roadPrefab, transform.TransformPoint(edge.ToLocalCoords()), edge.ToLocalRot(),
            transform).GetComponent<Road>();
        road.Edge = edge;
        road.Color = color;
        return road;
    }

    public void SetUnitVisible(HexCorner location, bool visible)
    {
        Board.Units[location].GetComponent<Renderer>().enabled = visible;
    }

    private void GenerateTileObjs()
    {
        foreach (HexCoords coords in Board.TileMap.Keys)
        {
            TileTypes type = Board.TileMap[coords];
            Tile tile = InstantiateTile(coords, type);
            tileObjs.Add(coords, tile);
        }
    }

    private void GenerateCollider()
    {
        Bounds colliderBounds = new Bounds();
        foreach (Tile tile in tileObjs.Values)
        {
            Bounds tileBounds = tile.gameObject.GetComponent<Renderer>().bounds;
            colliderBounds.Encapsulate(tileBounds);
        }
        boardCollider.center = colliderBounds.center - transform.position;
        boardCollider.size = colliderBounds.size;
    }

    private Tile InstantiateTile(HexCoords coords, TileTypes type)
    {
        Vector3 pos = transform.TransformPoint(coords.ToLocalCoords());
        return Instantiate(tilePrefabs[type], pos, coords.ToLocalRot(), transform).GetComponent<Tile>();
    }

}
