using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSystem : MonoBehaviour
{ 
    private MapTile mapTile;

    public GameObject playerPrefab; //�÷��̾� ������ ����
    public GameObject StartTile; //���� Ÿ��

    private Vector2 playerPosition; //�÷��̾� ��ġ(x,y)
    private Vector2 startPoint; //��������
    private Vector2 endPoint; //��������(���� Ÿ��)
    
    void Start()
    {
        mapTile = GetComponent<MapTile>();
        setupMap();
    }

    void Update()
    {

    }
    //Ÿ�� ���� �� �÷��̾� ����
    void setupMap()
    {
        //���� Ÿ����ġ ����
        Transform startTileForm = StartTile.transform;
        //�÷��̾� ����
        GameObject player = Instantiate(playerPrefab); //,startTileForm);
        if (!player.activeSelf)
        {
            player.SetActive(true);
        }

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
