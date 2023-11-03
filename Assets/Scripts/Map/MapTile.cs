using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MapTile : MonoBehaviour
{
    public TileData tileData;

    public GameObject EnemyPrefab; //�� ������ ����
    public Text tileName; //Ÿ���̸� Text
    public Button startButton; //��Ʋ���� ��ư

    [SerializeField] string tname; //*

    
    void Start()
    {
    }
    //Ÿ���̸� ����, �̸��� ���� �� ����
    public void SetTile(TileData _tileData)
    {
        tileData = _tileData;

        tname = tileData.name; //*
    }
    public void TileEffect()
    {
        PlayManager.instance.curTile = tileData;
        switch (tileData.type)
        {
            case "����":
                OnBattleStartButton();
                break;

            case "����":
                startButton.gameObject.SetActive(true);
                break;

            case "����":
                break;

            case "����":
                break;

            default:
                break;

        }
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
        //battleSystem.StartBattle();
        //tileName.gameObject.SetActive(false);
        //startButton.gameObject.SetActive(false);

        SceneManager.LoadScene("PlayScene");
    }
}
