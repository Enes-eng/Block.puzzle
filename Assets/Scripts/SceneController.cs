using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    // Bu fonksiyon bizi direkt Ana Menüye fırlatacak
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu"); 
    }
}