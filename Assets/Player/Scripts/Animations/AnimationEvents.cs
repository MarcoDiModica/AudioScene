using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
    [SerializeField] private PlayerReferences playerReferences;
    //[SerializeField] private MovementSFX movementSFX;
    //[SerializeField] private MovementVFX movementVFX;

    public void FootstepEvent()
    {
        // This would call your SFX/VFX systems
        // if (movementSFX != null)
        //     movementSFX.PlayFootstepSound();
        //
        // if (movementVFX != null)
        //     movementVFX.SpawnFootstepParticle();
    }

    public void JumpEvent()
    {
        // if (movementSFX != null)
        //     movementSFX.PlayJumpSound();
        //
        // if (movementVFX != null)
        //     movementVFX.SpawnJumpParticle();
    }

    public void LandEvent()
    {
        // if (movementSFX != null)
        //     movementSFX.PlayLandSound();
        //
        // if (movementVFX != null)
        //     movementVFX.SpawnLandParticle();
    }
}