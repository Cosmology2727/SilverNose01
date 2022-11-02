using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Rigidbody ThisEnemy;

    [SerializeField]
    public Animator AnimatorRef;

    [System.NonSerialized] public GameObject PlayerObj;
    [System.NonSerialized] public GameObject LastPlayerPos;

    [SerializeField]
    public GameObject SightOrigin;
    public LayerMask EnemyLayerMask;

    [SerializeField]
    public GameObject EnemyLoc1;
    [SerializeField]
    public GameObject EnemyLoc2;
    [SerializeField]
    public GameObject EnemyLoc3;
    [System.NonSerialized] public Vector3 NextWalkPos;

    [SerializeField]
    public float ThisSpeed = 1;

    [SerializeField]
    public GameObject GrenadeObj;
    [SerializeField]
    public GameObject SightViewObj;
    [SerializeField]
    public GameObject RangeViewObj;

    [SerializeField]
    public float ShootRange;

    [System.NonSerialized] public bool PlayerInSights = false;
    [System.NonSerialized] public bool PlayerInRange = false;
    [System.NonSerialized] public bool PlayerInLine = false;

    [SerializeField]
    public int AlertAmount = 90; //Make this bigger, like 900, 90 is only like 3 seconds for testing purposes
    [System.NonSerialized] public int AlertTimer;




    [SerializeField]
    public bool IsAlert = false;

    [SerializeField]
    public int ShootDelay = 60;
    [System.NonSerialized] public int ShootTimer;

    [System.NonSerialized] public int CurrentMood = 1;
    //0 = Dead
    //1 = Standing
    //2 = Crouching
    //3 = Walking a path
    //4 = Walking to random spots
    //5 = Running (is alerted, but no line of sight OR too far away)
    //6 = Shooting (is alerted, and has line of sight, and is close enough)
    //7 = Walking back to default
    //8 = Searching for player

    [SerializeField] public bool IsStanding;
    [SerializeField] public bool IsCrouching;
    [SerializeField] public bool IsWalkPath;
    [SerializeField] public bool IsWalkRandom;
    [SerializeField] public bool IsGrenader;
    [SerializeField] public bool IsSniper;

    [SerializeField]
    public Vector3 StandLoc;

    [System.NonSerialized] public Vector3 PlayerLastPos;

    [System.NonSerialized] public int WaitTimer = -1;
    [SerializeField]
    public int WaitAmount = 90;
    

    private NavMeshAgent ThisNavMesh;



    void Start()
    {
        ThisNavMesh = ThisEnemy.GetComponent<NavMeshAgent>();

        PlayerObj = FindObjectOfType<PlayerFinder>().gameObject;

        StandLoc = ThisEnemy.position;
        NextWalkPos = StandLoc;

        AlertTimer = AlertAmount;

        ShootTimer = ShootDelay;

        GrenadeObj.SetActive (false);
        if (IsGrenader)
        {
            ThisEnemy.GetComponent<EnemyStats>().HasGrenade = true;
        }

        if (IsSniper)
        {
            SightViewObj.transform.localScale *= 3;
        }

        if (IsStanding)
        {
            CurrentMood = 1;
        }
        if (IsCrouching)
        {
            CurrentMood = 2;
        }
        if (IsWalkPath)
        {
            CurrentMood = 3;
        }
        if (IsWalkRandom)
        {
            CurrentMood = 4;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if ((other.gameObject.tag == "Player" & CurrentMood != 0))
        //Debug.Log("enter sight");
        {
            PlayerInSights = true;
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player" & CurrentMood != 0)
        {
            //Debug.Log("exit sight");
            PlayerInSights = false;

            //Checks to see if the player is still in line of sight
            if (Physics.Raycast(ThisEnemy.position, (PlayerObj.transform.position - ThisEnemy.gameObject.transform.position), out var hit))
            {
                //Debug.Log("Sight exit ray " + hit.transform.tag);
                if (hit.transform.tag == "Player") //If the player leaves the field of sight, and there's still line of sight, then alert begins (leaving line of sight will be in update)
                {
                    IsAlert = true;
                    AlertTimer = AlertAmount;
                    PlayerLastPos = PlayerObj.transform.position;
                }
            }
        }
    }

    public void InRangeTrigger(GameObject OtherFromTrigger)
    {
        if (OtherFromTrigger.gameObject.tag == "Player" & CurrentMood != 0)
        {
            //Debug.Log("enter range");
            PlayerInRange = true;
            //IsAlert = true;
            //CurrentMood = 6;
        }
    }

    public void OutRangeTrigger(GameObject OtherFromTrigger)
    {
        if (OtherFromTrigger.gameObject.tag == "Player" & CurrentMood != 0)
        {
            //Debug.Log("exit range");
            PlayerInRange = false;
            //IsAlert = true;
            //CurrentMood = 5;
        }
    }

    public void DoneShooting()
    {
        ShootTimer = ShootDelay * 2;
    }

    void Update()
    {
        if (CurrentMood == 0) //Dead
        {
            ThisNavMesh.isStopped = true;
        }
        else if (CurrentMood != 0)
        {
            //Checks to see if the player is still in line of sight
            if (Physics.Raycast(SightOrigin.transform.position, (PlayerObj.transform.position - ThisEnemy.gameObject.transform.position), out var hit)) // - ThisEnemy.gameObject.transform.position
            {
                //Debug.Log("Update ray " + hit.transform.tag);
                //Debug.DrawRay(SightOrigin.transform.position, (PlayerObj.transform.position - ThisEnemy.gameObject.transform.position), Color.red); 
                
                if (hit.transform.tag != "Player" && PlayerInLine == true)
                {
                    //This makes it so that if the line of sight is lost WHILE STILL PLAYERINSIGHT then it will go alert mode
                    if (PlayerInSights)
                    {
                        IsAlert = true;
                        AlertTimer = AlertAmount;
                        PlayerLastPos = PlayerObj.transform.position;
                        PlayerInLine = false;
                        CurrentMood = 5;
                    }
                }
                
                else if (hit.transform.tag != "Player")
                {
                    PlayerInLine = false;
                }
                else if (hit.transform.tag == "Player")
                {
                    PlayerInLine = true;
                    if (PlayerInSights !& PlayerInRange)
                    {
                        IsAlert = true;
                        AlertTimer = AlertAmount;
                        CurrentMood = 5;
                        PlayerLastPos = PlayerObj.transform.position;
                    }
                }

                if (AlertTimer == 0)
                {
                    CurrentMood = 7;
                }
                if (PlayerInLine & PlayerInSights &! PlayerInRange)
                {
                    CurrentMood = 5;
                    PlayerLastPos = PlayerObj.transform.position;
                    IsAlert = true;
                    AlertTimer = AlertAmount;
                }

                if (PlayerInLine & PlayerInRange)
                {
                    CurrentMood = 6;
                    IsAlert = true;
                    AlertTimer = AlertAmount;
                    PlayerLastPos = PlayerObj.transform.position;
                }





                
            }

            //Determines the current mood based on sight, range, and line of sight
            //0 = Dead
            //1 = Standing
            //2 = Crouching
            //3 = Walking a path
            //4 = Walking to random spots
            //5 = Running (is alerted, but no line of sight OR too far away)
            //6 = Shooting (is alerted, and has line of sight, and is close enough)
                //TURN ALERT OFF
            //7 = Walking back to default
            //8 = Searching for player






            //Debug.Log(CurrentMood + "   Run " + AnimatorRef.GetBool("RunAnim") + "   Shoot3 " + AnimatorRef.GetBool("Shoot3Anim"));
        
            if ((IsAlert && !PlayerInSights) & CurrentMood != 0)

            {
                AlertTimer -= 1;
                if (AlertTimer >= 0)
                {
                    CurrentMood = 5;
                }
                
                if (AlertTimer < 0)
                {
                    IsAlert = false;
                    AnimatorRef.SetBool("WalkAnim", true);

                    AnimatorRef.SetBool("AimingAnim", false);
                    AnimatorRef.SetBool("ClimbingAnim", false);
                    AnimatorRef.SetBool("CrouchDownAnim", false);
                    AnimatorRef.SetBool("CrouchedAnim", false);
                    AnimatorRef.SetBool("CrouchedShootAnim", false);
                    AnimatorRef.SetBool("CrouchedUpAnim", false);
                    AnimatorRef.SetBool("GrenadeAnim", false);
                    AnimatorRef.SetBool("Idle1Anim", false);
                    AnimatorRef.SetBool("Idle2Anim", false);
                    AnimatorRef.SetBool("Idle3Anim", false);
                    AnimatorRef.SetBool("Idle4Anim", false);
                    AnimatorRef.SetBool("RunAnim", false);
                    AnimatorRef.SetBool("Shoot3Anim", false);
                    AnimatorRef.SetBool("Shoot5Anim", false);
                    AnimatorRef.SetBool("StandAnim", false);
                    //Walk back to StandLoc OR go back to walkpath, or walkrandom
                    CurrentMood = 7;
                }
            }



            if (CurrentMood == 1) //Standing
            {
                ThisNavMesh.isStopped = true;
                AnimatorRef.SetBool("StandAnim", true);

                AnimatorRef.SetBool("AimingAnim", false);
                AnimatorRef.SetBool("ClimbingAnim", false);
                AnimatorRef.SetBool("CrouchDownAnim", false);
                AnimatorRef.SetBool("CrouchedAnim", false);
                AnimatorRef.SetBool("CrouchedShootAnim", false);
                AnimatorRef.SetBool("CrouchedUpAnim", false);
                AnimatorRef.SetBool("GrenadeAnim", false);
                AnimatorRef.SetBool("Idle1Anim", false);
                AnimatorRef.SetBool("Idle2Anim", false);
                AnimatorRef.SetBool("Idle3Anim", false);
                AnimatorRef.SetBool("Idle4Anim", false);
                AnimatorRef.SetBool("RunAnim", false);
                AnimatorRef.SetBool("Shoot3Anim", false);
                AnimatorRef.SetBool("Shoot5Anim", false);
                AnimatorRef.SetBool("WalkAnim", false);

                //ThisEnemy.velocity = Vector3.zero;   probably not needed now that I use navmesh to move
                if (WaitTimer > 0)
                {
                    WaitTimer -= 1;
                }
                else if (WaitTimer <= 0)
                {
                    if (IsWalkPath)
                    {
                        CurrentMood = 3;
                    }
                    else if (IsWalkRandom)
                    {
                        CurrentMood = 4;
                    }
                }
            }
            if (CurrentMood == 2) //Crouching
            {
                ThisNavMesh.isStopped = true;
            }
            if (CurrentMood == 3) //Walk path
            {
                ThisNavMesh.isStopped = false;
                ThisNavMesh.speed = ThisSpeed * Time.deltaTime;
                AnimatorRef.SetBool("WalkAnim", true);

                AnimatorRef.SetBool("AimingAnim", false);
                AnimatorRef.SetBool("ClimbingAnim", false);
                AnimatorRef.SetBool("CrouchDownAnim", false);
                AnimatorRef.SetBool("CrouchedAnim", false);
                AnimatorRef.SetBool("CrouchedShootAnim", false);
                AnimatorRef.SetBool("CrouchedUpAnim", false);
                AnimatorRef.SetBool("GrenadeAnim", false);
                AnimatorRef.SetBool("Idle1Anim", false);
                AnimatorRef.SetBool("Idle2Anim", false);
                AnimatorRef.SetBool("Idle3Anim", false);
                AnimatorRef.SetBool("Idle4Anim", false);
                AnimatorRef.SetBool("RunAnim", false);
                AnimatorRef.SetBool("Shoot3Anim", false);
                AnimatorRef.SetBool("Shoot5Anim", false);
                AnimatorRef.SetBool("StandAnim", false);

                if ((NextWalkPos == StandLoc) & (NextWalkPos == transform.position))
                {
                    if (EnemyLoc3 != null)
                    {

                    }
                    if (NextWalkPos == StandLoc & EnemyLoc1 != null)
                    {
                        NextWalkPos = EnemyLoc1.transform.position;
                    }

                }
                ThisNavMesh.destination = NextWalkPos;
            }
            if (CurrentMood == 4) //Walk random
            {
                ThisNavMesh.isStopped = false;
                ThisNavMesh.speed = ThisSpeed * Time.deltaTime;
                AnimatorRef.SetBool("WalkAnim", true);

                AnimatorRef.SetBool("AimingAnim", false);
                AnimatorRef.SetBool("ClimbingAnim", false);
                AnimatorRef.SetBool("CrouchDownAnim", false);
                AnimatorRef.SetBool("CrouchedAnim", false);
                AnimatorRef.SetBool("CrouchedShootAnim", false);
                AnimatorRef.SetBool("CrouchedUpAnim", false);
                AnimatorRef.SetBool("GrenadeAnim", false);
                AnimatorRef.SetBool("Idle1Anim", false);
                AnimatorRef.SetBool("Idle2Anim", false);
                AnimatorRef.SetBool("Idle3Anim", false);
                AnimatorRef.SetBool("Idle4Anim", false);
                AnimatorRef.SetBool("RunAnim", false);
                AnimatorRef.SetBool("Shoot3Anim", false);
                AnimatorRef.SetBool("Shoot5Anim", false);
                AnimatorRef.SetBool("StandAnim", false);
            }
            if (CurrentMood == 5) //Running
            {
                ThisNavMesh.isStopped = false;
                ThisNavMesh.speed = ThisSpeed * 3 * Time.deltaTime;

                AnimatorRef.SetBool("RunAnim", true);

                AnimatorRef.SetBool("AimingAnim", false);
                AnimatorRef.SetBool("ClimbingAnim", false);
                AnimatorRef.SetBool("CrouchDownAnim", false);
                AnimatorRef.SetBool("CrouchedAnim", false);
                AnimatorRef.SetBool("CrouchedShootAnim", false);
                AnimatorRef.SetBool("CrouchedUpAnim", false);
                AnimatorRef.SetBool("GrenadeAnim", false);
                AnimatorRef.SetBool("Idle1Anim", false);
                AnimatorRef.SetBool("Idle2Anim", false);
                AnimatorRef.SetBool("Idle3Anim", false);
                AnimatorRef.SetBool("Idle4Anim", false);
                AnimatorRef.SetBool("Shoot3Anim", false);
                AnimatorRef.SetBool("Shoot5Anim", false);
                AnimatorRef.SetBool("StandAnim", false);
                AnimatorRef.SetBool("WalkAnim", false);
                ThisNavMesh.destination = PlayerLastPos;
                if ((!PlayerInLine | !PlayerInSights) & transform.position == PlayerLastPos)
                {
                    CurrentMood = 8;
                }
            }
            if (CurrentMood == 6) //Shooting
            {
                ThisNavMesh.isStopped = true;

                if (ShootTimer <= 0)
                {
                    AnimatorRef.SetBool("Shoot3Anim", true);
                    AnimatorRef.SetBool("AimingAnim", false);
                    AnimatorRef.SetBool("ClimbingAnim", false);
                    AnimatorRef.SetBool("CrouchDownAnim", false);
                    AnimatorRef.SetBool("CrouchedAnim", false);
                    AnimatorRef.SetBool("CrouchedShootAnim", false);
                    AnimatorRef.SetBool("CrouchedUpAnim", false);
                    AnimatorRef.SetBool("GrenadeAnim", false);
                    AnimatorRef.SetBool("Idle1Anim", false);
                    AnimatorRef.SetBool("Idle2Anim", false);
                    AnimatorRef.SetBool("Idle3Anim", false);
                    AnimatorRef.SetBool("Idle4Anim", false);
                    AnimatorRef.SetBool("RunAnim", false);
                    AnimatorRef.SetBool("Shoot5Anim", false);
                    AnimatorRef.SetBool("StandAnim", false);
                    AnimatorRef.SetBool("WalkAnim", false);
                }
                else
                {
                    ShootTimer -= 1;
                    AnimatorRef.SetBool("AimingAnim", true);
                    
                    AnimatorRef.SetBool("AimingAnim", false);
                    AnimatorRef.SetBool("ClimbingAnim", false);
                    AnimatorRef.SetBool("CrouchDownAnim", false);
                    AnimatorRef.SetBool("CrouchedAnim", false);
                    AnimatorRef.SetBool("CrouchedShootAnim", false);
                    AnimatorRef.SetBool("CrouchedUpAnim", false);
                    AnimatorRef.SetBool("GrenadeAnim", false);
                    AnimatorRef.SetBool("Idle1Anim", false);
                    AnimatorRef.SetBool("Idle2Anim", false);
                    AnimatorRef.SetBool("Idle3Anim", false);
                    AnimatorRef.SetBool("Idle4Anim", false);
                    AnimatorRef.SetBool("RunAnim", false);
                    AnimatorRef.SetBool("Shoot3Anim", false);
                    AnimatorRef.SetBool("Shoot5Anim", false);
                    AnimatorRef.SetBool("StandAnim", false);
                    AnimatorRef.SetBool("WalkAnim", false);
                }


                




                /*
                AnimatorRef.SetBool("Shoot5Anim", true);
                AnimatorRef.SetBool("RunAnim", false);
                AnimatorRef.SetBool("StandAnim", false);
                AnimatorRef.SetBool("WalkAnim", false);
                AnimatorRef.SetBool("CrouchAnim", false);
                AnimatorRef.SetBool("Shoot3Anim", false);
                */
            }
            if (CurrentMood == 7) //Walking back to default
            {
                ThisNavMesh.isStopped = false;
                ThisNavMesh.speed = ThisSpeed * Time.deltaTime;
                ThisNavMesh.destination = StandLoc;
                if (transform.position == StandLoc)
                {
                    CurrentMood = 1;
                    WaitTimer = WaitAmount;
                }
            }
            if (CurrentMood == 8) //Searching for player
            {
                ThisNavMesh.isStopped = true;
                //Walk to random locations near PlayerLastPos
            }
        }
    }



        //This one actually works, makes the enemy move toward the player
        //ThisNavMesh.destination = FindObjectOfType<PlayerFinder>().GetComponent<PlayerFinder>().PlayerPos;




        //Debug.Log("EnemyAI PlayerFinder ref " + FindObjectOfType<PlayerFinder>().GetComponent<PlayerFinder>().PlayerPos); 
        //Debug.Log("Destination = " + ThisNavMesh.destination);
        //Debug.Log(ThisNavMesh.destination + " " + ThisEnemy.transform.position);
        //Debug.Log(CurrentMood);

            //This code moves the enemy quite well, but it doesn't follow a path, just goes straight at the player
            //ThisEnemy.velocity = (PlayerObj.transform.position - ThisEnemy.gameObject.transform.position) * ThisSpeed * Time.deltaTime;

        //Doing it with Translate makes it ignore rigid bodies. It also makes it relative to local, which is a problem. You CAN give it a last variable of "Space" to make it worldspace.
        //ThisEnemy.gameObject.transform.Translate ((MoveTowards.transform.position - ThisEnemy.gameObject.transform.position).normalized * ThisSpeed * Time.deltaTime);
}