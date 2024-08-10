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
    

    #endregion


    // -----------------------------------------------------------------------
    // Public Methods
    // -----------------------------------------------------------------------

    #region Public Methods

    public void Deactivate()
    {
        // TODO : Add score for player 

        gameObject.SetActive(false);
        _body.gameObject.SetActive(true);

        OnDeathEvent?.Invoke();
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
    // Private Methods
    // -----------------------------------------------------------------------

    #region Private Methods

    private void OnEnable()
    {
        _body.transform.SetPositionAndRotation(transform.position, transform.rotation);
    }

    private void Awake()
    {
        _body = GetComponentInChildren<EnemyBody>(true);
    }

    private bool IsCorrectType() => (_type != TYPE.NumberOfEnemy);

    #endregion
}
