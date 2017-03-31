using Assets.defs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GenerateMap();
	}
	
	// Update is called once per frame
	void Update () {
		
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
