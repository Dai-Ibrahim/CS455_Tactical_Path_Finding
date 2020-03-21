using UnityEngine;

public class enemy : Graph
{
    float enemyWeight = 12f;
    public override void GetCost(Node[] nodes)
    {
        foreach (Node fromNode in nodes)
        {
            foreach (Node toNode in fromNode.ConnectsTo)
            {
                float cost = (toNode.transform.position - fromNode.transform.position).magnitude;  
				  if (toNode.tag == "enemy")
				  {
					  Debug.Log("It is an enemy");
					  cost *= enemyWeight;
				  }
                Connection c = new Connection(cost, fromNode, toNode);
                mConnections.Add(c);
            }
        }
    }
}