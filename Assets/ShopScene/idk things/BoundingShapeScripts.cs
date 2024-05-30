using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System.Globalization;

public class BoundingShapeScripts : MonoBehaviour
{
    private CinemachineConfiner2D mainCameraConfiner;
    private CompositeCollider2D colliderForCamera;

    private void Start()
    {
        GetCamera();
        GetCollider("ColliderForCamera");
    }
    private CinemachineConfiner2D GetCamera()
    {
        mainCameraConfiner = GameObject.Find("MainCamera").GetComponent<CinemachineConfiner2D>();
        return mainCameraConfiner;
    }
    private CompositeCollider2D GetCollider(string name)
    {
        colliderForCamera = GameObject.Find(name).GetComponent<CompositeCollider2D>();
        return colliderForCamera;
    }
    public void SetBoundingShape(CinemachineConfiner2D mainCameraConfiner, CompositeCollider2D collider)
    {
        mainCameraConfiner.m_BoundingShape2D = collider;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && gameObject.name == "CameraLoadTrigger")
        {
            Debug.Log("wykryto gracza w kamerze drugiego pokoju");
            Debug.Log(mainCameraConfiner.name);
            Debug.Log(GetCollider("ColliderForCamera").name);
            SetBoundingShape(mainCameraConfiner,GetCollider("ColliderForCamera"));
            Destroy(gameObject);
        }
        else if(collision.CompareTag("Player") && gameObject.name == "LoadTranisitionCameraTrigger")
        {
            SetBoundingShape(mainCameraConfiner, GetCollider("TransitionCameraCollider"));
            Destroy(gameObject);
            Debug.Log("wykryto gracza w kamerze miedzy pokojami");
            Debug.Log(mainCameraConfiner.name);
            Debug.Log(GetCollider("ColliderForCamera").name);
        }
    }
}
