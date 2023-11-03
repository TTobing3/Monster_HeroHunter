using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSystem : MonoBehaviour
{
    [SerializeField] GameObject tileParents;

    public GameObject playerPrefab; //�÷��̾� ������ ����
    public GameObject tilePrefab;

    public static bool moveCardDraw; // ī�� ��ο� ���� ����

    private Vector2 playerPosition; //�÷��̾� ��ġ(x,y)
    private Vector2 startPoint; //��������
    private Vector2 endPoint; //��������(���� Ÿ��)

    public List<MapTile> tileMap = new List<MapTile>(); 
    
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

        /*
        //�÷��̾� ����
        GameObject player = Instantiate(playerPrefab); //
        if (!player.activeSelf)
        {
            player.SetActive(true);
        }
        */
    }
    //�÷��̾� �̵�
    void PlayerMove()
    {
        //�̵�ī�� ȿ���� ���� �̵�
    }
    //�÷��̾� ��ġ ����
    void SetPlayerPosition()
    {
        playerPosition = playerPrefab.transform.position;
    }

}
