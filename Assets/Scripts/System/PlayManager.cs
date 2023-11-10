using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerData // �÷��̾� ���� ��Ȳ ����
{
    //���� ü��, ��ȣ��, ���� ���� ī��, ���� �̵� ī��, ���� ���ǹ�, �ֻ���

    public int maxHP;
    public int currentHP;
}

public class PlayManager : MonoBehaviour
{
    public static PlayManager instance;

    public PlayerData playerData;
    public TileData curTile;

    [SerializeField] GameObject faderPrefab;
    Fader fader;

    void Awake()
    {
        #region ��Ÿ �ý���
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
        #endregion

        #region Fade Object ����
        var faderObj = Instantiate(faderPrefab);
        faderObj.transform.parent = transform;
        fader = faderObj.GetComponent<Fader>();
        #endregion
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        fader.FadeOut();
    }

    void Start()
    {
        SetStart();
    }

    void SetStart()
    {
        playerData = new PlayerData();

        playerData.maxHP = 100;
        playerData.currentHP = playerData.maxHP;
    }

}