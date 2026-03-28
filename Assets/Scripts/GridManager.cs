using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using TMPro; 
public class GridManager : MonoBehaviour
{
    [SerializeField] private int width = 8;
    [SerializeField] private int height = 8;

    public static GridManager Instance;
    public static bool isGameOver = false; 

    [Header("Ekonomi Sistemi")]
    public TextMeshProUGUI coinText; 
    public int currentCoins = 0; 
    
    public GameObject winPanel;      
    public GameObject gameOverPanel; 

    public Transform[,] gridArray; 

    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private Color tileColor;

    [Header("Puzzle Settings")]
    public Image fillImage; 
    public int totalPiecesToWin = 5; 
    private int currentPieces = 0; 

    [Header("Görsel Efektler")]
    public GameObject fracturePrefab; // YENİ: Kırılma efektimizin kalıbı
    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        isGameOver = false;
        
        
        currentCoins = PlayerPrefs.GetInt("PlayerCoins", 0); 
        UpdateCoinUI(); 
        
        gridArray = new Transform[width, height];
        GenerateGrid();
        AdjustCamera(); 
        isGameOver = false;
    }

    void GenerateGrid()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                GameObject spawnedTile = Instantiate(tilePrefab, new Vector3(x, y, 0), Quaternion.identity);
                spawnedTile.name = $"Tile {x} {y}";
                spawnedTile.transform.parent = transform;

                SpriteRenderer sr = spawnedTile.GetComponent<SpriteRenderer>();
                sr.color = tileColor;
            }
        }
    }

    void AdjustCamera()
    {
        Camera.main.orthographicSize = 8f;
        Camera.main.transform.position = new Vector3((float)width / 2 - 0.5f, (float)height / 2 - 0.5f-0.8f, -10);
    }

    public bool IsValidPosition(int x, int y)
    {
        if (x < 0 || x >= width || y < 0 || y >= height)
        {
            return false;
        }

        if (gridArray[x, y] != null) 
        {
            return false;
        }

        return true;
    }


    public void CheckLines()
    {
        
        for (int y = 0; y < height; y++)
        {
            if (IsRowFull(y)) ClearRow(y);
        }

        for (int x = 0; x < width; x++)
        {
            if (IsColumnFull(x)) ClearColumn(x);
        }

        CheckIfGameOver();
    }

    private bool IsRowFull(int y)
    {
        for (int x = 0; x < width; x++)
        {
            if (gridArray[x, y] == null) return false; 
        }
        return true;
    }

    private bool IsColumnFull(int x)
    {
        for (int y = 0; y < height; y++)
        {
            if (gridArray[x, y] == null) return false;
        }
        return true;
    }

    private void ClearRow(int y)
    {
        AddCoins(50);
        for (int x = 0; x < width; x++)
        {
            if (gridArray[x, y] != null)
            {
                if (gridArray[x, y].CompareTag("PuzzlePiece"))
                {
                    currentPieces++; 
                  
                    
                    if (fillImage != null)
                    {
                        fillImage.fillAmount = (float)currentPieces / totalPiecesToWin;
                    }

                    if (currentPieces >= totalPiecesToWin)
                    {
                        isGameOver = true; 
                        AddCoins(100);
                        
                        if (winPanel != null)
                        {
                            winPanel.SetActive(true);
                        }
                    }
                }
                
                Vector3 fracturePos = gridArray[x, y].transform.position;
                
                // 2. O noktada YEPYENİ kırılma efektini yarat!
                GameObject fracture = Instantiate(fracturePrefab, fracturePos, Quaternion.identity);
                
                // 3. Efekt çöp yaratmasın diye 1 saniye sonra sahneden sil
                Destroy(fracture, 1f);


                Destroy(gridArray[x, y].gameObject);
                gridArray[x, y] = null;
            }
        }
    }

    private void ClearColumn(int x)
    {
        AddCoins(50);
        for (int y = 0; y < height; y++)
        {
            if (gridArray[x, y] != null)
            {
                if (gridArray[x, y].CompareTag("PuzzlePiece"))
                {
                    currentPieces++; 
                     
                    
                    if (fillImage != null)
                    {
                        fillImage.fillAmount = (float)currentPieces / totalPiecesToWin;
                    }

                   if (currentPieces >= totalPiecesToWin)
                    {
                        isGameOver = true; 
                        AddCoins(100);
                        
                        if (winPanel != null)
                        {
                            winPanel.SetActive(true);
                        }
                    }
                }
              Vector3 fracturePos = gridArray[x, y].transform.position;
                
                // 2. O noktada YEPYENİ kırılma efektini yarat!
                GameObject fracture = Instantiate(fracturePrefab, fracturePos, Quaternion.identity);
                
                // 3. Efekt çöp yaratmasın diye 1 saniye sonra sahneden sil
                Destroy(fracture, 1f);
                Destroy(gridArray[x, y].gameObject);
                gridArray[x, y] = null;
            }
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Tek bir bloğu alır ve tahtadaki TÜM KARELERİ tek tek gezerek sığıp sığmadığını dener
    private bool CanBlockFitAnywhere(GameObject block)
    {
        if (block.transform.childCount == 0) return false;

        List<Vector2Int> tileOffsets = new List<Vector2Int>();
        
        // Bloğun ilk parçasını "Çapa" (Merkez) olarak kabul edelim
        Vector3 anchorPos = block.transform.GetChild(0).localPosition; 

        // Bloğun tüm parçalarının bu çapaya göre aralarındaki mesafeyi (şeklini) çıkartalım
        foreach (Transform child in block.transform)
        {
            if (!child.gameObject.activeSelf) continue; 
            
            int offsetX = Mathf.RoundToInt(child.localPosition.x - anchorPos.x);
            int offsetY = Mathf.RoundToInt(child.localPosition.y - anchorPos.y);
            tileOffsets.Add(new Vector2Int(offsetX, offsetY));
        }

        // Şimdi tahtadaki (Grid) HER BİR KAREYİ tek tek dolaş
        for (int gridX = 0; gridX < width; gridX++)
        {
            for (int gridY = 0; gridY < height; gridY++)
            {
                bool canFitHere = true;

                // Bu blok bu (gridX, gridY) merkezine sığar mı?
                foreach (Vector2Int offset in tileOffsets)
                {
                    int targetX = gridX + offset.x;
                    int targetY = gridY + offset.y;

                    // Eğer bir parça bile dışarı taşıyorsa veya dolu bir yere geliyorsa, bu pozisyon geçersiz!
                    if (!IsValidPosition(targetX, targetY))
                    {
                        canFitHere = false;
                        break; // Diğer parçalara bakmaya gerek yok, buraya sığmıyor.
                    }
                }

                // Eğer döngü kırılmadıysa, yani tüm parçalar sığdıysa, blok buraya yerleşebilir!
                if (canFitHere)
                {
                    return true; 
                }
            }
        }

        // Tüm tahtayı denedik, hiçbir yere sığmadı!
        return false;
    }

    // Her hamleden sonra bu fonksiyonu çağırıp oyunu kontrol edeceğiz
    public void CheckIfGameOver()
    {
        if (isGameOver) return;

        BlockDrag[] allBlocks = FindObjectsByType<BlockDrag>(FindObjectsSortMode.None);
        int activeBlockCount = 0;
        bool hasAnyMove = false;

        Debug.Log("--- OYUN BİTTİ Mİ KONTROLÜ BAŞLADI ---");

        foreach (BlockDrag block in allBlocks)
        {
            // 1. KONTROL: Bloğun içi boşaldıysa (çocukları silindiyse) atla
            if (block.transform.childCount == 0) continue;

            // 2. KONTROL: YERLEŞMİŞ BLOKLARI ELE!
            // Oyun tahtası genelde Y=0 ve yukarısındadır. Aşağıda bekleyen bloklar eksili değerdedir.
            // Eğer blok 0'ın yukarısındaysa o zaten tahtadadır, onu test ETME!
            if (block.transform.position.y > -0.5f) 
            {
                continue; 
            }

            activeBlockCount++; // Gerçekten aşağıda bekleyen bir blok bulduk!

            bool canFit = CanBlockFitAnywhere(block.gameObject);
            Debug.Log("Test Edilen Aşağıdaki Blok: " + block.gameObject.name + " | Sığar mı?: " + canFit);

            if (canFit)
            {
                hasAnyMove = true;
            }
        }

        Debug.Log("Aşağıda bekleyen blok sayısı: " + activeBlockCount);
        Debug.Log("Herhangi bir hamle kaldı mı?: " + hasAnyMove);

        // ŞALTERİ İNDİRME ANI
        if (!hasAnyMove && activeBlockCount > 0)
        {
            Debug.Log("ŞALTER İNDİ! HAMLE KALMADI! 🚨");
            isGameOver = true;
            
            if (gameOverPanel != null)
            {
                gameOverPanel.SetActive(true);
            }
        }
    }
    // Oyuncuya altın vermek istediğimizde bu fonksiyonu çağıracağız
    public void AddCoins(int amount)
    {
        currentCoins += amount; // Parayı ekle
        PlayerPrefs.SetInt("PlayerCoins", currentCoins); // KASAYA KİLİTLE! (Oyunu kapatsa da silinmez)
        PlayerPrefs.Save(); // Kaydet
        
        UpdateCoinUI(); // Ekrandaki yazıyı güncelle
        
        Debug.Log(amount + " Altın Kazanıldı! Toplam Altın: " + currentCoins);
    }

    // Ekrandaki yazıyı güncelleyen küçük yardımcı fonksiyon
    private void UpdateCoinUI()
    {
        if (coinText != null)
        {
            coinText.text = currentCoins.ToString();
        }
    }
}