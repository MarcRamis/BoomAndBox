using UnityEngine;

public class OrbitState : IAIState
{
    AiBehaviour IAIState.GetId()
    {
        return AiBehaviour.ORBIT;
    }
    
    void IAIState.Enter(Agent agent)
    {
        Debug.Log("Enter - Orbit state");
    }

    void IAIState.Exit(Agent agent)
    {
        Debug.Log("Exit - Orbit state");
    }


    void IAIState.Update(Agent agent)
    {
        Debug.Log("Update - Orbit state");
    }
}