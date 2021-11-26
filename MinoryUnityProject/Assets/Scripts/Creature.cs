using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Creature {
    private int x;
    private int z;
    private string looking;
    private string type;
    private string status;
    private bool die = false;
    public GameObject creatureObject;
    public GameObject antenna;

    public Creature(int x, int z, string looking, string type, GameObject prefabCreature)
    {
        this.x = x;
        this.z = z;
        this.looking = looking;
        this.type = type;
        status = "off";

        creatureObject = GameObject.Instantiate(prefabCreature, new Vector3(x, prefabCreature.transform.position.y, z), Quaternion.identity);
        antenna = creatureObject.transform.GetChild(1).GetChild(2).GetChild(1).GetChild(0).GetChild(1).gameObject;

        float rotationDegree;
        switch (looking)
        {
            case "Up":
                rotationDegree = 0;
                break;
            case "Down":
                rotationDegree = 180;
                break;
            case "Left":
                rotationDegree = -90;
                break;
            default:
                rotationDegree = 90;
                break;
        }
        creatureObject.transform.Rotate(0.0f, rotationDegree, 0.0f, Space.Self);
    }

    public int GetX()
    {
        return this.x;
    }

    public int GetZ()
    {
        return this.z;
    }

    public string GetTypeCreature()
    {
        return this.type;
    }

    public string GetStatus()
    {
        return this.status;
    }
    public bool GetDie()
    {
        return this.die;
    }
    public string GetLooking()
    {
        return this.looking;
    }

    public void SetX(int x)
    {
        this.x = x;
    }

    public void SetZ(int z)
    {
        this.z = z;
    }

    public void SetTypeCreature(string type)
    {
        this.type = type;
    }

    public void SetStatus(string status)
    {
        this.status = status;
    }

    public void SetDie(bool isDie)
    {
        this.die = isDie;
    }

    public void SetLooking(string looking)
    {
        this.looking = looking;
    }

    public bool CanControl()
    {
        return status == "On" ? true: false;
    }

    public bool isOff()
    {
        return status != "Player" ? status != "On" ? true : false : false;
    }

    public void UpdateAntenna()
    {
        if (status == "Player")
        {
            antenna.GetComponent<Renderer>().material.color = new Color(255,0,0);
        } else if (status == "On")
        {
            antenna.GetComponent<Renderer>().material.color = new Color(255,145,0);
        } else
        {
            antenna.GetComponent<Renderer>().material.color = new Color(0,255,255);
        }
    }

    public void MovingCreature()
    {
        if (status == "On" || status == "Player")
        {
            //creatureObject.transform.position = new Vector3(creatureObject.transform.position.x, 1f, creatureObject.transform.position.z);
            creatureObject.transform.DOMove(new Vector3(creatureObject.transform.position.x, 1f, creatureObject.transform.position.z), 0.5f);
        }
        else
        {
            //creatureObject.transform.position = new Vector3(creatureObject.transform.position.x, 0.5f, creatureObject.transform.position.z);
            creatureObject.transform.DOMove(new Vector3(creatureObject.transform.position.x, 0.5f, creatureObject.transform.position.z), 0.5f);
        }
    }

    public void MovingDie(bool onTile)
    {
        if (onTile)
        {
            //creatureObject.transform.position = new Vector3(creatureObject.transform.position.x, 1f, creatureObject.transform.position.z);
            creatureObject.transform.DOMove(new Vector3(creatureObject.transform.position.x, 1f, creatureObject.transform.position.z),0.5f);
        }
        else
        {
            //creatureObject.transform.position = new Vector3(creatureObject.transform.position.x, 0.5f, creatureObject.transform.position.z);
            creatureObject.transform.DOMove(new Vector3(creatureObject.transform.position.x, 0.5f, creatureObject.transform.position.z), 0.5f);
        }
    }

    public float RotateLooking(string looking)
    {
        int valueAngular = 0;
        float timeRotate = 0.5f;
        switch (GetLooking())
        {
            case "Up":
                switch (looking)
                {
                    case "Left":
                        valueAngular = -90;
                        break;
                    case "Right":
                        valueAngular = 90;
                        break;
                    case "Down":
                        valueAngular = 180;
                        timeRotate = 1;
                        break;
                    case "Up":
                        timeRotate = 0;
                        break;
                }
                break;
            case "Left":
                switch (looking)
                {
                    case "Left":
                        timeRotate = 0;
                        break;
                    case "Right":
                        valueAngular = 90;
                        timeRotate = 1;
                        break;
                    case "Down":
                        valueAngular = -180;
                        break;
                    case "Up":
                        valueAngular = 0;
                        break;
                }
                break;
            case "Right":
                switch (looking)
                {
                    case "Left":
                        valueAngular = -90;
                        timeRotate = 1;
                        break;
                    case "Right":
                        timeRotate = 0;
                        break;
                    case "Down":
                        valueAngular = 180;
                        break;
                    case "Up":
                        valueAngular = 0;
                        break;
                }
                break;
            case "Down":
                switch (looking)
                {
                    case "Left":
                        valueAngular = -90;
                        break;
                    case "Right":
                        valueAngular = 90;
                        break;
                    case "Down":
                        timeRotate = 0;
                        break;
                    case "Up":
                        valueAngular = 0;
                        timeRotate = 1;
                        break;
                }
                break;
        }
        SetLooking(looking);
        if (timeRotate > 0)
        {
            creatureObject.transform.DORotate(new Vector3(creatureObject.transform.rotation.x, valueAngular, creatureObject.transform.rotation.z), timeRotate);
        }
        return timeRotate;
    }

    public void Move(int x, int z, string looking)
    {
        creatureObject.transform.DOMove(new Vector3(x, creatureObject.transform.position.y, z), 1).OnComplete(() => RotateLooking(looking));
    }

    public void MoveToDie(int x, int z)
    {
        SetDie(true);
        SetStatus("Off");
        creatureObject.transform.DOMove(new Vector3(x, creatureObject.transform.position.y, z), 1).OnComplete(() => Die());
    }

    public void Die()
    {
        Animator anim = creatureObject.GetComponent<Animator>();
        anim.SetTrigger("Die");
        foreach (Collider c in creatureObject.GetComponents<Collider>())
        {
            c.enabled = false;
        }
        Collider antennaCollider = creatureObject.transform.GetChild(1).GetChild(2).GetChild(1).GetChild(0).GetChild(1).gameObject.GetComponent<Collider>();
        antennaCollider.enabled = false;
    }
}
