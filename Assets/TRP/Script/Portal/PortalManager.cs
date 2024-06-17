using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalManager : MonoBehaviour
{
    public Vector3 Initpos;
    // Start is called before the first frame update
    void Start()
    {
        Initpos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Transfer();
        }
        transform.position = Initpos + Vector3.up *( Mathf.Sin(Time.time) * 0.4f+1);
        transform.Rotate(Vector3.up, 15*Time.deltaTime);
        GetComponent<Renderer>().material.color = Color.red * (Mathf.Cos(Time.time) + 1);
    }
    void Transfer()
    {





    }
}
