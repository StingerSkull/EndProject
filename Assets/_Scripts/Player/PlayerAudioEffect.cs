using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering.LookDev;
using UnityEngine;

public class PlayerAudioEffect : MonoBehaviour
{
    public AudioSource audioSource;
    public Casting casting;

    [Range(0.0f, 1.0f)]
    public float pitch;

    public List<SoundData> soundDataList; // Liste des sons configurables dans l’Inspector
    private Dictionary<string, SoundData> soundDictionary = new Dictionary<string, SoundData>();
    private Dictionary<string, int> lastPlayedIndexes = new Dictionary<string, int>(); // Stocke l’index du dernier son joué pour chaque type de son
    private Dictionary<string, int> lastCastPlayedIndexes = new Dictionary<string, int>(); // Stocke l’index du dernier son joué pour chaque type de son

    void Start()
    {
        // Remplit le dictionnaire avec les sons disponibles
        foreach (var data in soundDataList)
        {
            soundDictionary[data.soundName] = data;
            lastPlayedIndexes[data.soundName] = -1; // Initialise avec -1 (aucun son joué encore)
        }
    }

    public void PlayRandomSound(string sound)
    {
        if (soundDictionary.TryGetValue(sound, out SoundData soundData))
        {
            int newIndex = 0;
            if (soundData.sounds.Length > 1)
            {
                do
                {
                    newIndex = Random.Range(0, soundData.sounds.Length);
                } while (newIndex == lastPlayedIndexes[sound]);

                lastPlayedIndexes[sound] = newIndex;
            }

            PlaySound(soundData, newIndex);
        }
        else
        {
            Debug.LogWarning($"Son '{sound}' non trouvé dans le dictionnaire !");
        }
    }

    public void PlayRandomCastSound()
    {
        SoundData soundData = null;
        if (casting.animCast1)
        {
            soundData = casting.firstSpell.GetSound();
            if (!lastCastPlayedIndexes.ContainsKey(soundData.soundName))
            {
                lastCastPlayedIndexes[soundData.soundName] = -1;
            }
        }
        else if (casting.animCast2)
        {
            soundData = casting.secondSpell.GetSound();
            if (!lastCastPlayedIndexes.ContainsKey(soundData.soundName))
            {
                lastCastPlayedIndexes[soundData.soundName] = -1;
            }
        }

        if (soundData != null)
        {
            int newIndex = 0;
            if (soundData.sounds.Length > 1)
            {
                do
                {
                    newIndex = Random.Range(0, soundData.sounds.Length);
                } while (newIndex == lastCastPlayedIndexes[soundData.soundName]);

                lastCastPlayedIndexes[soundData.soundName] = newIndex;
            }
            PlaySound(soundData, newIndex);
        }
    }

    private void PlaySound(SoundData soundData, int index)
    {
        audioSource.volume = soundData.volume;
        audioSource.pitch = Random.Range(1-pitch, 1+pitch);
        audioSource.PlayOneShot(soundData.sounds[index]);
    }
}
