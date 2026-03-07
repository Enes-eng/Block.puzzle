using UnityEngine;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{
    [SerializeField] private int width = 8;
    [SerializeField] private int height = 8;

    public static GridManager Instance;

    // DEĞİŞİKLİK BURADA: Artık bool (true/false) değil, Transform (obje) tutuyoruz!
    public Transform[,] gridArray; 

    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private Color tileColor;

    [Header("Puzzle Settings")]
    public Image fillImage; // Dolan barımız
    public int totalPiecesToWin = 5; // Kazanmak için gereken parça sayısı
    private int currentPieces = 0; // Şu an toplanan

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        // Hafızayı obje tutacak şekilde başlatıyoruz
        gridArray = new Transform[width, height];
        GenerateGrid();
        AdjustCamera(); 
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
        Camera.main.transform.position = new Vector3((float)width / 2 - 0.5f, (float)height / 2 - 0.5f - 2.5f, -10);
    }

    public bool IsValidPosition(int x, int y)
    {
        if (x < 0 || x >= width || y < 0 || y >= height)
        {
            return false;
        }

        // Kural 2 Güncellendi: Orada kaydedilmiş bir obje var mı?
        if (gridArray[x, y] != null) 
        {
            return false;
        }

        return true;
    }

    // --- YENİ: SATIR VE SÜTUN PATLATMA SİSTEMİ ---

    public void CheckLines()
    {
        // Tüm yatay satırları kontrol et
        for (int y = 0; y < height; y++)
        {
            if (IsRowFull(y)) ClearRow(y);
        }

        // Tüm dikey sütunları kontrol et
        for (int x = 0; x < width; x++)
        {
            if (IsColumnFull(x)) ClearColumn(x);
        }
    }

    private bool IsRowFull(int y)
    {
        for (int x = 0; x < width; x++)
        {
            if (gridArray[x, y] == null) return false; // Bir tane bile boşluk varsa dolmamıştır
        }
        return true;
    }

    private bool IsColumnFull(int x)
    {
        for (int y = 0; y < height; y++)
        {
            if (gridArray[x, y] == null) return false; // Bir tane bile boşluk varsa dolmamıştır
        }
        return true;
    }

    private void ClearRow(int y)
    {
        for (int x = 0; x < width; x++)
        {
            if (gridArray[x, y] != null)
            {
                if (gridArray[x, y].CompareTag("PuzzlePiece"))
                {
                    currentPieces++; // Parça sayısını artır
                    Debug.Log($"YAPBOZ TOPLANDI! {currentPieces}/{totalPiecesToWin}");
                    
                    // Barı doldur! (Matematiksel olarak yüzdesini hesaplar)
                    if (fillImage != null)
                    {
                        fillImage.fillAmount = (float)currentPieces / totalPiecesToWin;
                    }

                    // Oyunu kazanma kontrolü
                    if (currentPieces >= totalPiecesToWin)
                    {
                        Debug.Log("TEBRİKLER! BÖLÜMÜ GEÇTİN! 🏆");
                        // İleride buraya Kazandın ekranı açma kodu gelecek
                    }
                }

                Destroy(gridArray[x, y].gameObject);
                gridArray[x, y] = null;
            }
        }
    }

    private void ClearColumn(int x)
    {
        for (int y = 0; y < height; y++)
        {
            if (gridArray[x, y] != null)
            {
                if (gridArray[x, y].CompareTag("PuzzlePiece"))
                {
                    currentPieces++; // Parça sayısını artır
                    Debug.Log($"YAPBOZ TOPLANDI! {currentPieces}/{totalPiecesToWin}");
                    
                    // Barı doldur! (Matematiksel olarak yüzdesini hesaplar)
                    if (fillImage != null)
                    {
                        fillImage.fillAmount = (float)currentPieces / totalPiecesToWin;
                    }

                    // Oyunu kazanma kontrolü
                    if (currentPieces >= totalPiecesToWin)
                    {
                        Debug.Log("TEBRİKLER! BÖLÜMÜ GEÇTİN! 🏆");
                    }
                }

                Destroy(gridArray[x, y].gameObject);
                gridArray[x, y] = null;
            }
        }
    }
}