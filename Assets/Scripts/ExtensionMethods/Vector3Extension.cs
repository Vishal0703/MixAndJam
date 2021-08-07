using UnityEngine;

public static class Vector3Extension
{
    public static Vector3 SetDirectionalValue(this Vector3 initialVector, float? x = null, float? y = null, float? z = null)
    {
        var xVelocity = (x != null) ? (float)x : initialVector.x;
        var yVelocity = (y != null) ? (float)y : initialVector.y;
        var zVelocity = (z != null) ? (float)z : initialVector.z;

        return new Vector3(xVelocity, yVelocity, zVelocity);
    }
}
