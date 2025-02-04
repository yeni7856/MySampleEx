using UnityEngine;
using UnityEngine.SceneManagement;

namespace MySampleEx
{
    public class TitleManager : MonoBehaviour
    {
        public GameObject mainMenu;
        public GameObject option;

#if AD_MODE
        private AdManager adManager;
#endif

        private void Start()
        {
#if AD_MODE
            adManager = AdManager.Instance;
#endif
        }

        public void StartPlay()
        {
#if AD_MODE
            adManager.HideBanner();
#endif
            SceneManager.LoadScene("PlayScene");
        }

        public void ShowOption()
        {
#if AD_MODE
            adManager.HideBanner();
            //adManager.ShowIinterstitialAd();
            adManager.ShowRewardAd();
#endif
            mainMenu.SetActive(false);
            option.SetActive(true);
        }

        public void HideOption()
        {
#if AD_MODE
            adManager.ShowBanner();
#endif
            mainMenu.SetActive(true);
            option.SetActive(false);
        }

    }

}
