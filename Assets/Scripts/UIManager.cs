using System.Collections;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject splashArt;
    [SerializeField]
    private bool menuOpen = false;
    [SerializeField]
    private GameObject menu;
    [SerializeField]
    private GameObject howToOpenMenu;
    [SerializeField]
    private GameObject howToCloseMenu;
    [SerializeField]
    private Slider healthBar;
    [SerializeField]
    private TextMeshProUGUI gameLostText;
    [SerializeField]
    private TextMeshProUGUI extraDeathReasonText;
    [SerializeField]
    private TextMeshProUGUI finalScore;
    [SerializeField]
    private TextMeshProUGUI dirtCleanedText;
    [SerializeField]
    private TextMeshProUGUI virusCleanedText;
    [SerializeField]
    private TextMeshProUGUI bacteriaCleanedText;

    [SerializeField]
    private TextMeshProUGUI spawnerCountUpgradeText;
    [SerializeField]
    private TextMeshProUGUI spawnerSpawnrateUpgradeText;
    [SerializeField]
    private TextMeshProUGUI bubbleLifetimeUpgradeText;
    [SerializeField]
    private TextMeshProUGUI bubbleSpeedUpgradeText;
    [SerializeField]
    private TextMeshProUGUI bubbleSizeUpgradeText;
    [SerializeField]
    private TextMeshProUGUI blowerSpeedUpgradeText;

    private void Start()
    {
        healthBar.maxValue = GameManager.Instance.MaxBabyHealth;
        gameLostText.transform.parent.gameObject.SetActive(false);
        UpdateMenuState();
        StartCoroutine(UIUpdate());
    }

    private IEnumerator UIUpdate()
    {
        while (true)
        {
            //splash art
            splashArt.SetActive(GameManager.Instance.IsStartGamePause);

            //health
            healthBar.value = GameManager.Instance.BabyHealth;

            //kill stats
            var enemyKillStatistics = GameManager.Instance.enemyKillStatistics;
            dirtCleanedText.text = $"{(enemyKillStatistics.TryGetValue("Dirt", out var dirtCount) ? dirtCount : 0)} Dirt";
            virusCleanedText.text = $"{(enemyKillStatistics.TryGetValue("Virus", out var virusCount) ? virusCount : 0)} Virus";
            bacteriaCleanedText.text = $"{(enemyKillStatistics.TryGetValue("Bacteria", out var bacteriaCount) ? bacteriaCount : 0)} Bacteria";

            //death message
            if (GameManager.Instance.DeathReasonText != string.Empty)
            {
                gameLostText.transform.parent.gameObject.SetActive(true);
                gameLostText.text = GameManager.Instance.DeathReasonText;
                extraDeathReasonText.text = GameManager.Instance.ExtraDeathReasonText;
                finalScore.text = $"FINAL SCORE: {GameManager.Instance.enemyKillStatistics.Select(keyValue => keyValue.Value).Aggregate(0, (value, total) => total + value)}";
            }

            //upgrades
            spawnerCountUpgradeText.text = $"Spawners lvl. {BubbleSpawnerUpgradeManager.Instance.bubbleSpawnerCountUpgrade.GetLevel()}";
            spawnerSpawnrateUpgradeText.text = $"Spawnrate lvl. {BubbleSpawnerUpgradeManager.Instance.bubbleSpawnerSpawnSpeedUpgrade.GetLevel()}";
            bubbleLifetimeUpgradeText.text = $"Lifetime lvl. {BubbleUpgradeManager.Instance.bubbleLifetimeUpgrade.GetLevel()}";
            bubbleSizeUpgradeText.text = $"Size lvl. {BubbleUpgradeManager.Instance.bubbleSizeUpgrade.GetLevel()}";
            bubbleSpeedUpgradeText.text = $"Speed lvl. {BubbleUpgradeManager.Instance.bubbleSpeedUpgrade.GetLevel()}";
            blowerSpeedUpgradeText.text = $"Rotation speed lvl. {BlowerUpgradeManager.Instance.blowerRotationSpeedUpgrade.GetLevel()}";

            yield return new WaitForSeconds(0.1f);
        }
    }


    private void Update()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) || Gamepad.current?.leftStick.ReadValue()[1] > 0.1)
        {
            CloseMenu();
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow) || Gamepad.current?.leftStick.ReadValue()[1] < -0.1)
        {
            OpenMenu();
        }
    }

    private void OpenMenu()
    {
        menuOpen = true;
        UpdateMenuState();

    }

    private void CloseMenu()
    {
        menuOpen = false;
        UpdateMenuState();
    }

    private void UpdateMenuState()
    {
        menu.SetActive(menuOpen);
        howToOpenMenu.SetActive(!menuOpen);
        howToCloseMenu.SetActive(menuOpen);
    }
}
