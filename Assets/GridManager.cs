using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private int width = 8;
    [SerializeField] private int height = 8;
    [SerializeField] private GameObject tilePrefab; // Ekranda belirecek olan tek bir kare görseli

    void Start()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // Instantiate komutu, verdiğimiz objeden (tilePrefab) yeni bir kopya oluşturur (spawnlar)
                GameObject spawnedTile = Instantiate(tilePrefab, new Vector3(x, y, 0), Quaternion.identity);
                
                // Unity hiyerarşisinde isimleri düzgün görünsün diye koordinatlarını ismine yazdırıyoruz (Örn: Tile 2 3)
                spawnedTile.name = $"Tile {x} {y}";
                
                // Ortalık karışmasın diye oluşturulan tüm kareleri GridManager'ın içine (child olarak) atıyoruz
                spawnedTile.transform.parent = transform;
            }
        }

        // Kamerayı otomatik olarak oluşturduğumuz 8x8 ızgaranın tam ortasına odaklar
        Camera.main.transform.position = new Vector3((float)width / 2 - 0.5f, (float)height / 2 - 0.5f, -10);
    }
}
