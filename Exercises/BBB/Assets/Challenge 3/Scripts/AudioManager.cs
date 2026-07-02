using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioMixer masterMixer; // mixes the volumes of the audio groups
    [SerializeField] private AudioSource musicSource; // outputs to the musicGroup
    [SerializeField] private AudioSource SFXSource; // outputs to the sfxGroup
    [SerializeField] private AudioClip[] soundClips;

    /// <summary>
    /// Plays the sound effect whose name corresponds to an AudioClip from the soundClips[]
    /// The sound effect is played via the SFXSource to allow for proper volume mixing
    /// </summary>
    /// <param name="soundName">name of the sound clip to be played</param>
    public void PlaySoundEffect(string soundName)
    {
        foreach (AudioClip sound in soundClips)
        {
            if (sound.name == soundName)
            {
                SFXSource.PlayOneShot(sound);
            }
        }
    }
}
