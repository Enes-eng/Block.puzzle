using UnityEngine;
using UnityEngine.UI;
using TMPro; // Yazılar için gerekli kütüphane

public class ThemeManager : MonoBehaviour
{
    [Header("Tema Değişecek Objeler")]
    public Image[] backgroundImages;     // Ana arka planlar
    public Image[] panelImages;          // Menü pencereleri, Grid çerçevesi, tepsiler
    public TextMeshProUGUI[] textElements; // Bütün yazılar (Puan, Menü isimleri vs.)

    [Header("Aydınlık Mod Renkleri (Krem)")]
    public Color lightBgColor;
    public Color lightPanelColor;
    public Color lightTextColor;

    [Header("Karanlık Mod Renkleri (Siyah/Antrasit)")]
    public Color darkBgColor;
    public Color darkPanelColor;
    public Color darkTextColor;

    private bool isDarkMode = false;

    void Start()
    {
        // Oyuncu oyuna girdiğinde önceki seçimini hatırla (0=Aydınlık, 1=Karanlık)
        isDarkMode = PlayerPrefs.GetInt("ThemePreference", 0) == 1;
        ApplyTheme(); // Seçime göre renkleri boya
    }

    // Butona basıldığında çalışacak fonksiyon
    public void ToggleTheme()
    {
        isDarkMode = !isDarkMode; // Durumu tersine çevir
        
        // Yeni seçimi hafızaya kaydet ki oyundan çıkınca silinmesin
        PlayerPrefs.SetInt("ThemePreference", isDarkMode ? 1 : 0);
        PlayerPrefs.Save();
        
        ApplyTheme(); // Renkleri güncelle
    }

    // Objeleri ilgili renklere boyayan ana sistem
    private void ApplyTheme()
    {
        // Hangi renkleri kullanacağımızı seçiyoruz
        Color currentBg = isDarkMode ? darkBgColor : lightBgColor;
        Color currentPanel = isDarkMode ? darkPanelColor : lightPanelColor;
        Color currentText = isDarkMode ? darkTextColor : lightTextColor;

        // Listeye koyduğumuz tüm arka planları boya
        foreach (Image img in backgroundImages)
        {
            if(img != null) img.color = currentBg;
        }

        // Listeye koyduğumuz tüm panelleri/tepsileri boya
        foreach (Image img in panelImages)
        {
            if(img != null) img.color = currentPanel;
        }

        // Listeye koyduğumuz tüm yazıları boya
        foreach (TextMeshProUGUI txt in textElements)
        {
            if(txt != null) txt.color = currentText;
        }
    }
}