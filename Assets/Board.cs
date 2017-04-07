using Assets.defs;
using Assets.util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{

    public List<GameObject> TilePrefabs;

    public GameObject GhostSettlementPrefab;
    private GameObject ghostSettlement;

    private System.Random random;

    private Dictionary<HexCoords, Tile> tileMap;

    private BoxCollider boardCollider;

    HexIntersection lastIntersection = null;

	void Start ()
    {
        random = new System.Random();
        tileMap = new Dictionary<HexCoords, Tile>();
        boardCollider = GetComponent<BoxCollider>();
        ghostSettlement = Instantiate(GhostSettlementPrefab, transform);
        GenerateMap();
	}
	
	void Update ()
    {
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(mouseRay, out hit))
        {
            HexIntersection current = HexIntersection.GetNearestIntersection(hit.point);
            if (ghostSettlement != null)
            {
                if (current != lastIntersection)
                {
                    ghostSettlement.transform.position = HexIntersection.ToLocalCoords(current);
                }
                if (Input.GetMouseButtonDown(0))
                {
                    ghostSettlement.GetComponent<GhostUnit>().Place();
                }
            }
            lastIntersection = current;
        }
	}

    private void GenerateMap()
    {
        //generate the list of tiles
        List<GameObject> tileList = new List<GameObject>();
        foreach (GameObject prefab in TilePrefabs)
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
