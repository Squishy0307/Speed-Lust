using System.Collections.Generic;
using UnityEngine;

public class Checkpoints : MonoBehaviour
{
    //[HideInInspector]
    public List<GameObject> chk = new List<GameObject>();
    public int index = 0;
    public VehicleMovement player;
    public GameObject currentCheckpoint;

    private void Start()
    {
        for (int i = 0; i < this.transform.childCount; i++)
        {
            if (this.transform.GetChild(i).CompareTag("Checkpoint") == true)
                chk.Add(this.transform.GetChild(i).gameObject);
            currentCheckpoint = this.transform.GetChild(i - chk.Count + 1).gameObject;
            
            index = i;
        }
    }

    public void GetNextCheckpoint()
    {
        player.nextCheckpoint = chk[chk.IndexOf(currentCheckpoint) + 1].transform;
    }
    public int ExtractNumberFromString(string s1)
    {
        //  chk (10) -> 10 -> int 10
        return System.Convert.ToInt32(System.Text.RegularExpressions.Regex.Replace(s1, "[^0-9]", ""));
    }
    public string GetNextCheckpointName(string CurrentCheckpointName)
    {
        // current = chk (2)
        // int 2
        // int 3
        // next chk (3)

        int CurrentNumber = ExtractNumberFromString(CurrentCheckpointName);
        int NextNumber = (CurrentNumber + 1 > chk.Count - 1) ? 0 : CurrentNumber + 1;
        string NextCheckpointName = string.Format("Instance-{0}", NextNumber);
        return NextCheckpointName;
    }
}
