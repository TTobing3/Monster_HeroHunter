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
}
