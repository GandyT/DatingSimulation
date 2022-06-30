using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanMovement : MonoBehaviour
{
    private float speed = 15f;
    private Vector3 movement;
    private float lastDirectionChange;
    private float directionCooldown = 5f;

    private void Start()
    {
        movement = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
        lastDirectionChange = Time.time; 
    }

    private void Update()
    {
        if (GameManager.instance.paused) return;

        if (Time.time - lastDirectionChange > directionCooldown) {
            
            lastDirectionChange = Time.time;

            if (Random.Range(0f, 1f) < 0.5f) {
                movement = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
            }
        }

        transform.Translate(movement * speed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Wall")
        {
            movement *= -1;
            lastDirectionChange = Time.time;
        }
    }
}
