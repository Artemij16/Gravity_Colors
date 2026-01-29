using UnityEngine;
using UnityEngine.Advertisements;

public class AdsManager : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
{
    public string androidGameId = "6022887";
    public string adUnitId = "Rewarded_Android";
    public bool testMode = true;

    void Awake() => InitializeAds();

    public void InitializeAds()
    {
        if (!Advertisement.isInitialized && Advertisement.isSupported)
            Advertisement.Initialize(androidGameId, testMode, this);
    }

    public void OnInitializationComplete() => LoadAd();

    public void LoadAd() => Advertisement.Load(adUnitId, this);

    public void ShowDoubleScoreAd() => Advertisement.Show(adUnitId, this);

    public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
    {
        if (showCompletionState == UnityAdsShowCompletionState.COMPLETED)
        {
            if (ScoreManager.Instance != null) ScoreManager.Instance.RewardDoubleScore();
            LoadAd();
        }
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message) { }
    public void OnUnityAdsAdLoaded(string adUnitId) { }
    public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message) => LoadAd();
    public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message) => LoadAd();
    public void OnUnityAdsShowStart(string adUnitId) { }
    public void OnUnityAdsShowClick(string adUnitId) { }
}