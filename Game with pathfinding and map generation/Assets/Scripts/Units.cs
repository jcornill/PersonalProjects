using UnityEngine;
using System.Collections;

public class Units : MonoBehaviour {

	public float speed;
	Vector3[] path;
	int targetIndex;
	bool followingPath;
	public bool foundPath { get; set; }
	public float test { get; set; }

	PathFinding temp;

	void Start()
	{
		test = -1;
		foundPath = false;
		followingPath = false;
		temp = GameObject.Find("Maps controller").GetComponent<PathFinding>();
	}

	void Update()
	{
		
	}

	public bool MoveToPosition(Vector3 worldPosition)
	{
		if (path == null && !followingPath)
		{
			PathRequestManager.RequestPath(transform.position, worldPosition, OnPathFound);
			followingPath = true;
		}
		else if (followingPath && path == null)
		{
			followingPath = false;
			return true;
		}
		return false;
	}
	
	public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
	{
		foundPath = pathSuccessful;
		test = Time.time;
		if (pathSuccessful && path == null && newPath.Length > 0)
		{
			path = newPath;
			StopCoroutine("FollowPath");
			StartCoroutine("FollowPath");
		}
	}

	IEnumerator FollowPath()
	{
		Vector3 currentWaypoint = path[0];

		while (true)
		{
			if (transform.position == currentWaypoint)
			{
				targetIndex++;
				if (targetIndex >= path.Length)
				{
					path = null;
					targetIndex = 0;
					yield break;
				}
				currentWaypoint = path[targetIndex];
			}
			transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
			yield return null;
		}
	}

	public void OnDrawGizmos()
	{
		if (temp.showPass)
		{
			if (path != null)
			{
				for (int i = targetIndex; i < path.Length; i++)
				{
					Gizmos.color = Color.black;
					Gizmos.DrawCube(path[i], Vector3.one);
					if (i == targetIndex)
					{
						Gizmos.DrawLine(transform.position, path[i]);
					}
					else
					{
						Gizmos.DrawLine(path[i - 1], path[i]);
					}
				}
			}
		}
	}
}
