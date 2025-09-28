using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class CutoutMaskUI : Image
{
    public override Material materialForRendering
    {
        get
        {
            Material baseMat = new Material(base.materialForRendering);
            baseMat.SetInt("_StencilComp", (int)CompareFunction.NotEqual);
            return baseMat;
        }
    }
    public override bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera)
    {
        return false;
    }
}
