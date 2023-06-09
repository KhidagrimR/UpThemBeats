using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class IntroductionNavigation : MonoBehaviour
{
    [SerializeField] private GameObject titleCard;
    [SerializeField] private GameObject title;
    [SerializeField] private GameObject txtFirstS;
    [SerializeField] private GameObject txtFirstD;
    [SerializeField] private GameObject txtSecondS;
    [SerializeField] private GameObject txtSecondD;
    [SerializeField] private GameObject crosshairS;
    [SerializeField] private GameObject crosshairD;
    [SerializeField] private GameObject endIntro;

    [SerializeField] private Button back;
    [SerializeField] private Button next;
    [SerializeField] private Button yes;
    [SerializeField] private Button no;

    [SerializeField] private GameObject launchGame;


 
    public List<GameObject> cards;
    int nb = 0;

    void Awake()
    {
        cards = new List<GameObject>(new GameObject[] { txtFirstS, txtFirstD, txtSecondS, txtSecondD, crosshairS, crosshairD, endIntro });
        next.onClick.AddListener(Next);
        back.onClick.AddListener(Back);
        yes.onClick.AddListener(Next);
        no.onClick.AddListener(Next);
    }

    public void Next()
    {
        back.interactable = true;
        if (nb > cards.Count - 2)
        {
            title.SetActive(false);
            back.gameObject.SetActive(false);
            next.gameObject.SetActive(false);
            EventSystem.current.SetSelectedGameObject(launchGame);
        }

        if (nb == 0)
        {
            titleCard.SetActive(false);
            title.SetActive(true);
        }
        if (nb > 1)
        {
            cards[nb - 1].SetActive(false);
            cards[nb - 2].SetActive(false);
        }
        
        cards[nb].SetActive(true);

        if(nb + 1 < cards.Count)
            cards[nb + 1].SetActive(true);

        nb = nb + 2;
    }

    public void Back()
    {
        nb = nb - 2;
        if (nb < 0) return;
        print(nb);

        if (nb == 0)
        {
            titleCard.SetActive(true);
            title.SetActive(false);
            back.interactable = false;
            EventSystem.current.SetSelectedGameObject(next.gameObject);
        }
        cards[nb].SetActive(false);
        cards[nb + 1].SetActive(false);

        if(nb - 1 > 0)
            cards[nb - 1].SetActive(true);

        if(nb - 2 > 0)
            cards[nb - 2].SetActive(true);
        print(nb);
    }
}
