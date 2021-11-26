using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell {
    private int x;
    private int z;
    public Creature creature = null;
    public Tile tile = null;
    public Cell(int x, int z, Tile tile)
    {
        this.x = x;
        this.z = z;
        this.creature = null;
        this.tile = tile;
    }

    public bool HaveCreature()
    {
        return creature == null ? false : true;
    }

    public void setCreature(Creature creature)
    {
        this.creature = creature;
    }
}
