using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//���� ���� ������
public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST}

public class BattleSystem : MonoBehaviour
{

    //�÷��̾�� ���� ������
    public GameObject playerPrefab;//10-15 ���� ����, MapSystem���� player ����
    public GameObject enemyPrefab; //10-15 ���� ����, MapTile���� enemy ����

    //�÷��̾�� ���� ��Ÿ���� �ٴڿ� �ִ� ����. ���ʿ�� ��������
    public Transform playerBattleStation;
    public Transform enemyBattleStation;

    Unit playerUnit;
    Unit enemyUnit;

    GameObject playerGO;
    GameObject enemyGO;
    //�ؽ�Ʈ �ʿ��ҽ� �ּ�����
    //public Text dialogueText;

    public BattleHUD playerHUD;
    public BattleHUD enemyHUD;

    public BattleState state;

    void Start()
    {  
        //���� ����
        state = BattleState.START;
        SetupBattle();
    }
    //10-15 ���� ����, MapTile���� Tileȿ���� ���� ����
    public void StartBattle()
    {
    }
    void SetupBattle()
    {
        //���� ���۽� �÷��̾�� ���� ȭ�鿡 ��Ÿ��.
        //player = GetComponent<MapSystem>().playerPrefab; //10-15 ���� ���� 
        playerGO = Instantiate(playerPrefab);
        playerUnit = playerGO.GetComponent<Unit>();
        enemyGO = Instantiate(enemyPrefab);
        enemyUnit = enemyGO.GetComponent<Unit>();
        //�� ����� �ؽ�Ʈ ���
        //dialogueText.text = "...";
        Debug.Log("Enemy Max HP: " + enemyUnit.maxHP);

        playerHUD.SetHUD(playerUnit);
        enemyHUD.SetHUD(enemyUnit);

        #region ���� ����

        //�� ����

        var tileData = (BattleTile)PlayManager.instance.curTile;

        enemyUnit.SetUnit(tileData.enemies[0]);

        //�÷��̾� ����

        playerUnit.SetUnit(PlayManager.instance.playerData);

        #endregion

        //�÷��̾��� ����
        state = BattleState.PLAYERTURN;
        PlayerTurn();

    }

    IEnumerator PlayerAttack()
    {
        print("�÷��̾� ����");
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
            Destroy(enemyGO);
            EndBattle();
        }
    }

    IEnumerator EnemyTurn()
    {
        print("�� ����");
        //dialogueText.text = enemyUnit.unitName + "����!"

        yield return new WaitForSeconds(1f);

        bool isDead = playerUnit.Takedamage(enemyUnit.damage);

        playerHUD.SetHP(playerUnit.currentHP);

        yield return new WaitForSeconds(1f);

        if (isDead)
        {
            state = BattleState.LOST;
            Destroy(playerGO);
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
            SceneManager.LoadScene("MoveScene");
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
