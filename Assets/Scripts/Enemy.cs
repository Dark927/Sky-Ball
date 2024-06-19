using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Main Settings")]

    [SerializeField] float basicSpeed = 3f;
    Rigidbody rb;

    Vector3 direction = Vector3.zero;

    [Space]
    [Header("Destroy bounds Settings")]

    [SerializeField] float defaultBound = -10f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if(transform.position.y < defaultBound)
        {
            // TODO : Add score for player 
            Destroy(gameObject);
        }

        Transform playerTransform = FindObjectOfType<PlayerController>().transform;
        direction = (playerTransform.position - transform.position).normalized;
    }

    private void FixedUpdate()
    {
        rb.AddForce(direction * basicSpeed);
    }
}
