using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//���� ���� ������
public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST}

public class BattleSystem : MonoBehaviour
{
    //�÷��̾�� ���� ������
    public GameObject playerPrefab;
    public GameObject enemyPrefab;
    //�÷��̾�� ���� ��Ÿ���� �ٴڿ� �ִ� ����. ���ʿ�� ��������
    public Transform playerBattleStation;
    public Transform enemyBattleStation;

    Unit playerUnit;
    Unit enemyUnit;
    //�ؽ�Ʈ �ʿ��ҽ� �ּ�����
    //public Text dialogueText;

    public BattleHUD playerHUD;
    public BattleHUD enemyHUD;

    public BattleState state;
    // Start is called before the first frame update
    void Start()
    {  
        //���� ����
        state = BattleState.START;
        SetupBattle();
    }

    void SetupBattle()
    {
        //���� ���۽� �÷��̾�� ���� ȭ�鿡 ��Ÿ��.
        GameObject playerGO = Instantiate(playerPrefab, playerBattleStation);
        playerUnit = playerGO.GetComponent<Unit>();
        GameObject enemyGO = Instantiate(enemyPrefab, enemyBattleStation);
        enemyUnit = enemyGO.GetComponent<Unit>();
        //�� ����� �ؽ�Ʈ ���
        //dialogueText.text = "...";

        playerHUD.SetHUD(playerUnit);
        enemyHUD.SetHUD(enemyUnit);
        //�÷��̾��� ����
        state = BattleState.PLAYERTURN;
        PlayerTurn();

    }

    IEnumerator PlayerAttack()
    {
        //������ �������� ������
        bool isDead = enemyUnit.Takedamage(playerUnit.damage);

        enemyHUD.SetHP(enemyUnit.currentHP);
        //dialogueText.text = "���� ����!"

        yield return new WaitForSeconds(2f);

        //���� �׾������� Ȯ��
        if (isDead)
        {
            //����Ȯ�� �� ���� ���¸� ��ȭ��Ŵ
            state = BattleState.WON;
            EndBattle();
        }
    }

    IEnumerator EnemyTurn()
    {
        //dialogueText.text = enemyUnit.unitName + "����!"

        yield return new WaitForSeconds(1f);

        bool isDead = playerUnit.Takedamage(enemyUnit.damage);

        playerHUD.SetHP(playerUnit.currentHP);

        yield return new WaitForSeconds(1f);

        if (isDead)
        {
            state = BattleState.LOST;
            EndBattle();
        }
        else
        {
            state = BattleState.PLAYERTURN;
            PlayerTurn();
        }
    }

    void EndBattle()
    {
        if(state == BattleState.WON)
        {
            //dialogueText.text = "�¸�!";
        }
        else if (state == BattleState.LOST)
        {
            //dialogueText.text = "�й�..."
        }
    }

    void PlayerTurn()
    {
        //dialogueText.text = "ī�带 �����Ͻʽÿ�";
    }
    //�ϴ� ���ݹ�ư�� �ִٴ� �����Ͽ� ����
    public void OnAttackButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        StartCoroutine(PlayerAttack());
    }

    public void OnFinishButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        state = BattleState.ENEMYTURN;
        StartCoroutine(EnemyTurn());
    }
}
