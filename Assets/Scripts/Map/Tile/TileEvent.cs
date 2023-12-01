using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using TreeEditor;
using UnityEngine.SceneManagement;

public class TileEvent : MonoBehaviour
{
    public MapTile mapTile;

    public TextMeshProUGUI DesName; // UI Des Text
    public TextMeshProUGUI resultText; // Result Text

    private bool isSpinning = false;

    public void SetEvent(MapTile _mapTile)
    {
        gameObject.SetActive(true);

        mapTile = _mapTile;

        SetEventUI(mapTile);
    }

    public void SetEventUI(MapTile _mapTile)
    {
        switch (_mapTile.tileData.name)
        {
            case "�ź��� ����":                
                MapSystem.instance.selectEvent.DesName.text = mapTile.tileData.desc;
                GameObject op = GameObject.Find("SelectUI").transform.Find("Options").gameObject;              
                op.SetActive(true);

                break;
            case "�緡����":
                MapSystem.instance.selectEvent.DesName.text = mapTile.tileData.desc;
                GameObject Comp = GameObject.Find("SelectUI").transform.Find("Comp").gameObject;
                Comp.SetActive(true);

                GameObject cardObjects = GameObject.Find("BattleCard3");

                for (int i = 0; i < 3; i++)
                {
                    GameObject getCard = cardObjects.transform.GetChild(i).gameObject;

                    string[] BattleCardNames = new string[] { "����ġ��", "�޼� ���", "�����ġ��", "��������", "�������Ǵ�", "�ӻ�", "���� ���" };

                    // �������� BattleCardNames���� ī�� �̸��� ����
                    string randomCardName = BattleCardNames[Random.Range(0, BattleCardNames.Length)];

                    // ������ ī�� �̸����� ���� �����͸� ����
                    BattleCardData card = DataManager.instance.AllBattleCardDatas[randomCardName];

                    // GetBattleCard ������Ʈ�� ���ͼ� ī�带 ����
                    var battleCard = getCard.GetComponent<GetBattleCard>();
                    battleCard.SetCard(card);

                    PlayerData.playerBattleCardDeck.Add(card);

                }

                break;
            case "������":
                MapSystem.instance.selectEvent.DesName.text = mapTile.tileData.desc;               
                break;
            case "���":
                MapSystem.instance.gainEvent.DesName.text = mapTile.tileData.desc;
                break;
            case "��������":
                MapSystem.instance.gainEvent.DesName.text = mapTile.tileData.desc;
                break;
        }
    }

    public void SelectEvent()
    {
       switch (mapTile.tileData.name)
        {
            case "�ź��� ����":
                
                break;
            case "�緡����":
                
                break;
            case "������":
                
                break;
        }
    }
    public void GainEvent()
    {
        switch (mapTile.tileData.name)
        {
            case "���":
                GetBattleCard();
                break;
            case "��������":             
                DeleteBattleCard();
                break;
        }
    }

    public void EndEvent()
    {
        gameObject.SetActive(false);
        MapSystem.instance.moveCardDraw = true;
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

            // GetBattleCard ������Ʈ�� ���ͼ� ī�带 ����
            var battleCard = getCard.GetComponent<GetBattleCard>();
            battleCard.SetCard(card);

            PlayerData.playerBattleCardDeck.Add(card);

        }
    }
    public void DeleteBattleCard()
    {      
        if (PlayerData.playerBattleCardDeck.Count != 0)
        {
            GameObject cardObjects = GameObject.Find("BattleCard2");

            GameObject getCard = cardObjects.transform.GetChild(0).gameObject;

            var randInt = Random.Range(0, PlayerData.playerBattleCardDeck.Count);

            BattleCardData card = PlayerData.playerBattleCardDeck[randInt];
            
            var battleCard = getCard.GetComponent<GetBattleCard>();
            battleCard.SetCard(card);

            PlayerData.playerBattleCardDeck.RemoveAt(randInt);

        }
    }
    public void TestofStone()
    {
        GameObject ClickButton = EventSystem.current.currentSelectedGameObject;
        string BtnText = ClickButton.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text;

        if(BtnText == "����")
        {
            PlayManager.instance.curTile = DataManager.instance.AllTileDatas["����"]; //�ӽ÷� ����
            SceneManager.LoadScene("PlayScene");

            SceneManager.sceneLoaded += OnSceneLoaded;

            GameObject options = GameObject.Find("Options");
            options.SetActive(false);

            PlayManager.instance.iscomp = true;
        }
        else if(BtnText == "����")
        {
            EndEvent();
        }
    }
    public void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        if(scene.name =="MoveScene" && PlayManager.instance.iscomp) 
        {
            MapSystem.instance.selectEvent.gameObject.SetActive(true);


            GameObject Comp = GameObject.Find("SelectUI").transform.Find("Comp").gameObject;
            Comp.SetActive(true);

            GameObject cardObjects = GameObject.Find("BattleCard3");

            for (int i = 0; i < 3; i++)
            {
                GameObject getCard = cardObjects.transform.GetChild(i).gameObject;

                string[] BattleCardNames = new string[] { "����ġ��", "�޼� ���", "�����ġ��", "��������", "�������Ǵ�", "�ӻ�", "���� ���" };

                // �������� BattleCardNames���� ī�� �̸��� ����
                string randomCardName = BattleCardNames[Random.Range(0, BattleCardNames.Length)];

                // ������ ī�� �̸����� ���� �����͸� ����
                BattleCardData card = DataManager.instance.AllBattleCardDatas[randomCardName];

                // GetBattleCard ������Ʈ�� ���ͼ� ī�带 ����
                var battleCard = getCard.GetComponent<GetBattleCard>();
                battleCard.SetCard(card);

                PlayerData.playerBattleCardDeck.Add(card);

            }

            PlayManager.instance.iscomp=false;
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
        
        // ����� ���� �¸� �Ǵ� �й� �ؽ�Ʈ ǥ��
        if (MapSystem.instance.selectEvent.resultText.text == expectedResult)
        {
            // ���̽� �߰� 
        }
        else
        {
            DeleteBattleCard();
        }
        
    }

}
