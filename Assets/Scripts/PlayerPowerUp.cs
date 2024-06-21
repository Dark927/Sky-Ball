using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPowerUp : MonoBehaviour
{
    // -----------------------------------------------------------------------
    // Parameters
    // -----------------------------------------------------------------------

    #region Parameters

    // Power settings 

    [Header("Power Settings")]
    [SerializeField] GameObject rocketPrefab;

    bool hasPowerUp = false;
    
    PowerUp power = null;
    PowerUpType powerType;
    float powerStrength = 0f;
    float powerActiveTime = 0f;
    float powerReloadTime = 1f;

    // Indicator settings

    [Header("Indicator Settings")]

    [SerializeField] GameObject powerUpIndicator;
    Material indicatorMaterial;
    Vector3 indicatorOffset = new Vector3(0, -0.55f, 0);

    PlayerController player;


    #endregion

    // -----------------------------------------------------------------------
    // Private Methods
    // -----------------------------------------------------------------------

    #region Private Methods

    private void Awake()
    {
        player = GetComponent<PlayerController>();
        indicatorMaterial = powerUpIndicator.GetComponent<MeshRenderer>().material;
    }

    private void Update()
    {
        powerUpIndicator.transform.position = player.transform.position + indicatorOffset;
    }

    private void OnTriggerEnter(Collider other)
    {
        power = other.GetComponent<PowerUp>();

        if (power != null)
        {
            // Remove current power up if it exists

            StopAllCoroutines();

            // Save new power up parameters 

            ConfigurePowerParameters(power);
            Destroy(power.gameObject);

            // Power up start parameters

            hasPowerUp = true;
            powerUpIndicator.SetActive(true);
            StartCoroutine(PowerupCountdownRoutine(powerActiveTime));

            // Activate different power types 

            if (powerType == PowerUpType.Power_rockets)
            {
                StartCoroutine(LaunchRocketsRoutine());
            }
        }
    }


    private void ConfigurePowerParameters(PowerUp power)
    {
        indicatorMaterial.color = power.GetIndicatorColor();

        powerStrength = power.GetPowerUpStrength();
        powerType = power.GetPowerType();
        powerActiveTime = power.GetPowerActiveTime();
        powerReloadTime = power.GetPowerReloadTime();
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && hasPowerUp)
        {
            Rigidbody enemyRb = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = enemyRb.gameObject.transform.position - transform.position;


            switch (powerType)
            {
                case PowerUpType.Power_strengthUP:
                    {
                        // Push enemy away from player
                        enemyRb.AddForce(awayFromPlayer * powerStrength, ForceMode.Impulse);
                    }
                    break;
            }
        }
    }


    IEnumerator LaunchRocketsRoutine()
    {
        while (true)
        {
            Enemy[] activeEnemyList = FindObjectsOfType<Enemy>();

            foreach (Enemy enemy in activeEnemyList)
            {
                LaunchRocket(transform, enemy.transform);
            }

            yield return new WaitForSeconds(powerReloadTime);
        }
    }


    IEnumerator PowerupCountdownRoutine(float powerupActiveTime)
    {
        yield return new WaitForSeconds(powerupActiveTime);

        DisablePowerUp();
    }

    private void DisablePowerUp()
    {
        powerUpIndicator.SetActive(false);
        hasPowerUp = false;
        StopAllCoroutines();
    }

    #endregion


    // -----------------------------------------------------------------------
    // Public Methods
    // -----------------------------------------------------------------------

    #region Public Methods 

    public void LaunchRocket(Transform source, Transform target)
    {
        Vector3 lookDirection = (target.position - source.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(lookDirection);

        GameObject rocket = Instantiate(rocketPrefab, source.position + Vector3.up, lookRotation);
        rocket.GetComponent<RocketLogic>().SetTarget(target);
    }

    #endregion
}
