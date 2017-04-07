using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostUnit : MonoBehaviour
{

    public GameObject RealUnitPrefab;


	void Start ()
	{
		
	}
	
	void Update () 
	{
		
    }

    public void Place()
    {
        Instantiate(RealUnitPrefab, transform.position, transform.rotation, transform.parent);
        Destroy(gameObject);
    }

}
