using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using TMPro;

public class MapTile : MonoBehaviour
{
    public TileData tileData;

    public TextMeshPro tileName; //Ÿ���̸� Text
    public Button startButton; //��Ʋ���� ��ư
    public bool isStepOn=false; //��Ҵ� Ÿ������ �˻�;
    public bool isTileDataUpdate=false;
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
                MapSystem.instance.tileEffect_UI.SetEvent(this);
                break;
            case "����":
                MapSystem.instance.tileEffect_UI.SetEvent(this);               
                break;

            case "����":              
                MapSystem.instance.tileEffect_UI.SetEvent(this);                
                break;

            default:               
                break;

        }
        //GameObject AudioManager = GameObject.Find("AudioManager");
        //AudioManager.GetComponent<SoundManager>().UISfxPlay(18);
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
