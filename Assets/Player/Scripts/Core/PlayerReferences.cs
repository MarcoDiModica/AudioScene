using UnityEngine;

public class PlayerReferences : MonoBehaviour
{
    [Header("Component References")]
    public CharacterController characterController;
    public Transform playerTransform;
    public Transform modelTransform;
    public Animator animator;

    private void Awake()
    {
        if (characterController == null)
            characterController = GetComponent<CharacterController>();

        if (playerTransform == null)
            playerTransform = transform;

        if (modelTransform == null && transform.childCount > 0)
            modelTransform = transform.GetChild(0);

        if (animator == null && modelTransform != null)
            animator = modelTransform.GetComponent<Animator>();
    }
}