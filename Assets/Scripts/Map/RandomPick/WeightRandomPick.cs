using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeightRandomPick : MonoBehaviour
{
    public List<Card> deck = new List<Card>();  // ī�� ��
    public int total = 0;  // ī����� ����ġ �� ��

    void Start()
    {
        for (int i = 0; i < deck.Count; i++)
        {
            // ��ũ��Ʈ�� Ȱ��ȭ �Ǹ� ī�� ���� ��� ī���� �� ����ġ�� �����ݴϴ�.
            total += deck[i].weight;
        }
        // ����
        ResultSelect();
    }

    public List<Card> result = new List<Card>();  // �����ϰ� ���õ� ī�带 ���� ����Ʈ

    public Transform parent;
    public GameObject cardprefab;

    public void ResultSelect()
    {
        for (int i = 0; i < 10; i++)
        {
            // ����ġ ������ �����鼭 ��� ����Ʈ�� �־��ݴϴ�.
            result.Add(RandomCard());
            // ��� �ִ� ī�带 �����ϰ�
            CardUI cardUI = Instantiate(cardprefab, parent).GetComponent<CardUI>();
            // ���� �� ī�忡 ��� ����Ʈ�� ������ �־��ݴϴ�.
            cardUI.CardUISet(result[i]);
        }
    }
    // ����ġ ������ ������ ������ ����.
    public Card RandomCard()
    {
        int weight = 0;
        int selectNum = 0;

        selectNum = Mathf.RoundToInt(total * Random.Range(0.0f, 1.0f));

        for (int i = 0; i < deck.Count; i++)
        {
            weight += deck[i].weight;
            if (selectNum <= weight)
            {
                Card temp = new Card(deck[i]);
                return temp;
            }
        }
        return null;
    }
}
