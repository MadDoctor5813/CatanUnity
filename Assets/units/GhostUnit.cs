using Assets.defs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostUnit : MonoBehaviour
{

    public UnitTypes Type;
    public GameObject RealUnitPrefab;


	void Start ()
	{
		
	}
	
	void Update () 
	{
		
    }

    public GameObject Place()
    {
        Destroy(gameObject);
        return Instantiate(RealUnitPrefab, transform.position, transform.rotation, transform.parent);
    }

}
