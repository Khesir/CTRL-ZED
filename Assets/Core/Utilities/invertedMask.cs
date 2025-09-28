using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class invertedMask : Mask
{

    public override bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
    {
        return !base.IsRaycastLocationValid(sp, eventCamera);
    }
}
