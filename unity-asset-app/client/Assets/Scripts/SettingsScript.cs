using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsScript : MonoBehaviour
{

    private string currentName;
    private string currentID;
    private string currentPrivateKey;

    public Text namePlaceholder;
    public Text idPlaceholder;
    public Text privateKeyPlaceholder;

    // Start is called before the first frame update
    void Start()
    {
        currentName = PlayerPrefs.GetString(PlayerDataKeys.name);
        currentID = PlayerPrefs.GetString(PlayerDataKeys.id);
        currentPrivateKey = PlayerPrefs.GetString(PlayerDataKeys.privateKey);

        UpdatePlaceholders();
    }

    private void UpdatePlaceholders()
    {
        namePlaceholder.text = PlayerPrefs.GetString(PlayerDataKeys.name);
        idPlaceholder.text = PlayerPrefs.GetString(PlayerDataKeys.id);
        privateKeyPlaceholder.text = PlayerPrefs.GetString(PlayerDataKeys.privateKey);
    }

    public void UpdateName(string newText)
    {
        currentName = newText;
    }

    public void UpdateId(string newText)
    {
        currentID = newText;
    }

    public void UpdatePrivateKey(string newText)
    {
        currentPrivateKey = newText;
    }

    public void Apply()
    {
        PlayerPrefs.SetString(PlayerDataKeys.name, currentName);
        PlayerPrefs.SetString(PlayerDataKeys.id, currentID);
        PlayerPrefs.SetString(PlayerDataKeys.privateKey, currentPrivateKey);
        UpdatePlaceholders();
    }
}
