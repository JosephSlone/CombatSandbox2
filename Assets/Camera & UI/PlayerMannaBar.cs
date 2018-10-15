using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMannaBar : MonoBehaviour {

    RawImage mannaBarRawImage;
    Player player;

    // Use this for initialization
    void Start()
    {
        player = FindObjectOfType<Player>();
        mannaBarRawImage = GetComponent<RawImage>();
    }

    // Update is called once per frame
    void Update()
    {
        float xValueManna = -(player.mannaAsPercentage / 2f) - 0.5f;
        mannaBarRawImage.uvRect = new Rect(xValueManna, 0f, 0.5f, 1f);
    }

}
