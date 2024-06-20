using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PowerUpType
{
    Power_strengthUP = 0,
    Power_massUP = 1,

    Power_firstIndex = 0,
    Power_lastIndex = Power_massUP,
    Power_numberOfPowers = Power_lastIndex + 1,
}

public class PowerUp : MonoBehaviour
{
    // -----------------------------------------------------------------------
    // Parameters
    // -----------------------------------------------------------------------

    #region Parameters

    [SerializeField] PowerUpType powerType = PowerUpType.Power_firstIndex;
    [SerializeField] float powerUpActiveTime = 5f;
    [SerializeField] float powerUpStrength = 5f;

    #endregion


    // -----------------------------------------------------------------------
    // Public Methods
    // -----------------------------------------------------------------------

    #region Public Methods

    public PowerUpType GetPowerType()
    {
        if (powerType != PowerUpType.Power_numberOfPowers)
        {
            return powerType;
        }
        else
        {
            Debug.Log($"# Warning -> {gameObject.name} - PowerUpType == Power_numberOfPowers. Return LAST power up.");
            return PowerUpType.Power_lastIndex;
        }
    }

    public float GetPowerActiveTime()
    {
        return powerUpActiveTime;
    }

    public float GetPowerUpStrength()
    {
        return powerUpStrength;
    }

    #endregion
}
