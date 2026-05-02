using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    [Header("Menü Paneli")]
    public GameObject pauseMenuPanel; // Menüyü buraya bağlayacağız

    // Menüyü AÇAN fonksiyon
    public void OpenMenu()
    {
        if (pauseMenuPanel != null)
        {
            pauseMenuPanel.SetActive(true);
        }
    }

    // Menüyü KAPATAN fonksiyon
    public void CloseMenu()
    {
        if (pauseMenuPanel != null)
        {
            pauseMenuPanel.SetActive(false);
        }
    }

    // Mevcut bölümü TEKRAR BAŞLATAN fonksiyon
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Bizi Ana Menüye fırlatacak fonksiyon (Zaten vardı)
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu"); 
    }
}