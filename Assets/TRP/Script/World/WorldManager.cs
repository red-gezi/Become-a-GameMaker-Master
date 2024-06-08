using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    public GameObject cubePrefab;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = -100; i < 100; i++)
        {
            for (int j = -20; j < 0; j++)
            {
                var cube = Instantiate(cubePrefab);
                cube.transform.position = new Vector3(i, j, 0);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
