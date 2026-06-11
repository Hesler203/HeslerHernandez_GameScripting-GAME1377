using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatePropeller : MonoBehaviour
{
    private const float SPEED = 1000f; // constant speed of rotation

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Rotate(Vector3.forward * SPEED * Time.deltaTime); // rotates the propeller along the x-axis at constant speed
    }
}
