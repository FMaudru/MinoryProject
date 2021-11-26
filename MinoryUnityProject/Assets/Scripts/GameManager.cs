using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public GameObject[] PrefabsTilesList;     // Basic: 0 -- Full: 1 -- Red: 2 -- Blue: 3 -- Yellow: 4 -- RedEscape: 5 -- BlueEscape: 6 -- YellowEscape: 7
    public string[] TypeTilesList;
    public GameObject[] PrefabsCreaturesList; // Red: 0 -- Blue: 1 -- Yellow: 2
    private LevelManager levelManager;
    public GameObject UIVictory;
    public GameObject UIGameOver;

    private Cell[,] level;
    private int playerPosX;
    private int playerPosZ;
    private int nextPlayerPosX;
    private int nextPlayerPosZ;
    private float DelayTimeFinal = 1;
    private bool lockActionPlayer = false;
    private bool gameover = false;

    // Start is called before the first frame update
    void Start()
    {

        // LOAD LEVEL
        level = new Cell[11,7];
        levelManager = FindObjectsOfType<LevelManager>()[0];
        LoadTutorial(levelManager.levelSelect);

        // Activate Player
        level[playerPosX, Mathf.Abs(playerPosZ)].creature.SetStatus("Player");
        ResetTilesWifi();

        VerifyWifi(playerPosX, playerPosZ, 0, true);
        UpdateAntenna();
        UpdateHight();
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0) && !lockActionPlayer){

            float rayLength = 100.0f;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, rayLength))
            {
                if (hit.collider != null)
                {
                    int PlayerClickX = Mathf.RoundToInt(hit.point.x);
                    int PlayerClickZ = Mathf.RoundToInt(hit.point.z);
                    SelectTiles(PlayerClickX, PlayerClickZ);
                }
            }
        }
    }

    public void LoadTutorial(int index)
    {
        if (index == 1)
        {
            // TUTORIAL 1 ---------------------------------------------------------------
            int[] tilesTutorial1 = {
            0, 0, 0, 0, 7, 6, 5, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0,
            0, 1, 0, 0, 0, 0, 7, 6, 5, 1, 0,
            0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};
            Creature[] creatureTutorial1 = {
            new Creature(2,-3,"Right","Yellow", PrefabsCreaturesList[2]),
            new Creature(3,-3,"Right","Blue", PrefabsCreaturesList[1]),
            new Creature(4,-3,"Right","Red", PrefabsCreaturesList[0]),
            new Creature(4,0,"Down","Yellow", PrefabsCreaturesList[2]),
            new Creature(5,0,"Down","Blue", PrefabsCreaturesList[1]),
            new Creature(6,0,"Down","Red", PrefabsCreaturesList[0])};
            int playerPosXTutorial1 = 4;
            int playerPosZTutorial1 = -3;
            LoadLevel(tilesTutorial1, creatureTutorial1, playerPosXTutorial1, playerPosZTutorial1);
        }
        else if (index == 2)
        {
            // TUTORIAL 2 ---------------------------------------------------------------
            int[] tilesTutorial2 = {
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
            1, 0, 0, 1, 0, 0, 1, 0, 0, 5, 1,
            1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};
            Creature[] creatureTutorial2 = {
            new Creature(1,-3,"Right","Yellow", PrefabsCreaturesList[2]),
            new Creature(4,-3,"Right","Blue", PrefabsCreaturesList[1]),
            new Creature(7,-3,"Right","Red", PrefabsCreaturesList[0])};
            int playerPosXTutorial2 = 1;
            int playerPosZTutorial2 = -3;
            LoadLevel(tilesTutorial2, creatureTutorial2, playerPosXTutorial2, playerPosZTutorial2);
        }
        else if (index == 3)
        {
            // TUTORIAL 3 ---------------------------------------------------------------
            int[] tilesTutorial3 = {
            0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0,
            0, 0, 0, 1, 3, 1, 1, 1, 0, 1, 0,
            0, 0, 0, 1, 0, 4, 7, 1, 0, 1, 0,
            1, 1, 1, 1, 0, 1, 1, 0, 0, 1, 0,
            0, 0, 0, 0, 0, 2, 1, 6, 1, 1, 0,
            0, 0, 0, 0, 1, 5, 1, 2, 1, 0, 0,
            0, 0, 0, 0, 1, 1, 1, 1, 1, 0, 0};
            Creature[] creatureTutorial3 = {
            new Creature(6, 0,"Down","Red", PrefabsCreaturesList[0]),
            new Creature(0,-5,"Right","Blue", PrefabsCreaturesList[1]),
            new Creature(1,-5,"Right","Yellow", PrefabsCreaturesList[2]),
            new Creature(2,-5,"Up","Red", PrefabsCreaturesList[0])};
            int playerPosXTutorial3 = 2;
            int playerPosZTutorial3 = -5;
            LoadLevel(tilesTutorial3, creatureTutorial3, playerPosXTutorial3, playerPosZTutorial3);
        }
        // ---------------------------------------------------------------------------

    }

    public void SelectTiles(int x, int z)
    {
        if (level[x, Mathf.Abs(z)].HaveCreature())
        {
            if (level[x, Mathf.Abs(z)].creature.CanControl())
            {
                level[playerPosX, Mathf.Abs(playerPosZ)].creature.SetStatus("On");
                playerPosX = x;
                playerPosZ = z;
                level[playerPosX, Mathf.Abs(playerPosZ)].creature.SetStatus("Player");
                UpdateAntenna();
            }
        } else if (x == playerPosX - 1 && z == playerPosZ && !level[playerPosX, Mathf.Abs(playerPosZ)].creature.GetDie() && level[playerPosX - 1, Mathf.Abs(playerPosZ)].tile.GetTypeTile() != "Full")
        {
            VerifyMoving(x, z, "Left");
        } else if (x == playerPosX + 1 && z == playerPosZ && !level[playerPosX, Mathf.Abs(playerPosZ)].creature.GetDie() && level[playerPosX + 1, Mathf.Abs(playerPosZ)].tile.GetTypeTile() != "Full")
        {
            VerifyMoving(x, z, "Right");
        } else if (z == playerPosZ - 1 && x == playerPosX && !level[playerPosX, Mathf.Abs(playerPosZ)].creature.GetDie() && level[playerPosX, Mathf.Abs(playerPosZ - 1)].tile.GetTypeTile() != "Full")
        {
            VerifyMoving(x, z, "Down");
        } else if (z == playerPosZ + 1 && x == playerPosX && !level[playerPosX, Mathf.Abs(playerPosZ)].creature.GetDie() && level[playerPosX, Mathf.Abs(playerPosZ + 1)].tile.GetTypeTile() != "Full")
        {
            VerifyMoving(x, z, "Up");
        }
    }

    public void VerifyMoving(int x, int z, string looking)
    {
        if(level[x, Mathf.Abs(z)].tile.GetStatus() != "Full")
        {
            lockActionPlayer = true;
            float timeRotate = level[playerPosX, Mathf.Abs(playerPosZ)].creature.RotateLooking(looking);
            nextPlayerPosX = x;
            nextPlayerPosZ = z;
            if (timeRotate > 0)
            {
                StartCoroutine(WaitingRotate(playerPosX, playerPosZ, x, z, looking, timeRotate));
            } else
            {
                Move(playerPosX, playerPosZ, x, z, looking);
            }
        }
    }
    IEnumerator WaitingRotate(int myX, int myZ,int newX, int newZ, string looking, float duration)
    {
        yield return new WaitForSeconds(duration);
        Move(myX, myZ, newX, newZ, looking);
    }

    public void Move(int myX, int myZ, int newX, int newZ, string looking)
    {
        string myType = level[myX, Mathf.Abs(myZ)].creature.GetTypeCreature();
        switch(level[newX, Mathf.Abs(newZ)].tile.GetTypeTile())
        {
            case "Blue":
                if (myType == "Blue")
                {
                    level[myX, Mathf.Abs(myZ)].creature.Move(newX, newZ, looking);
                } else
                {
                    level[myX, Mathf.Abs(myZ)].creature.MoveToDie(newX, newZ);
                    DelayTimeFinal = 1.5f;
                }
                break;
            case "Red":
                if (myType == "Red")
                {
                    level[myX, Mathf.Abs(myZ)].creature.Move(newX, newZ, looking);
                }
                else
                {
                    level[myX, Mathf.Abs(myZ)].creature.MoveToDie(newX, newZ);
                    DelayTimeFinal = 1.5f;
                }
                break;
            case "Yellow":
                if (myType == "Yellow")
                {
                    level[myX, Mathf.Abs(myZ)].creature.Move(newX, newZ, looking);
                }
                else
                {
                    level[myX, Mathf.Abs(myZ)].creature.MoveToDie(newX, newZ);
                    DelayTimeFinal = 1.5f;
                }
                break;
            default:
                level[myX, Mathf.Abs(myZ)].creature.Move(newX, newZ, looking);
                break;
        }

        if (level[myX, Mathf.Abs(myZ)].creature.GetLooking() != looking)
        {
            DelayTimeFinal = 1.5f;
        }

        string followMeLooking = "Left";
        
        if (myX == newX)
        {
            if (myZ < newZ)
            {
                followMeLooking = "Up";
            } else
            {
                followMeLooking = "Down";
            }
        } else
        {
            if (myX < newX)
            {
                followMeLooking = "Right";
            }
        }
        level[newX, Mathf.Abs(newZ)].creature = level[myX, Mathf.Abs(myZ)].creature;
        level[myX, Mathf.Abs(myZ)].creature = null;
        FollowMe(myX, myZ, followMeLooking);
    }

    public void FollowMe(int x, int z, string looking)
    {
        bool left = false;
        if (x - 1 >= 0 && level[x - 1, Mathf.Abs(z)].HaveCreature())
            if (level[x - 1, Mathf.Abs(z)].creature.GetLooking() == "Right" && !level[x - 1, Mathf.Abs(z)].creature.GetDie())
                left = true;

        bool right = false;
        if (x + 1 < 11 && level[x + 1, Mathf.Abs(z)].HaveCreature())
            if (level[x + 1, Mathf.Abs(z)].creature.GetLooking() == "Left" && !level[x + 1, Mathf.Abs(z)].creature.GetDie())
                right = true;

        bool down = false;
        if (z - 1 > -7 && level[x, Mathf.Abs(z - 1)].HaveCreature())
            if (level[x, Mathf.Abs(z - 1)].creature.GetLooking() == "Up" && !level[x, Mathf.Abs(z - 1)].creature.GetDie())
                down = true;

        bool up = false;
        if (z + 1 <= 0 && level[x, Mathf.Abs(z + 1)].HaveCreature())
            if (level[x, Mathf.Abs(z + 1)].creature.GetLooking() == "Down" && !level[x, Mathf.Abs(z + 1)].creature.GetDie())
                up = true;

        int totalFolow = 0;
        if (left)
            totalFolow++;
        if (right)
            totalFolow++;
        if (down)
            totalFolow++;
        if (up)
            totalFolow++;

        if (totalFolow == 1)
        {
            if (left){
                Move(x - 1, z, x, z, looking);
            } else if (right){
                Move(x + 1, z, x, z, looking);
            } else if (down){
                Move(x, z - 1, x, z, looking);
            } else{
                Move(x, z + 1, x, z, looking);
            }
        } else
        {
            StartCoroutine(FinishMove());
        }
    }

    IEnumerator FinishMove()
    {
        yield return new WaitForSeconds(DelayTimeFinal);
        if (level[nextPlayerPosX, Mathf.Abs(nextPlayerPosZ)].creature.GetDie())
        {
            bool findNewPlayer = false;

            for (int j = 0; j < level.GetLength(1); j++)
            {
                for (int i = 0; i < level.GetLength(0); i++)
                {
                    if (level[i, j].HaveCreature())
                    {
                       if (level[i, j].creature.GetStatus() == "On" && !findNewPlayer)
                        {
                            findNewPlayer = true;
                            playerPosX = i;
                            playerPosZ = j * -1;
                            level[playerPosX, Mathf.Abs(playerPosZ)].creature.SetStatus("Player");
                        }
                    }
                }
            }
            if (!findNewPlayer)
            {
                // GAME OVER
                gameover = true;
                playerPosX = nextPlayerPosX;
                playerPosZ = nextPlayerPosZ;
                UIGameOver.SetActive(true);
            }
        }
        else
        {
            playerPosX = nextPlayerPosX;
            playerPosZ = nextPlayerPosZ;
        }
        ResetTilesWifi();
        ResetWifiCreature();
        if (!gameover)
        {
            VerifyWifi(playerPosX, playerPosZ, 0, true);
            lockActionPlayer = false;
        }
        UpdateAntenna();
        UpdateHight();
        VerifyEscape();
    }

    public void LoadLevel(int[] tiles, Creature[] creatures, int startPlayerPosX, int startPlayerPosZ)
    {
        // CREATE LEVEL

        this.playerPosX = startPlayerPosX;
        this.playerPosZ = startPlayerPosZ;
        int tilePosition = 0;


        for (int j = 0; j < 7; j++)
        {
                for (int i = 0; i < 11; i++)
            {
                    int indexTile = tiles[tilePosition];
                    level[i, j] = new Cell(i, j*-1, new Tile(i, j*-1, TypeTilesList[indexTile], PrefabsTilesList[indexTile]));
                    tilePosition++;
            }
        }
        foreach (Creature creature in creatures)
        {
            level[creature.GetX(), creature.GetZ()*-1].setCreature(creature);
        }
    }

    public void VerifyWifi(int x, int z, float delay, bool start)
    {
        VerifyTileWifi(x, Mathf.Abs(z));
        RaycastHit hit;

        // UP
        if (Physics.Raycast(new Vector3(x,1.15f,z+0.5f), new Vector3(0,0,1.5f), out hit, 1.5f))
        {
            if (hit.collider != null)
            {
                launchVerifyWifi(hit.point, delay, start);
            }
        }

        // UP RIGHT
        if (Physics.Raycast(new Vector3(x+0.5f, 1.15f, z + 0.5f), new Vector3(0.5f, 0, 0.5f), out hit, 0.5f))
        {
            if (hit.collider != null)
            {
                launchVerifyWifi(hit.point, delay, start);
            }
        }

        // RIGHT
        if (Physics.Raycast(new Vector3(x + 0.5f, 1.15f, z), new Vector3(1.5f, 0, 0), out hit, 1.5f))
        {
            if (hit.collider != null)
            {
                launchVerifyWifi(hit.point, delay, start);
            }
        }

        // DOWN RIGHT
        if (Physics.Raycast(new Vector3(x + 0.5f, 1.15f, z-0.5f), new Vector3(0.5f, 0, -0.5f), out hit, 0.5f))
        {
            if (hit.collider != null)
            {
                launchVerifyWifi(hit.point, delay, start);
            }
        }

        // DOWN
        if (Physics.Raycast(new Vector3(x, 1.15f, z - 0.5f), new Vector3(0, 0, -1.5f), out hit, 1.5f))
        {
            if (hit.collider != null)
            {
                launchVerifyWifi(hit.point, delay, start);
            }
        }

        // DOWN LEFT
        if (Physics.Raycast(new Vector3(x - 0.5f, 1.15f, z - 0.5f), new Vector3(-0.5f, 0, -0.5f), out hit, 0.5f))
        {
            if (hit.collider != null)
            {
                launchVerifyWifi(hit.point, delay, start);
            }
        }

        // LEFT
        if (Physics.Raycast(new Vector3(x - 0.5f, 1.15f, z), new Vector3(-1.5f, 0, 0), out hit, 1.5f))
        {
            if (hit.collider != null)
            {
                launchVerifyWifi(hit.point, delay, start);
            }
        }

        // UP LEFT
        if (Physics.Raycast(new Vector3(x - 0.5f, 1.15f, z + 0.5f), new Vector3(-0.5f, 0, +0.5f), out hit, 0.5f))
        {
            if (hit.collider != null)
            {
                launchVerifyWifi(hit.point, delay, start);
            }
        }
    }

    public void launchVerifyWifi(Vector3 positionHit, float delay, bool start)
    {
        int posX = Mathf.RoundToInt(positionHit.x);
        int posZ = Mathf.RoundToInt(positionHit.z);
        if (level[posX, Mathf.Abs(posZ)].creature.isOff())
        {
            level[posX, Mathf.Abs(posZ)].creature.SetStatus("On");
            if (start)
            {
                VerifyWifi(posX, posZ, 0, true);
            } else
            {
                StartCoroutine(CoroutineLaunchWifi(posX, posZ, delay+0.5f));
            }
        }
    }
    IEnumerator CoroutineLaunchWifi(int posX, int posZ, float delay)
    {
        yield return new WaitForSeconds(delay);
        VerifyWifi(posX, posZ, delay, false);
    }

    public void VerifyTileWifi(int x, int z)
    {
        for (int i = (x - 1); i < (x+2); i++)
        {
            for (int j = (z - 1); j < (z+2); j++)
            {
                if (i >= 0 && i < 11 && j >= 0 && j < 7)
                {
                    level[i, j].tile.SetStatus("On");
                }
            }
        }
        if (z + 2 >= 0 && z + 2 < 7)
        {
            level[x, z + 2].tile.SetStatus("On");
        }
        if (z - 2 >= 0 && z - 2 < 7)
        {
            level[x, z - 2].tile.SetStatus("On");
        }
        if (x - 2 >= 0 && x - 2 < 11)
        {
            level[x - 2, z].tile.SetStatus("On");
        }
        if (x + 2 >= 0 && x + 2 < 11)
        {
            level[x + 2, z].tile.SetStatus("On");
        }
    }
    public void ResetTilesWifi()
    {
        for (int j = 0; j < level.GetLength(1); j++)
        {
            for (int i = 0; i < level.GetLength(0); i++)
            {
                level[i, j].tile.SetStatus("Off");
            }
        }
    }

    public void ResetWifiCreature()
    {
        for (int j = 0; j < level.GetLength(1); j++)
        {
            for (int i = 0; i < level.GetLength(0); i++)
            {
                if (level[i, j].HaveCreature())
                {
                    if (level[i, j].creature.GetStatus() == "On")
                    {
                        level[i, j].creature.SetStatus("Off");
                    }
                }
            }
        }
    }

    public void UpdateAntenna()
    {
        for (int j = 0; j < level.GetLength(1); j++)
        {
            for (int i = 0; i < level.GetLength(0); i++)
            {
                if (level[i, j].HaveCreature())
                {
                    level[i, j].creature.UpdateAntenna();
                }
            }
        }
    }

    public void UpdateHight()
    {
        for (int j = 0; j < level.GetLength(1); j++)
        {
            for (int i = 0; i < level.GetLength(0); i++)
            {
                if (level[i, j].HaveCreature())
                {
                    if (!level[i, j].creature.GetDie())
                    {
                        level[i, j].creature.MovingCreature();
                    } else
                    {
                        level[i, j].creature.MovingDie(level[i, j].tile.GetStatus() == "On");
                    }
                }
                level[i, j].tile.MovingTile();
            }
        }
    }
    public void VerifyEscape()
    {
        bool canEscape = true;
        for (int j = 0; j < level.GetLength(1); j++)
        {
            for (int i = 0; i < level.GetLength(0); i++)
            {
                if (level[i, j].tile.GetTypeTile() == "RedEscape")
                {
                    if (!level[i, j].HaveCreature())
                    {
                        canEscape = false;
                    }

                    if (level[i, j].HaveCreature())
                    {
                        if (level[i, j].HaveCreature())
                        {
                            if(level[i, j].creature.GetTypeCreature() != "Red")
                            {
                                canEscape = false;
                            }
                        }
                    }
                } else if (level[i, j].tile.GetTypeTile() == "BlueEscape")
                {
                    if (!level[i, j].HaveCreature())
                    {
                        canEscape = false;
                    }

                    if (level[i, j].HaveCreature())
                    {
                        if (level[i, j].HaveCreature())
                        {
                            if (level[i, j].creature.GetTypeCreature() != "Blue")
                            {
                                canEscape = false;
                            }
                        }
                    }
                } else if (level[i, j].tile.GetTypeTile() == "YellowEscape")
                {
                    if (!level[i, j].HaveCreature())
                    {
                        canEscape = false;
                    }

                    if (level[i, j].HaveCreature())
                    {
                        if (level[i, j].HaveCreature())
                        {
                            if (level[i, j].creature.GetTypeCreature() != "Yellow")
                            {
                                canEscape = false;
                            }
                        }
                    }
                }
            }
        }

        if (canEscape)
        {
            // VICTORY
            UIVictory.SetActive(true);
            lockActionPlayer = true;
        }
    }
}