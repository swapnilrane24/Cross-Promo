using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using System.IO;
using UnityEngine.UI;
using UnityEngine.Networking;

namespace Devshifu.CrossPromo
{
    public class CrossPromoManager : MonoBehaviour
    {
        private static CrossPromoManager instance;

        private const string KEY_ENABLED = "IsEnabled";
        private const string KEY_APP_TITLE = "AppTitle";
        private const string KEY_IMAGE_URL = "IconUrl";
        private const string KEY_PAGE_URL = "PageUrl";
        private const string KEY_ID = "Id";
        private const string KEY_APP_PREFIX = "Games";

        #region Serialized Variables
        [Tooltip("This ID is used to discart json data of this game so we dont show Ad of this game")]
        [SerializeField] private string gameID;
        [Tooltip("Delay is in seconds")]
        [SerializeField] private float buttonShowDelay = 10;
        [SerializeField] private Image icon;
        [SerializeField] private Button promoButton;
        [SerializeField] private string promoDataUrl;
        #endregion

        #region Private Variables
        private float currentDelay;
        private bool canShowAd = false;
        private bool buttonActive = false;
        private List<GameItem> gamesData;
        #endregion

        #region Getters & Setters
        public static CrossPromoManager Instance { get => instance; }
        #endregion

        #region Unity Methods
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            currentDelay = buttonShowDelay + Time.time;
            if (!canShowAd)
            {
                StartCoroutine(DownloadJsonData());
            }
        }

        private void Update()
        {
            if (canShowAd)
            {
                if (currentDelay <= Time.time)
                {
                    if (!buttonActive)
                    {
                        currentDelay = buttonShowDelay + Time.time;
                        LoadData();
                    }
                }
            }
        }
        #endregion

        #region Private Methods
        private void PromoButtonListner(int index) 
        {
            Application.OpenURL(gamesData[index].PageUrl);
            currentDelay = buttonShowDelay + Time.time;
            promoButton.gameObject.SetActive(false);
            buttonActive = false;
            //GameAnalyticsScript.Instance.DesignUnlockEvent("PromoButtonClicked");
        }

        private void LoadData()
        {
            int index = Random.Range(0, gamesData.Count);
            StartCoroutine(HandleImageResponse(index));
        }

        private IEnumerator DownloadJsonData()
        {
            canShowAd = true;

            UnityWebRequest webRequest = new UnityWebRequest(promoDataUrl);

            webRequest.downloadHandler = new DownloadHandlerBuffer();
            yield return webRequest.SendWebRequest();

            //In future use unity 2021+ and remove the 2019 code
#if UNITY_2019
            if (webRequest.isNetworkError)
            {
                canShowAd = false;
                Debug.LogError("CrossPromo: WWW error: " + webRequest.error);
            }
#elif UNITY_2021
            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                canShowAd = false;
                Debug.LogError("CrossPromo: WWW error: " + webRequest.error);
            }
#endif
            else
            {
                yield return StartCoroutine(HandleResponse(webRequest.downloadHandler.text));
            }
        }

        private IEnumerator HandleResponse(string responseJson)
        {
            CrossPromoData promoData = ParseResponse(responseJson);
            yield return null;
        }

        private CrossPromoData ParseResponse(string json)
        {
            try
            {
                CrossPromoData result = new CrossPromoData();
                JSONNode rootNode = JSON.Parse(json);
                result.IsEnabled = rootNode[KEY_ENABLED].AsBool;
                JSONArray gamesArray = rootNode[KEY_APP_PREFIX].AsArray;


                for (int i = 0; i < gamesArray.Count; i++)
                {
                    result.Games.Add(ParseItem(gamesArray[i]));
                }

                gamesData = result.Games;

                for (int i = 0; i < gamesData.Count; i++)
                {
                    if (gamesData[i].Id == gameID)
                    {
                        gamesData.RemoveAt(i);
                        break;
                    }
                }

                return result;
            }
            catch (System.Exception)
            {
                Debug.LogError("Json parse error");
            }
            

            return null;
        }

        private GameItem ParseItem(JSONNode node)
        {
            GameItem result = new GameItem();

            result.AppTitle = node[KEY_APP_TITLE];
            result.IconUrl = node[KEY_IMAGE_URL];
            result.PageUrl = node[KEY_PAGE_URL];
            result.Id = node[KEY_ID];

            return result;
        }

        private IEnumerator HandleImageResponse(int index)
        {
            CoroutineWithData cd = new CoroutineWithData(this, downloadImage(gamesData[index].IconUrl));
            yield return cd.coroutine;

            bool success = (bool)cd.result;

            if (!success)
            {
                Debug.LogError("Failded to load image");
                canShowAd = false;
            }
            else
            {
                icon.sprite = Utils.GetPromoSprite();
                buttonActive = true;
                promoButton.onClick.AddListener(() =>
                {
                    PromoButtonListner(index);
                });
                promoButton.gameObject.SetActive(true);
            }
        }

        private IEnumerator downloadImage(string url)
        {
            UnityWebRequest webRequest = new UnityWebRequest(url);

            webRequest.downloadHandler = new DownloadHandlerBuffer();
            yield return webRequest.SendWebRequest();

            //In future use unity 2021+ and remove the 2019 code
#if UNITY_2019
            if (webRequest.isNetworkError)
            {
                Debug.LogError("CrossPromo: WWW error: " + webRequest.error);
                yield return false;
            }
#elif UNITY_2021
            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("CrossPromo: WWW error: " + webRequest.error);
                yield return false;
            }
#endif
            else
            {
                File.WriteAllBytes(Utils.GetImagePath(), webRequest.downloadHandler.data);
                yield return true;
            }
        }
#endregion

#region Public Methods
#endregion
    }
}
