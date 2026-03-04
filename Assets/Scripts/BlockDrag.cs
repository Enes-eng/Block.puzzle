using UnityEngine;
using System.Collections.Generic; // <--- YENİ EKLENDİ (Liste kullanmak için)

public class BlockDrag : MonoBehaviour
{
    private Vector3 startPosition;
    private Vector3 offset;

    // YENİ: Bloğun ızgaraya yerleşip yerleşmediğini tutacak kilit (Flag)
    private bool isPlaced = false; 

    // 1. Fareyle bloğa İLK TIKLADIĞIMIZ an çalışır
    void OnMouseDown()
    {
        // KURAL 3 ÇÖZÜMÜ: Eğer blok zaten yerleştiyse, tekrar tutulamaz!
        if (isPlaced) return;

        // Bloğun yerini (aşağıdaki doğduğu noktayı) hafızaya alıyoruz!
        startPosition = transform.position;

        // KURAL 1 ÇÖZÜMÜ: Tıklandığı an bloğu hemen tam boyuta (1x1) çıkarıyoruz!
        transform.localScale = Vector3.one; // new Vector3(1f, 1f, 1f) demek.

        // Farenin ucu ile bloğun merkezi arasındaki mesafeyi hesapla
        offset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    // 2. Fareyi BASILI TUTUP SÜRÜKLEDİĞİMİZ sürece çalışır
    void OnMouseDrag()
    {
        // Zaten yerleştiyse sürüklenemez
        if (isPlaced) return;

        Vector3 newPos = Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset;
        newPos.z = 0; // Derinlik olmasın
        transform.position = newPos;
    }

    // 3. Fareyi BIRAKTIĞIMIZ an çalışır
    void OnMouseUp()
    {
        // Zaten yerleştiyse işlem yapmaya gerek yok
        if (isPlaced) return;

        // Hedef noktayı tam sayılara yuvarla
        float snapX = Mathf.Round(transform.position.x);
        float snapY = Mathf.Round(transform.position.y);
        Vector3 targetPos = new Vector3(snapX, snapY, 0f);

        // Bloğu geçici olarak bu hedefe oturt (karelerin yerini ölçmek için)
        transform.position = targetPos;

        bool canPlace = true;

        // Geçerli olursa GridManager'a göndermek için kapladığı koordinatların geçici listesi
        List<Vector2Int> tempOccupiedCells = new List<Vector2Int>();

        // Bloğun içindeki küçük kırmızı kareleri tek tek kontrol et
        foreach (Transform child in transform)
        {
            int x = Mathf.RoundToInt(child.position.x);
            int y = Mathf.RoundToInt(child.position.y);

            // GridManager'daki kuralı soruyoruz: İçerisi boş mu? Sınırların içinde mi?
            if (GridManager.Instance.IsValidPosition(x, y) == false)
            {
                canPlace = false; // Hamleyi iptal et!
                break;
            }
            // Eğer burası uygunsa listeye ekle
            tempOccupiedCells.Add(new Vector2Int(x, y));
        }

        // Testin Sonucu
       // Testin Sonucu
        if (canPlace == true)
        {
            // YENİ: Bloğun içindeki her bir küçük kareyi GridManager'ın hafızasına "Obje" olarak kaydediyoruz
            foreach (Transform child in transform)
            {
                int x = Mathf.RoundToInt(child.position.x);
                int y = Mathf.RoundToInt(child.position.y);
                GridManager.Instance.gridArray[x, y] = child; // O kordinata "bu kare yerleşti" diyoruz
            }

            isPlaced = true; 
            BlockSpawner.Instance.BlockPlaced(); // Spawner'a haber ver (zaten yapmıştın)

            // YENİ: Blok yerleştiği anda GridManager'a bağırıyoruz: "Tahtayı kontrol et, dolan satır var mı?"
            GridManager.Instance.CheckLines();
        }
        else
        {
            // Hamle GEÇERSİZ: Blok hafızaya aldığımız o ilk yerine (aşağıya) geri döner
            transform.position = startPosition;
            transform.localScale = new Vector3(0.6f, 0.6f, 1f); // Doğuş boyutuna geri dön
        }
    }
}