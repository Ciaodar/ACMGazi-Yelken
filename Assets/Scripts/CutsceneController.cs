using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CutsceneController : MonoBehaviour
{
    public Image cutsceneImage;
    public TextMeshProUGUI text;
    public AudioSource audioSource;
    public AudioClip[] audioClips;
    public Sprite[] cutsceneSprites;
    public string[] cutsceneStrings;
    public float displayTime = 5f;
    
    private int currentIndex = 0;

    private void Start()
    {
        StartCoroutine(PlayCutscene());
    }

    IEnumerator PlayCutscene()
    {
        while (currentIndex < cutsceneSprites.Length)
        {
            cutsceneImage.sprite = cutsceneSprites[currentIndex];
            text.text = cutsceneStrings[currentIndex];

            if (audioClips.Length > currentIndex && audioClips[currentIndex])
            {
                audioSource.clip = audioClips[currentIndex];
                audioSource.Play();
            }

            yield return new WaitForSeconds(displayTime);

            currentIndex++;
        }
        LoadNextScene();
    }

    private void LoadNextScene()
    {
        
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex==1?2:0);
    }
}
