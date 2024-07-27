using System.Collections.Generic;
using UnityEngine;

public class EnemyGroup : MonoBehaviour
{
    // -----------------------------------------------------------------------
    // Fields
    // -----------------------------------------------------------------------

    #region Fields

    [Header("Group Settings")]
    [Space]

    [SerializeField] private Enemy _groupLeader;
    [SerializeField] private List<Enemy> _groupMembers;

    private bool _hasLeader = false;

    #endregion


    // -----------------------------------------------------------------------
    // Private Methods
    // -----------------------------------------------------------------------

    #region Private Methods

    private void Start()
    {
        if(_groupLeader != null)
        {
            _hasLeader = true;
        }
    }

    private void Update()
    {
        if(_hasLeader && (_groupLeader == null))
        {
            DestroyGroup();
        }
    }

    private void DestroyGroup()
    {
        foreach (Enemy member in _groupMembers)
        {
            if (member != null)
            {
                member.DestroyEnemy();
            }
        }
        Destroy(gameObject);
    }

    #endregion
}
