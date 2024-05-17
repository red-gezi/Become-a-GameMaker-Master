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
    public int pointsCount = 5; // �����ϵ������
    public float amplitude = 2f; // ���ߵ����
    public float frequency = 1f; // ���Ҳ���Ƶ��
    public float lineWidth = 0.1f; // �������
    public GameObject pointPrefab; // ����ʵ�����ĵ��Ԥ����
    public bool isCurve;//�Ƿ�����������
    public RailPathController FM_RailPath;
    public RailPathController FL_RailPath;
    public RailPathController FR_RailPath;
    public RailPathController BM_RailPaths;
    public RailPathController BL_RailPath;
    public RailPathController BR_RailPath;
    public List<Vector3> Points { get; set; }
    //����������β������㳯��
    public Vector3 Direction => Points.Last() - Points.First();
    private string CurrentBranch { get; set; } = "M";// Ĭ���м��֧

    //�������������ϵͳ
    public RailPathsSystemController belongRailPathSystem;
    public Vector3 GetPutBias(bool isForward = true)
    {
        float angel = transform.eulerAngles.y;
        if (isCurve && isForward)
        {
            angel -= 90;
        }

        //���Ƕ�ӳ�䵽0~360��֮��
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
    //��ʼ��·����λ��
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
                float �� = s / r;
                x = Mathf.Cos(��) * r - r;
                y = 0;
                z = Mathf.Sin(��) * r - r;
            }

            // ʹ�����߼����θ߶�
            Vector3 rayOrigin = transform.position + new Vector3(x, 10, z); // ��һ���ϸߵ�λ�÷������ߣ�ȷ���ܻ��е���
            if (Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit))
            {
                // ���߻����˵��λ�������ײ��
                y = hit.point.y - 5; // ����y����Ϊ���θ߶ȼ���ƫ����
                                     //Debug.Log(hit.point.y);
            }
            else
            {
                Debug.LogWarning("Raycast did not hit anything.");
                // ���û�л����κ����壬������Ҫ�����������
            }
            Vector3 position = new Vector3(x, y, z);
            transform.GetChild(0).GetChild(i).transform.localPosition = position;
        }
        Points = transform.GetChild(0)
                          .Cast<Transform>()
                          .Select(child => child.position)
                          .ToList();
    }
    //��ʼ����֧������λ��
    private void RefreshBranchPoint()
    {
        foreach (Transform point in transform.GetChild(2))
        {
            Vector3 targetPos = point.localPosition;
            // ʹ�����߼����θ߶�
            Vector3 rayOrigin = transform.position + new Vector3(targetPos.x, 10, targetPos.z); // ��һ���ϸߵ�λ�÷������ߣ�ȷ���ܻ��е���
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
    [Button("������ǰ��������")]
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
    [Button("������ǰ��������")]
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
    [Button("������ǰ��������")]
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
    [Button("��������������")]
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
    [Button("�������������")]
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
    [Button("�����Һ�������")]
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
