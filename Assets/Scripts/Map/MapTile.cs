using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum TILETYPE { StartTile, GoblinTile, TombTile, LuckTile, BossTile } //Ÿ�� Ÿ�� �߰�
public class MapTile : MonoBehaviour
{
    public TileData tiledata;

    public TILETYPE tileType; 
    public GameObject EnemyPrefab; //�� ������ ����
    public Text tileName; //Ÿ���̸� Text
    public Button startButton; //��Ʋ���� ��ư

    private BattleSystem battleSystem; 
    
    void Start()
    {
        battleSystem = GetComponent<BattleSystem>();
        
    }
    //Ÿ���̸� ����, �̸��� ���� �� ����
    public void SetTileName()
    {
        switch (tileType)
        {
            case TILETYPE.GoblinTile:
                tileName.text = "��� ������";
                break;
            case TILETYPE.TombTile:
                tileName.text = "�ذ� ����";
                break;
            case TILETYPE.LuckTile:
                tileName.text = "���";
                break;
                // �ٸ� Ÿ�� Ÿ�Կ� ���� ó��
        }
    }
    public void TileEffect()
    {
        if(tileType == TILETYPE.GoblinTile)
        {
            startButton.gameObject.SetActive(true);
            //��� ���� ����
        }
        if(tileType == TILETYPE.TombTile)
        {
            startButton.gameObject.SetActive(true);
            //���� scene �̵�
        }
        if (tileType == TILETYPE.LuckTile)
        { 
            //��� ȿ�� �ߵ�(�������� �� 2�� ����)
        }
        //ī�� ���� �������� �߰� ����
    }

    //Ÿ�� ��ġ ����, ���� Ÿ�� �̸� ��ġ�� ��� ����
    public void SetTilePosition(int x, int y)
    {
        gameObject.transform.position = new Vector2(x, y);
    }

    //Ÿ�� Trasform ��ȯ, ���� ����
    public Transform GetTileTransform()
    {
        return gameObject.transform;
    }

    //��Ʋ���� ��ư
    public void OnBattleStartButton()
    {
        battleSystem.StartBattle();
        tileName.gameObject.SetActive(false);
        startButton.gameObject.SetActive(false);
    }
}
