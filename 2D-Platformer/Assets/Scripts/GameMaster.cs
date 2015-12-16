using UnityEngine;
using System.Collections;

public class GameMaster : MonoBehaviour {

	public static GameMaster gm;

	void Start(){
		if (gm == null) {
			gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
		}
	}

	public Transform playerPrefab;
	public Transform spawnPoint;
	public float spawnDelay = 3.5f;

	public Transform spawnPrefab;

	public IEnumerator RespawnPlayer(){
		GetComponent<AudioSource>().Play ();
		yield return new WaitForSeconds (spawnDelay);
		Instantiate (playerPrefab, spawnPoint.position, spawnPoint.rotation);
		GameObject clone = Instantiate (spawnPrefab, spawnPoint.position, spawnPoint.rotation) as GameObject;
		Destroy (clone, 3f);
	}

	public static void KillPlayer(Player player){
		Destroy (player.gameObject);
		gm.StartCoroutine(gm.RespawnPlayer());
	}

	public static void KillEnemy(Enemy enemy){
		Destroy (enemy.gameObject);

	}
}
