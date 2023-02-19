using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class IntroductionNavigation : MonoBehaviour
{
    [SerializeField] GameObject titleCard;
    [SerializeField] GameObject title;
    [SerializeField] GameObject txtFirstS;
    [SerializeField] GameObject txtFirstD;
    [SerializeField] GameObject txtSecondS;
    [SerializeField] GameObject txtSecondD;
    [SerializeField] GameObject crosshairS;
    [SerializeField] GameObject crosshairD;
    [SerializeField] GameObject endIntro;

    [SerializeField] Button back;
    [SerializeField] Button next;

    List<GameObject> cards;
    int nb = 0;

    void Awake()
    {
        cards = new List<GameObject>(new GameObject[] { txtFirstS, txtFirstD, txtSecondS, txtSecondD, crosshairS, crosshairD, endIntro });
        next.onClick.AddListener(Next);
        back.onClick.AddListener(Back);
    }

    public void Next()
    {
        back.interactable = true;
        if (nb > cards.Count - 2)
        {
            title.SetActive(false);
            back.gameObject.SetActive(false);
            next.gameObject.SetActive(false);
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
        }
        cards[nb].SetActive(false);
        cards[nb + 1].SetActive(false);

        cards[nb - 1].SetActive(true);
        cards[nb - 2].SetActive(true);
        print(nb);
    }
}
