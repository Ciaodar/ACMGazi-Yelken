using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour
{
    [SerializeField] private float loadDelay = 1f; // Geçişten önce bekleme süresi

    private bool isLoading = false; // Birden fazla kez tetiklenmesin diye

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isLoading) return; // Zaten yükleniyorsa bir daha çalışmasın

        if (other.CompareTag("Player"))
        {
            isLoading = true;
            StartCoroutine(LoadNextScene());
        }
    }

    private IEnumerator LoadNextScene()
    {
        yield return new WaitForSeconds(loadDelay);

        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.LogWarning("Son sahnedesin, ileri sahne yok!");
        }
    }
}
