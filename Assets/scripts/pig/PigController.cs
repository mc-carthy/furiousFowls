﻿using UnityEngine;
using System.Collections;

public class PigController : MonoBehaviour {

	public float health = 150f;
	public Sprite spriteShownWhenHurt;

	private AudioSource source;
	private float changeSpriteHealth;
	private float damageMultiplier = 10;

	private void Awake () {
		source = GetComponent<AudioSource> ();
		changeSpriteHealth = health / 2;
	}

	private void OnCollisionEnter2D (Collision2D col) {
		print ("Colliding");
		if (col.gameObject.GetComponent<Rigidbody2D> () != null) {
			print ("Hit by rigidbody");
			if (col.gameObject.tag == "bird") {
				source.Play ();
				Destroy (gameObject);
			} else {
				print ("Taking damage");
				float damage = col.gameObject.GetComponent<Rigidbody2D> ().velocity.magnitude * damageMultiplier;
				print (damage);
				if (damage >= 10) {
					source.Play ();
				}
				health -= damage;

				if (health < changeSpriteHealth) {
					gameObject.GetComponent<SpriteRenderer> ().sprite = spriteShownWhenHurt;
				}

				if (damage < 0) {
					Destroy (gameObject);
				}
			}
		}
	}
}
