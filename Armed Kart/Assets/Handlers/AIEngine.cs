using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class AIEngine : MonoBehaviour 
{
	public Transform[] checkPoints;

	private NavMeshAgent nmAgent;
	private Rigidbody aiRB;

	private Transform targetCheckpoint;
	private int checkPointIndex;
	public float moveSpeed;

	// Use this for initialization
	void Start () 
	{
		nmAgent = GetComponent<NavMeshAgent> ();
		aiRB = GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		//this.player = FindClosestPlayer ();

		nmAgent.destination = targetCheckpoint.position;
		aiRB.AddForce ((transform.forward * RealisticVelocity(moveSpeed))*Time.fixedDeltaTime);
	}

	float RealisticVelocity(float speed) 
	{
		return speed * 3f;
	}

	void SetTargetCheckpoint()
	{
		if (checkPointIndex > checkPoints.Length) 
		{
			checkPointIndex = 0;
		} 
		else 
			checkPointIndex++;

		targetCheckpoint = checkPoints [checkPointIndex];
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
