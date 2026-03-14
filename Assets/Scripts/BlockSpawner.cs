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

    public bool yapbozCiksinMi = true;

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

            float spawnX = slotCenters[i] - (blockWidth / 2f) - minX;

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

            currentBlocks.Add(newBlock); 
        } 

        AddPuzzlePieceToRandomBlock(currentBlocks); 
    }

    public void BlockPlaced()
    {
        usedBlocksCount++; 

        if (usedBlocksCount >= 3)
        {
            SpawnBlocks(); 
        }
    }

    private void AddPuzzlePieceToRandomBlock(List<GameObject> blocks)
    {
        if (yapbozCiksinMi == false) return;
        
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