using UnityEngine;

[System.Serializable]
public class SoundGroup
{
    public AudioClip[] clips;
    public float volume = 1f;
    public float cooldown = 0.5f;
    [HideInInspector] public float lastPlayTime;
}

public class MonsterSound : MonoBehaviour
{
    [Header("Sound Settings")]
    public SoundGroup footsteps;
    public SoundGroup rage;
    public SoundGroup chase;
    public SoundGroup attack;
    public SoundGroup breathe;

    private AudioSource audioSource;

    private float footstepInterval = 0.57f;
    private float footstepTimer;
    private float breatheTimer = 7f;
    public bool isStopped = false;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Update()
    {
        footstepTimer -= Time.deltaTime;
        breatheTimer -= Time.deltaTime;

        if (isStopped == true) return;


        if (breatheTimer <= 0)
        {
            PlayBreathe();
            breatheTimer = 7f;
        }
    }

    public void SetFootstepSpeed(bool isChasing)
    {
        footstepInterval = isChasing ? 0.08f : 0.57f;
    }

    public void PlayFootstep()
    {
        PlayRandom(footsteps);
    }

    public void PlayRage()
    {
        PlayRandom(rage);
    }

    public void PlayChase()
    {
        PlayRandom(chase);
    }

    public void PlayAttack()
    {
        PlayRandom(attack);
    }

    public void PlayBreathe()
    {
        PlayRandom(breathe);
    }

    private void PlayRandom(SoundGroup sound)
    {
        if(sound.clips.Length == 0) return;
        if(Time.time - sound.lastPlayTime < sound.cooldown) return;
        
        sound.lastPlayTime = Time.time;
        AudioClip clip = sound.clips[Random.Range(0, sound.clips.Length)];
        audioSource.PlayOneShot(clip, sound.volume);
    }
}