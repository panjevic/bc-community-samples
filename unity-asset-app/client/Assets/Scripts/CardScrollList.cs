using System.Collections.Generic;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.Purchasing;
using Nethereum.JsonRpc.UnityClient;
using Nethereum.Contracts;
using Nethereum.RPC.Eth.DTOs;

[System.Serializable]
public class Item 
{
    public string hero;
    public double price;
    public bool onSale = true;
    public string seller;
    public string productId = "";
    public int heroImageId = -1;
}

[System.Serializable]
public class HeroList
{
    public List<Item> list;
}

public enum Rarity {Common, Uncommon, Rare, Legendary};

public class CardScrollList : MonoBehaviour {

    // List of heroes, objects of Hero class.
    public HeroList heroList;
    // A pool of Hero prefabs, used for optimization purposes.
    public HeroCardPool heroCardPool;
    public Transform contentPanel;

    public ScrollRect scrollView;


    // We define the ABI of the contract we are going to use.
    public static string ABI = @"[{""constant"": true, ""inputs"": [{ ""name"": """", ""type"": ""uint256""}],""name"": ""tokenProperty"", ""outputs"": [  { ""name"": ""name"", ""type"": ""string""  },  { ""name"": ""wisdom"", ""type"": ""uint8""  },  { ""name"": ""inteligence"", ""type"": ""uint8""  },  { ""name"": ""charisma"", ""type"": ""uint8""  },  { ""name"": ""speed"", ""type"": ""uint8""  },  { ""name"": ""accuracy"", ""type"": ""uint8""  },  { ""name"": ""might"", ""type"": ""uint8""  } ], ""payable"": false, ""stateMutability"": ""view"", ""type"": ""function"", ""signature"": ""0x08c243aa"" }, { ""inputs"": [], ""payable"": false, ""stateMutability"": ""nonpayable"", ""type"": ""constructor"", ""signature"": ""constructor"" }, { ""anonymous"": false, ""inputs"": [  { ""indexed"": true, ""name"": ""previousOwner"", ""type"": ""address""  },  { ""indexed"": true, ""name"": ""newOwner"", ""type"": ""address""  } ], ""name"": ""OwnershipTransferred"", ""type"": ""event"", ""signature"": ""0x8be0079c531659141344cd1fd0a4f28419497f9722a3daafe3b4186f6b6457e0"" }, { ""anonymous"": false, ""inputs"": [  { ""indexed"": false, ""name"": ""from"", ""type"": ""address""  },  { ""indexed"": false, ""name"": ""to"", ""type"": ""address""  },  { ""indexed"": false, ""name"": ""tokenId"", ""type"": ""uint256""  } ], ""name"": ""Transfer"", ""type"": ""event"", ""signature"": ""0xddf252ad1be2c89b69c2b068fc378daa952ba7f163c4a11628f55a4df523b3ef"" }, { ""constant"": true, ""inputs"": [], ""name"": ""owner"", ""outputs"": [  { ""name"": """", ""type"": ""address""  } ], ""payable"": false, ""stateMutability"": ""view"", ""type"": ""function"", ""signature"": ""0x8da5cb5b"" }, { ""constant"": true, ""inputs"": [], ""name"": ""isOwner"", ""outputs"": [  { ""name"": """", ""type"": ""bool""  } ], ""payable"": false, ""stateMutability"": ""view"", ""type"": ""function"", ""signature"": ""0x8f32d59b"" }, { ""constant"": false, ""inputs"": [], ""name"": ""renounceOwnership"", ""outputs"": [], ""payable"": false, ""stateMutability"": ""nonpayable"", ""type"": ""function"", ""signature"": ""0x715018a6"" }, { ""constant"": false, ""inputs"": [  { ""name"": ""newOwner"", ""type"": ""address""  } ], ""name"": ""transferOwnership"", ""outputs"": [], ""payable"": false, ""stateMutability"": ""nonpayable"", ""type"": ""function"", ""signature"": ""0xf2fde38b"" }, { ""constant"": true, ""inputs"": [  { ""name"": ""tokenId"", ""type"": ""uint256""  } ], ""name"": ""tokenURI"", ""outputs"": [  { ""name"": """", ""type"": ""string""  } ], ""payable"": false, ""stateMutability"": ""view"", ""type"": ""function"", ""signature"": ""0xc87b56dd"" }, { ""constant"": true, ""inputs"": [  { ""name"": ""tokenId"", ""type"": ""uint256""  } ], ""name"": ""ownerOf"", ""outputs"": [  { ""name"": """", ""type"": ""address""  } ], ""payable"": false, ""stateMutability"": ""view"", ""type"": ""function"", ""signature"": ""0x6352211e"" }, { ""constant"": true, ""inputs"": [  { ""name"": ""_tmpOwner"", ""type"": ""address""  } ], ""name"": ""balanceOf"", ""outputs"": [  { ""name"": """", ""type"": ""uint256""  } ], ""payable"": false, ""stateMutability"": ""view"", ""type"": ""function"", ""signature"": ""0x70a08231"" }, { ""constant"": false, ""inputs"": [  { ""name"": ""_to"", ""type"": ""address""  },  { ""name"": ""_name"", ""type"": ""string""  },  { ""name"": ""_wisdom"", ""type"": ""uint8""  },  { ""name"": ""_inteligence"", ""type"": ""uint8""  },  { ""name"": ""_charisma"", ""type"": ""uint8""  },  { ""name"": ""_speed"", ""type"": ""uint8""  },  { ""name"": ""_accuracy"", ""type"": ""uint8""  },  { ""name"": ""_might"", ""type"": ""uint8""  } ], ""name"": ""mintCharacter"", ""outputs"": [  { ""name"": """", ""type"": ""bool""  } ], ""payable"": false, ""stateMutability"": ""nonpayable"", ""type"": ""function"", ""signature"": ""0x743ac699"" }, { ""constant"": false, ""inputs"": [  { ""name"": ""_to"", ""type"": ""address""  },  { ""name"": ""_name"", ""type"": ""string""  } ], ""name"": ""mint"", ""outputs"": [  { ""name"": """", ""type"": ""bool""  } ], ""payable"": false, ""stateMutability"": ""nonpayable"", ""type"": ""function"", ""signature"": ""0xd0def521"" }, { ""constant"": true, ""inputs"": [  { ""name"": ""_tokenId"", ""type"": ""uint256""  } ], ""name"": ""getToken"", ""outputs"": [  { ""name"": """", ""type"": ""string""  },  { ""name"": """", ""type"": ""uint8""  },  { ""name"": """", ""type"": ""uint8""  },  { ""name"": """", ""type"": ""uint8""  },  { ""name"": """", ""type"": ""uint8""  },  { ""name"": """", ""type"": ""uint8""  },  { ""name"": """", ""type"": ""uint8""  } ], ""payable"": false, ""stateMutability"": ""view"", ""type"": ""function"", ""signature"": ""0xe4b50cb8"" }, { ""constant"": true, ""inputs"": [], ""name"": ""getTokenCount"", ""outputs"": [  { ""name"": """", ""type"": ""uint8""  } ], ""payable"": false, ""stateMutability"": ""view"", ""type"": ""function"", ""signature"": ""0x78a89567"" }, { ""constant"": false, ""inputs"": [  { ""name"": ""from"", ""type"": ""address""  },  { ""name"": ""to"", ""type"": ""address""  },  { ""name"": ""tokenId"", ""type"": ""uint256""  } ], ""name"": ""transferFrom"", ""outputs"": [], ""payable"": false, ""stateMutability"": ""nonpayable"", ""type"": ""function"", ""signature"": ""0x23b872dd"" }, { ""constant"": false, ""inputs"": [  { ""name"": ""from"", ""type"": ""address""  },  { ""name"": ""to"", ""type"": ""address""  },  { ""name"": ""tokenId"", ""type"": ""uint256""  } ], ""name"": ""safeTransferFrom"", ""outputs"": [], ""payable"": false, ""stateMutability"": ""nonpayable"", ""type"": ""function"", ""signature"": ""0x42842e0e"" }]";

    // And we define the contract address here, in this case is a simple ping contract
    // (Remember this contract is deployed on the ropsten network)
    private static string contractAddress = "0x61BD22e83BA8516cEBD4b06124a0bA7BA01d3919";

    private static string _url = "https://ropsten.infura.io/";

    // We define a new contract (Netherum.Contracts)
    private Contract contract;

    // Use this for initialization.
    void Start () {
        // Here we assign the contract as a new contract and we send it the ABI and contact address
        this.contract = new Contract(null, ABI, contractAddress);

        ReloadData();
	}

    IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            if (webRequest.isNetworkError)
            {
                Debug.Log(pages[page] + ": Error: " + webRequest.error);
            }
            else
            {
                Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                heroList = JsonUtility.FromJson<HeroList>("{\"list\":" + webRequest.downloadHandler.text + "}");
                for (int i = 0; i < heroList.list.Count; i++)
                {
                    heroList.list[i].heroImageId = GetHeroImageId();
                }
                ReloadData();
            }
        }
    }

    private void ReloadData()
    {
        RemoveCards();
        AddCards();
    }
    

    public IEnumerator getCharacters(string address)
    {
        // We create a new pingsRequest as a new Eth Call Unity Request
        var charsRequest = new EthCallUnityRequest(_url);

        var charsCallInput = CreateCharsCallInput(address);
        Debug.Log("Getting characters from the blockchain...");
        // Then we send the request with the pingsCallInput and the most recent block mined to check.
        // And we wait for the response...
        yield return charsRequest.SendRequest(charsCallInput, Nethereum.RPC.Eth.DTOs.BlockParameter.CreateLatest());

        if (charsRequest.Exception == null)
        {
            // If we don't have exceptions we just display the raw result and the
            // result decode it with our function (decodePings) from the service, congrats!
            Debug.Log("Chars count (HEX): " + charsRequest.Result);
            Debug.Log("Chars count (INT):" + DecodeChars(charsRequest.Result));
        }
        else
        {
            // if we had an error in the UnityRequest we just display the Exception error
            Debug.Log("Error submitting getPings tx: " + charsRequest.Exception.Message);
        }
    }

    public int DecodeChars(string chars)
    {
        // We use this function later to parse the result of encoded pings (Hexadecimal 0x0f)
        // into a decoded output for easier readability (Integer 15)
        var function = GetBalanceFunction();
        return function.DecodeSimpleTypeOutput<int>(chars);
    }

    public CallInput CreateCharsCallInput( string address )
    {
        // For this transaction to the contract we dont need inputs,
        // its only to retreive the quantity of Ping transactions we did. (the pings variable on the contract)
        var function = GetBalanceFunction();
        return function.CreateCallInput(address);
    }
    public Function GetBalanceFunction()
    {
        return contract.GetFunction("balanceOf");
    }

    public void LoadHeroes() {
        RectTransform scrollViewTransform = scrollView.GetComponent<RectTransform>();
        scrollViewTransform.offsetMax = new Vector2(scrollViewTransform.offsetMax.x, -500);
        heroList.list = new List<Item>();
        ReloadData();
        string myAddress = "0x033A53F3962AaB889e13217A5a44241bA746BACd";

        // TODO get heroes for current player.
        StartCoroutine(getCharacters(myAddress));
        
    }

    public void LoadMarket()
    {
        RectTransform scrollViewTransform = scrollView.GetComponent<RectTransform>();
        scrollViewTransform.offsetMax = new Vector2(scrollViewTransform.offsetMax.x, -100);
        heroList.list = new List<Item>();
        ReloadData();
        StartCoroutine(GetRequest("https://heromarket.azurewebsites.net/api/Offers"));
    }

    public void LoadStore()
    {
        RectTransform scrollViewTransform = scrollView.GetComponent<RectTransform>();
        scrollViewTransform.offsetMax = new Vector2(scrollViewTransform.offsetMax.x, -100);

        heroList.list = new List<Item>();

        Item pack = new Item();
        pack.hero = "Random Bronze Hero";
        pack.seller = "Cryptic Legends";
        pack.price = 0.99;
        pack.productId = ProductID.NonConsumableBronze;
        pack.heroImageId = -1;
        heroList.list.Add(pack);

        Item packS = new Item();
        packS.hero = "Random Silver Hero";
        packS.seller = "Cryptic Legends";
        packS.price = 1.99;
        packS.productId = ProductID.NonConsumableSilver;
        packS.heroImageId = -2;
        heroList.list.Add(packS);

        Item packG = new Item();
        packG.hero = "Random Gold Hero";
        packG.seller = "Cryptic Legends";
        packG.price = 6.99;
        packG.productId = ProductID.NonConsumableGold;
        packG.heroImageId = -3;
        heroList.list.Add(packG);


        Item packL = new Item();
        packL.hero = "Random Legendary Hero";
        packL.seller = "Cryptic Legends";
        packL.price = 17.99;
        packL.productId = ProductID.NonConsumableLegendary;
        packL.heroImageId = -4;
        heroList.list.Add(packL);

        ReloadData();
    }

    private void RemoveCards() {
        while (contentPanel.childCount > 0)
        {
            GameObject toRemove = transform.GetChild(0).gameObject;
            heroCardPool.ReturnObject(toRemove);
        }
    }

    private static int currentImageId = -1;
    private int GetHeroImageId()
    {
        if (currentImageId > 19)
        {
            currentImageId = -1;
        }
        currentImageId++;
        return currentImageId;
    }

    private void AddCards () {
        for (int i = 0; i < heroList.list.Count; i++) {
            Item hero = heroList.list[i];
            GameObject newHeroCard = heroCardPool.GetObject();
            newHeroCard.transform.SetParent(contentPanel);

            HeroCard heroCard = newHeroCard.GetComponent<HeroCard>();
            heroCard.Setup(hero, this);
        }
    }
}
