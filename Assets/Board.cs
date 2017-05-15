using Assets.defs;
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
    private Dictionary<HexEdge, Road> roads;

    private HexCornerGraph cornerGraph;

    private BoxCollider boardCollider;

    private Dictionary<UnitTypes, GameObject> unitPrefabs;
    private Unit ghostUnit;

    private GameObject roadPrefab;
    private Road ghostRoad;

	void Start ()
    {
        prefabContainer = GameObject.Find("PrefabContainer").GetComponent<PrefabContainer>();
        tilePrefabs = prefabContainer.GetWithPrefix("tile");
        unitPrefabs = new Dictionary<UnitTypes, GameObject>();
        foreach (var unit in prefabContainer.GetWithPrefix("unit"))
        {
            unitPrefabs.Add(unit.GetComponent<Unit>().Type, unit);
        }
        roadPrefab = prefabContainer.Get("road");
        random = new System.Random();
        tileMap = new Dictionary<HexCoords, Tile>();
        units = new Dictionary<HexCorner, Unit>();
        roads = new Dictionary<HexEdge, Road>();
        cornerGraph = new HexCornerGraph();
        boardCollider = GetComponent<BoxCollider>();
        GenerateMap();
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
            if (IsValidUnitPlacement(newCorner, type, color))
            {
                //set the unit at this corner invisible, if it's there
                if (units.ContainsKey(newCorner))
                {
                    units[newCorner].GetComponent<Renderer>().enabled = false;
                }
                //set the unit at the old corner visible, if its there
                if (ghostUnit.Location != null && units.ContainsKey(ghostUnit.Location))
                {
                    units[ghostUnit.Location].GetComponent<Renderer>().enabled = true;
                }
                //move the ghost unit to the new location
                ghostUnit.Location = newCorner;
                ghostUnit.transform.position = transform.TransformPoint(HexCorner.ToLocalCoords(newCorner));
            }
        }
    }

    public bool PlaceGhostUnit()
    {
        AddUnit(ghostUnit.Location, ghostUnit);
        ghostUnit = null;
        return true;
    }

    public void AddUnit(HexCorner intersection, Unit unit)
    {
        //if something's already there, delete it
        if (units.ContainsKey(intersection))
        {
            units.Remove(intersection);
        }
        units.Add(intersection, unit);
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
            if (IsValidRoad(edge))
            {
                ghostRoad.transform.position = HexEdge.ToLocalCoords(edge);
                ghostRoad.transform.rotation = HexEdge.GetRotation(edge);
                ghostRoad.Edge = edge;
            }
        }
    }

    public bool PlaceGhostRoad()
    {
        if (IsValidRoad(ghostRoad.Edge))
        {
            roads.Add(ghostRoad.Edge, ghostRoad);
            ghostRoad = null;
            return true;
        }
        else
        {
            return false;
        }
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

    public bool IsValidUnitPlacement(HexCorner corner, UnitTypes type, PlayerColor color)
    {
        return (type == UnitTypes.Settlement && IsValidSettlement(corner)) || (type == UnitTypes.City && IsValidCity(corner, color));
    }

    public bool IsValidSettlement(HexCorner corner)
    {
        var unitLocations = units.Keys;
        foreach (var location in unitLocations)
        {
            if ((units[location].Type == UnitTypes.Settlement || units[location].Type == UnitTypes.City) && cornerGraph.CornerDistance(location, corner) < 2)
            {
                return false;
            }
        }
        return true;
    }

    public bool IsValidCity(HexCorner corner, PlayerColor color)
    {
        if (units.ContainsKey(corner) && units[corner].Type == UnitTypes.Settlement && units[corner].Color == color)
        {
            return true;
        }
        return false;
    }

    public bool IsValidRoad(HexEdge edge)
    {
        return !roads.ContainsKey(edge);
    }

    public void SetUnitVisible(HexCorner corner, bool visible)
    {
        if (units.ContainsKey(corner))
        {
            units[corner].GetComponent<Renderer>().enabled = visible;
        }
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
