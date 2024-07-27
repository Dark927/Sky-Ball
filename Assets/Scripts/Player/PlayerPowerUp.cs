using System.Collections;
using UnityEngine;

public class PlayerPowerUp : MonoBehaviour
{
    // -----------------------------------------------------------------------
    // Fields
    // -----------------------------------------------------------------------

    #region Fields

    // Powers settings 

    [Header("Rockets power Settings")]

    [SerializeField] private GameObject _rocketPrefab;


    [Header("Explosion power Settings")]

    [SerializeField] private GameObject _explosionPrefab;


    private bool _hasPowerUp = false;

    private PowerUp _power;
    private PowerUp.TYPE _powerType;
    private float _powerStrength = 0f;
    private float _powerActiveTime = 0f;
    private float _powerReloadTime = 1f;


    // Indicator settings

    [Header("Indicator Settings")]

    [SerializeField] private GameObject _powerUpIndicator;
    private Material _indicatorMaterial;
    private Vector3 _indicatorOffset = new Vector3(0, -0.55f, 0);
    private Vector3 _indicatorRotation = new Vector3(0, 90f, 0);

    private PlayerController _player;
    private RocketLauncher _rocketLauncher;

    #endregion

    // -----------------------------------------------------------------------
    // Private Methods
    // -----------------------------------------------------------------------

    #region Private Methods

    private void Awake()
    {
        _player = GetComponent<PlayerController>();
        _indicatorMaterial = _powerUpIndicator.GetComponent<MeshRenderer>().material;
        _rocketLauncher = GetComponentInChildren<RocketLauncher>();

        if (_rocketLauncher == null)
        {
            Debug.Log("# Warning : Rockets can not be launched. RocketLauncher == null. - " + gameObject.name);
        }
    }

    private void Update()
    {
        ConfigureIndicator();
    }

    private void ConfigureIndicator()
    {
        _powerUpIndicator.transform.position = _player.transform.position + _indicatorOffset;
        _powerUpIndicator.transform.Rotate(_indicatorRotation * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        _power = other.GetComponent<PowerUp>();

        if (_power != null)
        {
            // Remove current power up if it exists

            StopAllCoroutines();

            // Save new power up Fields 

            ConfigurePowerFields(_power);
            Destroy(_power.gameObject);

            // Power up start Fields

            _hasPowerUp = true;
            _powerUpIndicator.SetActive(true);
            StartCoroutine(PowerupCountdownRoutine(_powerActiveTime));

            // Activate different power types 

            ActivatePowerUp();
        }
    }

    private void ActivatePowerUp()
    {
        switch (_powerType)
        {
            case PowerUp.TYPE.Rockets:
                {
                    ActivateRocketLauncher();
                }
                break;

            case PowerUp.TYPE.Explosion:
                {
                    StartCoroutine(PushExplodeRoutine());
                }
                break;
        }
    }

    private void ActivateRocketLauncher()
    {
        if (_rocketLauncher != null)
        {
            _rocketLauncher.StartRocketAttack<Enemy>();
        }
    }

    private void ConfigurePowerFields(PowerUp power)
    {
        _indicatorMaterial.color = power.GetIndicatorColor();

        _powerStrength = power.GetPowerUpStrength();
        _powerType = power.GetPowerType();
        _powerActiveTime = power.GetPowerActiveTime();
        _powerReloadTime = power.GetPowerReloadTime();
    }


    private void OnCollisionEnter(Collision collision)
    {
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();

        if ((enemy != null) && _hasPowerUp)
        {
            Rigidbody enemyRb = enemy.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = enemyRb.gameObject.transform.position - transform.position;


            switch (_powerType)
            {
                case PowerUp.TYPE.Strength:
                    {
                        // Push enemy away from player
                        float massModifier = (enemyRb.mass < 1f) ? 1f : enemyRb.mass;

                        enemyRb.AddForce(awayFromPlayer * _powerStrength * massModifier, ForceMode.Impulse);
                    }
                    break;
            }
        }
    }

    private IEnumerator PushExplodeRoutine()
    {
        Rigidbody playerRb = _player.GetComponent<Rigidbody>();
        playerRb.AddForce(Vector3.up * _powerStrength * 50 * Time.deltaTime, ForceMode.Impulse);

        yield return new WaitForSeconds(1);

        do
        {
            yield return null;
        } while (!_player.OnGround());

        Vector3 cameraViewOffset = new Vector3(0, 0, -1.5f);
        GameObject explosionObj = Instantiate(_explosionPrefab, transform.position + cameraViewOffset, Quaternion.identity, transform.parent);
        explosionObj.GetComponent<Explosion>().Explode(_powerStrength);
    }

    private IEnumerator PowerupCountdownRoutine(float powerupActiveTime)
    {
        yield return new WaitForSeconds(powerupActiveTime);

        DisablePowerUp();
    }

    private void DisablePowerUp()
    {
        _powerUpIndicator.SetActive(false);
        _hasPowerUp = false;
        StopAllCoroutines();

        _rocketLauncher.StopRocketAttack();
    }

    #endregion


    // -----------------------------------------------------------------------
    // Public Methods
    // -----------------------------------------------------------------------

    #region Public Methods 



    #endregion
}
