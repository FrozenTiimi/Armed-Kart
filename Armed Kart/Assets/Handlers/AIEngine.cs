using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class AIEngine : MonoBehaviour 
{
	private List<GameObject> AICheckpoints;

	private NavMeshAgent nmAgent;
	private Rigidbody aiRB;

	private Transform targetCheckpoint;
	private int checkPointIndex;
	public float moveSpeed;
	private Transform destination;

	// Use this for initialization
	void Start () 
	{
		nmAgent = GetComponentInChildren<NavMeshAgent> ();
		aiRB = GetComponent<Rigidbody> ();

		this.AICheckpoints = GameObject.FindGameObjectsWithTag ("checkpoint").ToList ();
		targetCheckpoint = this.AICheckpoints.First ().transform;
	}
	
	// Update is called once per frame
	void Update () 
	{
		//this.player = FindClosestPlayer ();

		nmAgent.destination = targetCheckpoint.position;
		transform.LookAt (targetCheckpoint.position);
		aiRB.AddForce ((transform.forward * RealisticVelocity(moveSpeed))*Time.fixedDeltaTime);
		Debug.Log (nmAgent.destination);
	}

	float RealisticVelocity(float speed) 
	{
		return speed * 3f;
	}


	public void SetTargetCheckpoint()
	{
		if (checkPointIndex > AICheckpoints.Count) 
		{
			checkPointIndex = 0;
		} 
		else 
			checkPointIndex++;

		targetCheckpoint = AICheckpoints [checkPointIndex].transform;
	}


	/*Transform FindClosestPlayer() 
	{
		List<Vector3> distances = new List<Vector3>();

		//this.player = GameObject.FindGameObjectsWithTag("Player").First (p => true == true);
		var targerCheckPoint = GameObject.FindGameObjectsWithTag ("checkpoint").Where(p => p.name != this.name).ToList ();
		targerCheckPoint.ToList ().ForEach (p => 
		{
			distances.Add ((transform.position - p.transform.position).normalized);
		});

		Transform tempPlayer = transform;

		for (var i = 0; i < distances.Count; i++)
		{
			if (i < distances.Count - 1)
			{
				var dist1 = distances[i];
				var dist2 = distances[i + 1];
				
				if (dist1.magnitude < dist2.magnitude)
					tempPlayer = targerCheckPoint[i].transform;
				else 
					tempPlayer = targerCheckPoint[i + 1].transform;
			}
			else if (i > 0)
			{
				var dist1 = distances[i];
				var dist2 = distances[i - 1];
				
				if (dist1.magnitude < dist2.magnitude)
					tempPlayer = targerCheckPoint[i].transform;
				else 
					tempPlayer = targerCheckPoint[i - 1].transform;
			}
			else
				tempPlayer = targerCheckPoint[i].transform;
		}

		return tempPlayer;
	}*/
}
