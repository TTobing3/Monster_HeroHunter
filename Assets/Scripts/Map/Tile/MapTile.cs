using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MapTile : MonoBehaviour
{
    public TileData tileData;

    public TextMeshPro tileName; //Ÿ���̸� Text
    public Button startButton; //��Ʋ���� ��ư

    public List<GameObject> GetBattleCard;

    //Ÿ���̸� ����, �̸��� ���� �� ����
    public void SetTile(TileData _tileData)
    {
        tileData = _tileData;       
        //����Ÿ���� Ÿ�� �̸��� ������ ������
        if(tileData.name == "����")
        {
            tileName.text = "";
        }
        else
        {
            tileName.text = tileData.name;
        }
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
                MapSystem.instance.selectEvent.SetEvent(this);
                break;

            case "����":
                MapSystem.instance.gainEvent.SetEvent(this);
                break;

            case "����":
                MapSystem.instance.gainEvent.SetEvent(this);                
                foreach(GameObject i in GetBattleCard)
                {
                    i.SetActive(true);
                }
                break;

            default:               
                break;

        }
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
