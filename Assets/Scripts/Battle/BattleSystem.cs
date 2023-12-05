﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST}

public class BattleSystem : MonoBehaviour
{
    public static BattleSystem instance;

    public Unit[] units; // 플레이어, 적, 적, 적 
    public BattleHUD[] unitHUDs;

    public GameObject floatingTextPrefab;
    List<FloatingText> floatingTextQueue = new List<FloatingText>();

    public BattleCardDeck battleCardDeck;
    public int curDiceCount; // 해당턴의 다이스 개수, 사용하면 줄어드는 거

    public BattleState state;
    public Targeter targeter;
    public Unit playerSkillTarget;


    public playerSkill skill;
    public BattleCard battlecard;

    public List<BattleCardDataAndTarget> usedBattleCardQueue = new List<BattleCardDataAndTarget>(); // 사용 대기중인 배틀 카드

    bool cardActing = false;
    bool floating = false;

    int enemyOrder = 1;

    //public Animator 

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    void Start()
    {  
        for(int i = 0; i < units.Length; i++) units[i].battleHUD = unitHUDs[i];
        //
        state = BattleState.START;
        SetupBattle();
    }

    #region BattleSetting

    public void StartBattle()
    {
    }

    void SetupBattle()
    {
        foreach (Unit i in units) i.gameObject.SetActive(false);
        foreach (BattleHUD i in unitHUDs) i.gameObject.SetActive(false);

        #region Unit Setting

        var tileData = (BattleTile)PlayManager.instance.curTile;

        for (int i = 0; i < tileData.enemies.Count + 1; i++)
        {
            units[i].gameObject.SetActive(true);
            unitHUDs[i].gameObject.SetActive(true);

            if (i == 0) // Player
            {
                units[i].SetUnit();
                unitHUDs[i].SetHUD(units[i]);
            }
            else // Enemy
            {
                SetEnemyUnit(units[i], unitHUDs[i], tileData.enemies[i - 1]);
            }
        }

        curDiceCount = PlayerData.diceCount;

        #endregion

        //start Player Turn
        state = BattleState.PLAYERTURN;

        battleCardDeck.SetBattleCardDeck();
        PlayerTurn();

        GameObject AudioManager = GameObject.Find("AudioManager");
        AudioManager.GetComponent<SoundManager>().BgSoundPlay(1);
    }

    void SetEnemyUnit(Unit _unit, BattleHUD _hud, MonsterData _data)
    {
        _unit.SetUnit(_data);
        _unit.GetComponent<EnemyUnitSkin>().ChangeSkin(DataManager.instance.AllEnemySnAs[_unit.unitName].skinNames);
        //_hud.SetHUD(_unit);
    }

    #endregion

    #region PlayerTurn

    void PlayerTurn()
    {
        curDiceCount = PlayerData.diceCount;
        GameObject AudioManager = GameObject.Find("AudioManager");
        AudioManager.GetComponent<SoundManager>().UISfxPlay(4);

        battleCardDeck.SetPlayerTurn();
    }

    public void UseBattleCard(BattleCardDataAndTarget _battleCardDataAndTarget)
    {
        usedBattleCardQueue.Add(_battleCardDataAndTarget); //사용한 카드 스킬 대기열에 올림

        ActBattleCardSkill();
    }

    void ActBattleCardSkill()
    {
        if (cardActing || usedBattleCardQueue.Count < 1) return;

        cardActing = true;

        Act_PlayerAnimation(0); // 스킬별로 타입이 있도록 하기
    }

    void Act_PlayerAnimation(int _type)
    {
        units[0].animator.SetInteger("type", _type);
        units[0].animator.SetTrigger("attack");
    }

    //Unit -> Effect_PlayerAnimation 및 Finish_PlayerAnimation 작동

    public void EffectBattleCard()
    {
        var skillArray = usedBattleCardQueue[0].battleCard.enforced ? 
            usedBattleCardQueue[0].battleCard.battleCardData.skillData.enforcedEffects :
            usedBattleCardQueue[0].battleCard.battleCardData.skillData.effects;
        foreach (string i in skillArray)
        {
            print(i);
            var tmpEft = i.Split('/');
            if (tmpEft.Length > 1 && tmpEft[1] == "적전체") // 전체 -> 무조건 적 전체임
            {
                SkillUseSystem.Divide_Target(units[1], units[0], tmpEft[0]);
                SkillUseSystem.Divide_Target(units[2], units[0], tmpEft[0]);
                SkillUseSystem.Divide_Target(units[3], units[0], tmpEft[0]);
            }
            else
            {
                SkillUseSystem.Divide_Target(usedBattleCardQueue[0].target, units[0], tmpEft[0]);
            }
        }

        /*
                bool isDead = units[1].Takedamage(units[0].damage);

                if (isDead)
                {
                    state = BattleState.WON;
                    Destroy(units[1]);
                    EndBattle();
                }
        */
    }

    public void EfterPlayerTurn()
    {
        usedBattleCardQueue.RemoveAt(0);

        cardActing = false;

        ActBattleCardSkill();

        //모든 카드를 썻으면 자동으로 적 턴

        if (battleCardDeck.curHandCardCount > 0 || usedBattleCardQueue.Count > 0) return;
        
        ActEnemySideEffect(true);

        if(battleCardDeck.pocket.isOpen) battleCardDeck.pocket.ReturnDice();

        StartCoroutine( EnemyTurn());
    }

    public void ActEnemySideEffect(bool _endOfPlayerTurn)
    {
        for (int i = 1; i<units.Length; i++)
        {
            if(units[i].gameObject.activeSelf) units[i].ActSideEffect(_endOfPlayerTurn);
        }
    }

    #endregion
    
    #region EnemyTurn

    IEnumerator EnemyTurn()
    {
        state = BattleState.ENEMYTURN;

        yield return new WaitForSeconds(0.8f); // 이건 피격 애니메이션때문에 공격 애니메이션 씹히는 경우 있을 것 같아서 약간 텀 넣어준거

        Act_EnemyAnimation();
    }

    void Act_EnemyAnimation()
    {

        if (units[enemyOrder].ActStun()) return;//기절 시 넘김
        //job은 미리 설정
        units[enemyOrder].animator.SetInteger("type", 1);
        units[enemyOrder].animator.SetTrigger("change");
    }

    public void EffectEnemySkill(int _skillOrder)
    {
        unitHUDs[enemyOrder].SetHP();
        var tmpSkillData = DataManager.instance.AllSkillDatas[units[enemyOrder].monsterData.patterns[0][_skillOrder]];

        foreach (string i in tmpSkillData.effects)
        {
            var tmpEft = i.Split('/');
            if (tmpEft.Length > 1 && tmpEft[1] == "아군전체") 
            {
                print("in");
                SkillUseSystem.Divide_Target(units[0], units[1], tmpEft[0]);
                SkillUseSystem.Divide_Target(units[0], units[2], tmpEft[0]);
                SkillUseSystem.Divide_Target(units[0], units[3], tmpEft[0]);
            }
            else
            {
                SkillUseSystem.Divide_Target(units[0], units[enemyOrder], tmpEft[0]);
            }
        }


        /*
        bool isDead = units[0].Takedamage(units[1].damage);


        if (isDead)
        {
            state = BattleState.LOST;
            Destroy(units[0]);
            EndBattle();
        }
        GameObject AudioManager = GameObject.Find("AudioManager");
        AudioManager.GetComponent<SoundManager>().UISfxPlay(4);
        */
    }

    public void EfterEnemyTurn()
    {
        var tileData = (BattleTile)PlayManager.instance.curTile;

        if (enemyOrder == tileData.enemies.Count )
        {
            state = BattleState.PLAYERTURN;
            enemyOrder = 1; //배틀 시스템 유닛에서 적은 1번 부터라서 1로 초기화
            ActEnemySideEffect(false);
            PlayerTurn();
        }
        else
        {
            enemyOrder += 1;
            Act_EnemyAnimation();
        }

    }


    void EndBattle()
    {
        if (state == BattleState.WON)
        {
            SceneManager.LoadScene("MoveScene");
            GameObject AudioManager = GameObject.Find("AudioManager");
            AudioManager.GetComponent<SoundManager>().BgSoundPlay(0);
        }
        else if (state == BattleState.LOST)
        {
            Act_EnemyAnimation();
        }
    }

    #endregion


    public void OnFinishButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        state = BattleState.ENEMYTURN;
        StartCoroutine(EnemyTurn());

        //카드들 used false로 다 바구기
    }

    #region TEST



    #endregion

    #region FloatingText

    public IEnumerator ActFloatingText()
    {

        if (!floating && floatingTextQueue.Count > 0)
        {
            floating = true;
            floatingTextQueue[0].gameObject.SetActive(true);
            floatingTextQueue[0].Floating();
            yield return new WaitForSeconds(0.5f);
            floatingTextQueue.RemoveAt(0);
            floating = false;
            StartCoroutine(ActFloatingText());
        }
        else
        {
            /*
            Debug.Log("None floating");
            floatingTextQueue = new List<FloatingText>();
            yield break;
            */
        }
    }

    public void FloatText(GameObject _parents, string _text)
    {
        var tmp = Instantiate(floatingTextPrefab, _parents.transform);
        var tmpR = tmp.GetComponent<RectTransform>();
        var tmpF = tmp.GetComponent<FloatingText>();
        tmpR.localPosition = new Vector2(0, -150);

        floatingTextQueue.Add(tmpF);

        tmpF.SetText(_text, Color.white);
        tmp.SetActive(false);

        StartCoroutine(ActFloatingText());
    }

}

#endregion

public class SkillUseSystem
{
    static bool Per(Unit _target, Unit _caster, string _eft, bool _isToEnemy = true)
    {
        var eft = _eft.Split(':');

        if (eft.Length > 2 && Random.Range(0, 1f) > float.Parse(eft[2]))
        {
            if(_isToEnemy) BattleSystem.instance.FloatText(_target.battleHUD.gameObject, "빗나감!");
            else  BattleSystem.instance.FloatText(_caster.battleHUD.gameObject, "빗나감!");

            return false;
        }
        else return true;
    }
    public static void Divide_Target(Unit _target, Unit _caster, string _eft)
    {
        var eft = _eft.Split(':');

        var damage = float.Parse(eft[1]) + float.Parse(eft[1]) * _caster.stack[0];

        switch (eft[0])
        {
            case "물리피해":
                if (!Per(_target, _caster, _eft)) return;
                Damage(_target, (int)damage);
                _caster.stack[0] = 0;
                break;
            case "관통피해":
                if (!Per(_target, _caster, _eft)) return;
                PiercingDamage(_target, (int)damage);
                _caster.stack[0] = 0;

                if(PlayerData.CheckLostItem("독성 발톱") && _caster == BattleSystem.instance.units[0]) // 유실물 : 독성발톱
                    Divide_Target(_target, _caster, "지속피해:2");

                break;
            case "지속피해":
                if (!Per(_target, _caster, _eft)) return;
                DotDamage(_target, (int)damage);
                _caster.stack[0] = 0;
                break;
            case "추가피해":
                if (!Per(_target, _caster, _eft)) return;
                IncreaseDamage(_caster, float.Parse(eft[1]));
                break;
            case "기절":
                if (!Per(_target, _caster, _eft)) return;
                Stun(_target, float.Parse(eft[1]));
                break;
            case "회복":
                if (!Per(_target, _caster, _eft, false)) return;
                Heal(_caster, float.Parse(eft[1]));
                break;
            case "보호막":
                if (!Per(_target, _caster, _eft, false)) return;
                Shield(_caster, float.Parse(eft[1]));
                break;
            case "회피":
                if (!Per(_target, _caster, _eft, false)) return;
                Evade(_caster, float.Parse(eft[1]));
                break;
            case "정화":
                if (!Per(_target, _caster, _eft, false)) return;
                Cleanse(_caster, float.Parse(eft[1]));
                break;
        }
    }


    static void Damage(Unit _target, int _damage)
    {
        _target.Takedamage(_damage);
    }

    static void PiercingDamage(Unit _target, int _damage)
    {
        _target.Takedamage(_damage, true);

    }


    //유닛한테 쌓아야할듯? 스택을

    static void DotDamage(Unit _target, int _damage)
    {
        _target.dotDamage[0] += _damage;
    }
    //=> battleSystem의 efter turn에서 여기서 쌓인 도트데미지를 가하고 한 턴씩 미루는 메서드를 구현

    static void IncreaseDamage(Unit _target, float _damage)
    {
        _target.stack[0] += _damage;
    }

    static void ExplosiveDamage()
    {

    }

    //

    static void Stun(Unit _target, float _damage)
    {
        _target.stack[2] += _damage;

        BattleSystem.instance.FloatText(_target.battleHUD.gameObject, "기절 +" + _damage);

    }

    static void Heal(Unit _target, float _damage)
    {
        _target.TakeHeal(_damage);

    }

    static void Shield(Unit _target, float _damage)
    {
        _target.TakeShield(_damage);
    }

    static void Evade(Unit _target, float _damage)
    {
        _target.stack[3] += _damage;
        BattleSystem.instance.FloatText(_target.battleHUD.gameObject, "회피 +" + _damage);
    }

    static void Cleanse(Unit _target, float _damage)
    {

        BattleSystem.instance.FloatText(_target.battleHUD.gameObject, "정화 +" + _damage);

        if(_target.stack[1] > 0)
        {
            _target.stack[1] -= _damage;
            BattleSystem.instance.FloatText(_target.battleHUD.gameObject, "폭발피해 -" + _damage);
        }
        if (_target.stack[2] > 0)
        {
            _target.stack[2] -= _damage;
            BattleSystem.instance.FloatText(_target.battleHUD.gameObject, "기절 -" + _damage);
        }
        for(int i = 0; i<_target.dotDamage.Length; i++)
        {
            if (_target.dotDamage[i] <= 0) continue;
            _target.dotDamage[i] -= (int)(_damage * 10);
            BattleSystem.instance.FloatText(_target.battleHUD.gameObject, "지속피해 -" + (int)(_damage * 10));
        }
    }

    static void Resist()
    {

    }

    //

    static void Reroll()
    {

    }

    static void Copy()
    {

    }

}