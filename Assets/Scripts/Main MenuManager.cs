using UnityEngine;
using UnityEngine.SceneManagement; // Sahneler arası seyahat biletimiz bu kütüphane!

public class MainMenuManager : MonoBehaviour
{
    // Seviyeler butonuna basılınca çalışacak
    public void PlayLevelMode()
    {
        SceneManager.LoadScene("LevelMode"); // Tırnak içindeki isim, sahnenin adıyla BİREBİR aynı olmalı
    }

    // Klasik Mod butonuna basılınca çalışacak
    public void PlayClassicMode()
    {
        SceneManager.LoadScene("ClassicMode");
    }
}