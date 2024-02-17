using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using TMPro;
using UnityEngine.UIElements;

public class Leaderboard : MonoBehaviour
{
    public List<CheckpointTracker> car = new List<CheckpointTracker>();
    private TextMeshProUGUI tmpro = null;
    private StringBuilder sb = new StringBuilder();
    private string focuscar = "";
    public TextMeshProUGUI playerPosition;
    public CheckpointTracker player;
    public int padAmount = 35;

    private void Start()
    {
        //get reference to the leaderboard text component
        tmpro = GetComponent<TextMeshProUGUI>();
        tmpro.text = "Leaderboard";

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
        playerPosition.text = player.position + "/8";
    }

    public int DoLeaderboard(string DriverName)
    {
        int ret = -1;
        sb.Clear();

        //sort the cars by number of checkpoints passed (descending=most to least)
        car = car.OrderByDescending(x => x.checkpoints_passed).ToList();

        //compose the text list of cars
        for (int i = 0; i < car.Count; i++)
        {
            if (car[i].DriverName == focuscar)
                sb.AppendLine(string.Format("{0} {1} <-- ", i + 1, car[i].DriverName+ car[i].elapsedTimeDisplay));
            else
                sb.AppendLine(string.Format(car[i].DriverName.PadLeft(padAmount)));

            if (car[i].DriverName == DriverName)
                ret = i + 1;

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