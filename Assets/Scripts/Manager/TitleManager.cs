using UnityEngine;
using UnityEngine.SceneManagement;

namespace MySampleEx
{
    public class TitleManager : MonoBehaviour
    {
        public GameObject mainMenu;
        public GameObject option;

        private AdManager adManager;

        private void Start()
        {
            adManager = AdManager.Instance;
        }

        public void StartPlay()
        {
            adManager.HideBanner();
            SceneManager.LoadScene("PlayScene");
        }

        public void ShowOption()
        {
            adManager.HideBanner();
            //adManager.ShowIinterstitialAd();
            adManager.ShowRewardAd();

            mainMenu.SetActive(false);
            option.SetActive(true);
        }

        public void HideOption()
        {
            adManager.ShowBanner();

            mainMenu.SetActive(true);
            option.SetActive(false);
        }

    }

}
