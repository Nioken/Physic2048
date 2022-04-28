using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{
    public List<NumericCube> SpawnedCubes = new List<NumericCube>();
    private Vector3 mousePos;
    private Vector3 SpawnPos;
    [SerializeField]
    private GameObject CubePref;
    public static GameManagerScript GameManager;
    [SerializeField]
    private TMP_Text RecordText;
    [SerializeField]
    private GameObject LosePanel;
    private bool IsLose = false;
    private void Start()
    {
        Application.targetFrameRate = 60;
        if (PlayerPrefs.GetInt("Record") == 0)
        {
            RecordText.text = "2";
        }
        else
        {
            RecordText.text = PlayerPrefs.GetInt("Record").ToString();
        }
        GameManager = this;
        SpawnPos = SpawnedCubes[0].transform.position;
    }
    void Update()
    {
        InputDirection();
    }

    public void InputDirection()
    {
        if (!IsLose)
        {
#if UNITY_STANDALONE_WIN && !UNITY_EDITOR
            if (Input.GetMouseButtonDown(0))
                mousePos = Input.mousePosition;

            if (Input.GetMouseButtonUp(0))
            {
                ChangeSceneGravity((mousePos - Input.mousePosition) * -1 / 10);
                InstansiateCube();
            }
#endif
#if UNITY_ANDROID
            if(Input.touches.Length > 0)
            {
                if (Input.touches[0].phase == TouchPhase.Began)
                {
                    mousePos = Input.touches[0].position;
                }
                if(Input.touches[0].phase == TouchPhase.Ended)
                {
                    ChangeSceneGravity((mousePos - new Vector3(Input.touches[0].position.x, Input.touches[0].position.y,0)) * -1 / 10);
                    InstansiateCube();
                }
            }
#endif
#if UNITY_EDITOR
            if (Input.GetMouseButtonDown(0))
                mousePos = Input.mousePosition;

            if (Input.GetMouseButtonUp(0))
            {
                ChangeSceneGravity((mousePos - Input.mousePosition) * -1 / 10);
                InstansiateCube();
            }
#endif
        }
    }

    public void ChangeSceneGravity(Vector3 GravityDirection)
    {
        Physics.gravity = GravityDirection;
    }

    public void InstansiateCube()
    {
        int randomRetrys = 0;
        float minX = Camera.main.ScreenToWorldPoint(new Vector3(0,0)).x;
        float maxX = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0)).x;
        float minY = Camera.main.ScreenToWorldPoint(new Vector3(0, 0)).y;
        float maxY = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height)).y;
        Color randomColor = new Color(UnityEngine.Random.Range(0f,1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f));
        Vector3 RandomPos = new Vector3(UnityEngine.Random.Range(minX,maxX),UnityEngine.Random.Range(minY,maxY),1);
        while (Physics.CheckSphere(RandomPos, 0.65f))
        {
            RandomPos = new Vector3(UnityEngine.Random.Range(minX, maxX), UnityEngine.Random.Range(minY, maxY), 1);
            randomRetrys++;
            if(randomRetrys > 1000)
            {
                IsLose = true;
                for(int i = 0;i< SpawnedCubes.Count; i++)
                {
                    SpawnedCubes[i].GetComponent<Rigidbody>().isKinematic = true;
                    SpawnedCubes[i].transform.DOScale(0,UnityEngine.Random.Range(0.1f,2f));
                }
                LosePanel.SetActive(true);
                LosePanel.transform.DOScale(1, 0.3f);
                break;
            }
        }
        SpawnedCubes.Add(Instantiate(CubePref, RandomPos, Quaternion.identity).GetComponent<NumericCube>());
        SpawnedCubes[SpawnedCubes.Count-1].gameObject.GetComponent<Renderer>().material.color = randomColor;
    }

    public void UpdateRecord(int record)
    {
        RecordText.transform.DOScale(new Vector3(0.5f, 2.1f, 2.1f), 0.2f).OnComplete(() => RecordText.transform.DOScale(new Vector3(0.4f, 1.7f, 1.7f),0.2f));
        RecordText.text = record.ToString();
        PlayerPrefs.SetInt("Record", record);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }
}
