using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalPatrol : MonoBehaviour
{
    public float patrolDistance;

    public float speed = 1;

    private float count = 0;

    private float startingY;
    // Start is called before the first frame update
    void Start()
    {
        startingY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        count += Time.deltaTime * speed;
        Vector2 pos = transform.position;
        pos.y = startingY + patrolDistance * Mathf.Cos(count);
        transform.position = pos;
    }
}
