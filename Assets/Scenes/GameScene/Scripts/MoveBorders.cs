using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBorders : MonoBehaviour
{
    public GameObject LeftPlane;
    public GameObject RightPlane;
    public GameObject UpPlane;
    public GameObject DownPlane;
    void Start()
    {
        LeftPlane.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(0,0));
        RightPlane.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width,0));
        UpPlane.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height));
        DownPlane.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(0, 0));
    }
}
