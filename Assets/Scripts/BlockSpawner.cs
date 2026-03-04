using UnityEngine;

public class BlockSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] blockPrefabs;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private int gridWidth = 8;

    // Her slot'un merkez X'i (ızgara 0-7 arası, 3 eşit bölge)
    private float[] slotCenters = new float[] { 1f, 3.5f, 6f };
    // Her slot'un max genişliği (taşmayı önler)
    private float slotMaxWidth = 2.5f;
    public static BlockSpawner Instance; // Diğer kodlardan ulaşmak için köprü
    private int usedBlocksCount = 0; // Yerleşen blok sayacı

    void Awake()
    {
        Instance = this;
    }
    
    void Start()
    {
        SpawnBlocks();
    }

    public void SpawnBlocks()
    {
        usedBlocksCount = 0;
        
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            int randomIndex = Random.Range(0, blockPrefabs.Length);

            GameObject newBlock = Instantiate(
                blockPrefabs[randomIndex],
                Vector3.zero,
                Quaternion.identity
            );
            newBlock.transform.localScale = new Vector3(0.6f, 0.6f, 1f);

            // Bloğun gerçek sınırlarını hesapla
            float minX = float.MaxValue;
            float maxX = float.MinValue;
            float minY = float.MaxValue;

            foreach (Transform child in newBlock.transform)
            {
                if (child.position.x < minX) minX = child.position.x;
                if (child.position.x > maxX) maxX = child.position.x;
                if (child.position.y < minY) minY = child.position.y;
            }

            float blockWidth = (maxX - minX);
            
            // Bloğu slot'un ortasına hizala
            float spawnX = slotCenters[i] - (blockWidth / 2f) - minX;
            
            // Izgara sınırlarını aşmasın
            float rightEdge = spawnX + minX + blockWidth;
            if (rightEdge > gridWidth - 0.5f)
                spawnX -= (rightEdge - (gridWidth - 0.5f));
            if (spawnX + minX < 0)
                spawnX -= (spawnX + minX);

            Vector3 finalPos = new Vector3(
                spawnX,
                spawnPoints[i].position.y - minY,
                0
            );

            newBlock.transform.position = finalPos;
        }
    }

    // Blok ızgaraya yerleştiğinde bu fonksiyon çalışacak
    public void BlockPlaced()
    {
        usedBlocksCount++; // Yerleşen blok sayısını 1 artır

        // Eğer 3 blok da yerleşip bittiyse yenilerini getir!
        if (usedBlocksCount >= 3)
        {
            SpawnBlocks(); 
        }
    }
}