using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

namespace Atone_UI
{
    public class UI_LandingCanvasController : MonoBehaviour
    {
        // This is to determine whether to display the pause canvas or the main menu canvas
        [SerializeField] private GameObject mainMenuLanding;
        [SerializeField] private GameObject pauseMenuLanding;
        [SerializeField] private GameObject settingsLanding;

        [SerializeField] private GameObject volumeSettings;
        [SerializeField] private GameObject graphicSettings;
        [SerializeField] private GameObject gameplaySettings;
        
        [SerializeField] private GameObject selectableObjectWhenPause;

        private Dictionary<SubMenuType, GameObject> menuComponentsDict;
        private GameObject currrentlyActiveSettings = null;
        private SubMenuType currentSubMenu = SubMenuType.NONE;
        private MenuType currentMenuLanding = MenuType.NONE_GAME_PLAYING;

        private void Awake()
        {
            menuComponentsDict = new Dictionary<SubMenuType, GameObject>(){
                {SubMenuType.GAMEPLAY, gameplaySettings},
                {SubMenuType.GRAPHICS, graphicSettings},
                {SubMenuType.VOLUME, volumeSettings}
            };
        }

        private void Start()
        {
            // Subscribe to your events here
            InputManager.onMenu += TogglePauseMenu;

        }
        private void OnDestroy()
        {
            if (InputManager.Instance != null)
            {
                InputManager.onMenu -= TogglePauseMenu;
            }
        }

        private void TogglePauseMenu(bool isGameBeingPaused)
        {
            if (isGameBeingPaused)
            {
                SetLandingCanvas(MenuType.PAUSE_MENU);
                EventSystem.current.SetSelectedGameObject(selectableObjectWhenPause);
                Cursor.lockState = CursorLockMode.None; // Frees the cursor in order to navigate menu
            }
            else
            {
                SetLandingCanvas(MenuType.NONE_GAME_PLAYING);
                DisplayMenuSettings(SubMenuType.NONE);
                Cursor.lockState = CursorLockMode.Locked;
            }
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
            if(pauseMenuLanding != null)
                pauseMenuLanding.SetActive(menuType == MenuType.PAUSE_MENU);

            if(mainMenuLanding != null)
                mainMenuLanding.SetActive(menuType == MenuType.MAIN_MENU);

            if(settingsLanding != null)
                settingsLanding.SetActive(menuType == MenuType.SETTINGS);
        }

        private void DisplayMenuSettings(SubMenuType submenu)
        {
            if (currentSubMenu != submenu)
            {
                if (currrentlyActiveSettings != null)
                {
                    Debug.Log("current active settings not null");
                    currrentlyActiveSettings.SetActive(false);
                }

                currentSubMenu = submenu;

                if (submenu != SubMenuType.NONE)
                {
                    currrentlyActiveSettings = menuComponentsDict[submenu];
                    currrentlyActiveSettings.SetActive(true);
                }
            }
        }

        #region function called by menu buttons
        public void DisplayMenuSettingsFromClickEvent(int submenuIndex)
        {
            DisplayMenuSettings((SubMenuType)submenuIndex);
        }
        public void ResumeGameFromClickEvent()
        {
            DisplayMenuSettings(SubMenuType.NONE);
            TogglePauseMenu(false);
            GameManager.Instance.TogglePauseState();

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
        GAMEPLAY,
        GRAPHICS,
        VOLUME
    }

}
