using UnityEngine;

[CreateAssetMenu(fileName = "StateData", menuName = "Player/State Data")]
public class StateData : ScriptableObject
{
    [Header("Idle State")]
    public float idleRotationSpeed = 10f;

    [Header("Walk State")]
    public float walkRotationSpeed = 15f;

    [Header("Run State")]
    public float runRotationSpeed = 20f;

    [Header("Jump State")]
    public float jumpRotationSpeed = 5f;

    [Header("Fall State")]
    public float fallRotationSpeed = 5f;
}