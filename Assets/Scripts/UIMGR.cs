using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // UI 관련 코드
using UnityEngine.XR.WSA;

public class UIMGR : MonoBehaviour
{
    // 싱글톤 접근용 프로퍼티
    public static UIMGR instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<UIMGR>();
            }

            return m_instance;
        }
    }

    private static UIMGR m_instance; // 싱글톤이 할당될 변수
    private string startScene = "StartScene";
    private string levelScene = "LevelScene";
    private string gameScene = "GameScene";
    private Image playerImage;
    private int maxPlayer = 15;
    private int maxSlot = 6;


    public GameObject gameoverUI; // 게임 오버시 활성화할 UI 
    public Text coinText; // 점수 표시용 텍스트
    public Text zenText; // 점수 표시용 텍스트
    public bool loadingend = false;

    private void Start()
    {
        // 시작과 동시에 현재 코인 및 젠 표시
        UpdateCoinText(GameManager.instance.coin);
        UpdateZenText(GameManager.instance.zen);
        loadingend = false;
        if (SceneManager.GetActiveScene().name == startScene)
        {
            int countFull = 0;
            GameObject.Find("Canvas").transform.Find("PlayerUI").gameObject.SetActive(true);
            for (int i = 0; i < GameManager.instance.humen.Length; i++)
            {
                playerImage = Instantiate(GameManager.instance.humen[i].playerImage);
                playerImage.transform.SetParent(GameObject.Find("Player (" + i + ")").transform);
                playerImage.transform.SetAsFirstSibling();
                playerImage.GetComponent<RectTransform>().localPosition = new Vector3(0f, 5f, 0f);
                playerImage.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
                GameObject.Find("Player (" + i + ")").transform.Find("Coin").transform.Find("Text").GetComponent<Text>().text = GameManager.instance.humen[i].cost.ToString();

                if (GameManager.instance.humen[i].isOpened)
                {
                    GameObject.Find("Player (" + i + ")").transform.Find("Coin").gameObject.SetActive(true);
                }
                else
                {
                    GameObject.Find("Player (" + i + ")").transform.Find("Mask").gameObject.SetActive(true);
                    GameObject.Find("Player (" + i + ")").transform.Find("Mask").transform.Find("Cost").GetComponent<Text>().text = GameManager.instance.humen[i].opencost.ToString();
                }
            }
            for (int i = GameManager.instance.humen.Length; i < maxPlayer; i++)
            {
                GameObject.Find("Player (" + i + ")").transform.Find("ComingSoon").gameObject.SetActive(true);
            }
            for(int i = 0; i < GameManager.instance.seletedPlayer.Length; i++)
            {
                if (GameManager.instance.seletedPlayer[i] != -1)
                {
                    playerImage = Instantiate(GameManager.instance.humen[GameManager.instance.seletedPlayer[i]].playerImage);
                    playerImage.transform.SetParent(GameObject.Find("Slot" + i).transform.Find("Image").transform);
                    playerImage.GetComponent<RectTransform>().localPosition = new Vector3(0f, -5f);
                    playerImage.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
                    GameObject.Find("Slot" + i).transform.Find("Coin").gameObject.SetActive(true);
                    GameObject.Find("Slot" + i).transform.Find("Coin").transform.Find("Text").GetComponent<Text>().text = GameManager.instance.humen[GameManager.instance.seletedPlayer[i]].cost.ToString();
                    GameObject.Find("Player (" + GameManager.instance.seletedPlayer[i] + ")").transform.Find("SelectedMask").gameObject.SetActive(true);
                    GameObject.Find("Slot" + i).GetComponent<Button>().enabled = true;
                    countFull++;
                }
            }
            if (countFull == maxSlot)
            {
                for (int i = 0; i < GameManager.instance.humen.Length; i++)
                {
                    GameObject.Find("Player (" + i + ")").transform.Find("FullSelectedMask").gameObject.SetActive(true);
                }
            }

            GameObject.Find("Canvas").transform.Find("PlayerUI").gameObject.SetActive(false);
        }
            if (SceneManager.GetActiveScene().name == gameScene)
        {
            
            for (int i =0; i< GameManager.instance.seletedPlayer.Length; i++)
            {
                if (GameManager.instance.seletedPlayer[i] == -1)
                {
                    GameObject.Find("Slot" + (i + 1)).GetComponent<Image>().color = Color.black;
                    GameObject.Find("Slot" + (i + 1)).transform.Find("Coin").gameObject.SetActive(false);
                }
                else
                {
                    playerImage = Instantiate(GameManager.instance.humen[GameManager.instance.seletedPlayer[i]].playerImage);
                    playerImage.transform.SetParent(GameObject.Find("Slot" + (i + 1)).transform.Find("Image").transform);
                    playerImage.GetComponent<RectTransform>().localPosition = new Vector3(0f, -5f);
                    playerImage.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
                    GameObject.Find("Slot" + (i + 1)).transform.Find("Coin").transform.Find("Text").GetComponent<Text>().text = GameManager.instance.humen[GameManager.instance.seletedPlayer[i]].cost.ToString();
                }
            }
            StartCoroutine("LoadingDisplayFunction");
        }
    }

    /////////////////////////////공통////////////////////////////////////
    public void HomeButtonFunction()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(startScene);
    }
    public void LevelButtonFunction()
    {
        SceneManager.LoadScene(levelScene);
    }

    public void ExitButtonFunction()
    {
        Debug.Log("게임종료");
        Application.Quit();
    }

    // 코인 개수 표시
    public void UpdateCoinText(int newCoin)
    {
        if (newCoin < 100000)
        {
            coinText.text = newCoin.ToString();
        }
        else
        {
            int kilocoin = newCoin / 1000;
            coinText.text = kilocoin + "k";
        }
    }

    // 젠 개수 표시
    public void UpdateZenText(int newZen)
    {
        zenText.text = newZen.ToString();
    }





    /////////////////////////////StartScene에서////////////////////////////////////



    public void RuleButtonFunction()
    {
        GameObject.Find("Canvas").transform.Find("RuleUI").gameObject.SetActive(true);
    }

    public void ExitRuleButtonFunction()
    {
        GameObject.Find("Canvas").transform.Find("RuleUI").gameObject.SetActive(false);
    }

    public void OptionButtonFunction()
    {
        GameObject.Find("Canvas").transform.Find("OptionUI").gameObject.SetActive(true);
    }

    public void ExitOptionButtonFunction()
    {
        GameObject.Find("Canvas").transform.Find("OptionUI").gameObject.SetActive(false);
    }

    public void PlayerButtonFuntion()
    {
        GameObject.Find("Canvas").transform.Find("PlayerUI").gameObject.SetActive(true);
    }
    public void ExitPlayerButtonFuntion()
    {
        GameObject.Find("Canvas").transform.Find("PlayerUI").gameObject.SetActive(false);
    }

    public void OpenCharacterButtonFunction(int id)
    {
        if (GameManager.instance.zen >= GameManager.instance.humen[id].opencost)
        {
            GameManager.instance.RemoveZen(GameManager.instance.humen[id].opencost);
            GameManager.instance.humen[id].isOpened = true;
            GameObject.Find("Player (" + id + ")").transform.Find("Mask").gameObject.SetActive(false);
            GameObject.Find("Player (" + id + ")").transform.Find("Coin").gameObject.SetActive(true);
        }
    }

    public void SelectCharacterButtonFunction(int id)
    {
        int[] emptySlot = new int[6];
        int countEmpty = 0;
        for(int i=0; i< maxSlot; i++)
        {
            if(GameManager.instance.seletedPlayer[i] == -1)
            {
                emptySlot[countEmpty] = i;
                countEmpty++;
            }
        }
        GameManager.instance.seletedPlayer[emptySlot[0]] = id;
        playerImage = Instantiate(GameManager.instance.humen[id].playerImage);
        playerImage.transform.SetParent(GameObject.Find("Slot" + emptySlot[0]).transform.Find("Image").transform);
        playerImage.GetComponent<RectTransform>().localPosition = new Vector3(0f, -5f);
        playerImage.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
        GameObject.Find("Slot" + emptySlot[0]).transform.Find("Coin").gameObject.SetActive(true);
        GameObject.Find("Slot" + emptySlot[0]).transform.Find("Coin").transform.Find("Text").GetComponent<Text>().text = GameManager.instance.humen[id].cost.ToString();
        GameObject.Find("Player (" + id + ")").transform.Find("SelectedMask").gameObject.SetActive(true);
        GameObject.Find("Slot" + emptySlot[0]).GetComponent<Button>().enabled = true;
        if (countEmpty == 1)
        {
            for(int i = 0; i < GameManager.instance.humen.Length; i++)
            {
                GameObject.Find("Player (" + i + ")").transform.Find("FullSelectedMask").gameObject.SetActive(true);
            }
        }
    }

    public void RemoveCharacterButtonFunction(int id)
    {
        int countEmpty = 0;
        for (int i = 0; i < maxSlot; i++)
        {
            if (GameManager.instance.seletedPlayer[i] == -1)
            {
                countEmpty++;
            }
        }
        if (countEmpty == 0)
        {
            for (int i = 0; i < GameManager.instance.humen.Length; i++)
            {
                GameObject.Find("Player (" + i + ")").transform.Find("FullSelectedMask").gameObject.SetActive(false);
            }
        }

        GameObject.Find("Player (" + GameManager.instance.seletedPlayer[id] + ")").transform.Find("SelectedMask").gameObject.SetActive(false);
        GameManager.instance.seletedPlayer[id] = -1;
        Destroy(GameObject.Find("Slot" + id).transform.Find("Image").GetChild(0).gameObject);
        GameObject.Find("Slot" + id).transform.Find("Coin").gameObject.SetActive(false);
        GameObject.Find("Slot" + id).GetComponent<Button>().enabled = false;
    }


    ////////////////////////////////GameScene에서//////////////////////////////////////////////////
    public IEnumerator LoadingDisplayFunction()
    {
        GameObject.Find("LoadingUI").transform.Find("LvTitle").transform.Find("Text").GetComponent<Text>().text = "Level."+ GameManager.instance.currentlevel;
        GameObject.Find("LoadingUI").transform.Find("Text").GetComponent<Text>().text = "Loading";
        yield return new WaitForSeconds(0.5f);
        GameObject.Find("LoadingUI").transform.Find("Text").GetComponent<Text>().text = "Loading.";
        yield return new WaitForSeconds(0.5f);
        GameObject.Find("LoadingUI").transform.Find("Text").GetComponent<Text>().text = "Loading..";
        yield return new WaitForSeconds(0.5f);
        GameObject.Find("LoadingUI").transform.Find("Text").GetComponent<Text>().text = "Loading...";
        yield return new WaitForSeconds(0.5f);
        GameObject.Find("LoadingUI").transform.Find("Text").GetComponent<Text>().text = "GameStart!";
        yield return new WaitForSeconds(0.5f);
        GameObject.Find("LoadingUI").SetActive(false);
        loadingend = true;
    }


    public void PauseButtonFunction()
    {
        Time.timeScale = 0;
        GameObject.Find("Canvas").transform.Find("PauseUI").gameObject.SetActive(true);
    }

    //게임씬에서 옵션창에서 스타트버튼(노란색세모)를 누르면 이어서 게임시작
    public void UnPauseButtonFunction()
    {
        Time.timeScale = 1;
        GameObject.Find("Canvas").transform.Find("PauseUI").gameObject.SetActive(false);
    }

    // Slot을 누르면 Slot에 해당하는 플레이어 생성
    public void ClickSlot(int slotnum)
    {
        GameObject.Find("PlayerSpawn").GetComponent<PlayerSpawner>().CreatePlayer(GameManager.instance.seletedPlayer[slotnum-1]);
    }

    public void DisableSlot(string Slotname)
    {
        GameObject.Find(Slotname).GetComponent<Button>().enabled = false;
        GameObject.Find(Slotname).GetComponent<Image>().color = Color.gray;
    }
    public void EnableSlot(string Slotname)
    {
        GameObject.Find(Slotname).GetComponent<Button>().enabled = true;
        GameObject.Find(Slotname).GetComponent<Image>().color = Color.white;
    }
    public void GameOverUI()
    {
        GameObject.Find("Canvas").transform.Find("GameOverUI").gameObject.SetActive(true);
        Time.timeScale = 0;
    }
    public void GameClearUI()
    {
        GameObject.Find("Canvas").transform.Find("GameClearUI").gameObject.SetActive(true);
        Time.timeScale = 0;

        //GameObject.Find("Canvas").transform.Find("Lv2_Open").gameObject.SetActive(true);

        //if (GameObject.Find("Canvas").transform.Find("Lv2_Open").gameObject.SetActive = true)
        //{
        //    GameObject.Find("Canvas").transform.Find("Lv3_Open").gameObject.SetActive(true);
        //}
        //else if(GameObject.Find("Canvas").transform.Find("Lv2_Open").gameObject.SetActive = true 
        //    && GameObject.Find("Canvas").transform.Find("Lv3_Open").gameObject.SetActive = true)
        //{
        //    GameObject.Find("Canvas").transform.Find("Lv4_Open").gameObject.SetActive(true);
        //}

    }

    ////////////////////////////////LevelScene에서//////////////////////////////////////////////////

    
    public void LevelSelectFunction(int lv)
    {
        SceneManager.LoadScene(gameScene);
        GameManager.instance.currentlevel = lv;
        Time.timeScale = 1;
        

    }
    

}
