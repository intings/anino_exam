using UnityEngine;


public class SoundController : MonoBehaviour
{
    public static SoundController Instance;
    public AudioClip[] gameSounds;
    [SerializeField] private AudioSource audioSource;
    private float _sfxVolume;
    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    public void PlaySoundByIndex(int i)
    {
        audioSource.PlayOneShot(gameSounds[i]);
    }
}