using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class CameraRotate : MonoBehaviour
{
    [SerializeField] private bool isYRotate = false;
    [SerializeField] private float value = 0;
    [SerializeField] private float speed = 1f;

    private float time = 0;

    private void Update()
    {
        time += Time.deltaTime * speed;

        if (isYRotate)
        {
            transform.position = 3 * new Vector3(Mathf.Sin(time), value, Mathf.Cos(time));
        }
        else
        {
            transform.position = 3 * new Vector3(Mathf.Sin(time), Mathf.Cos(time), value);
        }

        transform.LookAt(Vector3.zero);
    }
}