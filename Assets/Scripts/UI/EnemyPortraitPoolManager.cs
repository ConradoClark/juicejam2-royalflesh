using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Unity.Pooling;
using UnityEngine;

public class EnemyPortraitPoolManager : CustomPrefabManager<EnemyPortraitPool, EnemyPortrait>
{
    public Vector3 ForcedLocalScale;
    private void OnEnable()
    {
        GetComponentInChildren<EnemyPortraitPool>().transform.localScale = ForcedLocalScale;
    }
    
}
