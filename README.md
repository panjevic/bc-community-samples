# Blockchain PoC using Azure

This repository holds the proof-of-concept projects using Azure and Ethereum created for the [Ethereal Hackathon](https://gitcoin.co/hackathon/ethhack2019/).

## Overview

- [Attested Delivery](supplychain-delivery-app/Readme.md)
Collect proof of delivery utilizing Ethereum blockchain
Dispatchers use Forms for scheduling deliveries. Delivery agents use a mobile app to get the list of scheduled deliveries for the date in a mobile app. App provides the delivery agent with directions to delivery address and ability to notify the customer that the truck is on the way. When on site, agent uses the mobile app to get the current GPS location, scans the package barcode, takes a photo and collects customers signature. This data is used to provide a proof of delivery and is saved to Azure and stored on private Ethereum network to provide an immutable log.
Using Xamarin.Forms, Office365, Azure Functions, Logic Apps and Azure Table Storage.

- [Game Asset Transfer](unity-asset-app/Readme.md)
Mobile game that enables players purchasing assets on one platform (eq. Google Play or App Store) to use them on all platforms as game assets are represented as non-fungible tokens (NFTs) on Ethereum blockchain. Besides in-app purchases, it also includes simple marketplace to facilitate player-to-player trades. Solution is implemented in Unity3D and Nethereum, Azure Functions, Logic Apps, Azure Storage and public Ethereum blockchain.

- [Tracking Salesforce Approvals](ebc-forms-salesforce/Readme.md)
Sales personnel are using Microsoft Forms on mobile or web to easily create new opportunities in Salesforce. In case of a discount higher than 40% applied on the opportunity, it needs to go through an approval process in Salesforce. Private Ethereum network is used to log all approval steps on the Salesforce opportunity. Using Salesforce, Azure Functions and Logic Apps 

- [Expenses app](ebc-forms-excel/Readme.md) where employees use Forms to enter expenses on web or mobile. Smart contract deployed to private Ethereum network are used to provide immutable log of the submitted expenses and keep track of totals for each category. Summary data is presented using Excel Online workbook (hosted on SharePoint Online) and updated from Ethereum smart contract. Using Office365, Azure Functions and Logic Apps 

- [Immutable audit log](ebc-sharepoint/Readme.md) of changes to the document hosted on SharePoint Online - each time an Office365 document is changed, document metadata and user info is stored on a private Ethereum network to provide an immutable log Using Office365, Azure and Logic Apps 

- [Logging images](ebc-computervision/Readme.md) uploaded to SharePoint Online and OCR of the text found on the image - When employees upload an image to a SharePoint Online library, OCR (optical character recognition) is run automatically and a log is stored on a private Ethereum network. Using Office365, Azure, Logic Apps and Computer Vision API


