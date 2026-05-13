using UnityEngine;

public class SFXManager : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] audioClips;
    [SerializeField]
    private AudioSource audioSource;

    public void warningSound() => audioSource.PlayOneShot(audioClips[0]);

    public void thudSound() => audioSource.PlayOneShot(audioClips[1]);

    public void footstepSound() => audioSource.PlayOneShot(audioClips[2]);

    public void phoneClickSound() => audioSource.PlayOneShot(audioClips[3]);
}
