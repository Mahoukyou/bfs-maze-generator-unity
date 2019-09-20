using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class Player : MonoBehaviour
{
    public float movementSpeed = 5.0f;

    PlayerController controller = null;

    void Start()
    {
        controller = GetComponent<PlayerController>();
    }

    void Update()
    {
        Vector3 movementInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        Vector3 velocity = movementInput.normalized * movementSpeed;

        controller.Move(velocity);
    }
}
