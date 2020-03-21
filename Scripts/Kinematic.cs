using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum steeringBehaviors
{
    Seek, Flee, Arrive, Align, Face, LookWhereGoing, PathFollow, Pursue, Separate, CollisionAvoidance, ObstacleAvoidance, Flocker, PathFind, None
}

public class Kinematic : MonoBehaviour
{

    public Vector3 linear;
    public float angular; //In degrees
    public GameObject newTarget;
    public float maxSpeed = 10.0f;
    public float maxAngularVelocity = 45.0f; // degrees


    public steeringBehaviors choiceOfBehavior;
    public GameObject[] path;
    public Kinematic[] targets;
    PathFollow follow = new PathFollow();
    LookWhereGoing lookwg = new LookWhereGoing();
	SteeringOutput steeringUpdate = new SteeringOutput();
	public GameObject[] myPath;
	public Node start;
    public Node goal;
	BlendedSteering mySteering;
	public GameObject myCohereTarget;
    PrioritySteering myAdvancedSteering = new PrioritySteering();
	Kinematic[] kBirds;
	public bool avoidObstacles = false;
	Graph newGraph = new Graph();
	//public GameObject myCohereTarget;

    // Update is called once per frame
    void Update()
    {
        switch (choiceOfBehavior)
        {
            case steeringBehaviors.None:
                ResetOrientation();
                break;
            default:
                MainSteeringBehaviors();
                break;
        }
    }
	private void Start()
    {
        switch (choiceOfBehavior)
        {
            case steeringBehaviors.PathFind:
                
                //lookwg = new LookWhereGoing();
                lookwg.character = this;
                lookwg.target = newTarget;
				newGraph = new enemy();
                newGraph.Build();
                List<Connection> path = Dijkstra.pathfind(newGraph, start, goal);
                // path is a list of connections - convert this to gameobjects for the FollowPath steering behavior
                myPath = new GameObject[path.Count + 1];
                int k = 0;
                foreach (Connection c in path)
                {
                    Debug.Log("from " + c.getFromNode() + " to " + c.getToNode() + " @" + c.getCost());
                    myPath[k] = c.getFromNode().gameObject;
                    k++;
                }
                myPath[k] = goal.gameObject;
                //follow = new PathFollow();
                follow.character = this;
                follow.path = myPath;
                break;
        }
    }


    void MainSteeringBehaviors()
    {
        ResetOrientation();

        switch (choiceOfBehavior)
        {
			case steeringBehaviors.PathFind:
                SteeringOutput lwyger = lookwg.getSteering();
                linear += follow.getSteering().linear * Time.deltaTime;
                angular += follow.getSteering().angular * Time.deltaTime;
                break;
            case steeringBehaviors.Seek:
                Seek seek = new Seek();
                seek.character = this;
                seek.target = newTarget;
                SteeringOutput seeking = seek.getSteering();
                if (seeking != null)
                {
                    linear += seeking.linear * Time.deltaTime;
                    angular += seeking.angular * Time.deltaTime;
                }
                break;
            case steeringBehaviors.Flee:
                Flee flee = new Flee();
                flee.character = this;
                flee.target = newTarget;
                SteeringOutput fleeing = flee.getSteering();
                if (fleeing != null)
                {
                    linear += fleeing.linear * Time.deltaTime;
                    angular += fleeing.angular * Time.deltaTime;
                }
                break;

            case steeringBehaviors.Align:
                Align align = new Align();
                align.character = this;
                align.target = newTarget;
                SteeringOutput aligning = align.getSteering();
                if (aligning != null)
                {
                    linear += aligning.linear * Time.deltaTime;
                    angular += aligning.angular * Time.deltaTime;
                }
                break;
            case steeringBehaviors.Face:
                Face face = new Face();
                face.character = this;
                face.target = newTarget;
                SteeringOutput facing = face.getSteering();
                if (facing != null)
                {
                    linear += facing.linear * Time.deltaTime;
                    angular += facing.angular * Time.deltaTime;
                }
                break;
            case steeringBehaviors.LookWhereGoing:
                LookWhereGoing look = new LookWhereGoing();
                look.character = this;
                look.target = newTarget;
                SteeringOutput looking = look.getSteering();
                if (looking != null)
                {
                    linear += looking.linear * Time.deltaTime;
                    angular += looking.angular * Time.deltaTime;
                }
                break;
            case steeringBehaviors.Arrive:
                Arrive arrive = new Arrive();
                arrive.character = this;
                arrive.target = newTarget;
                SteeringOutput arriving = arrive.getSteering();
                if (arriving != null)
                {
                    linear += arriving.linear * Time.deltaTime;
                    angular += arriving.angular * Time.deltaTime;
                }
                break;
            case steeringBehaviors.PathFollow:


                follow.character = this;
                lookwg.character = this;

                follow.path = path;
                lookwg.target = newTarget;

                SteeringOutput following = follow.getSteering();
                SteeringOutput lookingwg = lookwg.getSteering();

                if (following != null)
                {
                    linear += following.linear * Time.deltaTime;
                    // angular += lookingwg.angular* Time.deltaTime;
                }
                break;
            case steeringBehaviors.Pursue:
                Pursue pursue = new Pursue();
                LookWhereGoing PursuelookWhereGoing = new LookWhereGoing();
                pursue.character = this;
                PursuelookWhereGoing.character = this;
                pursue.target = newTarget;
                PursuelookWhereGoing.target = newTarget;
                SteeringOutput pursuing = pursue.getSteering();
                SteeringOutput pursuelookingWhereGoing = PursuelookWhereGoing.getSteering();

                if (pursuing != null)
                {
                    linear += pursuing.linear * Time.deltaTime;
                    angular += pursuing.angular * Time.deltaTime;
                }
                break;
            case steeringBehaviors.Separate:
                Separate separate = new Separate();
                LookWhereGoing lookWhereGoing = new LookWhereGoing();
                separate.character = this;
                lookWhereGoing.character = this;
                separate.targets = targets;
                lookWhereGoing.target = newTarget;
                SteeringOutput lookingWhereGoing = lookWhereGoing.getSteering();
                SteeringOutput separating = separate.getSteering();
                if (separating != null)
                {
                    linear += separating.linear * Time.deltaTime;
                    angular += separating.angular * Time.deltaTime;
                }
                break;
            case steeringBehaviors.CollisionAvoidance:
                CollisionAvoidance avoid = new CollisionAvoidance();
                LookWhereGoing AvoidlookWhereGoing = new LookWhereGoing();
                avoid.character = this;
                AvoidlookWhereGoing.character = this;
                avoid.targets = targets;
                AvoidlookWhereGoing.target = newTarget;
                SteeringOutput AvoidlookingWhereGoing = AvoidlookWhereGoing.getSteering();
                SteeringOutput avoiding = avoid.getSteering();
                if (avoiding != null)
                {
                    linear += avoiding.linear * Time.deltaTime;
                    angular += avoiding.angular * Time.deltaTime;
                }
                break;
            case steeringBehaviors.ObstacleAvoidance:
                ObstacleAvoidance obAvoid = new ObstacleAvoidance();
                LookWhereGoing obAvoidlookWhereGoing = new LookWhereGoing();
                obAvoid.character = this;
                obAvoidlookWhereGoing.character = this;
                obAvoid.target = newTarget;
                obAvoidlookWhereGoing.target = newTarget;
                SteeringOutput obAvoiding = obAvoid.getSteering();
                SteeringOutput obAvoidlookingWhereGoing = obAvoidlookWhereGoing.getSteering();
                if (obAvoiding != null)
                {
                    linear += obAvoiding.linear * Time.deltaTime;
                    angular += obAvoiding.angular * Time.deltaTime;
                }
                break;
			case steeringBehaviors.Flocker:
				//Separate from other birds
				Separate flockSeparate = new Separate();
				  Arrive arriveFlock = new Arrive();
                LookWhereGoing FlockLookWhereGoing = new LookWhereGoing();
                BlendedSteering mySteering = new BlendedSteering();
				flockSeparate.character = this;
				GameObject[] goBirds = GameObject.FindGameObjectsWithTag("bird");
				 kBirds = new Kinematic[goBirds.Length - 1];
                int j = 0;
                for (int i = 0; i < goBirds.Length - 1; i++)
                {
                    if (goBirds[i] == this)
                    {
                        continue;
                    }
                    kBirds[j++] = goBirds[i].GetComponent<Kinematic>();
                }
                flockSeparate.targets = kBirds;
                
                arriveFlock.character = this;
                //Debug.Log(arriveFlock.character);
                arriveFlock.target = newTarget;
                //Debug.Log(arriveFlock.target);
                FlockLookWhereGoing.character = this;
                FlockLookWhereGoing.target = newTarget;
                mySteering.behaviors = new BehaviorAndWeight[3];
                mySteering.behaviors[0] = new BehaviorAndWeight();
                mySteering.behaviors[0].behavior = flockSeparate;
                mySteering.behaviors[0].weight = 1f; 
                mySteering.behaviors[1] = new BehaviorAndWeight();
                mySteering.behaviors[1].behavior = arriveFlock;
                mySteering.behaviors[1].weight = 1f; 
                mySteering.behaviors[2] = new BehaviorAndWeight();
                mySteering.behaviors[2].behavior = FlockLookWhereGoing;
                mySteering.behaviors[2].weight = 1f;

                ObstacleAvoidance myAvoid = new ObstacleAvoidance();
                myAvoid.character = this;
                myAvoid.target = newTarget;
                myAvoid.flee = true; 

                BlendedSteering myHighPrioritySteering = new BlendedSteering();
                myHighPrioritySteering.behaviors = new BehaviorAndWeight[1];
                myHighPrioritySteering.behaviors[0] = new BehaviorAndWeight();
                myHighPrioritySteering.behaviors[0].behavior = myAvoid;
                myHighPrioritySteering.behaviors[0].weight = 1f;
                myAdvancedSteering.groups = new BlendedSteering[2];
                myAdvancedSteering.groups[0] = new BlendedSteering();
                myAdvancedSteering.groups[0] = myHighPrioritySteering;
                myAdvancedSteering.groups[1] = new BlendedSteering();
                myAdvancedSteering.groups[1] = mySteering;

                //steeringUpdate = mySteering.getSteering();
                avoidObstacles = true;
                if (!avoidObstacles)
                {
                    steeringUpdate = mySteering.getSteering();
                    if (steeringUpdate != null)
                    {
                        linear += steeringUpdate.linear * Time.deltaTime;
                        angular += steeringUpdate.angular * Time.deltaTime;
                    }
                }
                else
                {
                    steeringUpdate = myAdvancedSteering.getSteering();
                    if (steeringUpdate != null)
                    {
                        linear += steeringUpdate.linear * Time.deltaTime;
                        angular += steeringUpdate.angular * Time.deltaTime;
                    }
                }
                break;
                

        }

    }


    void ResetOrientation()
    {
        //Update position and rotation
        transform.position += linear * Time.deltaTime;
        Vector3 angularIncrement = new Vector3(0, angular * Time.deltaTime, 0);
        transform.eulerAngles += angularIncrement;
    }

}