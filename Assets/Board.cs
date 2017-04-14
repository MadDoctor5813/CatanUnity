﻿using Assets.defs;
using Assets.util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{

    PrefabContainer prefabContainer;
    private List<GameObject> tilePrefabs;

    private System.Random random;

    private Dictionary<HexCoords, Tile> tileMap;
    private Dictionary<HexCorner, Unit> units;

    private HexCornerGraph cornerGraph;

    private BoxCollider boardCollider;

	void Start ()
    {
        prefabContainer = GameObject.Find("PrefabContainer").GetComponent<PrefabContainer>();
        tilePrefabs = prefabContainer.GetWithPrefix("tile");
        random = new System.Random();
        tileMap = new Dictionary<HexCoords, Tile>();
        units = new Dictionary<HexCorner, Unit>();
        cornerGraph = new HexCornerGraph();
        boardCollider = GetComponent<BoxCollider>();
        GenerateMap();
	}
	
	void Update ()
    {
        foreach (var corner in cornerGraph.Neighbors.Keys)
        {
            HexCorner[] neighbors = cornerGraph.Neighbors[corner];
            for (int i = 0; i < neighbors.Length; i++)
            {
                Debug.DrawLine(HexCorner.ToLocalCoords(neighbors[i]), HexCorner.ToLocalCoords(corner), Color.red);
            }
        }
	}

    public void AddUnit(HexCorner intersection, Unit unit)
    {
        units.Add(intersection, unit);
    }

    private void GenerateMap()
    {
        //generate the list of tiles
        List<GameObject> tileList = new List<GameObject>();
        foreach (GameObject prefab in tilePrefabs)
        {
            TileTypes type = prefab.GetComponent<Tile>().Type;
            int tileCount = TileInfo.TileCounts[type];
            for (int i = 0; i < tileCount; i++)
            {
                tileList.Add(prefab);
            }
        }
        //generate columns of the map
        GenerateMapColumn(-2, 0, 3, tileList);
        GenerateMapColumn(-1, -1, 4, tileList);
        GenerateMapColumn(0, -2, 5, tileList);
        GenerateMapColumn(1, -2, 4, tileList);
        GenerateMapColumn(2, -2, 3, tileList);
        GenerateCollider();
    }

    public bool IsValidSettlement(HexCorner corner)
    {
        var unitLocations = units.Keys;
        foreach (var location in unitLocations)
        {
            if (units[location].Type == UnitTypes.Settlement && cornerGraph.CornerDistance(location, corner) < 2)
            {
                return false;
            }
        }
        return true;
    }

    public bool IsValidCity(HexCorner corner)
    {
        if (units.ContainsKey(corner) && units[corner].Type == UnitTypes.Settlement)
        {
            return true;
        }
        return false;
    }
    private void GenerateMapColumn(int x, int startZ, int count, List<GameObject> tileList)
    {
        for (int i = 0; i < count; i++)
        {
            HexCoords hexCoords = new HexCoords(x, startZ);
            int chosenTileIdx = random.Next(tileList.Count);
            Tile tile = Instantiate(tileList[chosenTileIdx], this.transform.TransformPoint(HexCoords.ToLocalCoords(hexCoords)),
                Quaternion.Euler(-90, 0, -90), this.transform).GetComponent<Tile>();
            //delete the chosen tile from the list so we don't use it again
            tileList.RemoveAt(chosenTileIdx);
            tile.HexCoords = hexCoords;
            tileMap.Add(hexCoords, tile);
            startZ++;
        }
    }

    private void GenerateCollider()
    {
        Bounds colliderBounds = new Bounds();
        foreach (Tile tile in tileMap.Values)
        {
            Bounds tileBounds = tile.gameObject.GetComponent<Renderer>().bounds;
            colliderBounds.Encapsulate(tileBounds);
        }
        boardCollider.center = colliderBounds.center - transform.position;
        boardCollider.size = colliderBounds.size;
    }

}
