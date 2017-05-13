using Assets.defs;
using Assets.util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{

    public UnitTypes Type;
    public HexCorner Location;
    public PlayerColor Color;
   
	void Start ()
	{
        GetComponent<Renderer>().material.color = PlayerInfo.GetPlayerColorRGB(Color);
	}
	
	void Update () 
	{
		
	}

}
