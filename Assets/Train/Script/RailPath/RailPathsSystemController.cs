using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TreeEditor;
using UnityEngine;

public class RailPathsSystemController : MonoBehaviour
{
    public List<RailPathController> railPathControllers = new List<RailPathController>();
    public GameObject LineRailPathPrefab;
    public GameObject CurveRailPathPrefab;
    public static RailPathsSystemController Instance { get; set; }
    private void Awake() => Instance = this;
    // Start is called before the first frame update
    //传入火车节点，找到最接近的铁路节点
    public void FindClostRailAndIndex(TrainNodeController trainNode)
    {

        var closestRailPath = railPathControllers
            .OrderBy(railPath => Vector3.Distance(railPath.transform.position, trainNode.transform.position))
            .FirstOrDefault();

        if (closestRailPath != null)
        {
            trainNode.currentRailPath = closestRailPath;
            var closestPoint = closestRailPath.Points
                .OrderBy(point => Vector3.Distance(point, trainNode.transform.position))
                .FirstOrDefault();
            trainNode.currentNodeIndex = closestRailPath.Points.IndexOf(closestPoint);
        }
        else
        {
            Debug.LogError("No rail path found within the collection.");
        }
    }
    //保存铁轨数据
    public void Save()
    {

    }
    //加载铁轨数据
    public void Load()
    {

    }
    //重新检查链接关系
    public void ReConnectRail()
    {

    }

    
}
