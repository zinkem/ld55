using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOutTitle : MonoBehaviour
{
    public Vector2 startForce;
    public int lifetime = 200;
    int counter = 0;
    // Start is called before the first frame update
    void Start()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if(rb != null ) rb.AddForce(startForce);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        counter++;

        if(counter > lifetime) {
            gameObject.SetActive(false);
        }
    }
}
