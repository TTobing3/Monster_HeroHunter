using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MapTile : MonoBehaviour
{
    public TileData tileData;

    public GameObject EnemyPrefab; //�� ������ ����
    public TextMeshPro tileName; //Ÿ���̸� Text
    public Button startButton; //��Ʋ���� ��ư

    
    //Ÿ���̸� ����, �̸��� ���� �� ����
    public void SetTile(TileData _tileData)
    {
        tileData = _tileData;

        tileName.text = tileData.name; 
    }
    public void TileEffect()
    {
        print("Act?");

        PlayManager.instance.curTile = tileData;
        switch (tileData.type)
        {
            case "����":
                OnBattleStartButton();
                break;

            case "����":
                EndTileEffect();
                //startButton.gameObject.SetActive(true);
                break;

            case "����":
                EndTileEffect();
                break;

            case "����":
                EndTileEffect();
                break;

            default:
                EndTileEffect();
                break;

        }
    }


    void EndTileEffect()
    {
        MapSystem.moveCardDraw = true;
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

        SceneManager.LoadScene("PlayScene"); // �ӽ÷� �ٷ� ����
    }
}
