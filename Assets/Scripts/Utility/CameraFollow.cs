using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public Transform followTransform;

    // Update is called once per frame
    void Update()
    {
        Vector2 playerPos = followTransform.position;
        transform.position = new Vector3(playerPos.x, 0, -10);
    }
}
