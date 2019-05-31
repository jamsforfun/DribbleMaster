using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class ExtPhysics
{
    /// <summary>
    /// Get the center of a sphereCast calculation.
    /// </summary>
    /// <param name="origin">origin of the spherreCast</param>
    /// <param name="directionCast">direction of the sphere cast</param>
    /// <param name="hitInfoDistance">hitInfo.distance of the hitInfo</param>
    /// <returns>center position of the hit info</returns>
    public static Vector3 GetCollisionCenterSphereCast(Vector3 origin, Vector3 directionCast, float hitInfoDistance)
    {
        return origin + (directionCast.normalized * hitInfoDistance);
    }

    /// <summary>
    /// from a given SphereCast normal, Get the good normal from the sphereCast
    /// </summary>
    /// <param name="hitInfo">Sphere cast info</param>
    /// <param name="castOrigin">origin of the ray used by the sphereCast</param>
    /// <param name="normalizedDirection">normalized direction used by the sphereCast</param>
    /// <param name="sphereRadius">radius used by the sphereCast</param>
    /// <returns>normals hit</returns>
    public static Vector3 GetTheRightNormalSphereCast(RaycastHit hitInfo, Vector3 castOrigin, Vector3 normalizedDirection, float sphereRadius)
    {
        Vector3 collisionCenter = castOrigin + (normalizedDirection * hitInfo.distance);
        Vector3 normals = (collisionCenter - hitInfo.point) / sphereRadius;
        return (normals);
    }

    /// <summary>
    /// from a given sphereCastHit, if we want to have the normal of the mesh hit, we have to do another raycast
    /// </summary>
    /// <param name="castOrigin"></param>
    /// <param name="direction"></param>
    /// <param name="magnitude"></param>
    /// <param name="radius"></param>
    /// <param name="hitPoint"></param>
    /// <param name="rayCastMargin"></param>
    /// <param name="layerMask"></param>
    /// <returns></returns>
    public static Vector3 GetSurfaceNormal(Vector3 castOrigin, Vector3 direction,
        float magnitude, float radius, Vector3 hitPoint,
        float rayCastMargin, int layerMask)
    {
        Vector3 centerCollision = GetCollisionCenterSphereCast(castOrigin, direction, magnitude);
        Vector3 dirCenterToHit = hitPoint - castOrigin;
        float sizeRay = dirCenterToHit.magnitude;
        Vector3 surfaceNormal = CalculateRealNormal(centerCollision, dirCenterToHit, sizeRay, rayCastMargin, layerMask);
        return (surfaceNormal);
    }
    private static Vector3 CalculateRealNormal(Vector3 origin, Vector3 direction, float magnitude, float rayCastMargin, int layermask)
    {
        RaycastHit hit;
        if (Physics.Raycast(origin, direction, out hit, magnitude + rayCastMargin, layermask))
        {
            return (hit.normal);
        }
        return (Vector3.zero);
    }
}

