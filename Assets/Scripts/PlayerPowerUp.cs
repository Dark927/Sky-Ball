using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerPowerUp : MonoBehaviour
{
    [Header("Indicator Settings")]
    [SerializeField] GameObject powerUpIndicator;
    [SerializeField] Color indicatorColor;
    Material indicatorMaterial;
    Vector3 indicatorOffset = new Vector3(0, -0.55f, 0);

    PlayerController player;

    bool hasPowerUp = false;

    PowerUp power = null;
    PowerUpType powerType;
    float powerStrength = 0f;
    float powerActiveTime = 0f;

    private void Awake()
    {
        player = GetComponent<PlayerController>();
        indicatorMaterial = powerUpIndicator.GetComponent<MeshRenderer>().material;
    }

    private void Start()
    {
        indicatorMaterial.color = indicatorColor;
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

            powerStrength = power.GetPowerUpStrength();
            powerType = power.GetPowerType();
            powerActiveTime = power.GetPowerActiveTime();

            // Destroy power up object from scene 

            Destroy(power.gameObject);
            power = null;


            hasPowerUp = true;
            powerUpIndicator.SetActive(true);
            StartCoroutine(PowerupCountdownRoutine(powerActiveTime));
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && hasPowerUp)
        {
            Rigidbody enemyRb = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = enemyRb.gameObject.transform.position - transform.position;

            if (powerType == PowerUpType.Power_strengthUP)
            {
                // Push enemy away from player
                enemyRb.AddForce(awayFromPlayer * powerStrength, ForceMode.Impulse);
            }
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
    }
}
