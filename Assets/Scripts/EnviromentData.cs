using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class EnviromentData : MonoBehaviour
{
    //TODO:
    //Esta clase va a contener toda la información necesaria 
    //para que el aldeano pueda decidir que hacer (acceden a esta info haciendo algo como
    //EnviromentData.Instance.foodQty).
    //Por ejemplo acá pueden poner:
    //1. Cuanta madera tiene (la necesita para construir)
    //2. Cuanta comida tiene (la necesita para vivir)
    //3. Estado del clima (si llueve no debería salir de su casa porque se enferma)
    //4. Momento del día (de noche es peligroso salir)	

    //La siguiente region les va a servir para
    //acceder al aldeano desde sus nodos 'Accion' (deberán heredar de los nodos 'Accion'		
    //y en el método Execute() hacer algo como 'EnviromentData.Instance.citizen.DoSomething()')
    
    [Header("AI Update")]
    public float timeStep;
    [Header("Player Resources")]
    public float foodQty;
    public float woodQty;
    public int maxPopulation;
    public int population;

    [Header("Global Variables")]
    public bool raining;
    public bool day;

    public enum Enemies{Infantry, Archers, Cavalry};

    public Enemies currentEnemies;

    public float houseWoodCost;
    public float workerSalary;

    public float counter;
    public float wholeDayDuration;

    public GameObject rain;
    public GameObject sun;

    [Header("UI")]
    public TextMeshProUGUI UIWood;
    public TextMeshProUGUI UIFood;
    public TextMeshProUGUI UIMaxPopulation;
    public TextMeshProUGUI UIPopulation;

    public List<GameObject> enemySprites;
    
    IEnumerator WeatherUpdate()
    {
        while (true)
        {
            yield return new WaitForSeconds(wholeDayDuration / 4);
            {
                float r = Random.value;
                if (raining && r > 0.2f)
                {
                    raining = false;
                }
                if (r > 0.8f)
                {
                    raining = true;
                }
                rain.SetActive(raining);
            }
        }
    }

    private void Update()
    {
        counter += Time.deltaTime;
        if (counter >= wholeDayDuration / 2)
        {
            day = !day;
            counter = 0f;
            foodQty -= citizen.livingCost / 2;
            RandomizeEnemies();
        }
        sun.transform.Rotate(Vector3.up, Time.deltaTime / wholeDayDuration * 360f);
        UIFood.text = foodQty.ToString();
        UIWood.text = woodQty.ToString();
        UIPopulation.text = population.ToString();
    }

    void RandomizeEnemies()
    {
        currentEnemies = (Enemies)Random.Range(0, 2); //Select a random enemy attacker
        foreach (GameObject enemy in enemySprites)
        {
            enemy.SetActive(false);
        }

        GameObject activeEnemy = enemySprites[(int)currentEnemies];
        activeEnemy.SetActive(true);
    }
    
    private void Start()
    {
        StartCoroutine(WeatherUpdate());
    }

    public Citizen citizen;
	private static EnviromentData _instance;
	public static EnviromentData Instance {	get	{	return _instance;	}	}
	void Awake()
	{
		_instance = this;
	}
}
