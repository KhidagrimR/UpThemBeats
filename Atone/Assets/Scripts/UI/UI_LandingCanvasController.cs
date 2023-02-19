using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Atone_UI
{
    public class UI_LandingCanvasController : MonoBehaviour
    {
        // This is to determine whether to display the pause canvas or the main menu canvas
        [SerializeField] private GameObject mainMenuLanding;
        [SerializeField] private GameObject pauseMenuLanding;
        [SerializeField] private GameObject settingsLanding;

        [SerializeField] private GameObject[] crosshairSettings;
        [SerializeField] private GameObject[] audioSettings;
        [SerializeField] private GameObject[] subtitlesSettings;

        private Dictionary<SubMenuType, GameObject[]> menuComponentsDict;
        private GameObject[] currrentlyActiveSettings = null;
        private SubMenuType currentSubMenu = SubMenuType.NONE;
        private MenuType currentMenuLanding = MenuType.NONE_GAME_PLAYING;

        public static bool forbidPauseToggle {get; private set;}

        private void Awake()
        {
            menuComponentsDict = new Dictionary<SubMenuType, GameObject[]>(){
                {SubMenuType.SUBTITLES, subtitlesSettings},
                {SubMenuType.AUDIO, audioSettings},
                {SubMenuType.CROSSHAIR, crosshairSettings}
            };
        }

        private void Start()
        {
            // Subscribe to your events here
            GameManager.onMenu += DisplayAppropriateMenuLanding;

        }
        private void OnDestroy()
        {
            if (GameManager.Instance != null)
            {
                GameManager.onMenu -= DisplayAppropriateMenuLanding;
            }
        }

        // private void TogglePauseMenu(bool isGameBeingPaused)
        // {
        //     if (isGameBeingPaused)
        //     {
        //         SetLandingCanvas(MenuType.PAUSE_MENU);
        //         Cursor.lockState = CursorLockMode.None; // Frees the cursor in order to navigate menu
        //     }
        //     else
        //     {
        //         SetLandingCanvas(MenuType.NONE_GAME_PLAYING);
        //         DisplayMenuSettings(SubMenuType.NONE);
        //         Cursor.lockState = CursorLockMode.Locked;
        //     }
        // }

        private void DisplayAppropriateMenuLanding(GeneralGameState oldStateWeWantToLeave)
        {
            // Switch to the next logical panel based on current game state: "if return called in game -> set to pause", etc.
            switch(oldStateWeWantToLeave)
            {
                case GeneralGameState.MAIN_MENU:                    
                    SetLandingCanvas(MenuType.MAIN_MENU);
                    Cursor.lockState = CursorLockMode.None;
                break;
                case GeneralGameState.PAUSED:
                    
                    if(currentMenuLanding == MenuType.SETTINGS)
                    {
                        Debug.Log("Handling PAUSED oldstate to return to pause main panel");
                        HideSettingsPanel();
                        // SetLandingCanvas(MenuType.PAUSE_MENU);
                    }
                    else
                    {
                        Debug.Log("Handling PAUSED oldstate to return to resume game");
                        forbidPauseToggle = false;
                        SetLandingCanvas(MenuType.NONE_GAME_PLAYING);
                        DisplayMenuSettings(SubMenuType.NONE);
                        Cursor.lockState = CursorLockMode.Locked;
                    }
                    
                break;
                case GeneralGameState.GAME:
                    // Brings up pause landing page. GameManager will handle audio and time scale on its own side
                    SetLandingCanvas(MenuType.PAUSE_MENU);
                    Cursor.lockState = CursorLockMode.None; // Frees the cursor in order to navigate menu
                break;
            }
        }
        private void NavigateOnReturnButtonPressed()
        {

        }


        public void PlayGame(GameObject menu)
        {
            menu.SetActive(false);
            SceneManager.LoadScene("Game");
        }

        public void Quit()
        {
            Application.Quit();
        }

        private void SetLandingCanvas(MenuType menuType)
        {
            pauseMenuLanding.SetActive(menuType == MenuType.PAUSE_MENU);
            mainMenuLanding.SetActive(menuType == MenuType.MAIN_MENU);
            // bool settingsActive = menuType == MenuType.SETTINGS;
            // settingsLanding.SetActive(menuType == MenuType.SETTINGS);
            // if(settingsActive){
            //     DisplayMenuSettings(SubMenuType.SUBTITLES);
            // }
            currentMenuLanding = menuType;
        }

        private void DisplayMenuSettings(SubMenuType submenu)
        {
            if (currentSubMenu != submenu)
            {
                if (currrentlyActiveSettings != null)
                {
                    Debug.Log("current active settings not null");
                    foreach(var c in currrentlyActiveSettings){ c.SetActive(false);}
                    //currrentlyActiveSettings.SetActive(false);
                }

                currentSubMenu = submenu;

                if (submenu != SubMenuType.NONE)
                {
                    currrentlyActiveSettings = menuComponentsDict[submenu];
                    foreach(var c in currrentlyActiveSettings){ c.SetActive(true);}
                    //currrentlyActiveSettings.SetActive(true);
                }
            }
        }

        #region function called by menu buttons
        public void DisplayMenuSettingsFromClickEvent(int submenuIndex)
        {
            DisplayMenuSettings((SubMenuType)submenuIndex);
        }
        public void DisplayCanvasFromClickEvent(int menuIndex)
        {
            SetLandingCanvas((MenuType)menuIndex);
        }
        public void DisplaySettingsPanel()
        {            
            forbidPauseToggle = true;
            settingsLanding.SetActive(true);            
            DisplayMenuSettings(SubMenuType.SUBTITLES);     
            currentMenuLanding = MenuType.SETTINGS;       
        }
        public void HideSettingsPanel()
        {
            settingsLanding.SetActive(false);            
            DisplayMenuSettings(SubMenuType.NONE);    
            if(mainMenuLanding.activeSelf) {
                currentMenuLanding = MenuType.MAIN_MENU;
            }
            else if(pauseMenuLanding.activeSelf) {
                currentMenuLanding = MenuType.PAUSE_MENU;
            }
            
        }
        public void ResumeGameFromClickEvent()
        {
            // DisplayMenuSettings(SubMenuType.NONE);
            // DisplayAppropriateMenuLanding(GeneralGameState.PAUSED);

            SetLandingCanvas(MenuType.NONE_GAME_PLAYING);
            DisplayMenuSettings(SubMenuType.NONE);
            Cursor.lockState = CursorLockMode.Locked;
            forbidPauseToggle = false;
            GameManager.Instance.ResumeGame();

            // TogglePauseMenu(false);
            // GameManager.Instance.TogglePauseState();
            // GameManager.Instance.ResumeGame();

        }
        
        #endregion
    }

       


    public enum MenuType
    {
        NONE_GAME_PLAYING,
        MAIN_MENU,
        PAUSE_MENU,
        SETTINGS
    }

    public enum SubMenuType
    {
        NONE,
        SUBTITLES,
        AUDIO,
        CROSSHAIR
    }

}
