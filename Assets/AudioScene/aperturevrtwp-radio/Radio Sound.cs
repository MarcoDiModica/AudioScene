using UnityEngine;

public class RadioSound : MonoBehaviour
{
    public AudioSource source;
    public AudioClip radioClip;

    public void PlayRadio()
    {
        if (source != null && radioClip != null)
        {
            source.PlayOneShot(radioClip);
        }
    }
}
