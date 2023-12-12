using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCEndPoint : MonoBehaviour
{
    private NPCRequesterController _occupant;

    public NPCRequesterController Occupant => _occupant;

    public void SetOccupant(NPCRequesterController newOccupant)
    {
        _occupant = newOccupant;
    }
}
