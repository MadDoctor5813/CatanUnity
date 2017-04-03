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
        //generate columns of the map
        GenerateMapColumn(-2, -2, 3);
        GenerateMapColumn(-1, -3, 4);
        GenerateMapColumn(0, -4, 5);
        GenerateMapColumn(1, -3, 4);
        GenerateMapColumn(2, -2, 3);
    }

    private void GenerateMapColumn(int x, int startZ, int count)
    {
        for (int i = 0; i < count; i++)
        {
            Vector2 hexCoords = new Vector2(x, startZ);
            Tile tile = Instantiate(TilePrefabs[random.Next(TilePrefabs.Count)], this.transform.TransformPoint(HexCoordsToLocalCoords(hexCoords)),
                Quaternion.Euler(-90, 0, -90), this.transform).GetComponent<Tile>();
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
