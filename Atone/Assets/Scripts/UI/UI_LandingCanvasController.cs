using System.Collections.Generic;
using UnityEngine;


namespace Atone_UI
{
    public class UI_LandingCanvasController : MonoBehaviour
    {
        // This is to determine whether to display the pause canvas or the main menu canvas
        [SerializeField]private GameObject m_mainMenuLanding;
        [SerializeField]private GameObject m_pauseMenuLanding;

        [SerializeField]private GameObject m_VolumeSettings;
        [SerializeField]private GameObject m_GraphicSettings;
        [SerializeField]private GameObject m_GameplaySettings;

        private Dictionary<SubMenuType, GameObject> m_MenuComponentsDict;
        private GameObject m_CurrrentlyActiveSettings = null;
        private SubMenuType m_CurrentSubMenu = SubMenuType.NONE;
        private MenuType m_CurrentMenuState = MenuType.NONE_GAME_PLAYING;



        private void Awake(){
            m_MenuComponentsDict = new Dictionary<SubMenuType, GameObject>(){
                {SubMenuType.GAMEPLAY, m_GameplaySettings},
                {SubMenuType.GRAPHICS, m_GraphicSettings},
                {SubMenuType.VOLUME, m_VolumeSettings}
            };
        }      

        private void Start(){
            // Subscribe to your events here
            InputManager.onMenu += TogglePauseMenu;

        } 
        private void OnDestroy(){
            if(InputManager.Instance != null){
                InputManager.onMenu -= TogglePauseMenu;
            }
        }

        public void TogglePauseMenu(bool isGameBeingPaused){
            if(isGameBeingPaused){
                SetLandingCanvas(MenuType.PAUSE_MENU);
            } else {
                SetLandingCanvas(MenuType.NONE_GAME_PLAYING);
            }
        }
        private void SetLandingCanvas(MenuType menuType){
            m_pauseMenuLanding.SetActive(menuType == MenuType.PAUSE_MENU);
            m_mainMenuLanding.SetActive(menuType == MenuType.MAIN_MENU);
        }

        public void DisplayMenuSettings(SubMenuType submenu){
            if(m_CurrentSubMenu != submenu){
                m_CurrrentlyActiveSettings.SetActive(false);
                m_CurrrentlyActiveSettings = m_MenuComponentsDict[submenu];
                if(submenu != SubMenuType.NONE){
                    m_CurrrentlyActiveSettings.SetActive(true);
                }
            }
        }
    }

    public enum MenuType {
        NONE_GAME_PLAYING,
        MAIN_MENU,
        PAUSE_MENU
    }

    public enum  SubMenuType {
        NONE,
        GAMEPLAY,
        GRAPHICS,
        VOLUME
    }

}

