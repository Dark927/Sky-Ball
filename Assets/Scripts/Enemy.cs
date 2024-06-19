using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float basicSpeed = 3f;
    Rigidbody rigidbody;

    Vector3 direction = Vector3.zero;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Transform playerTransform = FindObjectOfType<PlayerController>().transform;
        direction = (playerTransform.position - transform.position).normalized;
    }

    private void FixedUpdate()
    {
        rigidbody.AddForce(direction * basicSpeed);
    }
}
