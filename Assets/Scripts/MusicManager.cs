using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance; // Singleton instance
    public AudioClip[] musicClips; // Müzik klipleri
    public AudioMixerGroup musicMixerGroup; // AudioMixerGroup bağlantısı

    [SerializeField]private AudioSource audioSource;

    private void Awake()
    {
        // Singleton yapısı
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Sahne geçişlerinde yok etme
        }
        else
        {
            Destroy(gameObject); // Başka bir instance varsa yok et
        }
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.outputAudioMixerGroup = musicMixerGroup; // AudioMixerGroup'u bağla
        audioSource.loop = true; // Müzik döngüde çalsın
    }

    public void PlayMusic(int musicIndex)
    {
        if (musicClips == null || musicClips.Length == 0)
        {
            Debug.LogError("MusicClips array is not assigned or empty!");
            return;
        }

        if (musicIndex < 0 || musicIndex >= musicClips.Length)
        {
            Debug.LogError($"Invalid musicIndex: {musicIndex}. It must be between 0 and {musicClips.Length - 1}.");
            return;
        }

        if (audioSource == null)
        {
            Debug.LogError("AudioSource is not initialized!");
            return;
        }

        if (audioSource.clip != null && audioSource.clip == musicClips[musicIndex]) return;

        audioSource.clip = musicClips[musicIndex];
        audioSource.Play();
    }
} 