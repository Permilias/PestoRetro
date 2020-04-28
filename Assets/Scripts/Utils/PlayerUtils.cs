using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerUtils 
{
   public static Transform PlayerTransform
    {
        get
        {
            return PlayerController._instance.self;
        }
    }

    public static int GetLayer(int layerInt)
    {
        return layerInt;
    }
}
