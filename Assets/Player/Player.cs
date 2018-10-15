using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    [SerializeField] float maxHealthPoints = 100f;
    float currentHealthPoints = 100f;

    [SerializeField] float maxMannaPoints = 30f;
    float currentMannaPoints = 30f;

    public float healthAsPercentage
    {

        get
        {
            return currentHealthPoints / maxHealthPoints;
        }
    }

    public float mannaAsPercentage
    {

        get
        {
            return currentMannaPoints / maxMannaPoints;
        }
    }

}
