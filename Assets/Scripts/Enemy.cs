using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Main Settings")]

    [SerializeField] float basicSpeed = 3f;
    Rigidbody rigidbody = null;

    Vector3 direction = Vector3.zero;

    [Space]
    [Header("Destroy bounds Settings")]

    [SerializeField] float defaultBound = -10f;
    [SerializeField] float errorBound = -30f;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if(transform.position.y < defaultBound)
        {
            // Check if object reach error bound ( on spawn )

            if(transform.position.y < errorBound)
            {
                // Just destroy gameObject without giving any score etc.

                Destroy(gameObject);
                return;
            }

            // TODO : Add score for player 
            Destroy(gameObject);
        }

        Transform playerTransform = FindObjectOfType<PlayerController>().transform;
        direction = (playerTransform.position - transform.position).normalized;
    }

    private void FixedUpdate()
    {
        rigidbody.AddForce(direction * basicSpeed);
    }
}
