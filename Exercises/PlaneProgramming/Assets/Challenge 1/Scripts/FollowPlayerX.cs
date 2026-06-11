using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayerX : MonoBehaviour
{
    public GameObject plane; // reference to the player
    private Vector3 offset = new Vector3(30f, 0f, 12f); // camera offset distance for proper viewing

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = plane.transform.position + offset; // allows the camera to follow the plane from a fixed distance
    }
}
