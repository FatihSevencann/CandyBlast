using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TileManager : MonoBehaviour
{
    public Vector2Int index;
    public int Color;
    public bool isGroup = false, isDestroy = false;
    [SerializeField]
    Sprite[] tileSprites;
    public  int moveDownLenght = 0, maxColor;
    private bool moving = false;
    private Vector3 targetPosition;
    private Rigidbody2D Rb;
    
    private void Awake()=> Rb = GetComponent<Rigidbody2D>();
    
    public void SetTile(int colorChoose, Vector2Int newLocation)
    {
        index = newLocation;
        maxColor =colorChoose;
        ChangeColorRandom();
        targetPosition = transform.position;

    }
    public void ChangeColorRandom()
    {
        Color = Random.Range(0, maxColor);
        UpdateSprite(0);
    }
    public void UpdateSprite(int group)=>GetComponent<SpriteRenderer>().sprite = tileSprites[Color + group * maxColor];
    public void SetGroup(bool status)=> isGroup = status;
    public void OnMouseDown()=>  GameManager.instance.CheckForClick(index, Color); //tiklanan objeyi aktif eden metod
    

    public void MoveTileDown()
    {
        targetPosition += Vector3.down;
        moveDownLenght++;
    }
    public void StartMovingTile()
    {
        index += Vector2Int.down * moveDownLenght;
       GameManager.instance.AssingTiles(gameObject, index);
        moveDownLenght = 0;
        moving = true;
        Rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
    }

    private void FixedUpdate()
    {
        if (moving)
        {
            if (transform.position.y <= targetPosition.y)
            {
                Rb.constraints = RigidbodyConstraints2D.FreezeAll;
                moving = false;
                transform.position = targetPosition;
            }
        }
    }

    public void ReuseTile(Vector3 newPos, Vector2Int targetPos)
    {
        isDestroy = false;
        moveDownLenght = 0;
        transform.position = newPos;
        index = targetPos;
        targetPosition = new Vector3(targetPos.x, targetPos.y);
        ChangeColorRandom();
        StartMovingTile();
    }

}
