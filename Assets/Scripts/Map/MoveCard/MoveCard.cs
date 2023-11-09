using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MoveCard : MonoBehaviour
{
    public MoveCardData moveCardData;
    
    [SerializeField] Image illust;
    [SerializeField] TextMeshProUGUI nameText, desText;

    GameObject CardsHand;
    public void SetCard(string cardName)
    {
        moveCardData = DataManager.instance.AllMoveCardDatas[cardName];

        nameText.text = moveCardData.name;

        desText.text = "";
        foreach(string i in moveCardData.effects)
        {
            desText.text += i;
        }
        //ī�� ���� �����ͼ� �ش� ī�� �̹����� �ؽ�Ʈ ����
        //�̰Ŵ� �׳� ī�忡 ����ī�� �ְ� �ű⼭ �����ص� �� ��?
    }
    public void MoveEffect()
    {
        CardsHand = transform.parent.gameObject;

        switch (nameText.text)
        {
            case "�ȱ�":
                MapSystem.instance.PlayerMove();                             
                break;
            case "�޸���":
               
                break;
            case "�ް�����":
                break;
            case "�غ�":
                break;
            case "����ġ��":
                break;

        }

        for (int i = 0; i <= CardsHand.transform.childCount - 1; i++)
        {
            CardsHand.transform.GetChild(i).gameObject.SetActive(false);
        }
    }
  
}
