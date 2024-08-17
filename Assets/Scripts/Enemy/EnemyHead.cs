using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class EnemyHead : MonoBehaviour
{
    // -----------------------------------------------------------------------
    // Enums
    // -----------------------------------------------------------------------

    #region Enums

    public enum TYPE
    {
        Default = 0,
        Heavy,
        Fast,
        Boss,
        Range,

        Group,

        FirstIndex = 0,
        LastIndex = Group,
        NumberOfEnemy = LastIndex + 1,
    }

    #endregion

    // -----------------------------------------------------------------------
    // Fields 
    // -----------------------------------------------------------------------

    #region Fields

    [Header("Main Settings")]

    [SerializeField] private TYPE _type;

    [HideInInspector] public UnityEvent OnDeathEvent = new();

    protected EnemyBody mainBody;
    protected EnemyPowerUp powerUps;

    private EnemyVFX _enemyVFX;

    #endregion


    // -----------------------------------------------------------------------
    // Properties
    // -----------------------------------------------------------------------

    #region Properties

    public EnemyVFX VFX => _enemyVFX;

    public TYPE Type
    {
        get
        {
            if (!IsCorrectType())
            {
                _type = TYPE.LastIndex;

                string warningMsg = $"{gameObject.name} - {nameof(_type)} == {nameof(TYPE.NumberOfEnemy)}. Return {nameof(TYPE.LastIndex)} type.";
                ErrorsManager.Instance.SendWarningMsg(warningMsg);
            }

            return _type;
        }
    }

    #endregion


    // -----------------------------------------------------------------------
    // Public Methods
    // -----------------------------------------------------------------------

    #region Public Methods

    public virtual void TryDeactivate()
    {
        if (!mainBody.gameObject.activeInHierarchy)
        {
            if (HasActiveChildren())
            {
                StartCoroutine(DeactivateRoutine());
            }
            else
            {
                Deactivate();
            }
        }
    }

    public void TryActivatePowers()
    {
        if (powerUps != null)
        {
            powerUps.TryActivateRocketLauncher();
        }
    }

    #endregion


    // -----------------------------------------------------------------------
    // Protected Methods
    // -----------------------------------------------------------------------

    #region Protected Methods

    protected virtual void ResetSettings()
    {

    }

    protected virtual void InitialSettings()
    {
        Transform mainBodyTransform = transform.GetChild(0);
        mainBody = mainBodyTransform.GetComponent<EnemyBody>();
        powerUps = GetComponent<EnemyPowerUp>();
        _enemyVFX = GetComponent<EnemyVFX>();
    }

    protected virtual void Deactivate()
    {
        gameObject.SetActive(false);
        mainBody.gameObject.SetActive(true);

        OnDeathEvent?.Invoke();
    }

    #endregion


    // -----------------------------------------------------------------------
    // Private Methods
    // -----------------------------------------------------------------------

    #region Private Methods

    private void OnEnable()
    {
        ResetSettings();
    }

    private void Awake()
    {
        InitialSettings();
    }

    private void Start()
    {
        SetOnDeathEventListeners();
    }

    private bool IsCorrectType() => (_type != TYPE.NumberOfEnemy);

    private bool HasActiveChildren()
    {
        for (int currentChild = 0; currentChild < transform.childCount; currentChild++)
        {
            Transform child = transform.GetChild(currentChild);

            if (child.gameObject.activeSelf)
            {
                return true;
            }
        }
        return false;
    }

    private IEnumerator DeactivateRoutine()
    {
        while (true)
        {
            if (!HasActiveChildren())
            {
                break;
            }

            yield return null;
        }

        Deactivate();
    }

    private void SetOnDeathEventListeners()
    {
        OnDeathEvent.AddListener(WavesManager.Instance.DecrementAliveEnemiesCount);
        OnDeathEvent.AddListener(GameManager.Instance.IncrementSessionKills);

        OnDeathEvent.AddListener(WavesManager.Instance.TryStartNextWave);
        OnDeathEvent.AddListener(UIManager.Instance.Gameplay.UpdateAliveEnemiesCount);
    }

    #endregion
}
