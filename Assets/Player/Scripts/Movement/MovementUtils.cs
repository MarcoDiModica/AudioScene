using UnityEngine;

public static class MovementUtils
{
    public static bool IsZero(float value, float threshold = 0.01f)
    {
        return Mathf.Abs(value) < threshold;
    }

    public static bool IsZero(Vector3 vector, float threshold = 0.01f)
    {
        return vector.magnitude < threshold;
    }

    public static void SmoothRotateTowards(Transform transform, Vector3 direction, float rotationSpeed)
    {
        if (direction == Vector3.zero)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation,
            rotationSpeed * Time.deltaTime);
    }

    public static float GetSlopeSpeedMultiplier(float slopeAngle, float maxSlopeAngle,
        float uphillMultiplier, float downhillMultiplier, bool isGoingUphill)
    {
        float normalizedAngle = Mathf.Clamp01(slopeAngle / maxSlopeAngle);

        if (isGoingUphill)
        {
            return Mathf.Lerp(1f, uphillMultiplier, normalizedAngle);
        }
        else
        {
            return Mathf.Lerp(1f, downhillMultiplier, normalizedAngle);
        }
    }
}