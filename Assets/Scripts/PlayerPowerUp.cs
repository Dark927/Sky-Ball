using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPowerUp : MonoBehaviour
{
    // -----------------------------------------------------------------------
    // Parameters
    // -----------------------------------------------------------------------

    #region Parameters

    // Powers settings 

    [Header("Rockets power Settings")]
    [SerializeField] GameObject rocketPrefab;

    [Header("Explosion power Settings")]
    [SerializeField] GameObject explosionPrefab;

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
    Vector3 indicatorRotation = new Vector3(0, 90f, 0);

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
        ConfigureIndicator();
    }

    private void ConfigureIndicator()
    {
        powerUpIndicator.transform.position = player.transform.position + indicatorOffset;
        powerUpIndicator.transform.Rotate(indicatorRotation * Time.deltaTime);
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

            ActivatePowerUp();

        }
    }

    private void ActivatePowerUp()
    {
        switch (powerType)
        {
            case PowerUpType.Power_rockets:
                {
                    StartCoroutine(LaunchRocketsRoutine());
                }
                break;

            case PowerUpType.Power_explosion:
                {
                    StartCoroutine(PushExplodeRoutine());
                }
                break;
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
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();

        if ((enemy != null) && hasPowerUp)
        {
            Rigidbody enemyRb = enemy.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = enemyRb.gameObject.transform.position - transform.position;


            switch (powerType)
            {
                case PowerUpType.Power_strengthUP:
                    {
                        // Push enemy away from player
                        float massModifier = (enemyRb.mass < 1f) ? 1f : enemyRb.mass;

                        enemyRb.AddForce(awayFromPlayer * powerStrength * massModifier, ForceMode.Impulse);
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

    IEnumerator PushExplodeRoutine()
    {
        Rigidbody playerRb = player.GetComponent<Rigidbody>();
        playerRb.AddForce(Vector3.up * powerStrength * 50 * Time.deltaTime, ForceMode.Impulse);

        yield return new WaitForSeconds(1);

        do
        {
            yield return null;
        } while (!player.OnGround());

        Vector3 cameraViewOffset = new Vector3(0, 0, -1.5f);
        GameObject explosionObj = Instantiate(explosionPrefab, transform.position + cameraViewOffset, Quaternion.identity, transform.parent);
        explosionObj.GetComponent<Explosion>().Explode(powerStrength);
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
