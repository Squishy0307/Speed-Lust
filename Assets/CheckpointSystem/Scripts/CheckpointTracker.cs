using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class CheckpointTracker : MonoBehaviour
{
    public string DriverName = "";
    public int checkpoints_passed = 0;
    public int finishLinePass = 0;
    private bool isRightDirection;
    public string checkpoint_name = "";
    private Checkpoints script_checkpoints = null;
    private Leaderboard script_leaderboard = null;
    private TextMeshPro tmpro = null;
    public bool collectedCheckpoint = false;
    public Checkpoints chk;
    public GameObject Buttons;
    public int position;
    public TextMeshProUGUI positionText;
    public Timer timer;
    public float elapsedTime;
    public string elapsedTimeDisplay;
    public float startTime;
    private StartCountdown startCountdown;
    public string Name;
    public Transform respawnPoint;
    private Rigidbody rb;
    private VehicleMovement movement;
    public TextMeshProUGUI lapText;
    public float bestTime;
    public string bestTimeDisplay;
    public GameObject EndOfRaceUI;
    public GameObject ControlsUI;
    public GameObject disableAtEnd;
    public Quaternion rememberRotation;
    public Animator leaderboardAnim;
    public Animator bestTimeAnim;

    bool joshBool; // enabled at end of race. makes engine sound stop. found at the beginning of Update().

    // Start is called before the first frame update
    void Start()
    {
        //EndOfRaceUI.SetActive(false);
        movement = GetComponent<VehicleMovement>();

        rb = GetComponent<Rigidbody>(); 
        startCountdown = FindObjectOfType<StartCountdown>();
        Buttons.SetActive(false);
        GameObject go = null;
        go = GameObject.Find("TrackSpline");
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
        position = 8;
    }

    // Update is called once per frame
    void Update()
    {
        if (joshBool)
        {
            GetComponent<AudioSource>().pitch -= Time.deltaTime / 2;
            GetComponent<AudioSource>().volume -= Time.deltaTime / 2;
        }

        if(ControlsUI.activeSelf && movement.isPlayer)
        {
            if (movement.GetCurrentSpeed() >= 5f)
            {
                Debug.Log("Pressed");
                ControlsUI.SetActive(false);
                //startCountdown.BeginCountdown();
            }
        }
        if (finishLinePass >= 1)
        {

            elapsedTime = Time.time - startTime;
            elapsedTimeDisplay = elapsedTime.ToString("0:00".PadLeft(18 - Name.Length));
            //bestTimeDisplay = timer.currentAIBestTime.ToString("0:00");
            //Debug.Log(elapsedTime);
        }

        //if(Buttons.activeSelf)
        //{
        //    if (playerControlls.ShipControls.Select.IsPressed())
        //    {
        //        Debug.Log("PressedA");
        //        Scene_Manager.Instance.LoadScene(2);
        //    }
        //    if (playerControlls.ShipControls.Exit.IsPressed())
        //    {
        //        Debug.Log("PressedB");
        //        Scene_Manager.Instance.LoadScene(1);
        //    }
        //}

        if(gameObject.CompareTag("Player"))
        {
            if (elapsedTime > 0)
            {
                lapText.text = "Laps " + finishLinePass + "/2";
            }
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (script_checkpoints != null)
        {
            if (other.CompareTag("Checkpoint") == true && other.transform == gameObject.GetComponent<VehicleMovement>().nextCheckpoint.transform)
            {
                //Debug.Log("Working");

                if(gameObject.name == "Player SHIP")
                {
                    chk.GetNextCheckpoint();
                }
                

                //have to first pass finish line to start racing
                if (other.name == "Instance-0")
                {
                    finishLinePass += 1;
                    checkpoint_name = other.name;
                    //LBT.DoLeaderboard();

                    if (finishLinePass >= 3 && gameObject.CompareTag("Player"))
                    {
                        EndRace();
                    }
                    
                }

                //didnt get to finish line yet no counting
                if (finishLinePass == 0)
                    return;

                //count checkpoints passed
                checkpoint_name = script_checkpoints.GetNextCheckpointName(checkpoint_name);
                gameObject.GetComponent<VehicleMovement>().nextCheckpoint = chk.gameObject.transform.Find(checkpoint_name);
                checkpoints_passed ++;
                collectedCheckpoint = true;
                if (script_leaderboard != null)
                {
                    position = script_leaderboard.DoLeaderboard(DriverName);
                    if (position > -1)
                    {
                        //tmpro.text = string.Format("{0} {1}", position, DriverName);
                        //AddReward(rwd.mult_position / (float)position);
                    }
                    else
                    {
                        tmpro.text = DriverName;
                        //positionText.text = "Pos " + position + " : 8";
                    }
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        collectedCheckpoint = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("KillZone"))
        {
            Debug.Log("Killzoned");
            //rememberRotation = gameObject.transform.rotation;
            StartCoroutine(Respawn());
        }
    }

    IEnumerator Respawn()
    {
        if(gameObject.CompareTag("Player"))
        {
            Fader.Instance.RespawnFade();
        }
        
        movement.respawning = true;
        
        yield return new WaitForSeconds(0.75f);
        rb.velocity = new Vector3(0, 0, 0);
        transform.position = respawnPoint.transform.position + new Vector3(0, 0, 0);
        //transform.eulerAngles = new Vector3(respawnPoint.transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);
        //transform.rotation = respawnPoint.transform.rotation;
        //transform.rotation = rememberRotation;
        gameObject.transform.LookAt(movement.nextCheckpoint);
        
        movement.respawning = false;
    }

    private string GenerateName()
    {
        int len = Random.Range(3, 5);
        System.Random r = new System.Random();
        string[] consonants = { "b", "c", "d", "f", "g", "h", "j", "k", "l", "m", "l", "n", "p", "q", "r", "s", "t", "v", "w", "x" };
        string[] vowels = { "a", "e", "i", "o", "u", "ae", "y" };
        //string Name = "";
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
            Name = "Player";
            //GetComponent<TextMeshProUGUI>().color = new Color(1, 0, 0);
        }

        return Name;


    }

    public void EndRace()
    {
        disableAtEnd.SetActive(false);
        leaderboardAnim.SetTrigger("RaceEnded");
        bestTimeAnim.SetTrigger("ShowTime");
        GameManager.Instance.RaceStarted = false;
        Buttons.SetActive(true);
        EndOfRaceUI.SetActive(true);
        joshBool = true;
    }
}
