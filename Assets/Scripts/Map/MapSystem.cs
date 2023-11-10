using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MapSystem : MonoBehaviour
{
    public static MapSystem instance;

    [SerializeField] GameObject tileParents;

    public GameObject playerPrefab; //�÷��̾� ������ ����
    public GameObject tilePrefab;

    public static bool moveCardDraw; // ī�� ��ο� ���� ���� 

    private Vector2 playerPosition; //�÷��̾� ��ġ(x,y)
    private Vector2 startPoint; //��������
    private Vector2 endPoint; //��������(���� Ÿ��)

    public List<MapTile> tileMap = new List<MapTile>();

    GameObject player;
    public static int tileCount = 1;

    /*
    Transform stpos;
    Transform endpos;
    Rigidbody playerRb;
    */

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    void Start()
    {
        setupMap(); //��ŸƮ���� ����
    }

    //Ÿ�� ���� �� �÷��̾� ����
    void setupMap()
    {
        moveCardDraw = true;
        for (int i = 0; i<20; i++)
        {
            #region �� ������ ���� ����
            var tile = Instantiate(tilePrefab).GetComponent<MapTile>();
            tile.transform.parent = tileParents.transform;
            tile.SetTile( DataManager.instance.AllTileDatas // ������ Ÿ�� ������ ����
                [ DataManager.instance.AllTileList[ Random.Range(0, DataManager.instance.AllTileList.Count) ]]);

            #endregion

            #region �� ������Ʈ ����
            if (i == 0) tile.transform.position = new Vector3(-5, -3, 0);
            else
            {
                var lastTilePosition = tileMap[tileMap.Count-1].transform.position;
                tile.transform.position = new Vector3( lastTilePosition.x + 3.5f, lastTilePosition.y + 2.5f);
            }
            tileMap.Add(tile);
            #endregion
        }

        //�÷��̾� ����
        player = Instantiate(playerPrefab, tileMap[0].transform.position, tileMap[0].transform.rotation); 
        if (!player.activeSelf)
        {
            player.SetActive(true);
        }
   
    }
    //�÷��̾� �̵�
    public void PlayerMove()
    {
        player.transform.DOMoveX(tileMap[tileCount].transform.position.x, 1);
        player.transform.DOMoveY(tileMap[tileCount].transform.position.y, 1).SetEase(Ease.InOutBack).OnComplete(()=>EndPlayerMove());

        /*
        playerRb = player.GetComponent<Rigidbody>();
        stpos = player.transform; //�÷��̾� ��ġ
        endpos = tileMap[tileCount].transform; //�̵��� Ÿ�� ��ġ
        Vector3 topPos = stpos.position + ((endpos.position - stpos.position) / 2); // �÷��̾�, Ÿ�� �߰� ��ġ
        Vector3[] JumpPath ={new Vector3(stpos.position.x,stpos.position.y,stpos.position.z),
        new Vector3(topPos.x,topPos.y+1.5f,topPos.z),
        new Vector3(endpos.position.x,endpos.position.y,endpos.position.z) };
        //�̵� ���(topPos.y + ������ ���� ���� ����)
        playerRb.DOPath(JumpPath, 1.5f, PathType.CatmullRom, PathMode.TopDown2D).SetEase(Ease.InCubic);
        */
    }

    void EndPlayerMove()
    {
        tileMap[tileCount].TileEffect();
    }


    //�÷��̾� ��ġ ����
    void SetPlayerPosition()
    {
        playerPosition = playerPrefab.transform.position;
    }

}
