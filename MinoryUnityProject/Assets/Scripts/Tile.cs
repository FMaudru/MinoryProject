using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Tile {
    private int x;
    private int z;
    private string type;
    private string status;
    public GameObject tileObject;

    public Tile(int x, int z, string type, GameObject prefabTile)
    {
        this.x = x;
        this.z = z;
        this.type = type;
        this.status = "Off";

        tileObject = GameObject.Instantiate(prefabTile, new Vector3(x, prefabTile.transform.position.y, z), Quaternion.identity);
    }

    public int GetX()
    {
        return this.x;
    }

    public int GetZ()
    {
        return this.z;
    }

    public string GetTypeTile()
    {
        return this.type;
    }

    public string GetStatus()
    {
        return this.status;
    }

    public void SetX(int x)
    {
       this.x = x;
    }

    public void SetZ(int z)
    {
        this.z = z;
    }

    public void SetTypeTile(string type)
    {
        this.type = type;
    }

    public void SetStatus(string status)
    {
        this.status = status;
    }

    public void MovingTile()
    {
        if (status == "On")
        {
            //tileObject.transform.position = new Vector3(tileObject.transform.position.x, 0f, tileObject.transform.position.z);
            tileObject.transform.DOMove(new Vector3(tileObject.transform.position.x, 0f, tileObject.transform.position.z),0.5f);
        } else
        {
            //tileObject.transform.position = new Vector3(tileObject.transform.position.x, -0.5f, tileObject.transform.position.z);
            tileObject.transform.DOMove(new Vector3(tileObject.transform.position.x, -0.5f, tileObject.transform.position.z),0.5f);
        }
    }
}
