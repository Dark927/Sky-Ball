using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    // -----------------------------------------------------------------------
    // Parameters
    // -----------------------------------------------------------------------

    #region Parameters 

    [SerializeField] float basicSpeed = 5f;
    [SerializeField] Transform focalPoint;
    Rigidbody playerRb;

    float forwardInput = 0;
    bool onGround = true;

    #endregion


    // -----------------------------------------------------------------------
    // Private Methods
    // -----------------------------------------------------------------------

    #region Private Methods

    private void Awake()
    {
        playerRb = GetComponent<Rigidbody>();
        playerRb.WakeUp();
        //playerRb.AddForce(Vector3.up * 10, ForceMode.Impulse);
    }

    private void Update()
    {
        if(transform.position.y < -5f)
        {
            RestartGame();
        }

        forwardInput = Input.GetAxis("Vertical");
    }

    private void FixedUpdate()
    {
        playerRb.AddForce(focalPoint.forward * basicSpeed * forwardInput);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            onGround = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            onGround = false;
        }
    }


    private void RestartGame()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    #endregion


    // -----------------------------------------------------------------------
    // Private Methods
    // -----------------------------------------------------------------------

    #region Public Methods

    public bool OnGround()
    {
        return onGround;
    }

    #endregion
}

