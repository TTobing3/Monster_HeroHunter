 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public BattleHUD battleHUD; 

    public string unitName, unitType;
    public int damage;

    public int maxHP;
    public int currentHP;

    public int shield;

    public Animator animator;

    #region SubEffect

    public int[] dotDamage = new int[3] { 0, 0, 0 }; // 3������ �����ؼ� 3��¥��

    public float[] stack = new float[6] { 0,0,0 ,0,0,0 }; // increase / exp / stun / evade / clean /resist
    public int[] sideEffect = new int[6] { 0,0,0 ,0,0,0 }; // increase / exp / stun / evade / clean /resist

    #endregion

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    #region Setting

    public void SetUnit(MonsterData _monsterData)
    {
        unitName = _monsterData.name;
        unitType = _monsterData.type;

        maxHP = _monsterData.hp[0];
        currentHP = maxHP;

        shield = _monsterData.hp[1];
    }

    public void SetUnit()
    {
        maxHP = PlayerData.maxHP;
        currentHP = maxHP;
    }

    #endregion

    #region Animation

    //�÷��̾�
    public void Effect_PlayerAnimation()
    {
        BattleSystem.instance.EffectBattleCard();  
    }

    public void Finish_PlayerAnimation()
    {
        BattleSystem.instance.EfterPlayerTurn();
    }

    //��
    public void Effect_EnemyAnimation()
    {
        BattleSystem.instance.EffectEnemySkill();
    }

    public void Finish_EnemyAnimation()
    {
        BattleSystem.instance.EfterEnemyTurn();
    }

    #endregion

    public void Hit()
    {
        //�ǰ� ���
        //���ط�
    }

    public void ActSideEffect()
    {
        ActDotDamage();
    }
    void ActDotDamage()
    {
        foreach(int i in dotDamage)
        {
            Takedamage(i);
        }

        dotDamage[2] = dotDamage[1];
        dotDamage[1] = dotDamage[0];
        dotDamage[0] = 0;

    }
    public void Takedamage(int _damage, bool _p = false)
    {
        int remainDamage = 0;

        if (shield > 0 && _p == false)
        {
            remainDamage = shield < _damage ? _damage - shield : 0; // ��ȣ������ ū �������� ü���� ���
            shield = shield < _damage ? 0 : shield - _damage; //��ȣ�� ������
        }
        else
        {
            remainDamage = _damage;
        }

        currentHP -= remainDamage; // ü�� ������

        BattleSystem.instance.FloatText(battleHUD.gameObject, "-" + _damage);

        battleHUD.SetHP();

    }


}
