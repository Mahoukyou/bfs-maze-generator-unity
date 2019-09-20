using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Camera))]
public class CameraFollower : MonoBehaviour
{
    Camera camera = null;
    Player player = null;

    public float armLength = 10.0f;

    void Start()
    {
        camera = GetComponent<Camera>();
        player = FindObjectOfType<Player>();

        if (!player)
        {
            throw new System.Exception("Could not find player character");
        }
    }

    void Update()
    {
        Vector3 newPosition = player.transform.position;
        newPosition.y = armLength;

        camera.transform.position = newPosition;
    }
}
