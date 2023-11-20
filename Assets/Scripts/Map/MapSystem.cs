using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UIElements;
using System.Linq;

public class MapSystem : MonoBehaviour
{
    public static MapSystem instance;

    public bool moveCardDraw;
    public bool allowHealing = true;
    public bool allowEffect = true;

    public static int readyCount;

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

    public static bool jumpState = false;
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

        player = Instantiate(playerPrefab, tileMap[curTileNum].transform.position, tileMap[curTileNum].transform.rotation); 
        if (!player.activeSelf)
        {
            player.SetActive(true);
        }
    }

    void SetTileMapData()
    {
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
                    PlayerMove(moveValue + readyCount,_moveCard);                   
                }
                else
                {
                    var moveValue = int.Parse(_eft[1]);
                    PlayerMove(moveValue + readyCount, _moveCard);               
                }
                break;

            case "추격":
                                    
                if (PlayManager.instance.tileMapData[curTileNum - 1].type == "전투" &&
                        PlayManager.instance.tileMapData[curTileNum + 1].type == "전투")
                {
                    // 두 방향에 모두 전투 타일이 있는 경우 랜덤으로 왼쪽 또는 오른쪽으로 이동
                    PlayerMove(Random.Range(0, 2) == 0 ? -1 : 1, _moveCard);
                }
                else if (PlayManager.instance.tileMapData[curTileNum - 1].type == "전투")
                {
                    // 왼쪽에 전투 타일이 있는 경우 왼쪽으로 이동
                    PlayerMove(-1, _moveCard);
                }
                else if (PlayManager.instance.tileMapData[curTileNum + 1].type == "전투")
                {
                    // 오른쪽에 전투 타일이 있는 경우 오른쪽으로 이동
                    PlayerMove(1,_moveCard);
                }
                else
                {
                    _moveCard.MoveEffect();
                }
                
                break;

            case "회복":              
                if (PlayerData.currentHP < 100 && allowHealing)
                {
                    PlayerData.currentHP += 10;
                    _moveCard.MoveEffect();                
                }
                moveCardDraw = true;
                break;

            // 수정 필요함
            case "준비":
                readyCount = int.Parse(_eft[1]);
                moveCardDraw = true; 
                _moveCard.MoveEffect();
                break;
           // 수정 필요함
            case "무시":
                if (_moveCard.moveCardData.name == "조심스러운 발걸음")
                {
                    if(PlayManager.instance.tileMapData[curTileNum].type == "함정")
                    {                        
                        moveCardDraw = true;
                        allowEffect = false;
                    }
                    else
                    {                        
                        EndCardEffect();
                    }
                }
                else if (_moveCard.moveCardData.name == "전략적 후퇴")
                {
                    allowEffect = false;
                    moveCardDraw = true;
                }
                break;

            case "제약":
                allowHealing = false;            
                break;
            default:
                break;
        }

    }
    
    public void PlayerMove(int _n, MoveCard _moveCard)
    {
        if (_n >0)
        {
            PlayerMoveFoward(_n, _moveCard);
        }
        else if(_n < 0)
        {
            PlayerMoveBack(_n, _moveCard);
        }
        else 
        {
            if (readyCount > 0) readyCount = 0;
            _moveCard.MoveEffect();         
        }
    }

    void PlayerMoveFoward(int _stack, MoveCard _moveCard)
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
        playerRb.DOPath(JumpPath, 2f, PathType.CatmullRom, PathMode.TopDown2D).SetEase(Ease.InCubic).OnComplete(() => PlayerMove(_stack, _moveCard));
    }

    void PlayerMoveBack(int _stack, MoveCard _moveCard)
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
        playerRb.DOPath(JumpPath, 2f, PathType.CatmullRom, PathMode.TopDown2D).SetEase(Ease.InCubic).OnComplete(() => PlayerMove(_stack, _moveCard));
    }

    public void EndCardEffect()
    {
        
        tileMap[curTileNum].TileEffect();     
    }

    void MoveCameraToTargetTile(MapTile _mapTile)
    {
        if (curTileNum < 2) return;
        Camera.main.transform.DOMove(new Vector3(_mapTile.transform.position.x, _mapTile.transform.position.y, Camera.main.transform.position.z), 1.5f);
    }
}
