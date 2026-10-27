using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class eventFan : MonoBehaviour
{
    public float speedFan = 10f;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, speedFan * Time.deltaTime, 0);
    }
}
