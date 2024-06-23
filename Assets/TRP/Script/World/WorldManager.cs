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
    public static PlayerMode CurrentPlayerMode { get; set; } = PlayerMode.Build;
    ////////////////////////////////////////建造标记////////////////////////////////////////////////////
    public static bool IsFirstClick { get; set; } = true;
    public static Vector3 FirstClickPoint { get; set; }
    public static Vector3 SecondClickPoint { get; set; }


    // Start is called before the first frame update
    private void Awake() => Instance = this;
    public static void Init()
    {
        LoadCubePrefabs();
        FillCubes(new ItemData(2, 1), new Vector3(-100, -20), new Vector3(100, 0));
    }
    private void Update()
    {
        //填充模式
        if (Input.GetKeyDown(KeyCode.B) && Input.GetKey(KeyCode.LeftControl))
        {
            CurrentPlayerMode = PlayerMode.Fill;
            Debug.LogWarning("启动填充模式");
        }
        //复制模式
        if (Input.GetKeyDown(KeyCode.C) && Input.GetKey(KeyCode.LeftControl))
        {
            CurrentPlayerMode = PlayerMode.Copy;
            Debug.LogWarning("启动复制模式");
        }
        //粘贴模式
        if (Input.GetKeyDown(KeyCode.V) && Input.GetKey(KeyCode.LeftControl))
        {
            //根据复制的数据显示范围
            CurrentPlayerMode = PlayerMode.Paste;
            Debug.LogWarning("启动粘贴模式");
        }
        if (Input.GetMouseButtonDown(0))
        {
            switch (CurrentPlayerMode)
            {
                case PlayerMode.UseCard:
                    //触发技能组
                    break;
                case PlayerMode.UseBluePrint:
                    //生成虚影
                    break;
                case PlayerMode.Build:
                    //点击后直接放置
                    Use();
                    break;
                case PlayerMode.Fill:
                    if (IsFirstClick)
                    {
                        FirstClickPoint = SetCubeManager.setPos;
                    }
                    else
                    {
                        SecondClickPoint = SetCubeManager.setPos;
                    }
                    FillCubes(QuickItemManager.SelectItem, FirstClickPoint, SecondClickPoint);
                    CurrentPlayerMode = PlayerMode.Build;
                    break;
                case PlayerMode.Copy:
                    if (IsFirstClick)
                    {
                        FirstClickPoint = SetCubeManager.setPos;
                    }
                    else
                    {
                        SecondClickPoint = SetCubeManager.setPos;
                    }
                    CopyCubes(QuickItemManager.SelectItem, FirstClickPoint, SecondClickPoint);
                    CurrentPlayerMode = PlayerMode.Build;
                    break;
                case PlayerMode.Paste:
                    //从记录粘贴到游戏中
                    PasteCubes(QuickItemManager.SelectItem, SetCubeManager.setPos);
                    CurrentPlayerMode = PlayerMode.Build;
                    break;
                default:
                    break;
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            switch (CurrentPlayerMode)
            {
                case PlayerMode.UseCard:
                    //触发技能组
                    break;
                default:
                    //触发默认采集组
                    Destory();
                    break;
            }
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
        //已有将排除
        if (worldCubes.FirstOrDefault(cube => cube.transform.position == SetCubeManager.setPos) != null)
        {
            return;
        }
        //背包打开后将关闭放置交互
        if (BagManager.IsOpen)
        {
            return;
        }
        //为虚无，直接跳过
        if (QuickItemManager.SelectItem.ItemID==0)
        {
            return;
        }
        CreatCube(QuickItemManager.SelectItem, SetCubeManager.setPos);
    }
    public static void CreatCube(ItemData gameItem, Vector3 pos)
    {
        //根据方块的标签数据判断对应的预制体
        var tragetPrefab = cubePrefabs.FirstOrDefault(cube => cube.name == gameItem.ItemTag);
        //根据物体类型进行判断

        var cube = Instantiate(tragetPrefab);
        var cubemanager = cube.AddComponent<GameCube>();
        cubemanager.cubeData = gameItem.CubeData;
        cube.transform.position = pos;
        Instance.worldCubes.Add(cube);
    }
    public static void FillCubes(ItemData gameItem, Vector3 startPos, Vector3 endPos)
    {
        for (int i = (int)Mathf.Min(startPos.x, endPos.x); i < (int)Mathf.Max(startPos.x, endPos.x); i++)
        {
            for (int j = (int)Mathf.Min(startPos.y, endPos.y); j < (int)Mathf.Max(startPos.y, endPos.y); j++)
            {
                var item = gameItem;
                CreatCube(item, new Vector3(i, j, 0));
            }
        }
    }
    public static void CopyCubes(ItemData gameItem, Vector3 startPos, Vector3 endPos)
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
    public static void PasteCubes(ItemData gameItem, Vector3 startPos)
    {
        //for (int i = (int)Mathf.Min(startPos.x, endPos.x); i < (int)Mathf.Max(startPos.x, endPos.x); i++)
        //{
        //    for (int j = (int)Mathf.Min(startPos.y, endPos.y); j < (int)Mathf.Max(startPos.y, endPos.y); j++)
        //    {
        //        //var item = new StoneBrick(1);
        //        //CreatCube(item, new Vector3(i, j, 0));
        //    }
        //}
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
