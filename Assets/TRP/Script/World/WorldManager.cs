using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    public List<GameObject> cubePrefabs;

    List<GameObject> worldCubes = new();
    // Start is called before the first frame update
    void Start()
    {
        for (int i = -100; i < 100; i++)
        {
            for (int j = -20; j < 0; j++)
            {
                var cube = Instantiate(cubePrefabs[0]);
                cube.transform.position = new Vector3(i, j, 0);
                worldCubes.Add(cube);
            }
        }
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Use();
        }
        if (Input.GetMouseButtonDown(1))
        {
            Destory();
        }
    }
    public void Use()
    {
        string name = ItemManager.SelectItem.itemIcon.ToString();
        var tragetPrefab= cubePrefabs.FirstOrDefault(cube => cube.name == name); ;
        var cube = Instantiate(tragetPrefab);
        cube.transform.position = SetCubeManager.setPos;
        worldCubes.Add(cube);
    }
    public void Destory()
    {
        var target = worldCubes.FirstOrDefault(cube => cube.transform.position == SetCubeManager.setPos);
        worldCubes.Remove(target);
        DestroyImmediate(target);
    }
}
