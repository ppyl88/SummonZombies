using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // 싱글톤이 할당될 static 변수

    [System.Serializable]
    public class Human  // 아군 전체 리스트 및 스펙
    {
        public Player playerPrefab;     // 아군 프리펩
        public float health;            // 아군 체력
        public float damage;            // 아군 데미지
        public float speed;             // 아군 속도
        public float timeBetAttack;     // 아군 공격 간격
        public int cost;                // 아군 소환 비용
        public int level;               // 아군 레벨
        public Image playerImage;       // 아군 캐릭터 이미지
        public int opencost;            // 아군 오픈 비용
        public bool isOpened;            // 아군 캐릭터 오픈 여부
    }
    [System.Serializable]
    public class Zombie // 적군 전체 리스트 및 스펙
    {
        public Enemy enemyPrefab;       // 적군 프리펩
        public float health;            // 적군 체력
        public float damage;            // 적군 데미지
        public float speed;             // 적군 속도
        public float timeBetAttack;     // 적군 공격간격
        public float zenSpawnRate;      // 적군 젠 획득확률
    }
    [System.Serializable]
    public class Wave   // 적군 스폰 웨이브 스펙
    {
        public int[] zombieList;        // 적군 생성 목록
        public float spawnInterval;     // 적군 생성 간격
        public int maxEnemies;          // 적군 생성 개수
        public float waveInterval;      // 웨이브 간 간격
    }

    public Human[] humen; // 생성할 아군 AI
    public Zombie[] zombies; // 생성할 적 AI
    public Wave[] waves;

    public int coin = 0; // 현재 코인 개수
    public int zen = 0; // 현재 젠 개수
    public int currentlevel = 1;
    public int clearlevel = 0;

    public bool isGameover { get; private set; } // 게임 오버 상태
    public int[] seletedPlayer = new int[] { 0, -1, -1, -1, -1, -1 };     // 선택된 아군 리스트

    private void Awake()
    {
        // 씬에 싱글톤 오브젝트가 된 다른 GameManager 오브젝트가 있다면
        if(instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        if (instance != this)
        {
            // 자신을 파괴
            Destroy(gameObject);
        }
    }

    // 코인를 추가하고 UI 갱신
    public void AddCoin(int newCoin)
    {
        // 코인 추가
        coin += newCoin;
        // 점수 UI 텍스트 갱신
        UIMGR.instance.UpdateCoinText(coin);

    }
    // 코인를 감소시키고 UI 갱신
    public void RemoveCoin(int newCoin)
    {
        // 코인 감소
        coin -= newCoin;
        // 코인 UI 텍스트 갱신
        UIMGR.instance.UpdateCoinText(coin);
    }


    // 젠을 추가하고 UI 갱신
    public void AddZen(int newZen)
    {
        // 젠 추가
        zen += newZen;
        // 젠 UI 텍스트 갱신
        UIMGR.instance.UpdateZenText(zen);
    }

    // 젠을 감소시키고 UI 갱신
    public void RemoveZen(int newZen)
    {
        // 젠 감소
        zen -= newZen;
        // 젠 UI 텍스트 갱신
        UIMGR.instance.UpdateZenText(zen);
    }

    // 게임 오버 처리
    public void EndGame()
    {
        // 게임 오버 상태를 참으로 변경
        isGameover = true;
        // 게임 오버 UI를 활성화
        //UIManager.instance.SetActiveGameoverUI(true);
    }

}
