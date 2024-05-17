using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class RailPathController : MonoBehaviour
{
    public int pointsCount = 5; // 曲线上点的数量
    public float amplitude = 2f; // 曲线的振幅
    public float frequency = 1f; // 正弦波的频率
    public float lineWidth = 0.1f; // 线条宽度
    public GameObject pointPrefab; // 用于实例化的点的预制体
    public bool isCurve;//是否是曲线铁轨
    public RailPathController FM_RailPath;
    public RailPathController FL_RailPath;
    public RailPathController FR_RailPath;
    public RailPathController BM_RailPaths;
    public RailPathController BL_RailPath;
    public RailPathController BR_RailPath;
    public List<Vector3> Points { get; set; }
    //根据铁轨首尾坐标计算朝向
    public Vector3 Direction => Points.Last() - Points.First();
    private string CurrentBranch { get; set; } = "M";// 默认中间分支

    //所属的铁轨控制系统
    public RailPathsSystemController belongRailPathSystem;
    public Vector3 GetPutBias(bool isForward = true)
    {
        float angel = transform.eulerAngles.y;
        if (isCurve && isForward)
        {
            angel -= 90;
        }

        //将角度映射到0~360度之间
        angel = (angel + 360) % 360;
        return angel switch
        {
            0 => Vector3.forward,
            90 => Vector3.right,
            180 => Vector3.back,
            270 => Vector3.left,
            _ => Vector3.zero,
        };
    }

    private void Awake()
    {
        CreatPathPoint();
        RefreshPathPoint();
        RefreshBranchPoint();

    }
    private void CreatPathPoint()
    {
        for (int i = 0; i < pointsCount; i++)
        {
            GameObject point = Instantiate(pointPrefab);
            point.transform.parent = transform.GetChild(0);
        }
    }
    //初始化路径点位置
    private void RefreshPathPoint()
    {
        for (int i = 0; i <= pointsCount; i++)
        {
            float x, y, z, t;
            if (!isCurve)
            {
                t = (float)i / pointsCount;
                x = 0;
                y = 0;
                z = Mathf.Lerp(-10, 10, t);
            }
            else
            {
                int r = 10;
                float totalArcLength = Mathf.PI * r / 2;
                float arcLengthPerPoint = totalArcLength / pointsCount;

                float s = i * arcLengthPerPoint;
                float θ = s / r;
                x = Mathf.Cos(θ) * r - r;
                y = 0;
                z = Mathf.Sin(θ) * r - r;
            }

            // 使用射线检测地形高度
            Vector3 rayOrigin = transform.position + new Vector3(x, 10, z); // 从一个较高的位置发射射线，确保能击中地面
            if (Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit))
            {
                // 射线击中了地形或其他碰撞器
                y = hit.point.y - 5; // 设置y坐标为地形高度加上偏移量
                                     //Debug.Log(hit.point.y);
            }
            else
            {
                Debug.LogWarning("Raycast did not hit anything.");
                // 如果没有击中任何物体，可能需要处理这种情况
            }
            Vector3 position = new Vector3(x, y, z);
            transform.GetChild(0).GetChild(i).transform.localPosition = position;
        }
        Points = transform.GetChild(0)
                          .Cast<Transform>()
                          .Select(child => child.position)
                          .ToList();
    }
    //初始化分支创建点位置
    private void RefreshBranchPoint()
    {
        foreach (Transform point in transform.GetChild(2))
        {
            Vector3 targetPos = point.localPosition;
            // 使用射线检测地形高度
            Vector3 rayOrigin = transform.position + new Vector3(targetPos.x, 10, targetPos.z); // 从一个较高的位置发射射线，确保能击中地面
            if (Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit))
            {
                point.transform.localPosition = new Vector3(targetPos.x, hit.point.y - 3, targetPos.z);
                //Debug.Log(hit.point.y);
                Debug.DrawRay(rayOrigin, Vector3.down * 10);
            }
            else
            {
                Debug.LogWarning("Raycast did not hit anything.");
            }
        }
    }
    private void InitRailPath(RailPathController fM_RailPath)
    {
        transform.parent.GetComponent<RailPathsSystemController>().railPathControllers.Add(this);
    }
    private void Update()
    {
        //InitPathPoint();
        //InitBranchPoint();
        for (int i = 0; i < pointsCount; i++)
        {
            Vector3 start = transform.GetChild(0).GetChild(i).position;
            Vector3 end = transform.GetChild(0).GetChild(i + 1).position;
            Debug.DrawLine(start, end, Color.red, lineWidth);
        }
    }
    public void InitRailPath()
    {

    }
    [Button("铺设正前方向铁轨")]
    public void AddFM_RailPath()
    {
        if (FM_RailPath == null)
        {
            var newRailPath = Instantiate(RailPathsSystemController.Instance.LineRailPathPrefab, transform.parent);
            newRailPath.transform.position = transform.position + GetPutBias() * 20;
            newRailPath.transform.eulerAngles = transform.eulerAngles + (isCurve ? new Vector3(0, -90, 0) : Vector3.zero);
            FM_RailPath = newRailPath.GetComponent<RailPathController>();
            InitRailPath(FM_RailPath);
        }
    }
    [Button("铺设左前方向铁轨")]
    public void AddFL_RailPath()
    {
        if (FL_RailPath == null)
        {
            var newRailPath = Instantiate(RailPathsSystemController.Instance.CurveRailPathPrefab, transform.parent);
            newRailPath.transform.position = transform.position + GetPutBias() * 20;
            newRailPath.transform.eulerAngles = transform.eulerAngles + (isCurve ? new Vector3(0, -90, 0) : Vector3.zero);
            FL_RailPath = newRailPath.GetComponent<RailPathController>();
            InitRailPath(FL_RailPath);
        }

    }
    [Button("铺设右前方向铁轨")]
    public void AddFR_RailPath()
    {
        if (FR_RailPath == null)
        {
            var newRailPath = Instantiate(RailPathsSystemController.Instance.CurveRailPathPrefab, transform.parent);
            newRailPath.transform.position = transform.position + GetPutBias() * 20;
            newRailPath.transform.eulerAngles = transform.eulerAngles - new Vector3(0, 90, 0) + (isCurve ? new Vector3(0, -90, 0) : Vector3.zero);
            FR_RailPath = newRailPath.GetComponent<RailPathController>();
            InitRailPath(FR_RailPath);
        }
    }
    [Button("铺设正后方向铁轨")]
    public void AddBM_RailPath()
    {
        if (BM_RailPaths == null)
        {
            var newRailPath = Instantiate(RailPathsSystemController.Instance.LineRailPathPrefab, transform.parent);
            newRailPath.transform.position = transform.position - GetPutBias(false) * 20;
            newRailPath.transform.eulerAngles = transform.eulerAngles;// + (IsCurve ? new Vector3(0, -90, 0) : Vector3.zero);
            BM_RailPaths = newRailPath.GetComponent<RailPathController>();
            InitRailPath(BM_RailPaths);
        }
    }
    [Button("铺设左后方向铁轨")]
    public void AddBL_RailPath()
    {
        if (BR_RailPath == null)
        {
            var newRailPath = Instantiate(RailPathsSystemController.Instance.CurveRailPathPrefab, transform.parent);
            newRailPath.transform.position = transform.position - GetPutBias(false) * 20;
            newRailPath.transform.eulerAngles = transform.eulerAngles + new Vector3(0, 90, 0);//+ (IsCurve ? new Vector3(0, -90, 0) : Vector3.zero);
            BL_RailPath = newRailPath.GetComponent<RailPathController>();
            InitRailPath(BR_RailPath);
        }
    }
    [Button("铺设右后方向铁轨")]
    public void AddBR_RailPath()
    {
        if (BR_RailPath == null)
        {
            var newRailPath = Instantiate(RailPathsSystemController.Instance.CurveRailPathPrefab, transform.parent);
            newRailPath.transform.position = transform.position - GetPutBias(false) * 20;
            newRailPath.transform.eulerAngles = transform.eulerAngles + new Vector3(0, 180, 0);// + (IsCurve ? new Vector3(0, -90, 0) : Vector3.zero);
            BR_RailPath = newRailPath.GetComponent<RailPathController>();
            InitRailPath(BR_RailPath);
        }

    }

    public void ChangeRailPathAngel()
    {
        //angle += 90 % 360;
        transform.eulerAngles += new Vector3(0, 90, 0);
    }

}
