using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCtrl : MonoBehaviour
{
    public float speed = 7.0f;
    void Update()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            if(transform.position.x < 33)
            {
                transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
            }
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            if (transform.position.x > -8)
            {
                transform.position -= new Vector3(speed * Time.deltaTime, 0, 0);
            }
        }
    }
}
