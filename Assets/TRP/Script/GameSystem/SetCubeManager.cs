using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCubeManager : MonoBehaviour
{
    //放置位标记方块
    public GameObject setCube;
    public static Vector3 setPos;
    // Start is called before the first frame update
    public void Open()
    {
        setCube.SetActive(true);
    }
    public void Close()
    {
        setCube.SetActive(false);
    }
    private void Update()
    {
        Vector3 targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        targetPos.z = 0;
        targetPos.y -= 0.5f;
        targetPos.x = Mathf.Round(targetPos.x);
        targetPos.y = Mathf.Round(targetPos.y);
        setCube.transform.position = targetPos;
        setPos = targetPos;
    }
}
