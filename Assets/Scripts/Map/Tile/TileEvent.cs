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

    [SerializeField] List<TextMeshProUGUI> UIText;
    [SerializeField] TextMeshProUGUI resultText;
    [SerializeField] GameObject OddEvenGame;
    [SerializeField] GameObject Options;
    [SerializeField] GameObject Option;

    public List<GetBattleCard> getBattleCards;
    public static List<BattleCardData> getbattleCardDatas = new List<BattleCardData>();

    public static int SelectCardCount;
    private bool isSpinning = false;

    public void SetEvent(MapTile _mapTile)
    {
        gameObject.SetActive(true);

        mapTile = _mapTile;

        SetEventUI(mapTile);
    }
    public void SetEventUI(MapTile _mapTile)
    {
        UIText[0].text = _mapTile.tileData.title;
        UIText[1].text = _mapTile.tileData.desc;

        switch (_mapTile.tileData.name)
        {
            case "�ź��� ����":
                Options.SetActive(true);
                break;
            case "�緡����":
                SetGetBattleCard(_mapTile.tileData.cardCount[1]);
                break;
            case "������":
                OddEvenGame.SetActive(true);
                break;
            case "���":
                SetGetBattleCard(_mapTile.tileData.cardCount[1]);
                break;
            case "��������":
                SetGetBattleCard(_mapTile.tileData.cardCount[1]);
                break;
        }
    }
    public void SetGetBattleCard(int _n)
    {
        // ī�� �Ѹ���
        for (int i = 0; i < _n; i++)
        {
            getBattleCards[i].gameObject.SetActive(true);
            float xOffset;
            if (_n == 2)
            {
                xOffset = i == 1 ? 200 : -200;
            }
            else if (_n == 3 && i > 0)
            {
                xOffset = i == 2 ? 310 : -310;
            }
            else { xOffset = 0; }

            getBattleCards[i].gameObject.transform.localPosition = new Vector3(xOffset, -90, 0);

            //��Ʋ ī�� ����
            GameObject getCard = getBattleCards[i].gameObject;


            string[] BattleCardNames = new string[] { "����ġ��", "�����ġ��", "�޼� ���","������ �Ǵ�", "�ӻ�", "���� ���" };


            // �������� BattleCardNames���� ī�� �̸��� ����
            string randomName;

            if (mapTile.tileData.GetOrDelete == "ȹ��")
            {
                //randomName = DataManager.instance.AllBattleCardList[Random.Range(0, DataManager.instance.AllBattleCardDatas.Count)];
                randomName = BattleCardNames[Random.Range(0, BattleCardNames.Length)];
            }
            else
            {
                randomName = PlayerData.playerBattleCardDeck[Random.Range(0, PlayerData.playerBattleCardDeck.Count)].name;
            }
            // ������ ī�� �̸����� ���� �����͸� ����
            BattleCardData card = DataManager.instance.AllBattleCardDatas[randomName];

            // GetBattleCard ������Ʈ�� ���ͼ� ī�带 ����
            var battleCard = getCard.GetComponent<GetBattleCard>();
            battleCard.SetCard(card);
        }
        Option.SetActive(true);
    }
    public void EndEvent()
    {
        if (getbattleCardDatas.Count > 0 && SelectCardCount <= mapTile.tileData.cardCount[0])
        {
            if (mapTile.tileData.GetOrDelete == "ȹ��")
            {
                foreach(var card in getbattleCardDatas)
                {
                    PlayerData.playerBattleCardDeck.Add(card);
                    print("�߰�");
                }
            }
            else if (mapTile.tileData.GetOrDelete == "����")
            {
                foreach (var card in getbattleCardDatas)
                {
                    PlayerData.playerBattleCardDeck.Remove(card);
                    print("����");
                }
            }
        }
        resetTileEvent();
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

            Options.SetActive(false);
            PlayManager.instance.isStone = true;
        }
        else if(BtnText == "����")
        {
            EndEvent();
        }
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        if(scene.name =="MoveScene" && PlayManager.instance.isStone) 
        {
            MapSystem.instance.tileEffect_UI.gameObject.SetActive(true);
            Option.SetActive(true);
            SetGetBattleCard(mapTile.tileData.cardCount[1]);
            PlayManager.instance.isStone=false;
        }
        else
        {
            SetGetBattleCard(mapTile.tileData.cardCount[1]);
        }
    }

    
    //Ȧ, ¦ ����
    public void OddEvenGames()
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
            MapSystem.instance.tileEffect_UI.resultText.text = Random.Range(0, 2) == 0 ? "Ȧ" : "¦";

            startTime += Time.deltaTime;
            yield return new WaitForSeconds(0.05f); // 1�� �ٲ� ������ �ӵ� ����(���� ����)
        }

        // ����� ������
        isSpinning = false;

        MapSystem.instance.tileEffect_UI.resultText.color = Color.red;
        
        // ����� ���� �¸� �Ǵ� �й� �ؽ�Ʈ ǥ��
        if (MapSystem.instance.tileEffect_UI.resultText.text == expectedResult)
        {
            UIText[1].text = "�¸�";
            OddEvenGame.SetActive(false);
            SetGetBattleCard(mapTile.tileData.cardCount[1]);
        }
        else
        {
            UIText[1].text = "�й�";
            OddEvenGame.SetActive(false);
            mapTile.tileData.GetOrDelete = "����";
            SetGetBattleCard(1);
        }
        
    }
    
    public void resetTileEvent()
    {
        gameObject.SetActive(false);
        Option.SetActive(false);
        getbattleCardDatas.Clear();
        foreach(var card in getBattleCards)
        {
            card.gameObject.GetComponent<Outline>().enabled = false;
            card.gameObject.SetActive(false);

        }
        MapSystem.instance.moveCardDraw = true;

    }
}
