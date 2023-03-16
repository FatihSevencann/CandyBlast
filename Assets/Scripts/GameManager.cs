using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    [Range(2, 10)]
    private int GridX, GridY;

    [SerializeField]
    [Range(1, 6)]
    private int ColorChoose;

    [SerializeField]
    private int imageATreshold, imageBTreshold, imageCTreshold;

    [SerializeField]
    public Slider slider;
    [SerializeField]
    TextMeshProUGUI scoreText, howLeftMove;
    int Level = 0;
    int isLeftMove,textPoint,scoreBar;
    [SerializeField]
    private GameObject GameOverPanel,GamePanel,NextLevel;
    private TileManager tile_Manager;


    [SerializeField]
    private GameObject tileManager, spawnPointPrefab, CameraManager;
    private GameObject[,] Tiles;
    List<GameObject> SpawnPoints;
    public static GameManager instance; //singleton
    private bool ShuffleCheck = false;
    int addPoint;

    private void Awake()
    {
         if (instance != null)
            Debug.LogWarning("More than one Tile Manager");
        else
            instance = this;

        howLeftMove.text = isLeftMove.ToString();
        textPoint = 0; scoreBar = 0;addPoint = 0;
    }
    private void Start()
    {
        CameraLocation();
        CreateSpawnManager();
        CreateTileSet();
        
        if (instance.CompareTag("Level1"))
        {
            slider.maxValue = 100;
            isLeftMove = 25;
            howLeftMove.text = isLeftMove.ToString();

        }
            
        
        else if (instance.CompareTag("Level2"))
        {
            slider.maxValue = 150;
            isLeftMove = 30;
            howLeftMove.text = isLeftMove.ToString();

        }
           
        else if (instance.CompareTag("Level3"))
        {
            slider.maxValue = 200;
            isLeftMove = 35;
            howLeftMove.text = isLeftMove.ToString();

        }
            


    }

    private void CameraLocation()=>CameraManager.transform.position = new Vector3((GridX-0.85f) / 2, (GridY+5f ) / 2 ,-15);

    
    private void CreateSpawnManager()
    {
        SpawnPoints = new List<GameObject>();
        for (int i = 0; i < GridX; i++)
            SpawnPoints.Add(Instantiate(spawnPointPrefab, new Vector3(i, GridY), Quaternion.identity));
    }

    private void CreateTileSet()
    {
        Tiles = new GameObject[GridX,GridY];

        for (int x = 0; x < GridX; x++)
        {
            for (int y = 0; y < GridY; y++)
            {
               // print("x:" + x + "y: " + y);
                GameObject tile = Instantiate(tileManager, new Vector3(x,y), Quaternion.identity);
                Vector2Int tileLocation = new Vector2Int(x,y);
                tile.GetComponent<TileManager>().SetTile(ColorChoose,tileLocation);

                Tiles[x, y] = tile;
                tile.SetActive(true);
            }

        }
        CreateGroups();
    }

    private void CreateGroups()
    {
       ShuffleCheck = true;
        for (int x = 0; x < GridX; x++)
        {
            for (int y = 0; y < GridY; y++)
            {
                if (Tiles[x, y].GetComponent<TileManager>().isGroup == false)
                {
                   

                     CheckForNeighbours(x, y, Tiles[x, y].GetComponent<TileManager>().Color);
                }
            }

        }
        if (ShuffleCheck)
        {
            print("Shuffle Time");
            Shuffle();

        }
           

    }
    private void Shuffle()
    {
        for(int x=0; x < GridX; x++)
        {
            for(int y=0; y < GridY; y++)
            {
                Tiles[x, y].GetComponent<TileManager>().ChangeColorRandom();
                             
            }

        }
        CreateGroups();

    }



    private void CheckForNeighbours(int x, int y, int color)
    {
        List<GameObject> matchNeighbours = new List<GameObject>();
        List<GameObject> nextChecks = new List<GameObject>();


    //left check
    nextChecks:
        if (x > 0 && Tiles[x - 1, y].GetComponent<TileManager>().isGroup == false && Tiles[x - 1, y].GetComponent<TileManager>().Color == color)
        {
            matchNeighbours.Add(Tiles[x - 1, y]);
            nextChecks.Add(Tiles[x - 1, y]);
            Tiles[x - 1, y].GetComponent<TileManager>().isGroup = true;
        }
        //right check

        if (x < GridX - 1 && Tiles[x + 1, y].GetComponent<TileManager>().isGroup == false && Tiles[x + 1, y].GetComponent<TileManager>().Color == color)
        {
            matchNeighbours.Add(Tiles[x + 1, y]);
            nextChecks.Add(Tiles[x + 1, y]);
            Tiles[x + 1, y].GetComponent<TileManager>().isGroup = true;

        }

        //up check
        if (y < GridY - 1 && Tiles[x, y + 1].GetComponent<TileManager>().isGroup == false && Tiles[x, y + 1].GetComponent<TileManager>().Color == color)
        {
            matchNeighbours.Add(Tiles[x, y + 1]);
            nextChecks.Add(Tiles[x, y + 1]);
            Tiles[x, y + 1].GetComponent<TileManager>().isGroup = true;

        }

        //downcheck
        if (y > 0 && Tiles[x, y - 1].GetComponent<TileManager>().isGroup == false && Tiles[x, y - 1].GetComponent<TileManager>().Color == color)
        {
            matchNeighbours.Add(Tiles[x, y - 1]);
            nextChecks.Add(Tiles[x, y - 1]);
            Tiles[x, y - 1].GetComponent<TileManager>().isGroup = true;
        }

        if (matchNeighbours.Count > 0 && Tiles[x, y].GetComponent<TileManager>().isGroup == false)
        {
            matchNeighbours.Add(Tiles[x, y]);
            Tiles[x, y].GetComponent<TileManager>().isGroup = true;

        }

        if (Tiles[x, y].GetComponent<TileManager>().isGroup == false)
        {
            Tiles[x, y].GetComponent<TileManager>().UpdateSprite(0);
        }

        if (nextChecks.Count > 0)
        {
            x = nextChecks[0].GetComponent<TileManager>().index.x;
            y = nextChecks[0].GetComponent<TileManager>().index.y;
            nextChecks.RemoveAt(0);
            goto nextChecks;
        }
        else
        {
            if (matchNeighbours.Count > 0)
                ShuffleCheck = false;
            ChangeTileSprites(matchNeighbours);
        }
    }
    private void ChangeTileSprites(List<GameObject> tileGroup)
    {
        int count = tileGroup.Count;
        int variable;

        if (count > imageCTreshold)
            variable = 3;
        else if (count > imageBTreshold)
            variable = 2;
        else if (count > imageATreshold)
            variable = 1;
        else
            variable = 0;

        for (int i = 0; i < count; i++)
        {
            tileGroup[i].GetComponent<TileManager>().UpdateSprite(variable);
        }
        
    }
    private void FindDestroyObjects(int x, int y, int color)
    {
        List<GameObject> matchNeighbours = new List<GameObject>();
        List<GameObject> nextChecks = new List<GameObject>();

    Nextcheck2:
        //left check
        if (x > 0 && Tiles[x - 1, y].GetComponent<TileManager>().isDestroy == false && Tiles[x - 1, y].GetComponent<TileManager>().Color == color)
        {
            matchNeighbours.Add(Tiles[x - 1, y]);
            nextChecks.Add(Tiles[x - 1, y]);
            Tiles[x - 1, y].GetComponent<TileManager>().isDestroy = true;
        }
        //right check

        if (x < GridX - 1 && Tiles[x + 1, y].GetComponent<TileManager>().isDestroy == false && Tiles[x + 1, y].GetComponent<TileManager>().Color == color)
        {
            matchNeighbours.Add(Tiles[x + 1, y]);
            nextChecks.Add(Tiles[x + 1, y]);
            Tiles[x + 1, y].GetComponent<TileManager>().isDestroy = true;

        }

        //up check
        if (y < GridY - 1 && Tiles[x, y + 1].GetComponent<TileManager>().isDestroy == false && Tiles[x, y + 1].GetComponent<TileManager>().Color == color)
        {
            matchNeighbours.Add(Tiles[x, y + 1]);
            nextChecks.Add(Tiles[x, y + 1]);
            Tiles[x, y + 1].GetComponent<TileManager>().isDestroy = true;

        }

        //downcheck
        if (y > 0 && Tiles[x, y - 1].GetComponent<TileManager>().isDestroy == false && Tiles[x, y - 1].GetComponent<TileManager>().Color == color)
        {
            matchNeighbours.Add(Tiles[x, y - 1]);
            nextChecks.Add(Tiles[x, y - 1]);
            Tiles[x, y - 1].GetComponent<TileManager>().isDestroy = true;
        }

        if (matchNeighbours.Count > 0 && Tiles[x, y].GetComponent<TileManager>().isDestroy == false)
        {
            matchNeighbours.Add(Tiles[x, y]);

            Tiles[x, y].GetComponent<TileManager>().isDestroy = true;
            isLeftMove--;

        }

        if (nextChecks.Count > 0)
        {
            x = nextChecks[0].GetComponent<TileManager>().index.x;
            y = nextChecks[0].GetComponent<TileManager>().index.y;
            nextChecks.RemoveAt(0);
            goto Nextcheck2;
        }
        else
        {
           

            foreach (GameObject gameObject in matchNeighbours)
            {
                Vector2Int destroyIndex = gameObject.GetComponent<TileManager>().index;
                for (int yy = destroyIndex.y; yy < GridY - 1; yy++)
                {

                    Tiles[destroyIndex.x, yy + 1].GetComponent<TileManager>().MoveTileDown();
                }
            }
        

            foreach (GameObject gameObject in matchNeighbours)
            {
                DestroyTile(gameObject);
            }

            foreach (GameObject gameObject in Tiles)
            {
                gameObject.GetComponent<TileManager>().StartMovingTile();

            }


            SpawnTiles();
            RefreshTile();

            addPoint = matchNeighbours.Count;
            


            if (instance.CompareTag("Level1") )
            {
                if(slider.value < 100)
                {
                    if (addPoint < 5)
                    {
                        textPoint += addPoint;
                        scoreText.text = textPoint.ToString();
                        slider.value += addPoint;
                    }

                    else if (addPoint >= 5 && addPoint < 7)
                    {
                        textPoint += addPoint * 2;
                        scoreText.text = textPoint.ToString();
                        slider.value += addPoint * 2;
                    }
                    else if (addPoint >= 7 && addPoint < 9)
                    {
                        textPoint += addPoint * 4;
                        scoreText.text = textPoint.ToString();
                        slider.value += addPoint * 4;
                    }
                    else
                    {
                        textPoint += addPoint * 6;
                        scoreText.text = textPoint.ToString();
                        slider.value += addPoint * 6;

                    }

                }
                else
                {
                    GamePanel.SetActive(false);
                    NextLevel.SetActive(true);
                }
            }

            if (instance.CompareTag("Level2") )
            {
                if(slider.value < 150)
                {
                    if (addPoint < 5)
                    {
                        textPoint += addPoint;
                        scoreText.text = textPoint.ToString();
                        slider.value += addPoint;
                    }

                    else if (addPoint >= 5 && addPoint < 7)
                    {
                        textPoint += addPoint * 2;
                        scoreText.text = textPoint.ToString();
                        slider.value += addPoint*2;
                    }
                    else if (addPoint >= 7 && addPoint < 9)
                    {
                        textPoint += addPoint * 4;
                        scoreText.text = textPoint.ToString();
                        slider.value += addPoint*4;
                    }
                    else
                    {
                        textPoint += addPoint * 6;
                        scoreText.text = textPoint.ToString();
                        slider.value += addPoint*6;

                    }

                }
                else
                {
                    GamePanel.SetActive(false);
                    NextLevel.SetActive(true);

                }
                
            }


            if (instance.CompareTag("Level3") )
            {
                if (slider.value < 200)
                {
                    if (addPoint < 5)
                    {
                        textPoint += addPoint;
                        scoreText.text = textPoint.ToString();
                        slider.value += addPoint;
                    }

                    else if (addPoint >= 5 && addPoint < 8)
                    {
                        textPoint += addPoint * 2;
                        scoreText.text = textPoint.ToString();
                        slider.value += addPoint*2;
                    }
                    else if (addPoint >= 8 && addPoint < 10)
                    {
                        textPoint += addPoint * 4;
                        scoreText.text = textPoint.ToString();
                        slider.value += addPoint*4;
                    }
                    else
                    {
                        textPoint += addPoint * 6;
                        scoreText.text = textPoint.ToString();
                        slider.value += addPoint*6;

                    }

                }
                else
                {
                    GamePanel.SetActive(false);
                    NextLevel.SetActive(true);
                }
               
            }
          
        }


    }
    public void CheckForClick(Vector2Int location, int color)
    {
        FindDestroyObjects(location.x, location.y, color);
        howLeftMove.text = isLeftMove.ToString();
        if (isLeftMove <= 0)
        {
            GamePanel.SetActive(false);
            GameOverPanel.SetActive(true);

        }

    }

  

    public void AssingTiles(GameObject Tile, Vector2Int index)
    {
        Tiles[index.x, index.y] = Tile;
    }

    public void DestroyTile(GameObject tile)
    {
        tileManager.SetActive(false);
        SpawnPoints[tile.GetComponent<TileManager>().index.x].GetComponent<SpawnerManager>().AddToSpawnList(tile);

    }

    public void SpawnTiles()
    {
        for (int i = 0; i < GridX; i++)
        {

            SpawnPoints[i].GetComponent<SpawnerManager>().StartSpawning();
        }
    }

    public void RefreshTile()
    {
        for (int i = 0; i < GridX; i++)
        {
            for (int j = 0; j < GridY; j++)
            {
                Tiles[i, j].GetComponent<TileManager>().isGroup = false;
 
            }
        }
        CreateGroups();
    }

}
