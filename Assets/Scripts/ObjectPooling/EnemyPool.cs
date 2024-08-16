using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyPool : MultipleObjectsPool
{
    // -----------------------------------------------------------------------
    // Public Methods
    // -----------------------------------------------------------------------

    #region Public Methods

    public void AddEventListenerToAll(UnityAction action)
    {
        foreach (List<GameObject> objectsList in _pool)
        {
            foreach (GameObject obj in objectsList)
            {
                EnemyHead enemy = obj.GetComponent<EnemyHead>();
                enemy.OnDeathEvent.AddListener(action);
            }
        }
    }

    public EnemyHead RequestInactiveEnemy(EnemyHead.TYPE type)
    {
        foreach (List<GameObject> objectsList in _pool)
        {
            EnemyHead firstEnemy = objectsList[0].GetComponent<EnemyHead>();

            if (firstEnemy.Type == type)
            {
                GameObject inactiveEnemyObj = FindInactiveObjectInList(objectsList);

                if (inactiveEnemyObj != null)
                {
                    return inactiveEnemyObj.GetComponent<EnemyHead>();
                }
                break;
            }
        }

        return null;
    }

    public List<EnemyHead.TYPE> RequestAvailableEnemyTypes()
    {
        List<EnemyHead.TYPE> availableEnemyTypes = new();

        foreach (List<GameObject> objectsList in _pool)
        {
            EnemyHead firstEnemy = objectsList[0].GetComponent<EnemyHead>();
            availableEnemyTypes.Add(firstEnemy.Type);
        }

        return availableEnemyTypes;
    }

    #endregion


    // -----------------------------------------------------------------------
    // Protected Methods
    // -----------------------------------------------------------------------

    #region Protected Methods

    protected override bool FitsPushCondition(List<GameObject> sourceList, GameObject objToPush)
    {
        EnemyHead firstListEnemy = sourceList[0].GetComponent<EnemyHead>();
        EnemyHead enemyToPush = objToPush.GetComponent<EnemyHead>();

        if (firstListEnemy.Type == enemyToPush.Type)
        {
            return true;
        }

        return false;
    }

    #endregion

}
