using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShopSliderManager : MonoBehaviour
{
    [Header("Data & Items")]
    public int[] prices;
    public RectTransform[] items;
    private BackgroundItem[] itemScripts;

    [Header("UI References")]
    public TMP_Text priceText;
    public TMP_Text balanceText;
    public Button actionButton;
    public TMP_Text actionButtonText;

    [Header("Movement Settings")]
    public float spacing = 700f;
    public float moveSpeed = 10f;
    public float activeScale = 1.2f;
    public float idleScale = 0.8f;

    [Header("Swipe Settings")]
    public float swipeThreshold = 50f;
    private Vector2 touchStartPos;
    private Vector2 touchEndPos;
    private bool isDragging = false;

    [Header("Navigation")]
    public string menuSceneName = "main";
    private int currentIndex = 0;

    void Start()
    {
        itemScripts = new BackgroundItem[items.Length];
        for (int i = 0; i < items.Length; i++)
        {
            itemScripts[i] = items[i].GetComponent<BackgroundItem>();
            items[i].anchorMin = new Vector2(0.5f, 0.5f);
            items[i].anchorMax = new Vector2(0.5f, 0.5f);
            items[i].pivot = new Vector2(0.5f, 0.5f);
        }

        currentIndex = PlayerPrefs.GetInt("SelectedBG", 0);
        if (currentIndex >= items.Length) currentIndex = 0;

        UpdateUI();
        UpdateItemsPosition(true);
    }
    public void ExitToMenu()
    {
        SceneManager.LoadScene(menuSceneName);
    }
    void Update()
    {
        int currentBalance = DataCrypto.GetSecureInt("TotalBank", 0);
        if (balanceText != null) balanceText.text = "BANK: " + currentBalance;

        HandleSwipe();
        UpdateItemsPosition(false);
    }

    void HandleSwipe()
    {
        if (Input.GetMouseButtonDown(0))
        {
            touchStartPos = Input.mousePosition;
            isDragging = true;
        }

        if (Input.GetMouseButtonUp(0) && isDragging)
        {
            touchEndPos = Input.mousePosition;
            CheckSwipe();
            isDragging = false;
        }
    }

    void CheckSwipe()
    {
        float swipeDistance = touchEndPos.x - touchStartPos.x;

        if (Mathf.Abs(swipeDistance) > swipeThreshold)
        {
            if (swipeDistance > 0)
            {

                PrevItem();
            }
            else
            {
                NextItem();
            }
        }
    }

    void UpdateItemsPosition(bool instant)
    {
        for (int i = 0; i < items.Length; i++)
        {
            float targetX = (i - currentIndex) * spacing;
            Vector3 targetPos = new Vector3(targetX, 0, 0);
            float scaleValue = (i == currentIndex) ? activeScale : idleScale;
            Vector3 targetScale = new Vector3(scaleValue, scaleValue, 1f);

            if (instant)
            {
                items[i].anchoredPosition = targetPos;
                items[i].localScale = targetScale;
            }
            else
            {
                items[i].anchoredPosition = Vector3.Lerp(items[i].anchoredPosition, targetPos, Time.deltaTime * moveSpeed);
                items[i].localScale = Vector3.Lerp(items[i].localScale, targetScale, Time.deltaTime * moveSpeed);
            }
        }
    }

    public void NextItem()
    {
        if (currentIndex < items.Length - 1)
        {
            currentIndex++;
            UpdateUI();
        }
    }

    public void PrevItem()
    {
        if (currentIndex > 0)
        {
            currentIndex--;
            UpdateUI();
        }
    }

    public void UpdateUI()
    {
        if (items.Length == 0) return;

        bool isBought = IsBought(currentIndex);
        int selected = PlayerPrefs.GetInt("SelectedBG", 0);
        int currentBalance = DataCrypto.GetSecureInt("TotalBank", 0);

        for (int i = 0; i < itemScripts.Length; i++)
        {
            if (itemScripts[i] != null && itemScripts[i].lockIcon != null)
                itemScripts[i].lockIcon.SetActive(!IsBought(i));
        }

        if (!isBought)
        {
            priceText.text = "PRICE: " + prices[currentIndex];
            actionButtonText.text = "BUY";
            actionButton.interactable = (currentBalance >= prices[currentIndex]);
        }
        else
        {
            priceText.text = "OWNED";
            actionButtonText.text = (currentIndex == selected) ? "ACTIVE" : "SELECT";
            actionButton.interactable = (currentIndex != selected);
        }
    }

    public void OnActionClick()
    {
        int currentBalance = DataCrypto.GetSecureInt("TotalBank", 0);

        if (!IsBought(currentIndex))
        {
            if (currentBalance >= prices[currentIndex])
            {
                currentBalance -= prices[currentIndex];
                DataCrypto.SaveSecureInt("TotalBank", currentBalance);

                PlayerPrefs.SetInt("BG_Bought_" + currentIndex, 1);
                PlayerPrefs.Save();
                UpdateUI();
            }
        }
        else
        {
            PlayerPrefs.SetInt("SelectedBG", currentIndex);
            PlayerPrefs.Save();
            if (BackgroundManager.Instance != null)
            {
                BackgroundManager.Instance.ApplyBackground();
            }
            UpdateUI();
        }
    }

    bool IsBought(int index)
    {
        if (index == 0) return true;
        return PlayerPrefs.GetInt("BG_Bought_" + index, 0) == 1;
    }
}