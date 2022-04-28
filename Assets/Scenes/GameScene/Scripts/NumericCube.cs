using System.Collections;
using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class NumericCube : MonoBehaviour
{
    public bool alreadyDead = false;


    public void CheckCubesCollision(Collision collision)
    {
        if (!alreadyDead)
        {
            //if (collision.gameObject.tag == "Cube")
                //GameManagerScript.GameManager.InstansiateCube();

            NumericCube script = collision.gameObject.GetComponent<NumericCube>();
            if (script != null)
            {
                script.alreadyDead = true;
                if (collision.transform.GetComponentInChildren<TMP_Text>().text == transform.GetComponentInChildren<TMP_Text>().text)
                {
                    GameManagerScript.GameManager.GetComponent<AudioSource>().Play();
                    for (int i = 0; i < 6; i++)
                    {
                        transform.GetChild(i).GetComponent<TMP_Text>().text = (Convert.ToInt32(transform.GetChild(i).GetComponent<TMP_Text>().text) * 2).ToString();
                    }
                    if(Convert.ToInt32(transform.GetChild(0).GetComponent<TMP_Text>().text) > PlayerPrefs.GetInt("Record"))
                    {
                        Debug.Log("UpdateCalled");
                        GameManagerScript.GameManager.UpdateRecord(Convert.ToInt32(transform.GetChild(0).GetComponent<TMP_Text>().text));
                    }
                    if (collision.gameObject.tag == "Cube")
                    {
                        GameObject tmp = collision.gameObject;
                    collision.gameObject.GetComponent<Rigidbody>().isKinematic = true;
                    collision.collider.enabled = false;
                        collision.transform.DOScale(0, 0.3f);
                        collision.transform.DOMove(transform.position, 0.3f).OnComplete(() => { Destroy(tmp); GameManagerScript.GameManager.SpawnedCubes.Remove(tmp.GetComponent<NumericCube>());});
                    }
                }
            }
        }
    }
    public void OnCollisionEnter(Collision collision)
    {
        CheckCubesCollision(collision);
    }

    private void OnCollisionExit(Collision collision)
    {
        alreadyDead = false;
    }
}
