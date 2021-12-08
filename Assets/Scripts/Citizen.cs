using UnityEngine;
using System.Collections;
using System;
using Random = UnityEngine.Random;

public class Citizen : MonoBehaviour 
{
    //TODO: 
    //1. Colocar el nodo raíz.
    //2. Hacer lo que sea necesario para updatear las desiciones
    //del aldeano.
    private Node currentNode; //current Node no es mas descriptivo que initialNode? Porque este valor va cambiando

    [Header("Gather Rates")]
    public float harvestAmount;
    public float chopAmount;

    [Header("Costs")]
    public float dailyFoodIntake;
    public float livingCost;

    [Header("Military")]
    public GameObject militaryZone;
    public GameObject barracks;
    public GameObject archery;
    public GameObject stables;

    private void Start()
    {
        //To Do: Agregar protesta gremial (si el salario es comido por la inflacion)
        //Action Nodes
        var harvest = new ActionNode(GetFood);
        var chopWood = new ActionNode(GetWood);
        var build = new ActionNode(BuildHouses);
        var sleep = new ActionNode(GoToSleep);
        var buildBarracks = new ActionNode(BuildBarracks);
        var buildArchery = new ActionNode(BuildArchery);
        var buildStables = new ActionNode(BuildStables);
        //var Protest() = new ActionNode(Protest);

        //Question Nodes
        var q_whatTypeOfEnemies = new TripleQuestionNode(WhatTypeIsEnemy,buildStables, buildArchery, buildBarracks); // Choose counter unity depending on enemy
        var q_needsHouse = new QuestionNode(DoINeedMoreHouses, build, q_whatTypeOfEnemies);
        var q_hasEnoughWood = new QuestionNode(DoIHaveEnoughWood, q_needsHouse, chopWood);
        var q_hasEnoughFood = new QuestionNode(DoIHaveEnoughFood, q_hasEnoughWood, harvest);
        var q_isRaining = new QuestionNode(IsItRaining, sleep, q_hasEnoughFood);
        var q_isDay = new QuestionNode(IsItDay, q_isRaining, sleep);
        

        //var IsSalaryEnough() = new QuestionNode(IsMySalaryEnough, HasEnoughFood, Protest);

        currentNode = q_isDay;
        StartCoroutine(AIUpdate());
    }

    private bool IsItDay()
    {
        if (EnviromentData.Instance.day)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool IsItRaining()
    {
        if (EnviromentData.Instance.raining)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool DoIHaveEnoughFood()
    {
        if (EnviromentData.Instance.foodQty > dailyFoodIntake) 
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool DoIHaveEnoughWood()
    {
        if (EnviromentData.Instance.woodQty >= EnviromentData.Instance.houseWoodCost)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool DoINeedMoreHouses()
    {
	    return (EnviromentData.Instance.maxPopulation - 5 <= EnviromentData.Instance.population);
    }

    private EnviromentData.Enemies WhatTypeIsEnemy()
    {
	    return EnviromentData.Instance.currentEnemies;
	    Debug.Log("Current enemy: " + EnviromentData.Instance.currentEnemies);
    }

    private IEnumerator AIUpdate()
    {
        while (true)
        {
            yield return new WaitForSeconds(EnviromentData.Instance.timeStep);
            Debug.Log("t");
            currentNode.Action(); 
        }
    }

    
    public ParticleSystem sleepPos;
	public ParticleSystem getWoodPos;
	public ParticleSystem farmingPos;
	public ParticleSystem buildPos;

	public void GetFood()
	{
        EnviromentData.Instance.foodQty += 5;
		Debug.Log("Decision: Get Food!");
		DeactivateAllParticles();		
		SetPosAndPlayParticle(farmingPos);
	}

	public void GetWood()
	{
        EnviromentData.Instance.woodQty += 5;
		Debug.Log("Decision: Get Wood!");
		DeactivateAllParticles();		
		SetPosAndPlayParticle(getWoodPos);
	}

	public void BuildHouses()
	{
        EnviromentData.Instance.woodQty -= EnviromentData.Instance.houseWoodCost;
        HouseManager.Instance.BuildHouse();
		DeactivateAllParticles();		
		SetPosAndPlayParticle(buildPos);
	}

	public void GoToSleep()
	{
		Debug.Log("Decision: Go to Sleep!");
		DeactivateAllParticles();		
		SetPosAndPlayParticle(sleepPos);
	}

	private void DeactivateAllParticles()
	{
		sleepPos.Stop();
		getWoodPos.Stop();
		farmingPos.Stop();
		buildPos.Stop();
	}

	private void SetPosAndPlayParticle(ParticleSystem target)
	{
		transform.position = target.transform.position;
		target.Play();
	}

	public void BuildBarracks()
	{
		SpawnBuilding(barracks);
		Debug.Log("Decision:   Build barracks");
	}

	public void BuildArchery()
	{
		SpawnBuilding(archery);
		Debug.Log("Decision:   Build Archery");
	}

	public void BuildStables()
	{
		SpawnBuilding(stables);
		Debug.Log("Decision:   Build Stables");
	}

	void SpawnBuilding(GameObject newBuilding)
	{
		EnviromentData.Instance.woodQty -= EnviromentData.Instance.houseWoodCost;
		EnviromentData.Instance.population += 5;
		var randomVector = new Vector3(Random.Range(-20, 20), 0, Random.Range(-20, 20));
		Instantiate(newBuilding, militaryZone.transform.position + randomVector, Quaternion.identity, militaryZone.transform);
	}
}
