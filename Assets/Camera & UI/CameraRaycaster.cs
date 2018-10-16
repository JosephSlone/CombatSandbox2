﻿using UnityEngine;

public class CameraRaycaster : MonoBehaviour
{
    private Layer[] layerPriorities = {
        Layer.Enemy,
        Layer.Water,
        Layer.Buildings,
        Layer.Walkable       
    };

    [SerializeField] float distanceToBackground = 100f;
    Camera viewCamera;


    RaycastHit raycastHit;
    public RaycastHit hit
    {
        get { return raycastHit; }
    }

    Layer layerHit;
    public Layer currentLayerHit
    {
        get { return layerHit; }
    }

    public delegate void OnLayerChange(Layer newLayer);  // declare new delegate type
    public event OnLayerChange onLayerChange;  // Instantiate an observer set

    void Start() // TODO Awake?
    {
        viewCamera = Camera.main;
        foreach(Layer layer in layerPriorities)
        {
            print("Layer: " + layer.ToString());
        }
        
    }

    void Update()
    {
        // Look for and return priority layer hit
        foreach (Layer layer in layerPriorities)
        {
            var hit = RaycastForLayer(layer);
            if (hit.HasValue)
            {
                raycastHit = hit.Value;
                if (layerHit != layer)  // if layer has changed
                {
                    layerHit = layer;
                    onLayerChange(layer);
                }
                //layerHit = layer;
                return;
            }
        }

        // Otherwise return background hit
        raycastHit.distance = distanceToBackground;
        layerHit = Layer.RaycastEndStop;
    }

    RaycastHit? RaycastForLayer(Layer layer)
    {
        int layerMask = 1 << (int)layer; // See Unity docs for mask formation
        Ray ray = viewCamera.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit; // used as an out parameter
        bool hasHit = Physics.Raycast(ray, out hit, distanceToBackground, layerMask);
        if (hasHit)
        {
            return hit;
        }
        return null;
    }
}
