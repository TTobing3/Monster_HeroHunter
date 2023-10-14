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
    }
}
