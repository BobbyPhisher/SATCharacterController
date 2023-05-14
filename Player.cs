using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller2D))]
public class Player : MonoBehaviour
{
    Controller2D controller;

    GameObject floor;
    PolygonCollider2D floorCollider;

    PolygonCollider2D playerCollider;
    public Vector3 velocity;
    float moveSpeed = 2;
    public float jumpVelocity = 6;
    public float gravity;

    void Start()
    {
        controller = GetComponent<Controller2D>();
        floor = GameObject.FindGameObjectWithTag("Floor");

        floorCollider = floor.GetComponent<PolygonCollider2D>();
        playerCollider = GetComponent<PolygonCollider2D>();
    }

    void Update()
    {
        if (controller.collisions.above || controller.collisions.below)
        {
            velocity.y = 0;
        }

        Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0);

        if (Input.GetKeyDown(KeyCode.W) && controller.collisions.below)
        {
            velocity.y = jumpVelocity;
        }

        velocity.x = input.x * moveSpeed;
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime, playerCollider, floorCollider);

    }
}
