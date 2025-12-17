using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShopSliderManager : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [Header("Data & Prices")]
    public Sprite[] backgrounds;
    public int[] prices;

    [Header("Items (UI Objects)")]
    public BackgroundItem leftItem;
    public BackgroundItem centerItem;
    public BackgroundItem rightItem;

    [Header("Settings")]
    public float snapSpeed = 12f;
    public float dragThreshold = 0.2f;
    [Range(0.1f, 1f)] public float minScale = 0.7f;
    [Range(1f, 2f)] public float maxScale = 1.0f;

    [Header("UI References")]
    public TMP_Text priceText;
    public TMP_Text balanceText; // Текст для отображения ScoreManager.score
    public Button actionButton;  // Сама кнопка (чтобы менять её состояние)
    public TMP_Text actionButtonText; // Текст на кнопке (Buy/Select/Active)

    private int currentIndex = 0;
    private Vector2 touchStartPos;
    private bool isDragging = false;
    private Vector3 leftStartPos, centerStartPos, rightStartPos;
    private float itemStep;

    void Start()
    {
        // Рассчитываем шаг между элементами
        leftStartPos = leftItem.transform.localPosition;
        centerStartPos = centerItem.transform.localPosition;
        rightStartPos = rightItem.transform.localPosition;
        itemStep = Vector3.Distance(centerStartPos, rightStartPos);

        // Загружаем текущий выбранный фон из твоего BackgroundManager
        currentIndex = PlayerPrefs.GetInt("SelectedBG", 0);

        UpdateView();
        ApplyTransform(0);
    }

    void Update()
    {
        // Обновляем визуальный баланс очков каждый кадр
        if (balanceText != null)
            balanceText.text = "SCORE: " + ScoreManager.score;
    }

    // --- ЛОГИКА СВАЙПА ---

    public void OnBeginDrag(PointerEventData eventData)
    {
        StopAllCoroutines();
        touchStartPos = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        isDragging = true;
        float offset = eventData.position.x - touchStartPos.x;

        // Эффект "резинки" на границах списка
        if ((currentIndex == 0 && offset > 0) || (currentIndex == backgrounds.Length - 1 && offset < 0))
            offset *= 0.3f;

        ApplyTransform(offset);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;
        float offset = eventData.position.x - touchStartPos.x;
        int targetIndex = currentIndex;

        if (offset < -itemStep * dragThreshold && currentIndex < backgrounds.Length - 1)
            targetIndex++;
        else if (offset > itemStep * dragThreshold && currentIndex > 0)
            targetIndex--;

        StartCoroutine(SmoothSnap(targetIndex, offset));
    }

    IEnumerator SmoothSnap(int targetIndex, float currentOffset)
    {
        float targetOffset = 0;
        if (targetIndex > currentIndex) targetOffset = -itemStep;
        else if (targetIndex < currentIndex) targetOffset = itemStep;

        float animOffset = currentOffset;

        while (Mathf.Abs(animOffset - targetOffset) > 0.1f)
        {
            animOffset = Mathf.Lerp(animOffset, targetOffset, Time.deltaTime * snapSpeed);
            ApplyTransform(animOffset);
            yield return null;
        }

        currentIndex = targetIndex;
        UpdateView();
        ApplyTransform(0);
    }

    private void ApplyTransform(float offset)
    {
        centerItem.transform.localPosition = centerStartPos + new Vector3(offset, 0, 0);
        leftItem.transform.localPosition = leftStartPos + new Vector3(offset, 0, 0);
        rightItem.transform.localPosition = rightStartPos + new Vector3(offset, 0, 0);

        centerItem.transform.localScale = Vector3.one * CalculateScale(centerItem.transform.localPosition.x);
        leftItem.transform.localScale = Vector3.one * CalculateScale(leftItem.transform.localPosition.x);
        rightItem.transform.localScale = Vector3.one * CalculateScale(rightItem.transform.localPosition.x);
    }

    float CalculateScale(float xPos)
    {
        float distance = Mathf.Abs(xPos) / itemStep;
        return Mathf.Lerp(maxScale, minScale, Mathf.Clamp01(distance));
    }

    // --- ЛОГИКА МАГАЗИНА И ОБНОВЛЕНИЯ UI ---

    void UpdateView()
    {
        // 1. Обновляем картинки айтемов
        SetItem(centerItem, currentIndex);
        if (currentIndex > 0)
        {
            leftItem.gameObject.SetActive(true);
            SetItem(leftItem, currentIndex - 1);
        }
        else leftItem.gameObject.SetActive(false);

        if (currentIndex < backgrounds.Length - 1)
        {
            rightItem.gameObject.SetActive(true);
            SetItem(rightItem, currentIndex + 1);
        }
        else rightItem.gameObject.SetActive(false);

        // 2. Логика кнопки Купить/Выбрать
        bool isBought = IsBought(currentIndex);
        int selectedBG = PlayerPrefs.GetInt("SelectedBG", 0);

        if (!isBought)
        {
            priceText.text = "PRICE: " + prices[currentIndex];
            actionButtonText.text = "BUY";
            actionButton.interactable = (ScoreManager.score >= prices[currentIndex]);
        }
        else
        {
            if (currentIndex == selectedBG)
            {
                priceText.text = "CURRENTLY USED";
                actionButtonText.text = "ACTIVE";
                actionButton.interactable = false;
            }
            else
            {
                priceText.text = "OWNED";
                actionButtonText.text = "SELECT";
                actionButton.interactable = true;
            }
        }
    }

    // Этот метод вешаем на OnClick кнопки BuyButton
    public void OnActionButtonClick()
    {
        if (!IsBought(currentIndex))
        {
            // Покупка
            if (ScoreManager.score >= prices[currentIndex])
            {
                ScoreManager.score -= prices[currentIndex];
                // Сохраняем факт покупки
                PlayerPrefs.SetInt("BG_Bought_" + currentIndex, 1);
                PlayerPrefs.Save();

                // Автоматически выбираем после покупки
                SelectThisBackground();
            }
        }
        else
        {
            // Установка
            SelectThisBackground();
        }
    }
    public void GoBack()
    {
        SceneManager.LoadScene("main");
    }
    private void SelectThisBackground()
    {
        if (BackgroundManager.Instance != null)
        {
            BackgroundManager.Instance.SetBackground(currentIndex);
        }
        UpdateView();
    }

    void SetItem(BackgroundItem item, int index)
    {
        if (item != null)
            item.Set(backgrounds[index], !IsBought(index));
    }

    bool IsBought(int index)
    {
        if (index == 0) return true; // Первый всегда куплен
        return PlayerPrefs.GetInt("BG_Bought_" + index, 0) == 1;
    }
}