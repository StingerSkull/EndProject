using UnityEngine;

[CreateAssetMenu(fileName = "SoundData", menuName = "Scriptable Objects/SoundData")]
public class SoundData : ScriptableObject
{
    public string soundName;
    [Range(0f, 1f)]
    public float volume;
    public AudioClip[] sounds;
}
