using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class EnemyPool : MonoBehaviour
{
    // -----------------------------------------------------------------------
    // Fields
    // -----------------------------------------------------------------------

    #region Fields

    [SerializeField] private List<PoolObjectData> _enemyDataList;

    private List<List<EnemyHead>> _pool;

    #endregion

    // -----------------------------------------------------------------------
    // Properties
    // -----------------------------------------------------------------------

    #region Properties

    public int ActiveEnemiesCount
    {
        get
        {
            int count = 0;

            foreach (List<EnemyHead> enemyList in _pool)
            {
                foreach (EnemyHead enemy in enemyList)
                {
                    if (enemy.gameObject.activeInHierarchy)
                    {
                        count++;
                    }
                }
            }

            return count;
        }
    }

    #endregion


    // -----------------------------------------------------------------------
    // Public Methods
    // -----------------------------------------------------------------------

    #region Public Methods

    public EnemyHead RequestInactiveEnemy(EnemyHead.TYPE type)
    {
        foreach (List<EnemyHead> enemyList in _pool)
        {
            EnemyHead firstEnemy = enemyList.ElementAt(0);

            if (firstEnemy.Type == type)
            {
                return FindInactiveEnemyInList(enemyList);
            }
        }

        return null;
    }

    public List<EnemyHead.TYPE> RequestAvailableEnemyTypes()
    {
        List<EnemyHead.TYPE> availableEnemyTypes = new();

        foreach (List<EnemyHead> enemyList in _pool)
        {
            availableEnemyTypes.Add(enemyList.ElementAt(0).Type);
        }

        return availableEnemyTypes;
    }

    public void AddEventListenerToAll(UnityAction action)
    {
        foreach (List<EnemyHead> enemyList in _pool)
        {
            foreach (EnemyHead enemy in enemyList)
            {
                enemy.OnDeathEvent.AddListener(action);
            }
        }
    }

    #endregion


    // -----------------------------------------------------------------------
    // Private Methods
    // -----------------------------------------------------------------------

    #region Private Methods

    private void Awake()
    {
        _pool = new();
        InitPool();
    }

    private void InitPool()
    {
        foreach (PoolObjectData enemyObjectData in _enemyDataList)
        {
            GameObject enemyContainer = new(enemyObjectData.Title);
            enemyContainer.transform.parent = transform;

            for (int currentIndex = 0; currentIndex < enemyObjectData.CountToCreate; ++currentIndex)
            {
                EnemyHead enemy = enemyObjectData.Prefab.GetComponent<EnemyHead>();

                if (enemy != null)
                {
                    AddNewEnemy(enemy, enemyContainer.transform);
                }
                else
                {
                    string errorMsg = $"{nameof(enemyObjectData)}.Prefab does not contain {typeof(EnemyHead)} component!)";
                    ErrorsManager.instance.SendErrorMsg(errorMsg);
                }
            }
        }
    }

    private void AddNewEnemy(EnemyHead enemy, Transform parent = null)
    {
        if (parent == null)
        {
            parent = transform;
        }

        EnemyHead newEnemy = Instantiate(enemy, transform.position, Quaternion.identity, parent);
        newEnemy.name = enemy.name;
        newEnemy.gameObject.SetActive(false);

        PushToPool(newEnemy);
    }

    private void PushToPool(EnemyHead enemyToPush)
    {
        bool pushed = false;

        // Check if a list with the same type of enemy exists

        for (int currentListIndex = 0; currentListIndex < _pool.Count; ++currentListIndex)
        {
            EnemyHead.TYPE currentListType = _pool[currentListIndex].ElementAt(0).Type;

            if (currentListType == enemyToPush.Type)
            {
                _pool[currentListIndex].Add(enemyToPush);
                pushed = true;
                break;
            }
        }

        // Create a new list if it does not already exist

        if (!pushed)
        {
            List<EnemyHead> newList = new() { enemyToPush };
            _pool.Add(newList);
        }
    }

    private EnemyHead FindInactiveEnemyInList(List<EnemyHead> enemyList)
    {
        foreach (EnemyHead enemy in enemyList)
        {
            if (!enemy.gameObject.activeInHierarchy)
            {
                return enemy;
            }
        }
        return null;
    }

    #endregion

}