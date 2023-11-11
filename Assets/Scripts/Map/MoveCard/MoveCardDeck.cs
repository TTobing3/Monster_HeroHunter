using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;

public class MoveCardDeck : MonoBehaviour
{
    [SerializeField] List<MoveCard> cards;
    [SerializeField] int handPoint; // �ڵ带 �� �� ���� �� ���ϴ� �ɷ�ġ, �÷��̾� �ɷ�ġ���� ������. �ӽ÷� ���⿡ ����

    public void SetHand()
    {
        /*
        if (MapSystem.moveCardDraw == true) // �̵�ī�� �̱⸦ �ѹ��� ���� 
        {
            var center = -600 + Random.Range(-50, 50f) - 400 / 2 * handPoint;

            foreach (MoveCard i in cards)
            {
                var cardRect = i.GetComponent<RectTransform>();

                DOTween.Kill(cardRect);
                cardRect.anchoredPosition = new Vector2(0, 0);

                i.gameObject.SetActive(false);
            }

            for (int i = 0; i < handPoint; i++)
            {
                var cardRect = cards[i].GetComponent<RectTransform>();

                cardRect.gameObject.SetActive(true);
                cardRect.DOAnchorPos(new Vector3(center + 400 * i, Random.Range(-50, 250f)), 1 - i * 0.2f).SetEase(Ease.OutCirc);
                cardRect.DORotate(new Vector3(0, 0, Random.Range(-10, 10)), 2);

                //ī�� ���� �߰���

                cards[i].SetCard(CardPer());
            }
            MapSystem.moveCardDraw = false;
        }
        */
        var center = -600 + Random.Range(-50, 50f) - 400 / 2 * handPoint;

        foreach (MoveCard i in cards)
        {
            var cardRect = i.GetComponent<RectTransform>();

            DOTween.Kill(cardRect);
            cardRect.anchoredPosition = new Vector2(0, 0);

            i.gameObject.SetActive(false);
        }

        for (int i = 0; i < handPoint; i++)
        {
            var cardRect = cards[i].GetComponent<RectTransform>();

            cardRect.gameObject.SetActive(true);
            cardRect.DOAnchorPos(new Vector3(center + 400 * i, Random.Range(-50, 250f)), 1 - i * 0.2f).SetEase(Ease.OutCirc);
            cardRect.DORotate(new Vector3(0, 0, Random.Range(-10, 10)), 2);

            //ī�� ���� �߰���

            cards[i].SetCard(CardPer());
        }
    }

    // �̵� ī�� ����
    public string CardPer()
    {

        string[] commonNames = { "�ȱ�", "�޸���", "���� ����", "�غ�", "�޽�", "�߰�", "���ɽ����� �߰���", "�߸� �λ�" };

        if (MapSystem.curTileNum >= 3) // �̵� -3 ����
        {
            string[] lastNames = { "�ް�����", "������ ����", "����ġ��", "��ħ�� ����" };
            return GetRandomName(lastNames);
        }
        else if (MapSystem.curTileNum >= 2) // �̵� -2 ����
        {
            string[] middleNames = commonNames.Concat(new string[] { "�ް�����", "������ ����" }).ToArray();
            return GetRandomName(middleNames);
        }
        else if (MapSystem.curTileNum >= 1) // �̵� -1 ����
        {
            string[] basicNames = commonNames.Concat(new string[] { "�ް�����", "������ ����" }).ToArray();
            return GetRandomName(basicNames);
        }
        else if (MapSystem.curTileNum >= 0) // �̵� - �Ұ��� 
        {
            return GetRandomName(commonNames);
        }
        else
        {
            return "";
        }
    }

    private string GetRandomName(string[] nameList)
    {
        return nameList[Random.Range(0, nameList.Length)];
    }
}
