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
    protected EnemyBody _body;
    protected EnemyPowerUp _powerUps;

    #endregion


    // -----------------------------------------------------------------------
    // Public Methods
    // -----------------------------------------------------------------------

    #region Public Methods

    public virtual void TryDeactivate()
    {
        if (!_body.gameObject.activeInHierarchy)
        {
            Deactivate();
        }
    }

    public void TryActivatePowers()
    {
        if (_powerUps != null)
        {
            _powerUps.TryActivateRocketLauncher();
        }
    }

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
    // Protected Methods
    // -----------------------------------------------------------------------

    #region Private Methods

    protected virtual void ResetSettings()
    {

    }

    protected virtual void InitialSettings()
    {
        Transform mainBodyTransform = transform.GetChild(0);
        _body = mainBodyTransform.GetComponent<EnemyBody>();
        _powerUps = GetComponent<EnemyPowerUp>();
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

    private bool IsCorrectType() => (_type != TYPE.NumberOfEnemy);


    protected virtual void Deactivate()
    {
        gameObject.SetActive(false);
        _body.gameObject.SetActive(true);

        OnDeathEvent?.Invoke();
    }

    #endregion
}
