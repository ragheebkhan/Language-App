using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class CF_WaitingQ : MonoBehaviour
{
    private List<CF_NPC> NPCList;
    private List<Vector3> PosLst;
    private Vector3 entrancePos;

    private CF_NPC NPC;

    public CF_WaitingQ(List<Vector3> posLst)
    {
        this.PosLst = posLst;
        entrancePos = posLst[posLst.Count - 1] ; //entrance position is the last elememnt of the list. Vector3 moves the entrance to the left of the position
        foreach(Vector3 position in posLst)
        {
            World_Sprite.Create(position, new Vector3(.3f,.3f), Color.green);

        }
        World_Sprite.Create(entrancePos, new Vector3(.3f, .3f), Color.magenta);

        NPCList = new List<CF_NPC>();
    }

    public bool canAddNpc()
    {
        return NPCList.Count < PosLst.Count;
    }

    public void AddNPC(CF_NPC NPC) //
    {
        NPCList.Add(NPC); //add our NPC to the list
        //We move the NPC to the entrance position and then we move it to the index of the list where it belongs
        /*NPC.move.InitialmoveDestination(entrancePos, () => {
            NPC.MoveTo(PosLst[NPCList.IndexOf(NPC)]);
        });*/
        NPC.MoveTo(entrancePos);
    }

    /*
    public CF_WaitingQ(CF_NPC NPC, Vector3 entrancePos)
    {
        this.NPC = NPC;
        NPC.MoveTo(entrancePos);
    }*/

    
}
