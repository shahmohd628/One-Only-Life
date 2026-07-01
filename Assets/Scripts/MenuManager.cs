using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip clickSFX;
    void PlaySFX(AudioClip clip)
    {
        if (audioSource != null && clip != null)
            audioSource.PlayOneShot(clip);
    }
    public void PlayClickSFX()   => PlaySFX(clickSFX);

    public void OnPlayButtonClicked()
    {
        PlayClickSFX();
        Invoke("LoadLevel1",0.2f);
    }

    public void OnQuitButtonClicked()
    {
        PlayClickSFX();
        Application.Quit();
    }

    public void LoadLevel1()
    {
        SceneManager.LoadScene("Level1");
    }
}
