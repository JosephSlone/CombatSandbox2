﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(CameraRaycaster))]
public class CursorAffordance : MonoBehaviour {

    [SerializeField] Texture2D walkCursor = null;
    [SerializeField] Texture2D enemyCursor = null;
    [SerializeField] Texture2D unknownCursor = null;
    [SerializeField] Vector2 cursorHotSpot = new Vector2(0,0);

    CameraRaycaster cameraRaycaster;

	// Use this for initialization
	void Start () {
        cameraRaycaster = GetComponent<CameraRaycaster>();
        cameraRaycaster.onLayerChange += OnLayerChanged;  // Registering
    }

	void OnLayerChanged (Layer newLayer) {


        switch(newLayer)
        {
            case Layer.Enemy:
                Cursor.SetCursor(enemyCursor, cursorHotSpot, CursorMode.Auto);
                break;
            case Layer.Buildings:
                Cursor.SetCursor(unknownCursor, cursorHotSpot, CursorMode.Auto);
                break;
            case Layer.Water:
                Cursor.SetCursor(unknownCursor, cursorHotSpot, CursorMode.Auto);
                break;
            case Layer.RaycastEndStop:
                Cursor.SetCursor(unknownCursor, cursorHotSpot, CursorMode.Auto);
                break;
            case Layer.Walkable:
                Cursor.SetCursor(walkCursor, cursorHotSpot, CursorMode.Auto);
                break;
            default:
                Debug.LogError("Don't know what cursor to show");
                break;
        }
	}
}
