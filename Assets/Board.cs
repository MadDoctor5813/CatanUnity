using Assets.defs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{

    private GameObject oreTilePrefab;

    private const float HexRadius = 1f;
    private const float InnerHexRadius = 0.866f * HexRadius;

	void Start ()
    {
        GenerateMap();
	}
	
	void Update ()
    {
		
	}

    private void GenerateMap()
    {
        oreTilePrefab = LoadTilePrefab(ResourceTypes.Ore);
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
            Instantiate(oreTilePrefab, this.transform.TransformPoint(HexCoordsToLocalCoords(new Vector2(x, startZ))),
                Quaternion.Euler(-90, 0, -90), this.transform);
            //hex coords increase by two every time we move up one hex
            startZ += 2;
        }
    }

    private GameObject LoadTilePrefab(ResourceTypes resource)
    {
        string resourceStr = resource.ToString().ToLower();
        return Resources.Load<GameObject>(string.Format("terrain/{0}/tile_{0}", resourceStr));
    }

    private Vector3 HexCoordsToLocalCoords(Vector2 hexCoords)
    {
        return new Vector3(hexCoords.x * HexRadius * 1.5f, 0, hexCoords.y * InnerHexRadius);
    }

}
