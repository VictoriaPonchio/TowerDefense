using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    [SerializeField]
    private GameObject gameOverMenu;
    private bool gameOver = false;

    /// <summary>
    /// Amount of money
    /// </summary>
    [SerializeField]
    private float currency;
    [SerializeField]
    private Text currencyValue;

    [SerializeField]
    GameObject startPanel;
    [SerializeField]
    GameObject infoPanel;

    [SerializeField]
    Text projectileCost;
    [SerializeField]
    GameObject projectileTower;
    [SerializeField]
    Text debuffCost;
    [SerializeField]
    GameObject debuffTower;
    [SerializeField]
    Text aoeCost;
    [SerializeField]
    GameObject aoeTower;

    Tower detailedTower;
    [SerializeField]
    GameObject upgradePanel;
    [SerializeField]
    GameObject statsPanel;
    [SerializeField]
    Text tooltipText;
    [SerializeField]
    Text sellCostText;
    [SerializeField]
    Text upgradeCostText;
    [SerializeField]
    Button upgradeButton;

    /// <summary>
    /// Wave indicator
    /// </summary>
    private int wave = 0;
    [SerializeField]
    private Text waveIndicator;
    [SerializeField]
    private GameObject waveBtn;
    /// <summary>
    /// Control the amount of enemies each wave
    /// </summary>
    int activeEnemies;
    [SerializeField]
    private int totalWaves = 20;
    TowerButton towerButton;

    /// <summary>
    /// 
    /// </summary>
    public TowerButton ClickedTower { get; private set; }

    public bool waveActive
    {
        get
        {
            return activeEnemies > 0;
        }
    }

    /// <summary>
    /// Regra para gerar waves
    /// </summary>
    public int hordeSize
    {
        get
        {
            return (wave * 3) + 2;
        }
    }

    public ObjectPool Pool { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        startPanel.SetActive(true);
        Currency = currency;
        projectileCost.text = $"<color=#FFFECE>{projectileTower.transform.GetChild(0).GetComponent<Tower>().cost}</color> <color=yellow>$</color>";
        debuffCost.text = $"<color=#FFFECE>{debuffTower.transform.GetChild(0).GetComponent<Tower>().cost}</color> <color=yellow>$</color>";
        aoeCost.text = $"<color=#FFFECE>{aoeTower.transform.GetChild(0).GetComponent<Tower>().cost}</color> <color=yellow>$</color>";
    }

    // Update is called once per frame
    void Update()
    {
        HandleEscape();
    }

    private void Awake()
    {
        Pool = GetComponent<ObjectPool>();
    }

    #region Game/Player Settings

    /// <summary>
    /// Format currency value
    /// </summary>
    public float Currency
    {
        get
        {
            return currency;
        }
        set
        {
            this.currency = value;
            this.currencyValue.text = value.ToString() + "<color=yellow>$</color> ";
        }
    }

    /// <summary>
    /// Finishes the game and shows game over menu
    /// </summary>
    public void GameOver()
    {
        if (!gameOver)
        {
            gameOver = true;
            infoPanel.SetActive(false);
            gameOverMenu.SetActive(true);
        }
    }

    /// <summary>
    /// Starts the game
    /// </summary>
    public void StartGame()
    {        
        Time.timeScale = 1;
        startPanel.SetActive(false);
    }
    
    /// <summary>
    /// Resets game scene
    /// </summary>
    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        startPanel.SetActive(false);
    }

    /// <summary>
    /// Closes the application
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }

    #endregion Game/Player Settings

    #region Enemy Spawn

    /// <summary>
    /// Calls the wave spawn and set active enemies
    /// </summary>
    public void StartHorde()
    {
        wave++;
        waveIndicator.text = $"Wave: {wave}/{totalWaves}";
        //Set the amount of enemies in current wave
        activeEnemies = hordeSize;
        StartCoroutine(SpawnHorde());
        waveBtn.SetActive(false);
    }

    /// <summary>
    /// Controls the enemey spawn
    /// </summary>
    /// <returns></returns>
    private IEnumerator SpawnHorde()
    {
        string type = string.Empty;
        int qtd1 = (hordeSize / 2) + 3;
        int qtd2 = hordeSize - qtd1;

        List<string> waveLst = new List<string>();

        for (int i = 0; i < qtd1; i++)
        {
            waveLst.Add("Enemy1");
        }
        for (int i = 0; i < qtd2; i++)
        {
            waveLst.Add("Enemy2");
        }
        for (int i = 3; i < qtd2; i += 3)
        {
            waveLst.Remove("Enemy2");
            waveLst.Add("Enemy3");
        }

        for (int i = 0; i < waveLst.Count; i++)
        {
            System.Random rnd = new System.Random();
            int index = rnd.Next(waveLst.Count);

            Enemy enemy = Pool.GetObject(waveLst[index]).GetComponent<Enemy>();
            enemy.Spawn();

            yield return new WaitForSeconds(2.0f);
        }
    }

    /// <summary>
    /// Removes enemy from enemies list 
    /// </summary>
    /// <param name="enemy">Enemy to be removed</param>
    public void RemoveEnemy()
    {
        activeEnemies--;
        //If there are no more enemies and the game isn't over, display the wave button
        if (!waveActive && !gameOver && activeEnemies == 0)
        {
            waveBtn.SetActive(true);
        }
    }

    #endregion

    #region Towers

    /// <summary>
    /// Render current clicked tower from button
    /// </summary>
    /// <param name="tower"></param>
    public void ChooseTower(TowerButton tower)
    {
        this.ClickedTower = tower;
        DeselectPlacedTower();
        Hover.Instance.Show(ClickedTower.Sprite, ClickedTower.RangeSize);
    }

    /// <summary>
    /// Removes instance from current tower and decrements the currency
    /// </summary>
    public void BuyTower(float cost)
    {
        Hover.Instance.Hide();
        Currency -= cost;
        this.ClickedTower = null;
    }

    /// <summary>
    /// Destroy the selected tower from level and increment currency
    /// </summary>
    public void SellTower()
    {
        if (detailedTower != null)
        {
            Currency += detailedTower.SellCost;
            detailedTower.Sell();
            DeselectPlacedTower();
        }
    }

    /// <summary>
    /// Upgrades current tower and updates next upgrade values
    /// </summary>
    public void UpgradeTower()
    {
        if (detailedTower != null)
        {
            if (detailedTower.NextUpgrade != null && Currency >= detailedTower.NextUpgrade.Cost)
            {
                Currency -= detailedTower.NextUpgrade.Cost;
                detailedTower.UpgradeTowerStats();
                if (detailedTower.NextUpgrade != null)
                {
                    upgradeButton.interactable = true;
                    upgradeCostText.text = $"<color=#FFFECE>{detailedTower.NextUpgrade.Cost}</color> <color=yellow>$</color>";
                }
                sellCostText.text = $"<color=#FFFECE>{detailedTower.SellCost}</color> <color=yellow>$</color>";
            }
        }
    }

    /// <summary>
    /// Select placed tower 
    /// </summary>
    /// <param name="tower">Tower</param>
    public void SelectPlacedTower<T>(T tower) where T : Tower
    {
        if (detailedTower != null)
        {
            detailedTower.Detail();
        }
        detailedTower = tower;
        detailedTower.Detail();
        if (detailedTower.NextUpgrade != null)
        {
            upgradeButton.interactable = true;           
            upgradeCostText.text = $"<color=#FFFECE>{detailedTower.NextUpgrade.Cost}</color> <color=yellow>$</color>";
        }
        else //Disable button if doesn't exist upgrade
            upgradeButton.interactable = false;

        sellCostText.text = $"<color=#FFFECE>{detailedTower.SellCost}</color> <color=yellow>$</color>";
        upgradePanel.SetActive(true);
    }

    /// <summary>
    /// Deselect placed tower 
    /// </summary>
    /// </summary>
    public void DeselectPlacedTower()
    {
        if (detailedTower != null)
        {
            detailedTower.Detail();
        }
        detailedTower = null;
        upgradePanel.SetActive(false);
    }

    /// <summary>
    /// Shows panel for current tower stats
    /// </summary>
    public void ShowStats()
    {
        statsPanel.SetActive(!statsPanel.activeSelf);
    }

    /// <summary>
    /// Update stats panel tooltip text
    /// </summary>
    /// <param name="txt">New tooltip text</param>
    public void SetTowerTooltip(string txt)
    {
        tooltipText.text = txt;
    }

    /// <summary>
    /// Shows upgrade panel for current tower upgrade
    /// </summary>
    public void ShowSelectTowerUpgrade()
    {
        statsPanel.SetActive(!statsPanel.activeSelf);
        UpdateUpgradeTooltip();
    }

    /// <summary>
    /// Update upgrade panel tooltip text
    /// </summary>
    /// <param name="txt">New tooltip text</param>
    public void UpdateUpgradeTooltip()
    {
        if (detailedTower != null)
        {
            SetTowerTooltip(detailedTower.GetStats());
        }
    }

    /// <summary>
    /// Removes clicked tower on 'esc'
    /// </summary>
    public void HandleEscape()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Hover.Instance.Hide();
            ClickedTower = null;
        }
    }

    #endregion

}
