using UnityEngine;

[CreateAssetMenu(fileName = "NewFootstepSurface", menuName = "Audio/FootstepSurface")]
public class FootstepSurface : ScriptableObject
{
    public string surfaceTag;
    public AudioClip[] footstepSounds;
}
