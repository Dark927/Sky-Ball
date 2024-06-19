using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController: MonoBehaviour
{
    [SerializeField] float basicSpeed = 5f;
    [SerializeField] Transform focalPoint;
    Rigidbody playerRb;

    bool hasPowerUp = false;

    float forwardInput = 0;

    private void Awake()
    {
        playerRb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        forwardInput = Input.GetAxis("Vertical");
    }

    private void FixedUpdate()
    {
        playerRb.AddForce(focalPoint.forward * basicSpeed * forwardInput);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Powerup"))
        {
            Destroy(other.gameObject);
            hasPowerUp = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Enemy") && hasPowerUp)
        {
            Debug.Log("test");
        }
    }
}

