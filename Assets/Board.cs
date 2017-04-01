using Assets.defs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{

	void Start ()
    {
        GenerateMap();
	}
	
	void Update ()
    {
		
	}

    private void GenerateMap()
    {
        GameObject orePrefab = LoadTilePrefab(ResourceTypes.Ore);
        Instantiate(orePrefab, this.transform);
    }

    private GameObject LoadTilePrefab(ResourceTypes resource)
    {
        string resourceStr = resource.ToString().ToLower();
        return Resources.Load<GameObject>(string.Format("terrain/{0}/tile_{0}", resourceStr));
    }

}
