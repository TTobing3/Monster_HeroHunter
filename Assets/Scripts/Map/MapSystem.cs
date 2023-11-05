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
    Transform stpos;
    Transform endpos;
    Rigidbody playerRb;
    public static int tileCount = 1;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    void Start()
    {
        setupMap();
    }

    //Ÿ�� ���� �� �÷��̾� ����
    void setupMap()
    {
        moveCardDraw = true;
        for (int i = 0; i<20; i++)
        {
            var tile = Instantiate(tilePrefab).GetComponent<MapTile>();
            tile.transform.parent = tileParents.transform;
            tile.SetTile( DataManager.instance.AllTileDatas // ������ Ÿ�� ������ ����
                [ DataManager.instance.AllTileList[ Random.Range(0, DataManager.instance.AllTileList.Count) ]]);

            if (i == 0) tile.transform.position = new Vector3(-5, -3, 0);
            else
            {
                var lastTilePosition = tileMap[tileMap.Count-1].transform.position;
                tile.transform.position = new Vector3(
                    lastTilePosition.x + 3, //+ Random.Range(-1, 1f)
                    lastTilePosition.y + 2); //+ Random.Range(-1, 1f));
                //tile.transform.Rotate(new Vector3(0, 0, Random.Range(-5, 5f)));
                //�ϴ� Ÿ�� ��ġ�� �����ϰ� ��
            }

            tileMap.Add(tile);
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
        playerRb = player.GetComponent<Rigidbody>();
        stpos = player.transform; //�÷��̾� ��ġ
        endpos = tileMap[tileCount].transform; //�̵��� Ÿ�� ��ġ
        Vector3 topPos = stpos.position + ((endpos.position - stpos.position) / 2); // �÷��̾�, Ÿ�� �߰� ��ġ
        Vector3[] JumpPath ={new Vector3(stpos.position.x,stpos.position.y,stpos.position.z),
        new Vector3(topPos.x,topPos.y+1.5f,topPos.z),
        new Vector3(endpos.position.x,endpos.position.y,endpos.position.z) };
        //�̵� ���(topPos.y + ������ ���� ���� ����)
        playerRb.DOPath(JumpPath, 1.5f, PathType.CatmullRom, PathMode.TopDown2D);

    }
    //�÷��̾� ��ġ ����
    void SetPlayerPosition()
    {
        playerPosition = playerPrefab.transform.position;
    }

}
