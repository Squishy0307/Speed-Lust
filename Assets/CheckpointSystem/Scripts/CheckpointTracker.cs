using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CheckpointTracker : MonoBehaviour
{
    public string DriverName = "";
    public int checkpoints_passed = 0;
    public int finishLinePass = 0;
    private bool isRightDirection;
    private string checkpoint_name = "";
    private Checkpoints script_checkpoints = null;
    private Leaderboard script_leaderboard = null;
    private TextMeshPro tmpro = null;
    public bool collectedCheckpoint = false;


    // Start is called before the first frame update
    void Start()
    {
        GameObject go = null;
        go = GameObject.Find("Checkpoints");
        if (go != null)
            script_checkpoints = go.GetComponent<Checkpoints>();
        go = GameObject.Find("Leaderboard");
        if (go != null)
            script_leaderboard = go.GetComponent<Leaderboard>();

        DriverName = GenerateName();
        if (this.transform.Find("txtName") != null)
        {
            tmpro = this.transform.Find("txtName").GetComponent<TextMeshPro>();
            tmpro.text = DriverName;

            
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (script_checkpoints != null)
        {
            if (other.CompareTag("Checkpoint") == true && !collectedCheckpoint)
            {
                //have to first pass finish line to start racing
                if (other.name == "chk (0)")
                {
                    finishLinePass += 1;
                    checkpoint_name = other.name;
                }

                //didnt get to finish line yet no counting
                if (finishLinePass == 0)
                    return;

                //count checkpoints passed
                checkpoint_name = script_checkpoints.GetNextCheckpointName(checkpoint_name);
                checkpoints_passed += 1;
                collectedCheckpoint = true;
                if (script_leaderboard != null)
                {
                    int position = script_leaderboard.DoLeaderboard(DriverName);
                    if (position > -1)
                    {
                        //tmpro.text = string.Format("{0} {1}", position, DriverName);
                        //AddReward(rwd.mult_position / (float)position);
                    }
                    else
                    {
                        tmpro.text = DriverName;
                        
                    }
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        collectedCheckpoint = false;
    }

    private string GenerateName()
    {
        int len = Random.Range(3, 5);
        System.Random r = new System.Random();
        string[] consonants = { "b", "c", "d", "f", "g", "h", "j", "k", "l", "m", "l", "n", "p", "q", "r", "s", "sh", "zh", "t", "v", "w", "x" };
        string[] vowels = { "a", "e", "i", "o", "u", "ae", "y" };
        string Name = "";
        Name += consonants[r.Next(consonants.Length)].ToUpper();
        Name += vowels[r.Next(vowels.Length)];
        int b = 2; //b tells how many times a new letter has been added. It's 2 right now because the first two letters are already in the name.
        while (b < len)
        {
            Name += consonants[r.Next(consonants.Length)];
            b++;
            Name += vowels[r.Next(vowels.Length)];
            b++;
        }

        if (gameObject.CompareTag("Player"))
        {
            Name = "<color=red>Player</color>";
            //GetComponent<TextMeshProUGUI>().color = new Color(1, 0, 0);
        }

        return Name;


    }
}
