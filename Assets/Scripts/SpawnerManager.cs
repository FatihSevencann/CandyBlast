using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
    public List<GameObject> ObjectstoSpawn = new List<GameObject>();
    int gridX, gridY;
    [SerializeField]
    float gravity, spawnerYOffset;
    bool moving = false;
    Vector3 groundForSpawner;
    Rigidbody2D Rb;
    private void Awake()=>  Rb = GetComponent<Rigidbody2D>();
    private void Start()
    {
        gridY = (int)transform.position.y;
        gridX = (int)transform.position.x;
        groundForSpawner = transform.position + Vector3.up * spawnerYOffset;
        transform.position = groundForSpawner;
    }
    public void AddToSpawnList(GameObject tile) => ObjectstoSpawn.Add(tile);
    public void StartSpawning()
    {
        int TileAmount = ObjectstoSpawn.Count;
        if (TileAmount>0)
        {
            IncreaseSpawn(TileAmount);
            for (int i = 0; i < TileAmount; i++)
            {
                GameObject objects = ObjectstoSpawn[i];
                objects.SetActive(true);
                objects.GetComponent<TileManager>().ReuseTile(transform.position + new Vector3(0, i - TileAmount), new Vector2Int(gridX, gridY + i - TileAmount));
            }
            ObjectstoSpawn = new List<GameObject>();           
        }
    }

    public void IncreaseSpawn(int tile)
    {
        transform.position += Vector3.up * (tile);
        moving = true;
        Rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
    }
    private void FixedUpdate()
    {
        if (moving)
        {
            if (transform.position.y <= groundForSpawner.y)
            {
                Rb.constraints = RigidbodyConstraints2D.FreezeAll;
                moving = false;
                Rb.MovePosition(groundForSpawner);
            }
        }
    }
}
