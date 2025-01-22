using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering.LookDev;
using UnityEngine;

public class SpellAudioEffect : MonoBehaviour
{
    public AudioSource audioSource;

    [Range(0.0f, 1.0f)]
    public float pitch;

    public List<SoundData> soundDataList; // Liste des sons configurables dans l’Inspector
    private Dictionary<string, SoundData> soundDictionary = new Dictionary<string, SoundData>();

    void Start()
    {
        // Remplit le dictionnaire avec les sons disponibles
        foreach (SoundData data in soundDataList)
        {
            soundDictionary[data.soundName] = data;
        }
    }

    public void PlayRandomSound(string sound)
    {
        if (soundDictionary.TryGetValue(sound, out SoundData soundData))
        {
            PlaySound(soundData, Random.Range(0, soundData.sounds.Length));
        }
        else
        {
            Debug.LogWarning($"Son '{sound}' non trouvé dans le dictionnaire !");
        }
    }

    private void PlaySound(SoundData soundData, int index)
    {
        audioSource.volume = soundData.volume;
        audioSource.pitch = Random.Range(1-pitch, 1+pitch);
        audioSource.PlayOneShot(soundData.sounds[index]);
    }
}
