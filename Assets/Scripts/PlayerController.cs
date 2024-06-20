using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController: MonoBehaviour
{
    // -----------------------------------------------------------------------
    // Parameters
    // -----------------------------------------------------------------------

    #region Parameters 

    [SerializeField] float basicSpeed = 5f;
    [SerializeField] Transform focalPoint;
    Rigidbody playerRb;

    float forwardInput = 0;

    #endregion


    // -----------------------------------------------------------------------
    // Parameters
    // -----------------------------------------------------------------------

    #region Private Methods

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

    #endregion
}

