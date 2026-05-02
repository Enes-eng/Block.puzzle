using UnityEngine;
using TMPro;

public class ShopManager : MonoBehaviour
{
    [Header("Fiyat Etiketleri")]
    public int hammerPrice = 100;
    public int bombPrice = 250;

    [Header("Oyuncunun Çantası")]
    public int hammerCount = 0;
    public int bombCount = 0;

    [Header("Arayüz Yazıları")]
    public TextMeshProUGUI hammerCountText; // Ekranda kaç çekici olduğunu gösterecek
    public TextMeshProUGUI bombCountText;   // Ekranda kaç bombası olduğunu gösterecek

    void Start()
    {
        // Oyuncu oyuna girdiğinde çantasındaki eski eşyaları yükle
        hammerCount = PlayerPrefs.GetInt("HammerCount", 0);
        bombCount = PlayerPrefs.GetInt("BombCount", 0);
        UpdateUI();
    }

    // Çekiç Satın Alma Butonuna Bağlanacak
    public void BuyHammer()
    {
        // Kasada yeterli para var mı kontrol et
        if (GridManager.Instance.currentCoins >= hammerPrice)
        {
            // GridManager'daki fonsiyonu eksi (-) değerle çağırarak parayı kesiyoruz
            GridManager.Instance.AddCoins(-hammerPrice); 
            
            // Çekici çantaya ekle ve kaydet
            hammerCount++;
            PlayerPrefs.SetInt("HammerCount", hammerCount);
            PlayerPrefs.Save();
            
            UpdateUI();
            Debug.Log("Çekiç satın alındı! Kalan Para: " + GridManager.Instance.currentCoins);
        }
        else
        {
            Debug.Log("Çekiç için yeterli altın yok!");
            // İleride buraya ekranda sallanan bir "Paran yetersiz" uyarısı da koyabiliriz
        }
    }

    // Bomba Satın Alma Butonuna Bağlanacak
    public void BuyBomb()
    {
        if (GridManager.Instance.currentCoins >= bombPrice)
        {
            GridManager.Instance.AddCoins(-bombPrice); 
            
            bombCount++;
            PlayerPrefs.SetInt("BombCount", bombCount);
            PlayerPrefs.Save();
            
            UpdateUI();
            Debug.Log("Bomba satın alındı! Kalan Para: " + GridManager.Instance.currentCoins);
        }
        else
        {
            Debug.Log("Bomba için yeterli altın yok!");
        }
    }

    // Çantadaki eşya sayılarını ekranda günceller
    private void UpdateUI()
    {
        if(hammerCountText != null) hammerCountText.text = hammerCount.ToString();
        if(bombCountText != null) bombCountText.text = bombCount.ToString();
    }
}