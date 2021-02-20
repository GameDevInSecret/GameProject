using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinSpikeyBall : MonoBehaviour
{
    private bool stopped = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (stopped) return;
        transform.Rotate(new Vector3(0, 0, 0.4F));
    }

    public void TestResponse()
    {
        stopped = true;
    }
}
