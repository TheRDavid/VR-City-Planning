using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WASDMove : MonoBehaviour
{
    private float speed = 0.01f;
    private Camera cam;

    private void Start()
    {
        cam = gameObject.transform.Find("Camera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            gameObject.transform.position += cam.transform.forward * speed;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            gameObject.transform.position += cam.transform.forward * -speed;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            gameObject.transform.position += Quaternion.Euler(0, -90, 0) * cam.transform.forward * -speed;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            gameObject.transform.position += Quaternion.Euler(0, 90, 0) * cam.transform.forward * -speed;
        }


    }
}
