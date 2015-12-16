using UnityEngine;
using Pathfinding;
using System.Collections;

[RequireComponent (typeof (Seeker))]
[RequireComponent (typeof (Rigidbody2D))]
public class EnemyAI: MonoBehaviour {
	
	//what to chase
	public Transform target;
	
	//how much per second we will update our path
	public float updateRate = 2f;
	
	//caching
	private Seeker seeker;
	private Rigidbody2D rb;
	
	//calculated path
	public Path path;
	
	//ai speed per second
	public float speed = 300;
	
	public ForceMode2D fmode;
	
	[HideInInspector]
	public bool pathIsEnded = false;
	
	//the max distance from the ai to a waypoint for it to continue to the next waypoint
	public float nextWaypointDistance = 3;
	
	private int currentWaypoint = 0;

	private bool searchingForPlayer = false;
	
	void Start (){
		seeker = GetComponent<Seeker> ();
		rb = GetComponent<Rigidbody2D> ();
		
		if (target == null) {
			if(!searchingForPlayer){
				searchingForPlayer = true;
				StartCoroutine(SearchForPlayer());
			}
			return;
		}
		
		//start a new path to the target position return to the result of the on path complete method
		seeker.StartPath (transform.position, target.position, OnPathComplete);
		
		StartCoroutine (UpdatePath ());
	}

	IEnumerator SearchForPlayer(){
		GameObject sResult = GameObject.FindGameObjectWithTag("Player");
		if(sResult == null){
			yield return new WaitForSeconds(0.5f);
			StartCoroutine(SearchForPlayer());
		}
		else{
			searchingForPlayer = false;
			target = sResult.transform;
			StartCoroutine(UpdatePath());
			return false;
		}
	}
	
	IEnumerator UpdatePath(){
		if (target == null) {
			if(!searchingForPlayer){
				searchingForPlayer = true;
				StartCoroutine(SearchForPlayer());
			}
			return false;
		}
		seeker.StartPath (transform.position, target.position, OnPathComplete);
		
		yield return new WaitForSeconds (1 / updateRate);
		StartCoroutine (UpdatePath ());
	}
	
	public void OnPathComplete(Path p){
		if (!p.error) {
			path = p;
			currentWaypoint = 0;
		}
	}
	
	void FixedUpdate(){
		if (target == null) {
			if(!searchingForPlayer){
				searchingForPlayer = true;
				StartCoroutine(SearchForPlayer());
			}
			return;
		}
		
		//TODO:always look at player
		if(path == null){
			return;
		}
		
		if(currentWaypoint >= path.vectorPath.Count){
			if(pathIsEnded){
				return;
			}
			pathIsEnded = true;
			return;
		}
		pathIsEnded = false;
		
		//Direction to next waypoint
		Vector3 dir = (path.vectorPath[currentWaypoint] - transform.position).normalized;
		
		dir *= speed * Time.fixedDeltaTime;
		
		//Move the enemy
		rb.AddForce(dir, fmode);
		
		float dist = Vector3.Distance (transform.position, path.vectorPath[currentWaypoint]);
		if(dist < nextWaypointDistance){
			currentWaypoint++;
			return;
		}
	}
}