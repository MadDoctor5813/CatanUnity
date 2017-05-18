using Assets.board;
using Assets.defs;
using Assets.util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BoardView : MonoBehaviour
{
    public Board Board { get; set; }

    PrefabContainer prefabContainer;
    private Dictionary<TileTypes, GameObject> tilePrefabs;
    private Dictionary<HexCoords, Tile> tileObjs;

    private BoxCollider boardCollider;

    private Dictionary<UnitTypes, GameObject> unitPrefabs;
    private Unit ghostUnit;

    private GameObject roadPrefab;
    private Road ghostRoad;

	void Start ()
    {
        board = new Board();
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

    public void SetGhostUnit(HexCorner newCorner, UnitTypes type, PlayerColor color)
    {
        //if the unit is null, create it
        if (ghostUnit == null)
        {
            ghostUnit = Instantiate(unitPrefabs[type], transform).GetComponent<Unit>();
            ghostUnit.Color = color;
        }
        //if the corner is the same as the temp's corner, nothing has changed
        if (newCorner.Equals(ghostUnit.Location))
        {
            return;
        }
        else
        {
            if (board.IsValidUnitPlacement(newCorner, type, color))
            {
                //set the unit at this corner invisible, if it's there
                if (board.Units.ContainsKey(newCorner))
                {
                    board.Units[newCorner].GetComponent<Renderer>().enabled = false;
                }
                //set the unit at the old corner visible, if its there
                if (ghostUnit.Location != null && board.Units.ContainsKey(ghostUnit.Location))
                {
                    board.Units[ghostUnit.Location].GetComponent<Renderer>().enabled = true;
                }
                //move the ghost unit to the new location
                ghostUnit.Location = newCorner;
                ghostUnit.transform.position = transform.TransformPoint(newCorner.ToLocalCoords());
            }
        }
    }

    public bool PlaceGhostUnit()
    {
        board.AddUnit(ghostUnit.Location, ghostUnit);
        ghostUnit = null;
        return true;
    }

    public void SetGhostRoad(HexEdge edge, PlayerColor color)
    {
        if (ghostRoad == null)
        {
            ghostRoad = Instantiate(roadPrefab, transform).GetComponent<Road>();
            ghostRoad.Color = color;
        }
        if (edge.Equals(ghostRoad.Edge))
        {
            return;
        }
        else
        {
            if (board.IsValidRoad(edge))
            {
                ghostRoad.transform.position = edge.ToLocalCoords();
                ghostRoad.transform.rotation = edge.ToLocalRot();
                ghostRoad.Edge = edge;
            }
        }
    }

    public bool PlaceGhostRoad()
    {
        if (board.IsValidRoad(ghostRoad.Edge))
        {
            board.Roads.Add(ghostRoad.Edge, ghostRoad);
            ghostRoad = null;
            return true;
        }
        else
        {
            return false;
        }
    }

    public void SetUnitVisible(HexCorner corner, bool visible)
    {
        if (board.Units.ContainsKey(corner))
        {
            board.Units[corner].GetComponent<Renderer>().enabled = visible;
        }
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
