using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Checkpoint : MonoBehaviour
{
    public string niveau;
    public string sequence;
    public bool endGame = false;

    public static string path = "Assets/ScoreBoard/PlayerScore";
    public static string pathLeaderboard = "Assets/ScoreBoard";
    public void OnTriggerEnter(Collider other){
        Debug.Log(endGame + "end");
        if(!GameManager.Instance.isReady) {return;}
        
        if (other.CompareTag(PlayerManager.PLAYER_TAG)){
            PlayerManager.Instance.playerController.currentCheckpoint = gameObject.transform.position;
            StartCoroutine(DisplayScore.Instance.DisplayScoreWhenPlay(PlayerManager.scoreSequence));
            SaveScore();
            PlayerManager.scoreSequence = 0;
        }

    }

    public void SaveScore()
    {
        if (!PlayerManager.scoreBoard.ContainsKey(niveau))
            PlayerManager.scoreBoard.Add(niveau, new Dictionary<string, float>());
        if (!PlayerManager.scoreBoard[niveau].ContainsKey(sequence))
            PlayerManager.scoreBoard[niveau].Add(sequence, PlayerManager.scoreSequence);
            
        if (endGame){
            //Load White Room
            SceneManager.LoadScene("White Room");
        }
            
    }


}
