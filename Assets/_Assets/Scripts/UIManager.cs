using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    [SerializeField] Animator goldAnimator;
    [Header("UI")]
    [SerializeField] GameObject winPanel;
    [SerializeField] GameObject losePanel;
    [SerializeField] GameObject startImage;
    [SerializeField] GameObject pausePanel;
    [SerializeField] GameObject pauseBtn;
    [SerializeField] GameObject buyBtn;
    [SerializeField] GameObject huntBtn;
    [Header("Text")]
    [SerializeField] TextMeshProUGUI gold;
    [SerializeField] TextMeshProUGUI rewardGold;
    [SerializeField] TextMeshProUGUI lvlText;
    bool isStart = true;
    bool isEnd = false;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        lvlText.text = "lvl " + SaveLoadSystem.instance.CurrentData.lvl + 1;
        if (SaveLoadSystem.instance.CurrentData.gold > 999)
        {
            gold.text = SaveLoadSystem.instance.CurrentData.gold / 1000 + "k";
            return;
        }
        gold.text = SaveLoadSystem.instance.CurrentData.gold + "";
    }
    private void Update()
    {
        StartingFunc();
    }
    void StartingFunc()
    {
        if (Input.GetMouseButtonDown(0) && isStart)
        {
            isStart = false;
            startImage.SetActive(false);
            buyBtn.SetActive(true);
            GameManager.instance.StartAnimalPlaces();
        }
    }
    public void WinGame()
    {
        if (isEnd)
        {
            return;
        }
        ShowWinGameUI();
        isEnd = true;
    }
    public void LoseGame()
    {
        if (isEnd)
        {
            return;
        }
        ShowLoseGameUI();
        isEnd = true;
    }
    public void EnableHuntBtn()
    {
        huntBtn.SetActive(true);
    }
    public void PauseGame()
    {
        pausePanel.SetActive(true);
        pauseBtn.SetActive(false);
        Time.timeScale = 0;
    }
    public void UnPauseGame()
    {
        pausePanel.SetActive(false);
        pauseBtn.SetActive(true);
        Time.timeScale = 1;
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }
    public void ShowWinGameUI()
    {
        Reward(GameManager.instance.goldIncome);
        winPanel.SetActive(true);
        gold.transform.parent.gameObject.SetActive(false);
        pauseBtn.SetActive(false);
    }
    public void ShowLoseGameUI()
    {
        losePanel.SetActive(true);
        gold.transform.parent.gameObject.SetActive(false);
        pauseBtn.SetActive(false);
    }
    public void LoseGameUI()
    {
        RestartGame();
    }
    public void WinGameUI()
    {
        GameManager.instance.WinGameUI();
        RestartGame();
    }
    public void ToHunt()
    {
        buyBtn.SetActive(false);
        huntBtn.SetActive(false);
    }
    public bool BuyStuff(int price)
    {
        if (price <= SaveLoadSystem.instance.CurrentData.gold)
        {
            ChangeGold(price);
            return true;
        }
        NotEnoughGold();
        return false;
    }
    void ChangeGold(int gold)
    {
        goldAnimator.SetTrigger("Pay");
        SaveLoadSystem.instance.CurrentData.gold -= gold;
        SaveLoadSystem.instance.SaveDataIntoFile();
        if(SaveLoadSystem.instance.CurrentData.gold > 999)
        {
            this.gold.text = SaveLoadSystem.instance.CurrentData.gold / 1000 + "k";
            return;
        }
        this.gold.text = SaveLoadSystem.instance.CurrentData.gold + "";
    }
    void NotEnoughGold()
    {
        goldAnimator.SetTrigger("NoGold");
    }
    public void Reward(int goldAmount)
    {
        StartCoroutine(RewardGoldIE(goldAmount));
    }
    IEnumerator RewardGoldIE(int amount)
    {
        for (int i = 0; i <= amount; i++)
        {
            rewardGold.text = i + "";
            yield return new WaitForFixedUpdate();
        }
    }
}