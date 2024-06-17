using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using SimpleJSON;
using UnityEngine.XR;

public class Items : MonoBehaviour
{
    public GameObject itemPrefab; // Thêm biến public để lưu trữ Prefab

    Action<string> _createItemCallBack;

    void Start()
    {
        _createItemCallBack = (jsonArray) =>
        {
            Debug.Log("Received item IDs: " + jsonArray);
            StartCoroutine(CreateItemRoutine(jsonArray));
        };
        CreateItems();
    }

    public void CreateItems()
    {
        if (Main.Instance == null || Main.Instance.userInfo == null || string.IsNullOrEmpty(Main.Instance.userInfo.userId))
        {
            Debug.LogError("Main instance or UserInfo is not initialized properly.");
            return;
        }

        string userId = Main.Instance.userInfo.userId;
        Debug.Log("Requesting items for user ID: " + userId);
        StartCoroutine(Main.Instance.web.GetItemID(userId, _createItemCallBack));
    }

    IEnumerator CreateItemRoutine(string jsonArrayString)
    {
        // Parsing json array string as an array
        JSONArray jsonArray = JSON.Parse(jsonArrayString) as JSONArray;
        if (jsonArray == null)
        {
            Debug.LogError("Failed to parse JSON array.");
            yield break;
        }

        for (int i = 0; i < jsonArray.Count; i++)
        {
            // Create local variables
            bool isDone = false;
            string itemId = jsonArray[i].AsObject["itemID"];
            Debug.Log("Processing item ID: " + itemId);
            JSONObject itemInfoJson = new JSONObject();

            // Create a callback to get information from Web.cs
            Action<string> getItemInfoCallback = (itemInfo) =>
            {
                isDone = true;
                JSONArray tempArray = JSON.Parse(itemInfo) as JSONArray;
                if (tempArray != null && tempArray.Count > 0)
                {
                    itemInfoJson = tempArray[0].AsObject;
                    Debug.Log("Received item info for ID " + itemId + ": " + itemInfoJson.ToString());
                }
                else
                {
                    Debug.LogError("Failed to parse item info JSON for item ID: " + itemId);
                }
            };

            StartCoroutine(Main.Instance.web.GetItem(itemId, getItemInfoCallback));

            // Wait until the callback is called from Web (info finished downloading)
            yield return new WaitUntil(() => isDone == true);

            // Instantiate GameObject (item prefab)
            if (itemPrefab == null)
            {
                Debug.LogError("Item prefab not assigned in Inspector.");
                yield break; // End the coroutine if the item prefab is not found
            }

            GameObject item = Instantiate(itemPrefab); // Sử dụng itemPrefab từ Inspector
            if (item == null)
            {
                Debug.LogError("Failed to instantiate item prefab.");
                yield break; // End the coroutine if instantiation fails
            }

            item.transform.SetParent(this.transform);
            item.transform.localScale = Vector3.one;
            item.transform.localPosition = Vector3.zero;

            // Fill Information
            var nameText = item.transform.Find("Name")?.GetComponent<Text>();
            var priceText = item.transform.Find("Price")?.GetComponent<Text>();
            var descriptionText = item.transform.Find("Description")?.GetComponent<Text>();
            var imageComponent = item.transform.Find("Image")?.GetComponent<Image>();

            if (nameText == null || priceText == null || descriptionText == null || imageComponent == null)
            {
                Debug.LogError("UI components not found on item prefab.");
                yield break; // End the coroutine if any UI component is not found
            }
            nameText.text = itemInfoJson["name"];
            priceText.text = itemInfoJson["price"];
            descriptionText.text = itemInfoJson["description"];

            int imgVer = itemInfoJson["imgVer"].AsInt;

            byte[] bytes = ImageManager.Instance.LoadImage(itemId, imgVer);
            if (bytes.Length == 0)
            {
                // Create a callback to get the SPRITE from Web.cs
                Action<byte[]> getItemIconCallback = (downloadedBytes) =>
                {
                    if (downloadedBytes != null)
                    {
                        Sprite sprite = ImageManager.Instance.BytesToSprite(downloadedBytes);
                        imageComponent.sprite = sprite;
                        ImageManager.Instance.SaveImage(itemId, downloadedBytes, imgVer);
                        Debug.Log("Successfully loaded sprite for item ID: " + itemId);
                        ImageManager.Instance.SaveVersionJson();
                    }
                    else
                    {
                        Debug.LogError("Failed to load sprite for item ID: " + itemId);
                    }
                };
                Debug.Log("Requesting sprite for item ID: " + itemId);
                StartCoroutine(Main.Instance.web.GetItemIcon(itemId, getItemIconCallback));
            }
            // Load from device
            else
            {
                Sprite sprite = ImageManager.Instance.BytesToSprite(bytes);
                item.transform.Find("Image").GetComponent<Image>().sprite = sprite;
            }

            // Set Sell button
            item.transform.Find("SellButton").GetComponent<Button>().onClick.AddListener(() =>
            {
                string iId = itemId;
                string uId = Main.Instance.userInfo.userId;
                Debug.Log("Selling item ID: " + iId + " for user ID: " + uId);
                StartCoroutine(Main.Instance.web.SellItem(iId, uId));
            });
        }
    }
}
