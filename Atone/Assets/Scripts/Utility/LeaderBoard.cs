using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

public class LeaderBoard : MonoBehaviour
{
    public static string path = "Assets/ScoreBoard";

    public static void WriteLeaderBoard() {
        if (!Directory.Exists(path))
            print("Repertoire inexistant");

        string[] files = Directory.GetFiles(path);
        Dictionary<string, float> leaderboard = new Dictionary<string, float>();

        foreach(string file in files){
            StreamReader sr = new StreamReader(file);
            string line = sr.ReadLine();
            while (line != null) {
                if (line.Contains("Score Total"))
                    leaderboard.Add(file.Split("/")[file.Split("/").Length - 1].Split("_")[0], float.Parse(line.Split(" : ")[1]));
                line = sr.ReadLine();
            }   
        }

        StreamWriter sw = new StreamWriter(path + "/Leaderboard.txt");
        foreach (KeyValuePair<string, float> player in leaderboard.OrderBy(key => key.Value))
            sw.WriteLine(player.Key + "    " + player.Value);
        sw.Close();
    }

}
