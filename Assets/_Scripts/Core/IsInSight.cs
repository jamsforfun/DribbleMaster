using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsInSight : MonoBehaviour
{
    [FoldoutGroup("GamePlay", Order = 0, Expanded = true), Tooltip("head sight")]
    public float radius = 3f;
    [FoldoutGroup("GamePlay"), Tooltip("head sight")]
    public float offsetPlayerDist = 0.1f;
    [FoldoutGroup("GamePlay"), Tooltip("head sight")]
    public Transform headSight;
    [FoldoutGroup("GamePlay"), Tooltip("where to do  raycast")]
    public Transform target;

    
    [Button]
    public bool IsTargetInSight()
    {
        RaycastHit hitInfo;
        Vector3 dir = target.position - headSight.position;

        int layerMask = Physics.AllLayers;
        layerMask = ~LayerMask.GetMask("Enemy");

        if (Physics.SphereCast(headSight.position, radius, dir, out hitInfo,
                               dir.magnitude + offsetPlayerDist, layerMask, QueryTriggerInteraction.Ignore))
        {
            Debug.Log(hitInfo.collider.gameObject.name);

            ExtDrawGuizmos.DebugWireSphere(hitInfo.point, Color.blue, radius, 0.1f);
            Debug.DrawLine(headSight.position, hitInfo.point, Color.blue, 0.1f);
            return (true);
        }
        return (false);
    }
    /*
    private void Update()
    {
        IsTargetInSight();
    }
    */
}
