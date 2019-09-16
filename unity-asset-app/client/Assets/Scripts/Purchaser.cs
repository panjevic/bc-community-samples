using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using System.Text;

public static class ProductID
{
    // General product identifiers for the consumable, non-consumable, and subscription products.
    // Use these handles in the code to reference which product to purchase. Also use these values 
    // when defining the Product Identifiers on the store. Except, for illustration purposes, the 
    // kProductIDSubscription - it has custom Apple and Google identifiers. We declare their store-
    // specific mapping to Unity Purchasing's AddProduct, below.
    public static string NonConsumableBronze = "randomBronzeHero";
    public static string NonConsumableSilver = "randomSilverHero";
    public static string NonConsumableGold = "randomGoldHero";
    public static string NonConsumableLegendary = "randomLegendaryHero";
}

// Deriving the Purchaser class from IStoreListener enables it to receive messages from Unity Purchasing.
public class Purchaser : MonoBehaviour, IStoreListener
{
    private static IStoreController m_StoreController;          // The Unity Purchasing system.
    private static IExtensionProvider m_StoreExtensionProvider; // The store-specific Purchasing subsystems.

    // Product identifiers for all products capable of being purchased: 
    // "convenience" general identifiers for use with Purchasing, and their store-specific identifier 
    // counterparts for use with and outside of Unity Purchasing. Define store-specific identifiers 
    // also on each platform's publisher dashboard (iTunes Connect, Google Play Developer Console, etc.)

    // Google Play Store-specific product identifier subscription product.
    //private static string kProductNameGooglePlaySubscription = "com.unity3d.subscription.original";
    
    void Start()
    {
        // If we haven't set up the Unity Purchasing reference
        if (m_StoreController == null)
        {
            // Begin to configure our connection to Purchasing
            InitializePurchasing();
        }
    }

    public ProductCollection products()
    {
        return m_StoreController.products;
    }

    public void InitializePurchasing()
    {
        // If we have already connected to Purchasing ...
        if (IsInitialized())
        {
            // ... we are done here.
            return;
        }

        // Create a builder, first passing in a suite of Unity provided stores.
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        // Add a product to sell / restore by way of its identifier, associating the general identifier
        // with its store-specific identifiers.
        // Adding the non-consumable products.
        builder.AddProduct(ProductID.NonConsumableBronze, ProductType.NonConsumable);
        builder.AddProduct(ProductID.NonConsumableSilver, ProductType.NonConsumable);
        builder.AddProduct(ProductID.NonConsumableGold, ProductType.NonConsumable);
        builder.AddProduct(ProductID.NonConsumableLegendary, ProductType.NonConsumable);

        // Notice this uses store-specific IDs, illustrating
        // if the Product ID was configured differently between Apple and Google stores. Also note that
        // one uses the general kProductIDSubscription handle inside the game - the store-specific IDs 
        // must only be referenced here.

        // Kick off the remainder of the set-up with an asynchrounous call, passing the configuration 
        // and this class' instance. Expect a response either in OnInitialized or OnInitializeFailed.
        UnityPurchasing.Initialize(this, builder);
    }

    private bool IsInitialized()
    {
        // Only say we are initialized if both the Purchasing references are set.
        return m_StoreController != null && m_StoreExtensionProvider != null;
    }

    public void BuyNonConsumable(string productId)
    {
        // Buy the non-consumable product using its general identifier. Expect a response either 
        // through ProcessPurchase or OnPurchaseFailed asynchronously.
        BuyProductID(productId);
    }

    void BuyProductID(string productId)
    {
        // If Purchasing has been initialized ...
        if (IsInitialized())
        {
            // ... look up the Product reference with the general product identifier and the Purchasing 
            // system's products collection.
            Product product = m_StoreController.products.WithID(productId);

            // If the look up found a product for this device's store and that product is ready to be sold ... 
            if (product != null && product.availableToPurchase)
            {
                Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
                // ... buy the product. Expect a response either through ProcessPurchase or OnPurchaseFailed 
                // asynchronously.
                m_StoreController.InitiatePurchase(product);
            }
            // Otherwise ...
            else
            {
                // ... report the product look-up failure situation  
                Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
            }
        }
        // Otherwise ...
        else
        {
            // ... report the fact Purchasing has not succeeded initializing yet. Consider waiting longer or 
            // retrying initiailization.
            Debug.Log("BuyProductID FAIL. Not initialized.");
        }
    }

    IEnumerator PostProductPurchase(string bodyJsonString)
    {
        string url = "https://prod-112.westeurope.logic.azure.com/workflows/0dd323d78d754c018e1dee87af8defef/triggers/manual/paths/invoke?api-version=2016-10-01&sp=%2Ftriggers%2Fmanual%2Frun&sv=1.0&sig=KwRsD23_JgfdtVxhOSidwXUXVr4NXHVSrBDQd6smBjc";
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

    //  
    // --- IStoreListener
    //

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        // Purchasing has succeeded initializing. Collect our Purchasing references.
        Debug.Log("OnInitialized: PASS");

        // Overall Purchasing system, configured with products for this application.
        m_StoreController = controller;
        // Store specific subsystem, for accessing device-specific store features.
        m_StoreExtensionProvider = extensions;
    }


    public void OnInitializeFailed(InitializationFailureReason error)
    {
        // Purchasing set-up has not succeeded. Check error for reason. Consider sharing this reason with the user.
        Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
    }


    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        // One of the non-consumable products has been purchased by this user.
        if (String.Equals(args.purchasedProduct.definition.id, ProductID.NonConsumableBronze, StringComparison.Ordinal))
        {
            Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
            initiateBlockchainTransfer(args.purchasedProduct);
        }
        else if (String.Equals(args.purchasedProduct.definition.id, ProductID.NonConsumableSilver, StringComparison.Ordinal))
        {
            Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
            initiateBlockchainTransfer(args.purchasedProduct);
        }
        else if (String.Equals(args.purchasedProduct.definition.id, ProductID.NonConsumableGold, StringComparison.Ordinal))
        {
            Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
            initiateBlockchainTransfer(args.purchasedProduct);
        }
        else if (String.Equals(args.purchasedProduct.definition.id, ProductID.NonConsumableLegendary, StringComparison.Ordinal))
        {
            Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
            initiateBlockchainTransfer(args.purchasedProduct);
        }
        // Or ... an unknown product has been purchased by this user. Fill in additional products here....
        else
        {
            Debug.Log(string.Format("ProcessPurchase: FAIL. Unrecognized product: '{0}'", args.purchasedProduct.definition.id));
        }

        // Return a flag indicating whether this product has completely been received, or if the application needs 
        // to be reminded of this purchase at next app launch. Use PurchaseProcessingResult.Pending when still 
        // saving purchased products to the cloud, and when that save is delayed. 
        return PurchaseProcessingResult.Complete;
    }

    public void initiateBlockchainTransfer(Product product)
    {
        string bodyJsonString = "{\n\n\u00a0\u00a0\"packageId\": \"" + product.definition.id + "\",\n\n\u00a0\u00a0\"buyer\": \"" + PlayerPrefs.GetString(PlayerDataKeys.id) + "\",\n\n\u00a0\u00a0\"key\": \"receipt json\"\n\n}";
        Debug.Log(product.receipt);
        StartCoroutine(PostProductPurchase(bodyJsonString));
    }


    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        // A product purchase attempt did not succeed. Check failureReason for more detail. Consider sharing 
        // this reason with the user to guide their troubleshooting actions.
        Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
    }
}
