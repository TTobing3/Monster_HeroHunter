using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class BattleCardDeck : MonoBehaviour
{
    [SerializeField] RectTransform[] battleCardPool;

    //�ڵ� �� ��
    //�ϳ� ���� Ŀ����, ������ �۾�����
    //�� ���� ���������� �ٽ� �����·� ����

    int curHandCardCount = 0;

    public void SetHand(int _handCardCount)
    {
        foreach (RectTransform i in battleCardPool) i.gameObject.SetActive(false);

        curHandCardCount = _handCardCount;

        SetHandCardPosition();
    }

    public void SetHandCardPosition()
    {
        //���� ��ġ ��°�
        for (int i = 0; i < curHandCardCount; i++)
        {
            battleCardPool[i].gameObject.SetActive(true);
            battleCardPool[i].DOAnchorPos(new Vector2(800 + (curHandCardCount * 20) - (200 - 10 * curHandCardCount) * (curHandCardCount - i + 1),
                -400 - System.MathF.Abs(curHandCardCount / 2 - i) * 20), 0.2f * (i + 1));
            battleCardPool[i].DORotate(new Vector3(0, 0, -5 + (5 * curHandCardCount) - (10 * i)), 0.2f * (i + 1));
        }

    }
}
