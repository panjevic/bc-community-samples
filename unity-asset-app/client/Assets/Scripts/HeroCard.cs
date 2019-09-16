using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class HeroCard : MonoBehaviour {

    public Text heroDetailLabel;
    public Button transactionButton;
    public Text transactionLabel;
    public Image heroImage;
    public Item hero;
    
    // Use this for initialization
    void Start()
    {

    }

    public void Setup(Item hero, CardScrollList cardScrollList) {
        this.hero = hero;
        this.heroDetailLabel.text = hero.hero;
        this.transactionButton.transform.gameObject.SetActive(true);
        SetupHeroImageForId(hero.heroImageId);

        GameScreen currentScreen = GetComponentInParent<Canvas>().GetComponent<MenuScript>().currentGameScreen;

        switch (currentScreen) {
            case GameScreen.Heroes:
                {
                    if (PlayerPrefs.GetString(PlayerDataKeys.id) != hero.seller) 
                    { 
                        if (hero.onSale)
                        {
                            // Current player can cancel the sale of his hero.
                            SetupTransactionButton("Revoke", TextAnchor.MiddleLeft, "Ξ" + hero.price.ToString());
                        }
                        else
                        {
                            // Current player can put up for sale his own heroes that are not already for sale.
                            SetupTransactionButton("Sell", TextAnchor.MiddleCenter, "");
                        }
                    }
                    else
                    {
                        if (hero.onSale)
                        {
                            // Current player can buy a hero of another player that is for sale.
                            SetupTransactionButton("Buy", TextAnchor.MiddleLeft, "Ξ" + hero.price.ToString());
                        }
                        else
                        {
                            // Current player might be able to place an offer for a hero that is not for sale but he wants to buy.
                            SetupTransactionButton("Offer", TextAnchor.MiddleCenter, "");
                        }
                    }
                }
                break;
            case GameScreen.Store:
                {
                    // Buy hero directly from the developer for fiat currency - through the Apple App or Google Play Store.
                    SetupTransactionButton("Buy", TextAnchor.MiddleLeft, "$" + hero.price.ToString());
                }
                break;
            case GameScreen.Market:
                {
                    // Current player can buy a hero of another player that is for sale.
                    SetupTransactionButton("Buy", TextAnchor.MiddleLeft, "Ξ" + hero.price.ToString());
                }
                break;
            default:
                {

                }
                break;
        }
    }

    private void SetupHeroImageForId(int imageId)
    {
        Debug.Log(imageId);
        if (imageId < 0)
        {
            heroImage.gameObject.SetActive(true);
            switch (imageId)
            {
                case -1:
                    heroImage.color = new Color(1, 99.0f / 255.0f, 79.0f / 255);
                    Debug.Log("reveal hero image C");
                    break;
                case -2:
                    heroImage.color = new Color(1, 1, 1);
                    Debug.Log("reveal hero image U");
                    break;
                case -3:
                    heroImage.color = new Color(1, 201.0f / 255, 50.0f / 255);
                    Debug.Log("reveal hero image G");
                    break;
                case -4:
                    heroImage.color = new Color(1, 120.0f / 255, 235.0f / 255);
                    Debug.Log("reveal hero image L");
                    break;
            }

            return;
        }

        heroImage.gameObject.SetActive(false);
        Debug.Log("hide hero image");
    }

    private void SetupTransactionButton(string text, TextAnchor alignment, string price)
    {
        this.transactionLabel.text = text;
        this.transactionLabel.alignment = alignment;
        this.transactionButton.GetComponentInChildren<Text>().text = price;
    }

    public void initiateTransaction()
    {
        GameScreen currentScreen = GetComponentInParent<Canvas>().GetComponent<MenuScript>().currentGameScreen;

        switch (currentScreen)
        {
            case GameScreen.Heroes:
                {
                    if (PlayerPrefs.GetString(PlayerDataKeys.id) != hero.seller)
                    {
                        if (hero.onSale)
                        {
                            // Current player can cancel the sale of his hero.
                            Revoke();
                        }
                        else
                        {
                            // Current player can put up for sale his own heroes that are not already for sale.
                            Sell();
                        }
                    }
                    else
                    {
                        if (hero.onSale)
                        {
                            // Current player can buy a hero of another player that is for sale.
                            Buy();
                        }
                        else
                        {
                            // Current player might be able to place an offer for a hero that is not for sale but he wants to buy.
                            Offer();
                        }
                    }
                }
                break;
            case GameScreen.Store:
                {
                    // Buy hero directly from the developer for fiat currency - through the Apple App or Google Play Store.
                    BuyDirectly();
                }
                break;
            case GameScreen.Market:
                {
                    // Current player can buy a hero of another player that is for sale.
                    Buy();
                }
                break;
            default:
                {

                }
                break;
        }
    }

    public void BuyDirectly()
    {
        GetComponent<Purchaser>().BuyNonConsumable(hero.productId);
        Debug.Log("Buy pack from developers.");
    }

    public void Buy()
    {
        Debug.Log("Buy from player with id: " + hero.seller + ".");
        // TODO initiate transfer of hero and money between players.
    }

    public void Sell()
    {
        Debug.Log("Sell hero on Market.");
        string item = "{\n\n\u00a0\u00a0\u00a0\u00a0\"hero\": \"" + Random.Range(1000000, 9999999) + "\",\n\n\u00a0\u00a0\u00a0\u00a0\"price\": 0.02,\n\n\u00a0\u00a0\u00a0\u00a0\"seller\": \"0x123455677\"\n\n}";
        StartCoroutine(PostNewOffer(item));
    }

    public void Revoke()
    {
        // TODO player can cancel sale of his hero.
        Debug.Log("Stop selling the hero on the Market.");
    }

    public void Offer()
    {
        // TODO player can place an offer to buy another player's hero that is not currently on sale.
        Debug.Log("Offer to buy hero from player with id: " + hero.seller + ".");
    }

    IEnumerator PostNewOffer(string bodyJsonString)
    {
        string url = "https://prod-28.westeurope.logic.azure.com/workflows/ce14ab3732094b34818b88281fbe2f7f/triggers/manual/paths/invoke?api-version=2016-10-01&sp=%2Ftriggers%2Fmanual%2Frun&sv=1.0&sig=msHfv3FMPKXn7UZ6HNaFlPpW1uOO_eHr_ObWzbO0naE";
        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(bodyJsonString);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log("Upload Error: " + request.error);
        }
        else
        {
            Debug.Log("Form upload complete!");
        }
    }
}
