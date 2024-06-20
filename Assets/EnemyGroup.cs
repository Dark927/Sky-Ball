using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGroup : MonoBehaviour
{
    // -----------------------------------------------------------------------
    // Parameters
    // -----------------------------------------------------------------------

    #region Parameters

    [Header("Group Settings")]
    [Space]

    [SerializeField] Enemy groupLeader;
    [SerializeField] List<Enemy> groupMembers;

    bool hasLeader = false;

    #endregion


    // -----------------------------------------------------------------------
    // Private Methods
    // -----------------------------------------------------------------------

    #region Private Methods

    // Start is called before the first frame update
    private void Start()
    {
        if(groupLeader != null)
        {
            hasLeader = true;
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if(hasLeader && (groupLeader == null))
        {
            DestroyGroup();
        }
    }

    private void DestroyGroup()
    {
        foreach (Enemy member in groupMembers)
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
