using UnityEngine;



public class PowerUp : MonoBehaviour
{
    // -----------------------------------------------------------------------
    // Enums
    // -----------------------------------------------------------------------

    #region Enums

    public enum TYPE
    {
        Strength = 0,
        Rockets = 1,
        Explosion,
        LightningStrike,
        MassUP,

        FirstIndex = 0,
        LastIndex = MassUP,
        NumberOfPowers = LastIndex + 1,
    }

    #endregion


    // -----------------------------------------------------------------------
    // Structs
    // -----------------------------------------------------------------------

    #region Structs

    [System.Serializable]   
    public struct Data
    {
        [SerializeField] private TYPE _type;

        [SerializeField] private float _activeTime;
        [SerializeField] private float _strength;
        [SerializeField] private float _reloadTime;

        [SerializeField] private Color _indicatorColor;


        public TYPE Type => _type;
        public float ActiveTime => _activeTime;
        public float Strength => _strength;
        public float ReloadTime => _reloadTime;
        public Color IndicatorColor => _indicatorColor;
    }

    #endregion


    // -----------------------------------------------------------------------
    // Fields
    // -----------------------------------------------------------------------

    #region Fields

    [SerializeField] private Data _stats;

    #endregion


    // -----------------------------------------------------------------------
    // Properties
    // -----------------------------------------------------------------------

    #region Properties

    public Data Stats => _stats;

    #endregion
}
