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

    [SerializeField] private EnemyDefaultHead _groupLeader;
    [SerializeField] private List<EnemyDefaultHead> _groupMembers;

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
        foreach (EnemyDefaultHead member in _groupMembers)
        {
            if (member != null)
            {
                member.Deactivate();
            }
        }
        Destroy(gameObject);
    }

    #endregion
}
