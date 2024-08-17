using System.Collections;
using UnityEngine;
using System;

public class PlayerPowerUp : MonoBehaviour
{
    // -----------------------------------------------------------------------
    // Fields
    // -----------------------------------------------------------------------

    #region Fields


    [Header("Indicator Settings")]
    [Space]
    [SerializeField] private PowerUpIndicator _powerUpIndicator;

    [Header("Rockets power Settings")]
    [Space]

    [SerializeField] private RocketLauncher _rocketLauncher;

    [Header("Explosion power Settings")]
    [Space]

    [SerializeField] private ExplosionsSource _explosionsSource;
    private float _jumpForceMultiplier = 50f;
    private float _checkGroundTimeDelay = 1f;
    private Vector3 _explosionPositionOffset = new(0f, 0.1f, 0f);

    [Header("Lightnings power Settings")]
    [Space]

    [SerializeField] private LightningsSource _lightningsSource;
    [SerializeField] private int _minStrikesCount = 1;
    [SerializeField] private int _maxStrikesCount = 4;

    private bool _hasPowerUp = false;
    private PowerUp.Data _powerStats;

    private PlayerMovement _player;

    #endregion

    // -----------------------------------------------------------------------
    // Private Methods
    // -----------------------------------------------------------------------

    #region Private Methods

    private void Awake()
    {
        _player = GetComponent<PlayerMovement>();
        _powerUpIndicator.SetPlayerTransform(transform);

        if (_rocketLauncher == null)
        {
            string warningMsg = $"Rockets can not be launched. {nameof(_rocketLauncher)} is null. - {gameObject.name}";
            ErrorsManager.Instance.SendWarningMsg(warningMsg);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        PowerUp foundPower = other.GetComponent<PowerUp>();

        if (foundPower != null)
        {
            _powerStats = foundPower.Stats;
            foundPower.gameObject.SetActive(false);
            
            DisablePowerUp();

            _hasPowerUp = true;
            _powerUpIndicator.gameObject.SetActive(true);
            _powerUpIndicator.SetColor(_powerStats.IndicatorColor);

            StartCoroutine(PowerupCountdownRoutine(_powerStats.ActiveTime));
            ActivatePowerUp(_powerStats.Type);
        }
    }

    private void ActivatePowerUp(PowerUp.TYPE powerType)
    {
        switch (powerType)
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
            StartCoroutine(UpdateShootPointRoutine());
            _rocketLauncher.StartRocketAttack<EnemyMovement>();
        }
    }

    private IEnumerator UpdateShootPointRoutine()
    {
        while (_hasPowerUp)
        {
            _rocketLauncher.UpdateShootPointPosition(transform.position);
            yield return null;
        }
    }

    private void ActivateLightnings()
    {
        if (_lightningsSource != null)
        {
            try
            {
                _lightningsSource.ActivateLightnings<EnemyBody>(_powerStats.Strength, _minStrikesCount, _maxStrikesCount);
            }
            catch (Exception exception)
            {
                Debug.Log($"{gameObject.name} obj. -> {exception.Message}");
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        EnemyBody enemy = collision.gameObject.GetComponent<EnemyBody>();

        if ((enemy != null) && _hasPowerUp)
        {
            Rigidbody enemyRb = enemy.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = enemyRb.gameObject.transform.position - transform.position;


            switch (_powerStats.Type)
            {
                case PowerUp.TYPE.Strength:
                    {
                        // Push enemy away from player
                        float massModifier = (enemyRb.mass < 1f) ? 1f : enemyRb.mass;

                        enemyRb.AddForce(awayFromPlayer * _powerStats.Strength * massModifier, ForceMode.Impulse);
                    }
                    break;
            }
        }
    }

    private IEnumerator JumpAndExplodeRoutine()
    {
        Rigidbody rb = _player.GetComponent<Rigidbody>();
        rb.AddForce(Vector3.up * (_powerStats.Strength * _jumpForceMultiplier * Time.deltaTime), ForceMode.Impulse);

        yield return new WaitForSeconds(_checkGroundTimeDelay);
        while (!_player.OnGround) yield return null;

        if (_explosionsSource != null)
        {
            _explosionsSource.ActivateExplosion(_player.GroundContactPoint + _explosionPositionOffset, _powerStats.Strength);
        }
        else
        {
            string errorMsg = $"{nameof(_explosionsSource)} == null. Explosion can not be executed. - {gameObject.name}";
            ErrorsManager.Instance.SendErrorMsg(errorMsg);
        }
    }

    private IEnumerator PowerupCountdownRoutine(float powerupActiveTime)
    {
        yield return new WaitForSeconds(powerupActiveTime);

        if (_hasPowerUp)
        {
            DisablePowerUp();
        }
    }

    private void DisablePowerUp()
    {
        _powerUpIndicator.gameObject.SetActive(false);
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
