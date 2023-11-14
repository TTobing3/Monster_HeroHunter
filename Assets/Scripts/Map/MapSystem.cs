using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UIElements;
using System.Linq;

public class MapSystem : MonoBehaviour
{
    public static MapSystem instance;

    public static bool moveCardDraw;

    public TileEvent selectEvent, gainEvent;

    public GameObject tilePrefab;
    public GameObject playerPrefab;

    [SerializeField] Camera mainCam;
    [SerializeField] GameObject background;
    [SerializeField] GameObject tileParents;

    public List<MapTile> tileMap = new List<MapTile>();
   
    public static int curTileNum
    {
        get
        {
            return PlayManager.instance.curTileNum;
        }
        set
        {
            PlayManager.instance.curTileNum = value;
        }
    }

    #region player
    GameObject player;
    Transform stpos;
    Transform endpos;
    Rigidbody playerRb;
    Vector3 playerPosition;
    #endregion

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    void Start()
    {
        setupMap();
    }
    
    void setupMap()
    {
        moveCardDraw = true;

        GenerateTileObjects(20);
        SetTileMapData();
        MoveCameraToTargetTile(tileMap[curTileNum]);

        /*
        foreach(MapTile i in tileMap)
        {
            print(i.name);
        }
        */

        player = Instantiate(playerPrefab, tileMap[curTileNum].transform.position, tileMap[curTileNum].transform.rotation); 
        if (!player.activeSelf)
        {
            player.SetActive(true);
        }
    }

    void SetTileMapData()
    {
        //���� ���

        if(PlayManager.instance.tileMapData.Count < tileMap.Count)
        {
            for(int i = 0; i< tileMap.Count; i++)
            {
                if (i < PlayManager.instance.tileMapData.Count) continue;

                var tileData = DataManager.instance.AllTileDatas[DataManager.instance.AllTileList[Random.Range(0, DataManager.instance.AllTileList.Count)]];
                PlayManager.instance.tileMapData.Add(tileData);
            }
        }

        for (int i = 0; i < tileMap.Count; i++) tileMap[i].SetTile(PlayManager.instance.tileMapData[i]);
    }

    void GenerateTileObjects(int _count)
    {
        for (int i = 0; i < _count; i++)
        {
            var tile = Instantiate(tilePrefab).GetComponent<MapTile>();
            tile.transform.parent = tileParents.transform;

            if (i == 0)
            {
                tile.transform.position = new Vector3(-5, -3, 0);
            }
            else
            {
                var lastTilePosition = tileMap[tileMap.Count - 1].transform.position;
                tile.transform.position = new Vector3(lastTilePosition.x + 3.5f, lastTilePosition.y + 2.5f);
            }

            tile.SetTile(DataManager.instance.AllTileDatas
                [DataManager.instance.AllTileList[Random.Range(0, DataManager.instance.AllTileList.Count)]]);
            tileMap.Add(tile);
        }
    }

    public void ActMoveCardEffect(string[] _eft, MoveCard _moveCard)
    {
        switch (_eft[0])
        {
            case "이동":

                if (_eft[1].Contains('~'))
                {
                    var eftValue = _eft[1].Split('~').Select(x => int.Parse(x)).ToArray();
                    var moveValue = Random.Range(eftValue[0], eftValue[1] + 1);
                    PlayerMove(moveValue + PlayerData.readyCount);
                    PlayerData.readyCount = 0;
                }
                else
                {
                    var moveValue = int.Parse(_eft[1]);
                    PlayerMove(moveValue + PlayerData.readyCount);
                    PlayerData.readyCount = 0;
                }
                break;

            case "추격":
                Debug.Log("추격 실행");  
                /*         
                if (PlayManager.instance.tileMapData[curTileNum - 1].type == "전투" &&
                        PlayManager.instance.tileMapData[curTileNum + 1].type == "전투")
                {
                    // 두 방향에 모두 전투 타일이 있는 경우 랜덤으로 왼쪽 또는 오른쪽으로 이동
                    PlayerMove(Random.Range(0, 2) == 0 ? -1 : 1);
                }
                else if (PlayManager.instance.tileMapData[curTileNum - 1].type == "전투")
                {
                    // 왼쪽에 전투 타일이 있는 경우 왼쪽으로 이동
                    PlayerMove(-1);
                }
                else if (PlayManager.instance.tileMapData[curTileNum + 1].type == "전투")
                {
                    // 오른쪽에 전투 타일이 있는 경우 오른쪽으로 이동
                    PlayerMove(1);
                }
                */
                break;

            case "회복":
                /*
                if (PlayerData.currentHP < 100)
                {
                    PlayerData.currentHP += 10;
                }
                */
                break;

            case "준비":
                PlayerData.readyCount = int.Parse(_eft[1]);
                break;

            case "무시":
                if (_moveCard.moveCardData.name == "조심스러운 발걸음")
                {

                }
                else if (_moveCard.moveCardData.name == "전략적 후퇴") ;
                {

                }
                break;

            case "제약":
                break;
            default:
                break;
        }

        // 0번 인덱스 실행하고 삭제하면 그 다음 효과가 0번이 됨
        // 현재 0번째 인덱스가 끝나기도 전에 다음 효과도 일어남
        if (_moveCard.remainEffect.Count == 2)
        {
            _moveCard.remainEffect.RemoveAt(0);
        } 
    }
    
    public void PlayerMove(int _n = 0)
    {
        if (0 < _n)
        {
            PlayerMoveFoward(_n);
        }
        else if(_n < 0)
        {
            PlayerMoveBack(_n);
        }
        else
        {
            EndPlayerMove();
        }
    }

    void PlayerMoveFoward(int _stack)
    {

        _stack--;
        curTileNum++;
        MoveCameraToTargetTile(tileMap[curTileNum]);

        playerRb = player.GetComponent<Rigidbody>();
        stpos = player.transform;
        endpos = tileMap[curTileNum].transform;
        Vector3 topPos = stpos.position + ((endpos.position - stpos.position) / 2);
        Vector3[] JumpPath ={new Vector3(stpos.position.x,stpos.position.y,stpos.position.z),
            new Vector3(topPos.x,topPos.y+1.5f,topPos.z),
            new Vector3(endpos.position.x,endpos.position.y,endpos.position.z) };
        playerRb.DOPath(JumpPath, 2f, PathType.CatmullRom, PathMode.TopDown2D).SetEase(Ease.InCubic).OnComplete(() => PlayerMove(_stack));
    }

    void PlayerMoveBack(int _stack)
    {
        _stack++;
        curTileNum--;
        MoveCameraToTargetTile(tileMap[curTileNum]);

        playerRb = player.GetComponent<Rigidbody>();
        stpos = player.transform;
        endpos = tileMap[curTileNum].transform;
        Vector3 topPos = stpos.position + ((endpos.position - stpos.position) / 2);
        Vector3[] JumpPath ={new Vector3(stpos.position.x,stpos.position.y,stpos.position.z),
            new Vector3(topPos.x,topPos.y+1.5f,topPos.z),
            new Vector3(endpos.position.x,endpos.position.y,endpos.position.z) };
        playerRb.DOPath(JumpPath, 2f, PathType.CatmullRom, PathMode.TopDown2D).SetEase(Ease.InCubic).OnComplete(() => PlayerMove(_stack));
    }

    void EndPlayerMove()
    {

        tileMap[curTileNum].TileEffect();
    }

    void MoveCameraToTargetTile(MapTile _mapTile)
    {
        if (curTileNum < 2) return;
        Camera.main.transform.DOMove(new Vector3(_mapTile.transform.position.x, _mapTile.transform.position.y, Camera.main.transform.position.z), 1.5f);
    }

      
}
