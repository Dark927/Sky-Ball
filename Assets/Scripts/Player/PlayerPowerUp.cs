using System.Collections;
using UnityEngine;
using System;

public class PlayerPowerUp : MonoBehaviour
{
    // -----------------------------------------------------------------------
    // Fields
    // -----------------------------------------------------------------------

    #region Fields

    // Powers settings 

    [Header("Rockets power Settings")]
    [Space]

    [SerializeField] private RocketLauncher _rocketLauncher;

    [Header("Explosion power Settings")]
    [Space]

    [SerializeField] private ExplosionsSource _explosionsSource;
    private float _jumpForceMultiplier = 50f;
    private float _checkGroundTimeDelay = 1f;
    private Vector3 _explosionPositionOffset = new Vector3(0f, 0.1f, 0f);

    [Header("Lightnings power Settings")]
    [Space]

    [SerializeField] private LightningsSource _lightningsSource;
    [SerializeField] private int _minStrikesCount = 1;
    [SerializeField] private int _maxStrikesCount = 4;

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
    private Collider _collider;

    #endregion

    // -----------------------------------------------------------------------
    // Private Methods
    // -----------------------------------------------------------------------

    #region Private Methods

    private void Awake()
    {
        _player = GetComponent<PlayerController>();
        _collider = GetComponent<Collider>();
        _indicatorMaterial = _powerUpIndicator.GetComponent<MeshRenderer>().material;

        if (_rocketLauncher == null)
        {
            Debug.Log("# Warning : Rockets can not be launched. RocketLauncher == null. - " + gameObject.name);
        }
    }

    private void Update()
    {
        ConfigureIndicator();
        UpdateRocketLauncherPosition();
    }

    private void UpdateRocketLauncherPosition()
    {
        _rocketLauncher.transform.position = new Vector3(transform.position.x, 0f, transform.position.z);
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
                    StartCoroutine(JumpAndExplodeRoutine());
                }
                break;

            case PowerUp.TYPE.LightningStrike:
                {
                    ActivateLightnings();
                }
                break;
        }
    }

    private void ActivateRocketLauncher()
    {
        if (_rocketLauncher != null)
        {
            _rocketLauncher.StartRocketAttack<EnemyMovement>();
        }
    }

    private void ActivateLightnings()
    {
        if (_lightningsSource != null)
        {
            try
            {
                _lightningsSource.ActivateLightnings<EnemyBody>(_powerStrength, _minStrikesCount, _maxStrikesCount);
            }
            catch (Exception exception)
            {
                Debug.Log($"{gameObject.name} obj. -> {exception.Message}");
            }
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
        EnemyBody enemy = collision.gameObject.GetComponent<EnemyBody>();

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

    private IEnumerator JumpAndExplodeRoutine()
    {
        Rigidbody playerRb = _player.GetComponent<Rigidbody>();
        playerRb.AddForce(Vector3.up * _powerStrength * _jumpForceMultiplier * Time.deltaTime, ForceMode.Impulse);

        yield return new WaitForSeconds(_checkGroundTimeDelay);
        while (!_player.OnGround) yield return null;

        if (_explosionsSource != null)
        {
            _explosionsSource.ActivateExplosion(_player.GroundContactPoint + _explosionPositionOffset, _powerStrength);
        }
        else
        {
            Debug.Log($"# Error : {nameof(_explosionsSource)} == null. Explosion can not be executed. - {gameObject.name}");
        }
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
