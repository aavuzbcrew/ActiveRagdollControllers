using UnityEngine;
using MLAgents;
using System.Collections.Generic;
using System.Linq;
using System;

public class Controller004InputAgent : Agent {
    public float AxisX;
    public bool Jump;
    float lastLowerStepReward;

    // List<float> actionEntropies = new List<float>();
    List<float> lowerRewards = new List<float>();
    List<float> myRewards = new List<float>();

    public override void AgentReset()
    {
        AxisX = 0f;
        // actionEntropies = new List<float>();
        lowerRewards = new List<float>();
        myRewards = new List<float>();
    }
    public override void CollectObservations()
    {
        AddVectorObs(AxisX);
        AddVectorObs(lastLowerStepReward);
    }
    public override void AgentAction(float[] vectorAction, string textAction)
    {
        int action = (int)vectorAction[0];
        switch (action)
        {
            case 0:
                AxisX = 0f;
                Jump = false;
                break;
            case 1:
                AxisX = 1f;
                Jump = false;
                break;
            case 2:
                AxisX = -1f;
                Jump = false;
                break;
            case 3:
                AxisX = 0f;
                Jump = true;
                break;
            case 4:
                AxisX = 1f;
                Jump = true;
                break;
            case 5:
                AxisX = -1f;
                Jump = true;
                break;
            default:
                throw new NotImplementedException();
        }
    }

    public void LowerStepReward(float stepReward)
    {
        lastLowerStepReward = stepReward;
        lowerRewards.Add(stepReward);
        var diff = Mathf.Abs(AxisX - stepReward); //0 to 2
        // float reward = (2f-diff) *.5f;
        AddReward(diff);
        myRewards.Add(diff);

    }
    public void LowerEpisodeEnd(Agent lowerAgent)
    {
        if (myRewards.Count == 0)
            myRewards.Add(0f);
        var maxStep = lowerAgent.agentParameters.maxStep / lowerAgent.agentParameters.numberOfActionsBetweenDecisions; 
        var earlyPenalty = maxStep - (lowerAgent.GetStepCount()/lowerAgent.agentParameters.numberOfActionsBetweenDecisions);
        earlyPenalty = Mathf.Clamp(earlyPenalty,0,maxStep-1);
        // var aveScore = myRewards.Average() * 1f;
        AddReward(earlyPenalty);
        // if (actionEntropies.Count == 0)
        //     actionEntropies.Add(0f);
        // if (lowerRewards.Count == 0)
        //     lowerRewards.Add(0f);
        // var entropy = actionEntropies.Average(); // range 0-2
        // var aveReward = lowerRewards.Average(); // range 0-1
        // entropy = Mathf.Clamp(entropy, 0, 1f);
        // var delta = 1 - Mathf.Abs(entropy - aveReward);
        // var reward = entropy * 500;
        // //reward += aveReward * 250;
        // reward += delta *500;

        // SetReward(reward);
        Done();
    }
}