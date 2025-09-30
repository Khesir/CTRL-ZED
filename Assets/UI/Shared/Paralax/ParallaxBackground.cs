using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    public ParallaxCamera parallaxCamera;
    public List<ParallaxLayer> parallaxLayers = new List<ParallaxLayer>();

    public void Initialize()
    {
        if (parallaxCamera == null)
            parallaxCamera = Camera.main.GetComponent<ParallaxCamera>();

        if (parallaxCamera != null)
            parallaxCamera.onCameraTranslate += Move;

    }

    public void SetLayers()
    {
        parallaxLayers.Clear();

        for (int i = 0; i < transform.childCount; i++)
        {
            ParallaxLayer layer = transform.GetChild(i).GetComponent<ParallaxLayer>();

            if (layer != null)
            {
                layer.name = "Layer-" + i;
                layer.customRenderer = layer.gameObject.GetComponent<Renderer>();
                parallaxLayers.Add(layer);
            }
        }
    }

    void Move(float delta)
    {
        foreach (ParallaxLayer layer in parallaxLayers)
        {
            layer.Move(delta);
        }
    }
    public void SetupParallaxLayerMaterial(List<Material> sprites)
    {
        int layerCount = parallaxLayers.Count;

        for (int i = 0; i < layerCount; i++)
        {

            if (i < sprites.Count && sprites[i] != null)
            {
                parallaxLayers[i].gameObject.SetActive(true);
                parallaxLayers[i].customRenderer.material = new Material(sprites[i]);
            }
            else
            {
                parallaxLayers[i].gameObject.SetActive(false);
            }
        }
    }
}
