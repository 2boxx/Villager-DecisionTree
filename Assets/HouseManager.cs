using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HouseManager : MonoBehaviour
{
    public static HouseManager Instance;
    public GameObject[] houses;
    private int nextHouseIndex;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
        foreach (var h in houses)
        {
            h.SetActive(false);
        }
    }

    public void BuildHouse()
    {
        houses[nextHouseIndex].SetActive(true);
        EnviromentData.Instance.maxPopulation += 5;
        EnviromentData.Instance.UIMaxPopulation.text = EnviromentData.Instance.maxPopulation.ToString();
        if (nextHouseIndex < houses.Length - 1)
        {
            nextHouseIndex++;
        }
        else
        {
            Debug.Log("NO MORE HOUSES TO BUILD (visual limit)");
        }
    }
}
