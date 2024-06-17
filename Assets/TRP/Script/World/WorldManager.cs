using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.IO;
using static WorldManager.WorldData;
using Sirenix.Utilities;

public partial class WorldManager : MonoBehaviour
{
    [ShowInInspector]

    public static List<GameObject> cubePrefabs = new();

    public List<GameObject> worldCubes = new();
    [ShowInInspector]
    public List<PortalCube> portalList = new();
    public GameObject mapObject;
    public static WorldManager Instance { get; set; }
    // Start is called before the first frame update
    private void Awake() => Instance = this;
    public static void Init()
    {
        LoadCubePrefabs();
        FillCubes(new GameItem("Sand",1), new Vector3(-100, -20), new Vector3(-20, 0));
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            switch (ItemManager.SelectItem.itemType)
            {
                case ItemType.Cube:
                    Use();
                    break;
                case ItemType.Wall:
                    Use();
                    break;
                case ItemType.Attachment:
                    Use();
                    break;
                case ItemType.Blueprint:
                    Use();
                    break;
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            Destory();
        }
    }
    static void LoadCubePrefabs()
    {
        cubePrefabs.Clear();
        var prefabs = Resources.LoadAll<GameObject>("CubePrefabs").ToList();
        cubePrefabs.AddRange(prefabs);
    }
    public void Use()
    {
        if (worldCubes.FirstOrDefault(cube => cube.transform.position == SetCubeManager.setPos) != null)
        {
            return;
        }
        if (BagManager.IsOpen)
        {
            return;
        }
        CreatCube(ItemManager.SelectItem, SetCubeManager.setPos);
    }
    public static void CreatCube(GameItem gameItem,Vector3 pos)
    {
        var tragetPrefab = cubePrefabs.FirstOrDefault(cube => cube.name == gameItem.ItemTag);
        var cube = Instantiate(tragetPrefab);
        var cubemanager = cube.AddComponent<GameCube>();
        cubemanager.itemTag = gameItem.ItemTag;
        cube.transform.position = pos;
        Instance.worldCubes.Add(cube);
    }
    public static void FillCubes(GameItem gameItem, Vector3 startPos , Vector3 endPos)
    {
        for (int i = (int)Mathf.Min(startPos.x, endPos.x); i < (int)Mathf.Max(startPos.x, endPos.x); i++)
        {
            for (int j = (int)Mathf.Min(startPos.y, endPos.y); j < (int)Mathf.Max(startPos.y, endPos.y); j++)
            {
                var item = ItemManager.SelectItem;
                CreatCube(item, new Vector3(i, j, 0));
            }
        }
    }
    public static void CopyCubes(GameItem gameItem, Vector3 startPos, Vector3 endPos)
    {
        for (int i = (int)Mathf.Min(startPos.x, endPos.x); i < (int)Mathf.Max(startPos.x, endPos.x); i++)
        {
            for (int j = (int)Mathf.Min(startPos.y, endPos.y); j < (int)Mathf.Max(startPos.y, endPos.y); j++)
            {
                //var item = new ItemManager.SelectItem;
                //CreatCube(item, new Vector3(i, j, 0));
            }
        }
    }
    public static void PasteCubes(GameItem gameItem, Vector3 startPos, Vector3 endPos)
    {
        for (int i = (int)Mathf.Min(startPos.x, endPos.x); i < (int)Mathf.Max(startPos.x, endPos.x); i++)
        {
            for (int j = (int)Mathf.Min(startPos.y, endPos.y); j < (int)Mathf.Max(startPos.y, endPos.y); j++)
            {
                //var item = new StoneBrick(1);
                //CreatCube(item, new Vector3(i, j, 0));
            }
        }
    }
    public void Destory()
    {
        var target = worldCubes.FirstOrDefault(cube => cube.transform.position == SetCubeManager.setPos);
        worldCubes.Remove(target);
        DestroyImmediate(target);
    }
    [Button]
    public void Save(WorldTag worldTag)
    {
        Debug.Log(new WorldData(worldCubes, portalList).ToJson());
        File.WriteAllText($"Assets/TRP/SaveData/{worldTag}.json", new WorldData(worldCubes, portalList).ToJson(Newtonsoft.Json.Formatting.None));
    }
    [Button]
    public void Load(WorldTag worldTag)
    {

    }
}
