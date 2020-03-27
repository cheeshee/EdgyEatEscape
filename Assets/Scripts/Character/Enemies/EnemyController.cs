﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// All Enemies that can damage the player from contact.
// Interactions with the prefab, spawning, and prefab cleanup will be here


public class EnemyController : CharacterController
{
    private GameObject enemyHealth; //The full bar itself
    private GameObject enemyHealthBar; //The red stuff

    //Instances for above
    private GameObject enemyHealthInst;
    private GameObject enemyHealthBarInst;
    private float healthX;

    protected override void OnEnable()
    {
        base.OnEnable();
    }


    protected override void Start()
    {
        base.Start();
    }

    protected override void Awake()
    {
        base.Awake();

        currentHealth = maxHealth;

        //grab resources
        enemyHealth = Resources.Load("EnemyHealth/EnemyHealth") as GameObject;
        enemyHealthBar = Resources.Load("EnemyHealth/EnemyHealthBar") as GameObject;

        //Debug.Log("HEALTH BAR");
        enemyHealthInst = Instantiate(enemyHealth);
        enemyHealthBarInst = Instantiate(enemyHealthBar);

        enemyHealthInst.transform.position = gameObject.transform.position + new Vector3(0.0f, 1.0f, 0.0f);
        enemyHealthBarInst.transform.position = gameObject.transform.position + new Vector3(-0.5f, 1.0f, -1.0f);
        enemyHealthInst.transform.parent = gameObject.transform;
        enemyHealthBarInst.transform.parent = gameObject.transform;
        healthX = enemyHealthBarInst.transform.localScale.x;

        //Debug.Log("Current Health = " + currentHealth + " | Max Health = " + maxHealth);
    }

    private void updateHealth() {
        //Debug.Log("Current Health = " + currentHealth + " | Max Health = " + maxHealth);

        float ratio = (float)currentHealth / (float)maxHealth;
        //Debug.Log("ratio = " + ratio);
        if (ratio < 0)
        {
            ratio = 0;
        }
        enemyHealthBarInst.transform.localScale = new Vector3(healthX * ratio, enemyHealthBarInst.transform.localScale.y, enemyHealthBarInst.transform.localScale.z);
        //Debug.Log("scale = " + enemyHealthBarInst.transform.localScale.x);
    }


    protected override void Update()
    {
        base.Update();
        updateHealth();
    }


    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected override void Movement(Vector2 move, bool yMovement)
    //Function for movement
    {
        base.Movement(move, yMovement);
    }

    protected override void ComputeVelocity() // no implentation in this class, but called in update
    {
        base.ComputeVelocity();
    }

    protected override void OnDeath()
    {
        //onCharacterDeath();
    }

}
