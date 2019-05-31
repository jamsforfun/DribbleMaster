using UnityEngine;

public struct ExtTriangle
{
    public Vector3 A => EdgeAb.A;
    public Vector3 B => EdgeBc.A;
    public Vector3 C => EdgeCa.A;

    public readonly ExtLine EdgeAb;
    public readonly ExtLine EdgeBc;
    public readonly ExtLine EdgeCa;

    public bool unidirectionnal;
    public bool inverseDirection;
    public bool infinitePlane;
    public bool noGravityBorders;

    public ExtTriangle(Vector3 a, Vector3 b, Vector3 c,
        bool _unidirectionnal, bool _inverseDirection, bool _infinitePlane,
        bool _noGravityBorders)
    {
        EdgeAb = new ExtLine(a, b);
        EdgeBc = new ExtLine(b, c);
        EdgeCa = new ExtLine(c, a);

        unidirectionnal = _unidirectionnal;
        inverseDirection = _inverseDirection;
        infinitePlane = _infinitePlane;
        noGravityBorders = _noGravityBorders;

        TriNorm = Vector3.Cross(a - b, a - c);

        //try to inverse normal
        //if (unidirectionnal && inverseDirection)
        //    TriNorm *= -1;
    }

    public Vector3[] Verticies => new[] { A, B, C };

    public readonly Vector3 TriNorm;

    //private static readonly RangeDouble ZeroToOne = new RangeDouble(0, 1);

    public ExtPlane TriPlane => new ExtPlane(A, TriNorm);

    // The below three could be pre-calculated to
    // trade off space vs time

    public ExtPlane PlaneAb => new ExtPlane(EdgeAb.A, Vector3.Cross(TriNorm, EdgeAb.Delta));
    public ExtPlane PlaneBc => new ExtPlane(EdgeBc.A, Vector3.Cross(TriNorm, EdgeBc.Delta));
    public ExtPlane PlaneCa => new ExtPlane(EdgeCa.A, Vector3.Cross(TriNorm, EdgeCa.Delta));

    //public static readonly RangeDouble Zero1 = new RangeDouble(0, 1);

    private Vector3 CalculateCornerAndLine(Vector3 p, double uab, double uca, ref bool isInPlane)
    {
        if (uca > 1 && uab < 0)
        {
            isInPlane = false;
            return (A);
        }
        var ubc = EdgeBc.Project(p);
        if (uab > 1 && ubc < 0)
        {
            isInPlane = false;
            return (B);
        }
        if (ubc > 1 && uca < 0)
        {
            isInPlane = false;
            return (C);
        }

        //line
        if (uab >= 0 && uab <= 1 && !PlaneAb.IsAbove(p))
        {
            isInPlane = false;
            return (EdgeAb.PointAt(uab));
        }
        if (ubc >= 0 && ubc <= 1 && !PlaneBc.IsAbove(p))
        {
            isInPlane = false;
            return (EdgeBc.PointAt(ubc));
        }
        if (uca >= 0 && uca <= 1 && !PlaneCa.IsAbove(p))
        {
            isInPlane = false;
            return (EdgeCa.PointAt(uca));
        }
        isInPlane = true;
        return (Vector3.zero);
    }

    public Vector3 ClosestPointTo(Vector3 p)
    {
        // Find the projection of the point onto the edge

        var uab = EdgeAb.Project(p);
        var uca = EdgeCa.Project(p);
        bool isInPlane = false;

        Vector3 rightPositionIfOutsidePlane = CalculateCornerAndLine(p, uab, uca, ref isInPlane);

        if (!infinitePlane)
        {
            //ici on est dans un plan fini, si on dis de ne pas prendre
            //en compte les borders, juste tomber !
            if (noGravityBorders && !isInPlane)
            {
                //Debug.Log("ici on est PAS dans le plane, juste tomber !");
                return (ExtVector3.GetNullVector());
            }
            else if (noGravityBorders && isInPlane)
            {
                //Debug.Log("ici on est DANS le plane, ET en noGravityBorder: tester la normal ensuite !");
                if (unidirectionnal)
                {
                    return (GetGoodPointUnidirectionnal(p, TriPlane.Project(EdgeAb.A, TriNorm.normalized, p))); //get the good point (or null) in a plane unidirectionnal
                }
                else
                {
                    //ici en gravityBorder, et on est dans le plan, et c'esst multi directionnel, alors OK dac !
                    return (TriPlane.Project(EdgeAb.A, TriNorm.normalized, p));
                }
            }
            else if (!noGravityBorders && unidirectionnal)
            {
                //here not infinite, and WITH borders AND unidirectionnal
                if (isInPlane)
                {
                    return (GetGoodPointUnidirectionnal(p, TriPlane.Project(EdgeAb.A, TriNorm.normalized, p))); //get the good point (or null) in a plane unidirectionnal
                }
                else
                {
                    return (GetGoodPointUnidirectionnal(p, rightPositionIfOutsidePlane));
                }
            }
            else
            {
                //here Not infinite, WITH borders, NO unidirectionnal
                if (isInPlane)
                {
                    return (TriPlane.Project(EdgeAb.A, TriNorm.normalized, p));
                }
                else
                {
                    return (rightPositionIfOutsidePlane);
                }
            }
        }
        else
        {
            if (unidirectionnal)
            {
                return (GetGoodPointUnidirectionnal(p, TriPlane.Project(EdgeAb.A, TriNorm.normalized, p))); //get the good point (or null) in a plane unidirectionnal
            }
        }



        //ici le plan est infini, OU fini mais on est dedant


        // The closest point is in the triangle so 
        // project to the plane to find it
        //Vector3 projectedPoint = TriPlane.Project(EdgeAb.A, TriNorm.normalized, p);
        return (TriPlane.Project(EdgeAb.A, TriNorm.normalized, p));
    }

    /// <summary>
    /// take into acount unidirectinnal option, return null if not found
    /// </summary>
    /// <returns></returns>
    private Vector3 GetGoodPointUnidirectionnal(Vector3 p, Vector3 foundPosition)
    {
        //Vector3 projectedOnPlane = TriPlane.Project(EdgeAb.A, TriNorm.normalized, p);
        Vector3 dirPlayer = p - foundPosition;

        float dotPlanePlayer = ExtVector3.DotProduct(dirPlayer.normalized, TriNorm.normalized);
        if ((dotPlanePlayer < 0 && !inverseDirection) || dotPlanePlayer > 0 && inverseDirection)
        {
            return (foundPosition);
        }
        else
        {
            Debug.DrawRay(p, dirPlayer, Color.yellow, 5f);
            Debug.DrawRay(p, TriNorm.normalized, Color.black, 5f);
            return (ExtVector3.GetNullVector());
        }
    }
}

public struct ExtLine
{
    public readonly Vector3 A;
    public readonly Vector3 B;
    public readonly Vector3 Delta;

    public ExtLine(Vector3 a, Vector3 b)
    {
        A = a;
        B = b;
        Delta = b - a;
    }

    public Vector3 PointAt(double t) => A + (float)t * Delta;
    public double LengthSquared => Delta.sqrMagnitude;
    public double LengthLine => Delta.magnitude;

    public Vector3 ClosestPointTo(Vector3 p)
    {
        //The normalized "distance" from a to your closest point 
        double distance = Project(p); 

        if (distance < 0)     //Check if P projection is over vectorAB     
        {
            return A;
        }
        else if (distance > 1)
        {
            return B;
        }
        else
        {
            return A + Delta * (float)distance;
        }
    }

    public double GetLenght()
    {
        return (LengthLine);
    }

    public double Project(Vector3 p) => ExtVector3.DotProduct(Delta, p - A) / LengthSquared;
}

public struct ExtPlane
{
    public Vector3 Point;
    public Vector3 Direction;

    public ExtPlane(Vector3 point, Vector3 direction)
    {
        Point = point;
        Direction = direction;
    }

    //public bool IsAbove(Vector3 q) => Direction.Dot(q - Point) > 0;
    public bool IsAbove(Vector3 q) => ExtVector3.DotProduct(q - Point, Direction) > 0;

    public Vector3 Project(Vector3 randomPointInPlane, Vector3 normalPlane, Vector3 pointToProject)
    {
        Vector3 v = pointToProject - randomPointInPlane;
        Vector3 d = Vector3.Project(v, normalPlane.normalized);

        
        Vector3 projectedPoint = pointToProject - d;

        

        return (projectedPoint);
    }
}