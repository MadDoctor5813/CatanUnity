using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabContainer : MonoBehaviour
{

    public List<GameObject> Prefabs;

    private Dictionary<string, GameObject> prefabDict;

	void Awake ()
	{
        prefabDict = new Dictionary<string, GameObject>();
		foreach (GameObject go in Prefabs)
        {
            prefabDict.Add(go.name, go);
        }
	}
	
	void Update () 
	{
		
	}

    public GameObject Get(string name)
    {
        return prefabDict[name];
    }

    public List<GameObject> GetWithPrefix(string prefix)
    {
        return Prefabs.FindAll(prefab => prefab.name.StartsWith(prefix));
    }

}
