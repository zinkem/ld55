using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{

    private GameObject go;
    // Start is called before the first frame update
    void Start()
    {
        go = GameObject.Find("Wizard");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 offset = go.transform.position * -1;
        transform.position = offset * 0.05f;
    }
}
