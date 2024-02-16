using System.Collections.Generic;
using System.Text;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class LeaderboardTimes : MonoBehaviour
{
    public List<CheckpointTracker> car = new List<CheckpointTracker>();
    private TextMeshProUGUI tmpro = null;
    private StringBuilder sb = new StringBuilder();
    private string focuscar = "";
    public CheckpointTracker player;

    private void Start()
    {
        //get reference to the leaderboard text component
        tmpro = GetComponent<TextMeshProUGUI>();
        tmpro.text = "0:00";

        //Transform lookat = Camera.main.GetComponent<CamFollow>().lookat;

        //get reference to all the cars
        GameObject parent = GameObject.Find("Ships");
        for (int i = 0; i < parent.transform.childCount; i++)
        {
            car.Add(parent.transform.GetChild(i).gameObject.GetComponent<CheckpointTracker>());
            //if (lookat == parent.transform.GetChild(i))
            // focuscar = car[car.Count - 1].DriverName;
        }

    }

    private void Update()
    {
        
    }

    public int DoLeaderboard()
    {
        int ret = -1;
        sb.Clear();

        //sort the cars by number of checkpoints passed (descending=most to least)
        //car = car.OrderByDescending(x => x.bestLapAI).ToList();

        //compose the text list of cars
        for (int i = 0; i < car.Count; i++)
        {
            if (car[i].DriverName == focuscar)
                sb.AppendLine(string.Format("0:00 <-- ", i + 1, car[i].elapsedTimeDisplay));
            else
                sb.AppendLine(string.Format("0:00", i + 1, car[i].bestTimeDisplay));

            //if (car[i].DriverName == DriverName)
            //    ret = i + 1;

            if (car[i].DriverName == "Player")
            {
                car[i].DriverName = "<color=red>Player</color>";
            }
        }
        tmpro.text = sb.ToString();


        return ret;
    }

    private void ChangeColor(TextMeshProUGUI textMeshPro, int characterIndex, Color color)
    {
        textMeshPro.ForceMeshUpdate();
        Color[] colors = textMeshPro.mesh.colors;
        colors[4 * characterIndex] = color;
        colors[4 * characterIndex + 1] = color;
        colors[4 * characterIndex + 2] = color;
        colors[4 * characterIndex + 3] = color;
        textMeshPro.mesh.colors = colors;
        textMeshPro.UpdateGeometry(textMeshPro.mesh, 0);
    }
}