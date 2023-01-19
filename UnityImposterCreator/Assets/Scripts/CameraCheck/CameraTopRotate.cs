using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTopRotate : MonoBehaviour
{
    [SerializeField] private bool isTop = false;
    [SerializeField] private float speed = 0;
    private float time = 0;

    private int flag = 1;
    
    private void Start()
    {
        flag = isTop ? 1 : -1;
        transform.position = new Vector3(0, flag * 3, 0);
    }

    // Update is called once per frame
    void Update()
    {
        time += speed * Time.deltaTime;
        transform.eulerAngles = new Vector3(90 * flag, time, 0);
    }
}
