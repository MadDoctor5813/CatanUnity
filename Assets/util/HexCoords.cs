using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[System.Serializable]
public class HexCoords
{

    public const float HexRadius = 1f;
    public const float InnerHexRadius = 0.866f * HexRadius;

    private static readonly Vector2 XBasis = new Vector2(1.5f * HexRadius, InnerHexRadius);
    private static readonly Vector2 ZBasis = new Vector2(0, 2f * InnerHexRadius);

    [SerializeField]
    private int x, z;

    public HexCoords(int x, int z)
    {
        this.x = x;
        this.z = z;
    }

    public int X
    {
        get
        {
            return x;
        }

        set
        {
            x = value;
        }
    }

    public int Z
    {
        get
        {
            return z;
        }

        set
        {
            z = value;
        }
    }

    public int Y
    {
        get
        {
            return -x - z;
        }
    }

    public static Vector3 ToLocalCoords(HexCoords hexCoords)
    {
        Vector2 localCoords = (hexCoords.X * XBasis) + (hexCoords.Z * ZBasis);
        return new Vector3(localCoords.x, 0, localCoords.y);
    }

}
