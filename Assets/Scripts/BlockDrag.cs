using UnityEngine;

public class BlockDrag : MonoBehaviour
{
    private Vector3 startPosition; // Şeklin aşağıda beklediği orijinal yeri
    private Vector3 offset;        // Fareyle şeklin merkezi arasındaki tutma payı

    // Fareyle objenin üzerine tıklandığı ilk an çalışır
    void OnMouseDown()
    {
        // Yanlış bir yere bırakırsak geri dönsün diye ilk yerini hafızaya alıyoruz
        startPosition = transform.position;

        // Farenin ucu tam bloğun neresinden tuttuysa orayı hesaplıyoruz
        offset = transform.position - GetMouseWorldPos();

        // Bloğu tuttuğumuzda orijinal oyunlardaki gibi %100 boyutuna (büyük haline) getiriyoruz
        transform.localScale = new Vector3(1f, 1f, 1f);
    }

    // Fareye basılı tutup sürüklediğimiz sürece çalışır
    void OnMouseDrag()
    {
        // Şeklin pozisyonunu, farenin pozisyonuna eşitliyoruz (peşimizden geliyor)
        transform.position = GetMouseWorldPos() + offset;
    }

    // Fare tıklamasını bıraktığımız an çalışır
    void OnMouseUp()
    {
        // ŞİMDİLİK: Sadece bıraktığımızda eski yerine ve ufak boyutuna geri dönsün.
        // (Izgaraya yapışma algoritmasını bir sonraki adımda yazacağız!)
        transform.position = startPosition;
        transform.localScale = new Vector3(0.6f, 0.6f, 1f);
    }

    // Farenin ekrandaki piksel konumunu, oyun dünyası koordinatlarına çeviren fonksiyon
    private Vector3 GetMouseWorldPos()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = -Camera.main.transform.position.z; 
        return Camera.main.ScreenToWorldPoint(mousePos);
    }
}