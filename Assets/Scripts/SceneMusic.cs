using UnityEngine;

public class SceneMusic : MonoBehaviour
{
    public int musicIndex; // Bu sahne için çalınacak müziğin indexi

    private void Start()
    {
        if (MusicManager.Instance != null)
        {
            MusicManager.Instance.PlayMusic(musicIndex);
        }
    }
}