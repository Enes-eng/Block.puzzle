using UnityEngine;
using System.Collections.Generic; 

public class BlockSpawner : MonoBehaviour
{
    public static BlockSpawner Instance; 
    private int usedBlocksCount = 0; 

    [SerializeField] private GameObject[] blockPrefabs;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private int gridWidth = 8;

    private float[] slotCenters = new float[] { 1f, 3.5f, 6f };
    private float slotMaxWidth = 2.5f;

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
        List<GameObject> currentBlocks = new List<GameObject>(); 

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

            // Blok tamamen hazırlandıktan sonra listeye ekliyoruz
            currentBlocks.Add(newBlock); 
        } // FOR DÖNGÜSÜ BURADA BİTİYOR

        // 3 blok da sahnede doğduktan SONRA yapbozu aralarından birine gizliyoruz
        AddPuzzlePieceToRandomBlock(currentBlocks); 
    }

    // Blok ızgaraya yerleştiğinde bu fonksiyon çalışacak
    public void BlockPlaced()
    {
        usedBlocksCount++; 

        if (usedBlocksCount >= 3)
        {
            SpawnBlocks(); 
        }
    }

    // 3 blok arasından rastgele birine yapboz ikonu ekler
    private void AddPuzzlePieceToRandomBlock(List<GameObject> blocks)
    {
        int luckyBlockIndex = Random.Range(0, blocks.Count);
        GameObject luckyBlock = blocks[luckyBlockIndex];

        int childCount = luckyBlock.transform.childCount;
        if (childCount > 0)
        {
            int luckyTileIndex = Random.Range(0, childCount);
            Transform luckyTile = luckyBlock.transform.GetChild(luckyTileIndex);

            if (luckyTile.childCount > 0)
            {
                Transform puzzleIcon = luckyTile.GetChild(0); 
                SpriteRenderer sr = puzzleIcon.GetComponent<SpriteRenderer>();
                
                if (sr != null)
                {
                    sr.enabled = true; 
                    luckyTile.tag = "PuzzlePiece"; 
                }
            }
        }
    }
}