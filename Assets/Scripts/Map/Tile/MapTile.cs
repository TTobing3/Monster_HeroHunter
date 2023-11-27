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
                DeleteBattleCard();
                break;

            case "����":
                MapSystem.instance.gainEvent.SetEvent(this);
                GetBattleCard();
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
    public void GetBattleCard()
    {
        for (int i = 0; i < 2; i++)
        {
            GameObject cardObject = GameObject.Find("BattleCard").transform.GetChild(i).gameObject;
            print(cardObject.name);

            string[] BattleCardNames = new string[] { "����ġ��", "�޼� ���", "�����ġ��", "��������", "�������Ǵ�", "�ӻ�", "���� ���" };

            // �������� BattleCardNames���� ī�� �̸��� ����
            string randomCardName = BattleCardNames[Random.Range(0, BattleCardNames.Length)];

            // ������ ī�� �̸����� ���� �����͸� ����
            BattleCardData card = DataManager.instance.AllBattleCardDatas[randomCardName];
            print(card.name);

            // GetBattleCard ������Ʈ�� ���ͼ� ī�带 ����
            var battleCard = cardObject.GetComponent<GetBattleCard>();
            battleCard.SetCard(card);

            PlayerData.playerBattleCardDeck.Add(card);
        }
    }
    public void DeleteBattleCard()
    {   
        if (PlayerData.playerBattleCardDeck.Count != 0)
        {
            PlayerData.playerBattleCardDeck.RemoveAt(
                Random.Range(0, PlayerData.playerBattleCardDeck.Count));
        }
    }
    public void SelectEvent()
    {

    }
}
