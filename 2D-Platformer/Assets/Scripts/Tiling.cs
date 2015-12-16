using UnityEngine;
using System.Collections;

[RequireComponent (typeof(SpriteRenderer))]

public class Tiling : MonoBehaviour {
	
	public int offsetX = 2;			// the offset so that we don't get any weird errors
	
	// these are used for checking if we need to instantiate stuff
	public bool hasARightTile = false;
	public bool hasALeftTile = false;
	
	public bool reverseScale = false;	// used if the object is not tilable
	
	private float spriteWidth = 0f;		// the width of our element
	private Camera cam;
	private Transform myTransform;
	
	void Awake () {
		cam = Camera.main;
		myTransform = transform;
	}
	
	// Use this for initialization
	void Start () {
		SpriteRenderer sRenderer = GetComponent<SpriteRenderer>();
		spriteWidth = sRenderer.sprite.bounds.size.x;
	}
	
	// Update is called once per frame
	void Update () {
		// does it still need buddies? If not do nothing
		if (hasALeftTile == false || hasARightTile == false) {
			// calculate the cameras extend (half the width) of what the camera can see in world coordinates
			float camHorizontalExtend = cam.orthographicSize * Screen.width/Screen.height;
			
			// calculate the x position where the camera can see the edge of the sprite (element)
			float edgeVisiblePositionRight = (myTransform.position.x + spriteWidth/2) - camHorizontalExtend;
			float edgeVisiblePositionLeft = (myTransform.position.x - spriteWidth/2) + camHorizontalExtend;
			
			// checking if we can see the edge of the element and then calling MakeNewTile if we can
			if (cam.transform.position.x >= edgeVisiblePositionRight - offsetX && hasARightTile == false)
			{
				MakeNewTile (1);
				hasARightTile = true;
			}
			else if (cam.transform.position.x <= edgeVisiblePositionLeft + offsetX && hasALeftTile == false)
			{
				MakeNewTile (-1);
				hasALeftTile = true;
			}
		}
	}
	
	// a function that creates a buddy on the side required
	void MakeNewTile (int direction) {
		// calculating the new position for our new buddy
		Vector3 newPosition = new Vector3 (myTransform.position.x + spriteWidth * direction, myTransform.position.y, myTransform.position.z);
		// instantating our new body and storing him in a variable
		Transform newTile = Instantiate (myTransform, newPosition, myTransform.rotation) as Transform;
		
		// if not tilable let's reverse the x size og our object to get rid of ugly seams
		if (reverseScale == true) {
			newTile.localScale = new Vector3 (newTile.localScale.x*-1, newTile.localScale.y, newTile.localScale.z);
		}
		
		newTile.parent = myTransform.parent;
		if (direction > 0) {
			newTile.GetComponent<Tiling>().hasALeftTile = true;
		}
		else {
			newTile.GetComponent<Tiling>().hasARightTile = true;
		}
	}
}
