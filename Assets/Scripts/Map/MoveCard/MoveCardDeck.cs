using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MoveCardDeck : MonoBehaviour
{
    [SerializeField] List<MoveCard> cards;
    [SerializeField] int handPoint; // �ڵ带 �� �� ���� �� ���ϴ� �ɷ�ġ, �÷��̾� �ɷ�ġ���� ������. �ӽ÷� ���⿡ ����

    public void SetHand()
    {
        if (MapSystem.moveCardDraw == true) // �̵�ī�� �̱⸦ �ѹ��� ���� 
        {
            //-600 �������� 2 ���� ������ 400 ����
            //-600 �������� ���� ���ϰ�, 400 �������� ī�尣 ����
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


                var names = new string[] { "�ȱ�", "�ް�����", "�޸���","�غ�", "����ġ��" };


                //Ÿ�� ������ ���� �̵�ī�� ����(���߿� �߰��� �����ؾ� �ҵ�?)
                if(MapSystem.tileCount >= 1)
                {
                    cards[i].SetCard(names[2]);
                }
                //cards[i].SetCard(names[Random.Range(0, 4)]);
            }
            MapSystem.moveCardDraw = false;
        }
    }
}
