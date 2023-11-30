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

    private bool isSpinning = false;
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
                SelectEvent();
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
        GameObject cardObjects = GameObject.Find("BattleCard");

        for (int i = 0; i < 2; i++)
        {
            GameObject getCard = cardObjects.transform.GetChild(i).gameObject;

            string[] BattleCardNames = new string[] { "����ġ��", "�޼� ���", "�����ġ��", "��������", "�������Ǵ�", "�ӻ�", "���� ���" };

            // �������� BattleCardNames���� ī�� �̸��� ����
            string randomCardName = BattleCardNames[Random.Range(0, BattleCardNames.Length)];

            // ������ ī�� �̸����� ���� �����͸� ����
            BattleCardData card = DataManager.instance.AllBattleCardDatas[randomCardName];
            print(card.name);

            // GetBattleCard ������Ʈ�� ���ͼ� ī�带 ����
            var battleCard = getCard.GetComponent<GetBattleCard>();
            battleCard.SetCard(card);

            //PlayerData.playerBattleCardDeck.Add(card);
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
        switch (tileData.name)
        {
            case "�ź��� ����":
                MapSystem.instance.selectEvent.DesName.text = tileData.desc;
                break;
            case "�緡����":
                MapSystem.instance.selectEvent.DesName.text = tileData.desc;
                break;
            case "������":
                MapSystem.instance.selectEvent.DesName.text = tileData.desc;
                break;
        }
    }
    //Ȧ, ¦ ����
    public void OddEvenGame()
    {
        //Ŭ���� ��ư�� ������Ʈ ��������
        GameObject ClickButton = EventSystem.current.currentSelectedGameObject;

        //��ư�� text ��������
        string BtnText = ClickButton.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text;

        //��ư Ŭ���� ��ư�� ���� �ٲ�(UI�ٲٸ� ���� ����)
        Image BtnImg = ClickButton.GetComponent<Image>();
        BtnImg.color = Color.red;
        
        if (!isSpinning)
        {
            StartCoroutine(SpinResult(BtnText));
        }
    }
    private IEnumerator SpinResult(string expectedResult)
    {
        isSpinning = true;
       
        float spinTime = 1.5f;
        float startTime = 0f;

        while (startTime < spinTime)
        {           
            MapSystem.instance.selectEvent.resultText.text = Random.Range(0, 2) == 0 ? "Ȧ" : "¦";

            startTime += Time.deltaTime;
            yield return new WaitForSeconds(0.05f); // 1�� �ٲ� ������ �ӵ� ����(���� ����)
        }

        // ����� ������
        isSpinning = false;

        MapSystem.instance.selectEvent.resultText.color = Color.red;
        /*
        // ����� ���� �¸� �Ǵ� �й� �ؽ�Ʈ ǥ��
        if (MapSystem.instance.selectEvent.resultText.text == expectedResult)
        {
            MapSystem.instance.selectEvent.resultText.text = "�¸�!";
        }
        else
        {
            MapSystem.instance.selectEvent.resultText.text = "�й�!";
        }
        */
    }


}
