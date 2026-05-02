using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ThemeManager : MonoBehaviour
{
    [Header("Arka Plan Kamerası")]
    public Camera mainCamera; 

    [Header("Tema Değişecek Objeler")]
    public Image[] panelImages;          
    public TextMeshProUGUI[] textElements; 
    public Image[] iconImages; // İkonlarımız için olan liste burada!

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
        isDarkMode = PlayerPrefs.GetInt("ThemePreference", 0) == 1;
        ApplyTheme(); 
    }

    public void ToggleTheme()
    {
        isDarkMode = !isDarkMode; 
        PlayerPrefs.SetInt("ThemePreference", isDarkMode ? 1 : 0);
        PlayerPrefs.Save();
        ApplyTheme(); 
    }

    private void ApplyTheme()
    {
        Color currentBg = isDarkMode ? darkBgColor : lightBgColor;
        Color currentPanel = isDarkMode ? darkPanelColor : lightPanelColor;
        Color currentText = isDarkMode ? darkTextColor : lightTextColor;

        // Kameranın Rengini Değiştir
        if (mainCamera != null)
        {
            mainCamera.backgroundColor = currentBg;
        }

        // Panelleri Boya
        foreach (Image img in panelImages)
        {
            if(img != null) img.color = currentPanel;
        }

        // Yazıları Boya
        foreach (TextMeshProUGUI txt in textElements)
        {
            if(txt != null) txt.color = currentText;
        }

        // İkonları Yazı Rengine Boya (Karanlıkta açık, aydınlıkta koyu)
        foreach (Image icon in iconImages)
        {
            if(icon != null) icon.color = currentText;
        }
    }
}