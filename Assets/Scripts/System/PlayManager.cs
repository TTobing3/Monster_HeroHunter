using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerData // �÷��̾� ���� ��Ȳ ����
{
    //���� ü��, ��ȣ��, ���� ���� ī��, ���� �̵� ī��, ���� ���ǹ�, �ֻ���

    public int maxHP;
    public int currentHP;
}

public class PlayManager : MonoBehaviour
{
    public static PlayManager instance;

    public PlayerData playerData;
    public TileData curTile;
    public int curTileNum = 0;

    public List<TileData> tileMapData = new List<TileData>();

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }


    void Start()
    {
        SetStart();
    }

    void SetStart()
    {
        playerData = new PlayerData();

        playerData.maxHP = 100;
        playerData.currentHP = playerData.maxHP;
    }

}