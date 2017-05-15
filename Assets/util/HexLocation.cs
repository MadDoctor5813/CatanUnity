using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.util
{
    public abstract class HexLocation
    {

        public abstract Vector3 ToLocalCoords();

        //Most classes won't need a custom rotation, so a default is provided
        public virtual Quaternion ToLocalRot()
        {
            return Quaternion.identity;
        }

    }
}
