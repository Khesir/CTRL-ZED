using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{
    public float parallaxFactor;
    public Renderer customRenderer;
    public void Move(float delta)
    {
        Vector3 newPos = transform.localPosition; // Ensure customRenderer is used instead of renderer
        newPos.x -= delta * parallaxFactor;

        transform.localPosition = newPos;
    }
}
