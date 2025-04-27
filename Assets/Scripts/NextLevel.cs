using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour
{
    //this script will call fade in fade out from RadialFade script
    //and load the next level
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            RadialFade radialFade = FindObjectOfType<RadialFade>();
            if (radialFade != null)
            {
                radialFade.StartFadeOut(other.transform.position);
                StartCoroutine(LoadNextLevel());
            }
        }
    }
    private IEnumerator LoadNextLevel()
    {
        yield return new WaitForSeconds(1f); // Wait for the fade out to complete
        // Load the next level here
        // For example, using UnityEngine.SceneManagement;
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
