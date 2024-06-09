using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ItemManager.Init(new List<GameItem>()
        {
            new Sand(18),
            new StoneBrick(33),
            new Soil(64),
            null,
            null,
            null,
            null,
            null,
            null,
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
