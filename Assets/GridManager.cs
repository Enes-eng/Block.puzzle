using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private int width = 8;
    [SerializeField] private int height = 8;
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private Color tileColor; 

    void Start()
    {
        GenerateGrid();
        AdjustCamera(); // Kamerayı ekrana göre ayarlayan yeni fonksiyonumuz
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
        // 1. Zoom Ayarı: Kameranın görüş alanını genişletiyoruz ki 8 sütun tam sığsın
        Camera.main.orthographicSize = 8f; 

        // 2. Pozisyon Ayarı: Kamerayı X ekseninde tam ortaya, Y ekseninde ise biraz aşağıya (-2f) alıyoruz.
        // Kamera aşağı inince, ızgaramız ekranda otomatik olarak yukarı kaymış olacak.
        Camera.main.transform.position = new Vector3((float)width / 2 - 0.5f, (float)height / 2 - 0.5f - 2.5f, -10);
    }
}