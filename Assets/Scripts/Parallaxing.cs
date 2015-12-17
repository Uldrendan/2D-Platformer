using UnityEngine;
using System.Collections;

public class Parallaxing : MonoBehaviour {

	public Transform[] backgrounds; //list of back and foregrounds to be paralllaxed
	private float[] parallaxScales; //porportion of camera's movement to move the backgrounds by
	public float smoothing = 1; //how smooth parallax will be; set to above 0

	private Transform cam; //reference to main camera's transform
	private Vector3 previousCamPosition; //store position of camera in the previous frame

	//called before start, all logic before start but after gameobjects, great for references
	void Awake() {
		//set up camera reference
		cam = Camera.main.transform;
	}

	// Use this for initialization
	void Start () {
		previousCamPosition = cam.position;

		parallaxScales = new float[backgrounds.Length];

		for (int i = 0; i < backgrounds.Length; i++) {
			parallaxScales[i] = backgrounds[i].position.z*-1;
		}

	}
	
	// Update is called once per frame
	void Update () {
		for (int i = 0; i < backgrounds.Length; i++) {
			float parallax = (previousCamPosition.x - cam.position.x) * parallaxScales[i];

			float backgroundTargetPosX = backgrounds[i].position.x + parallax;

			Vector3 backgroundTargetPos = new Vector3 (backgroundTargetPosX, backgrounds[i].position.y, backgrounds[i].position.z);

			backgrounds[i].position = Vector3.Lerp(backgrounds[i].position, backgroundTargetPos, smoothing * Time.deltaTime);
		}

		previousCamPosition = cam.position;
	}
}
