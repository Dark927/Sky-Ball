using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyGroupHead : EnemyHead
{
    private List<EnemyBody> _childrenBody;

    protected override void InitialSettings()
    {
        base.InitialSettings();
        _childrenBody = new List<EnemyBody>(GetComponentsInChildren<EnemyBody>().Where(child => child != _body));
    }

    protected override void Deactivate()
    {
        gameObject.SetActive(false);

        _body.gameObject.SetActive(true);

        foreach (EnemyBody child in _childrenBody)
        {
            child.ResetSettings();
            child.gameObject.SetActive(true);
        }

        OnDeathEvent?.Invoke();
    }
}
