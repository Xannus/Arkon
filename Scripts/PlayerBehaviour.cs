using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
	private float playerVelocity =0.1f;
	public float boundary;
	public GameObject extraBall;
	private Vector3 playerPosition;
	private bool stop;
	private int ammo;
	private string weapon;
	public GameObject blaster;
	public GameObject rocket;
	private bool ballInPlay;

	// Initialization
	void Start ()
	{
		ballInPlay = false;
		playerPosition = gameObject.transform.position;
		InvokeRepeating ("updateSpeed", 0.01f, 0.01f);

	}
	
	// Update is called once per frame
	void Update ()
	{
		if (ballInPlay) {
			checkFire ();
		}
		//exits app when esc. is pressed
		if (Input.GetKeyDown (KeyCode.Escape)) {
			Application.Quit ();
		}
		//These if statements compare the xpos to the value of boundary
		//and prevent the vaus obj from moving beyond the boundary point

		if (playerPosition.x < -boundary) {//for left hand side boundary
			playerPosition = new Vector3 (-boundary, playerPosition.y, playerPosition.z);
		}
		if (playerPosition.x > boundary) {// for right hand side boundary 
			playerPosition = new Vector3 (boundary, playerPosition.y, playerPosition.z);
		
		}
		//Then we move the object to the position calculated above
		transform.position = playerPosition;

	}

	public void setGameOver(bool b){
		stop =b;
	}

	private void updateSpeed(){
		//eachstep we set the playerpos vec3 to the horAxis+
		if (!stop) {
			playerPosition.x += Input.GetAxis ("Horizontal") * playerVelocity;
		}
	}
	void checkFire(){
		
		if (Input.GetButtonDown ("Jump")) {
			if(weapon=="blaster"){
			if(ammo>0){
					GameObject projectile =Instantiate (blaster,transform,true);
					projectile.transform.position = new Vector3 (this.gameObject.transform.position.x,this.gameObject.transform.position.y+0.25f,0);
					ammo--;
					GameObject o = GameObject.FindGameObjectsWithTag ("GameController") [0];
					o.SendMessage("addAmmo",-1);

				}
			}
			if(weapon=="rocket"){
				if(ammo>3){
					StartCoroutine (missleVolley ());
					ammo-=4;
				}
		}
	}
	}
	public void setWeapon(string w){
		weapon = w;
	}
	void OnCollisionEnter2D(Collision2D col){
		
		if (col.collider.tag == "Blaster") {
			//Updtates the graphics on the screen and gives the player ammo to shoot bullets
			if (weapon == "extended") {
				StartCoroutine (unextend ());
			} else {
				GetComponent<Animator> ().Play ("PowerUpGreen");
			}
			ammo += 10;
			if (ammo > 32) {ammo = 32;}

			GameObject o = GameObject.FindGameObjectsWithTag ("GameController") [0];
			o.SendMessage("addAmmo",10);

			weapon = "blaster";
		}
		if (col.collider.tag == "Rocket") {
			//Updtates the graphics on the screen and allows the player to shoot rockets
			if (weapon == "extended") {
				StartCoroutine (unextend ());
			} else {
				GetComponent<Animator> ().Play ("Blue");
			}
			ammo += 12;
			if (ammo > 32) {ammo = 32;}

			GameObject o = GameObject.FindGameObjectsWithTag ("GameController") [0];
			o.SendMessage("addAmmo",12);

			weapon = "rocket";
		}
		if (col.collider.tag == "Extender") {
			if (weapon == "extended")boundary+= 0.25f;
			//Extends the paddle 
			StartCoroutine (extend());
			weapon = "extended";
		}
		if (col.collider.tag == "Balls") {
			GameObject b1 = Instantiate (extraBall, GameObject.FindGameObjectsWithTag ("Ball") [0].transform.position, GameObject.FindGameObjectsWithTag ("Ball") [0].transform.rotation);
			GameObject b2 = Instantiate (extraBall, GameObject.FindGameObjectsWithTag ("Ball") [1].transform.position, GameObject.FindGameObjectsWithTag ("Ball") [0].transform.rotation);

		}
		if (col.collider.tag == "OneUp") {
			GameObject o = GameObject.FindGameObjectsWithTag ("GameController") [0];
			o.SendMessage ("addLives",1);
		}
			
	}
	IEnumerator missleVolley(){
		GameObject o = GameObject.FindGameObjectsWithTag ("GameController") [0];

		yield return new WaitForSecondsRealtime (0.25f);
		GameObject projectile1 =Instantiate (rocket,o.gameObject.transform,true);
		o.SendMessage("addAmmo",-1);
		projectile1.transform.position = new Vector3 (this.gameObject.transform.position.x,this.gameObject.transform.position.y+0.25f,0);
		yield return new WaitForSecondsRealtime (0.25f);
		o.SendMessage("addAmmo",-1);
		GameObject projectile2 =Instantiate (rocket,o.gameObject.transform,true);
		projectile2.transform.position = new Vector3 (this.gameObject.transform.position.x,this.gameObject.transform.position.y+0.25f,0);
		yield return new WaitForSecondsRealtime (0.25f);
		o.SendMessage("addAmmo",-1);
		GameObject projectile3 =Instantiate (rocket,o.gameObject.transform,true);
		projectile3.transform.position = new Vector3 (this.gameObject.transform.position.x,this.gameObject.transform.position.y+0.25f,0);
		yield return new WaitForSecondsRealtime (0.25f);
		o.SendMessage("addAmmo",-1);
		GameObject projectile4 =Instantiate (rocket,o.gameObject.transform,true);
		projectile4.transform.position = new Vector3 (this.gameObject.transform.position.x,this.gameObject.transform.position.y+0.25f,0);

	}
	IEnumerator extend(){
		GetComponent<Animator> ().Play("Extend");
		yield return new WaitForSecondsRealtime (0.25f);
		GetComponent<Animator> ().Play("Extended");
		GetComponent<CapsuleCollider2D> ().size = new Vector2 (1.56f,0.32f);
		boundary -= 0.25f;
	}
	IEnumerator unextend(){
		GetComponent<Animator> ().speed=-1;
		GetComponent<Animator> ().Play("Extend");
		yield return new WaitForSecondsRealtime (0.25f);
		GetComponent<Animator> ().Play("idle");
		GetComponent<Animator> ().speed=1;

		if(weapon=="blaster")GetComponent<Animator> ().Play ("PowerUpGreen");
		if(weapon=="rocket")GetComponent<Animator> ().Play ("Blue");

		GetComponent<CapsuleCollider2D> ().size = new Vector2 (0.96f,0.32f);
		boundary += 0.25f;
	}
	public void setBall(bool b){
		
		ballInPlay = b;
	}
}
