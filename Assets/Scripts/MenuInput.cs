using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuInput : MonoBehaviour
{
    public InputField humanName;
    public InputField age;
    public InputField salary;
    public InputField appearance;
    public InputField race;
    public InputField height;
    public InputField weight;
    public InputField gender;
    public InputField sameRacePref;
    public InputField simSize;

    public void Awake()
    {
        if (GameManager.instance.playerData != null)
        {
            humanName.text = GameManager.instance.playerData.name;
            age.text = GameManager.instance.playerData.age.ToString();
            salary.text = GameManager.instance.playerData.income.ToString();
            appearance.text = GameManager.instance.playerData.appearance.ToString();
            race.text = GameManager.instance.playerData.race.ToString();
            height.text = GameManager.instance.playerData.height.ToString();
            weight.text = GameManager.instance.playerData.weight.ToString();
            gender.text = GameManager.instance.playerData.gender ? "m" : "f";
            sameRacePref.text = GameManager.instance.playerData.sameRacePref ? "y" : "n";
            simSize.text = GameManager.instance.simAmount.ToString();
        }
    }
    public void StartSimulation()
    {
        int h = int.Parse(height.text);
        int w = int.Parse(weight.text);

        HumanData playerData = new HumanData()
        {
            name = humanName.text,
            age = int.Parse(age.text),
            income = int.Parse(salary.text),
            appearance = int.Parse(appearance.text),
            race = int.Parse(race.text),
            height = h,
            weight = w,
            bmi = (int)(w / Mathf.Pow(h, 2)) * 703,
            gender = gender.text == "m" ? true : false,
            sameRacePref = sameRacePref.text == "y" ? true : false,
            num = 0,
            coupleWith = -1,
            objRef = null // set this later in player
        };

        GameManager.instance.playerData = playerData;
        GameManager.instance.humanDatas = new List<HumanData>();
        GameManager.instance.humanDatas.Add(playerData);
        GameManager.instance.simAmount = int.Parse(simSize.text);

        SceneManager.LoadScene("Main");
    }
}
