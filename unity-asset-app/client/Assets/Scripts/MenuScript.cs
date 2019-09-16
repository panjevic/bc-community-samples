using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameScreen { Heroes, Store, Market, Settings };

public static class PlayerDataKeys
{
    public static string name = "PlayerDataKey-Name";
    public static string privateKey = "PlayerDataKey-PrivateKey";
    public static string id = "PlayerDataKey-Id";
}

public class MenuScript : MonoBehaviour
{
    public ScrollRect scrollView;
    public RectTransform settingsPanel;

    public GameScreen currentGameScreen;
    public Text screenTitle;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start Menu Script");
        PlayerPrefs.SetString(PlayerDataKeys.id, "0x3ce63a2d1cEe07Fc476fAbfb8b24755006935f5C");
        PlayerPrefs.SetString(PlayerDataKeys.name, "Warthorius");
        PlayerPrefs.SetString(PlayerDataKeys.privateKey, "kojekude");
        LoadHeroes();
    }

    public void LoadSettings()
    {
        Debug.Log("Load Settings");
        scrollView.gameObject.SetActive(false);
        settingsPanel.gameObject.SetActive(true);

        currentGameScreen = GameScreen.Settings;
        screenTitle.text = "Settings";
    }

    public void LoadStore()
    {
        Debug.Log("Load Store");
        scrollView.gameObject.SetActive(true);
        settingsPanel.gameObject.SetActive(false);
        scrollView.content.position = new Vector2(0, 0);
        currentGameScreen = GameScreen.Store;

        screenTitle.text = "Store";
        scrollView.gameObject.GetComponentInChildren<CardScrollList>().LoadStore();
    }

    public void LoadMarket()
    {
        Debug.Log("Load Market");
        scrollView.gameObject.SetActive(true);
        settingsPanel.gameObject.SetActive(false);
        scrollView.content.position = new Vector2(0, 0);
        currentGameScreen = GameScreen.Market;

        screenTitle.text = "Market";
        scrollView.gameObject.GetComponentInChildren<CardScrollList>().LoadMarket();
    }

    public void LoadHeroes()
    {
        Debug.Log("Load Heroes");
        scrollView.gameObject.SetActive(true);
        settingsPanel.gameObject.SetActive(false);
        scrollView.content.position = new Vector2(0, 0);
        currentGameScreen = GameScreen.Heroes;

        screenTitle.text = PlayerPrefs.GetString(PlayerDataKeys.name) + "'s Heroes";
        scrollView.gameObject.GetComponentInChildren<CardScrollList>().LoadHeroes();
    }

}

