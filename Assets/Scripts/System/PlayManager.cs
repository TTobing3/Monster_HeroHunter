using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData 
{
    public static int maxHP;
    public static int currentHP;

    public static List<BattleCardData> playerBattleCardDeck = new List<BattleCardData>();
    public static List<LostItem> playerLostItems = new List<LostItem>();

    public static int diceCount = 2;


    public static bool CheckLostItem(string _name)
    {
        return playerLostItems.Contains(DataManager.instance.AllLostItemDatas[_name]);
    }

    public static void GainCard(string _name)
    {
        PlayerData.playerBattleCardDeck.Add(DataManager.instance.AllBattleCardDatas[_name]);
    }

    public static void DeleteCard(string _name)
    {
        PlayerData.playerBattleCardDeck.Remove(DataManager.instance.AllBattleCardDatas[_name]);
    }

    public static void GainLostItem(string _lostItem)
    {
        playerLostItems.Add(DataManager.instance.AllLostItemDatas[ _lostItem]);
    }

    public static void GainDice(int _count)
    {
        diceCount += _count;
    }
}

public class PlayManager : MonoBehaviour
{
    public static PlayManager instance;
    public PlayerData playerData;

    public TileData curTile;
    public int curTileNum = 0;

    public List<TileData> tileMapData = new List<TileData>();

    public bool isStone=false;

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

        for(int i = 0; i<10; i++) // 임시
        {
            PlayerData.GainCard("갈라치기"); // PlayerData.playerBattleCardDeck.Add(DataManager.instance.AllBattleCardDatas["갈라치기"]);
            PlayerData.GainCard("속사");
        }

        PlayerData.GainLostItem("독성 발톱");
    }

}