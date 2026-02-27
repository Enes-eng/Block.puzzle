using UnityEngine;

public class BlockSpawner : MonoBehaviour
{
    
    [SerializeField] private GameObject[] blockPrefabs; 
    
    
    [SerializeField] private Transform[] spawnPoints;   

    void Start()
    {
        SpawnBlocks();
    }

    public void SpawnBlocks()
    {
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            int randomIndex = Random.Range(0, blockPrefabs.Length);
            
            GameObject newBlock = Instantiate(blockPrefabs[randomIndex], spawnPoints[i].position, Quaternion.identity);
        
            newBlock.transform.localScale = new Vector3(0.6f, 0.6f, 1f);
        }
    }
}