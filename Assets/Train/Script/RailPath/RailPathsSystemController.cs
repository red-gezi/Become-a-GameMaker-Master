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
    //����𳵽ڵ㣬�ҵ���ӽ�����·�ڵ�
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
    //������������
    public void Save()
    {

    }
    //������������
    public void Load()
    {

    }
    //���¼�����ӹ�ϵ
    public void ReConnectRail()
    {

    }

    
}
