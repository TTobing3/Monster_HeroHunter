using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerData  // �÷��̾� ���� ��Ȳ ����
{
    //���� ü��, ��ȣ��, ���� ���� ī��, ���� �̵� ī��, ���� ���ǹ�, �ֻ���

    public static int maxHP;
    public static int currentHP;
}

public class PlayManager : MonoBehaviour
{
    public static PlayManager instance;

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
        PlayerData.maxHP = 100;
        PlayerData.currentHP = PlayerData.maxHP;
    }

}