using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsPool : MonoBehaviour
{
    // -----------------------------------------------------------------------
    // Fields
    // -----------------------------------------------------------------------

    #region Fields

    [SerializeField] private GameObject _objectPrefab;
    [SerializeField] private int _startCount;

    private List<GameObject> _pool;

    #endregion


    // -----------------------------------------------------------------------
    // Private Methods
    // -----------------------------------------------------------------------

    #region Private Methods


    private void Awake()
    {
        _pool = new();
    }

    private void Start()
    {
        InitPool();
    }

    private void InitPool()
    {
        for (int i = 0; i < _startCount; ++i)
        {
            AddNewElement();
        }
    }

    private GameObject AddNewElement()
    {
        GameObject newObject = Instantiate(_objectPrefab, transform.position, Quaternion.identity, transform);
        newObject.SetActive(false);
        _pool.Add(newObject);

        return newObject;
    }

    #endregion


    // -----------------------------------------------------------------------
    // Public Methods
    // -----------------------------------------------------------------------

    #region Public Methods
    public GameObject RequestInactiveObject(bool allowExpansion = false)
    {
        foreach(GameObject element in _pool)
        {
            if(!element.activeInHierarchy)
            {
                return element;
            }
        }


        // Create new element if all elements are active.

        if(allowExpansion)
        {
            return AddNewElement();
        }


        return null;
    }

    #endregion
}
