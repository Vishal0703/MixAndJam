using UnityEngine;

public static class QuaternionExtension
{
    public static Quaternion CalculateFinalRotationAboutYGivenDirection(this Quaternion initialRotation, Vector2 direction)
    {
        if (direction.y < 0)
            direction = -direction;

        var angleToRotate = -Vector2.SignedAngle(Vector2.up, direction);
        var finalRotation = Quaternion.Euler(initialRotation.eulerAngles.x, initialRotation.eulerAngles.y + angleToRotate, initialRotation.eulerAngles.z);

        return finalRotation;
    }
}
