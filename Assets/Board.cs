using Assets.defs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{

    public List<GameObject> TilePrefabs;

    private const float HexRadius = 1f;
    private const float InnerHexRadius = 0.866f * HexRadius;

    private System.Random random;

	void Start ()
    {
        random = new System.Random();
        GenerateMap();
	}
	
	void Update ()
    {
		
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
        GenerateMapColumn(-2, -2, 3, tileList);
        GenerateMapColumn(-1, -3, 4, tileList);
        GenerateMapColumn(0, -4, 5, tileList);
        GenerateMapColumn(1, -3, 4, tileList);
        GenerateMapColumn(2, -2, 3, tileList);
    }

    private void GenerateMapColumn(int x, int startZ, int count, List<GameObject> tileList)
    {
        for (int i = 0; i < count; i++)
        {
            Vector2 hexCoords = new Vector2(x, startZ);
            int chosenTileIdx = random.Next(tileList.Count);
            Tile tile = Instantiate(tileList[chosenTileIdx], this.transform.TransformPoint(HexCoordsToLocalCoords(hexCoords)),
                Quaternion.Euler(-90, 0, -90), this.transform).GetComponent<Tile>();
            //delete the chosen tile from the list so we don't use it again
            tileList.RemoveAt(chosenTileIdx);
            tile.HexCoords = hexCoords;
            //hex coords increase by two every time we move up one hex
            startZ += 2;
        }
    }

    private GameObject LoadTilePrefab(TileTypes resource)
    {
        string resourceStr = resource.ToString().ToLower();
        return Resources.Load<GameObject>(string.Format("terrain/{0}/tile_{0}", resourceStr));
    }

    private Vector3 HexCoordsToLocalCoords(Vector2 hexCoords)
    {
        return new Vector3(hexCoords.x * HexRadius * 1.5f, 0, hexCoords.y * InnerHexRadius);
    }

}
