using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public HumanData data;
    private int num;
    private float[] incomeMultipliers = { 0.34f, 0.68f, 1.02f, 1.36f };
    private MeshRenderer meshRenderer;
    private float startTime;
    private Text statText;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        Transform canvas = transform.GetChild(0);
        statText = canvas.GetChild(0).GetComponent<Text>();
    }

    private void Start()
    {
        GameManager.instance.playerData.objRef = gameObject;
        data = GameManager.instance.playerData;
        Debug.Log(data.objRef);

        if (data.gender)
        {
            meshRenderer.material = GameManager.instance.male;
        }
        else
        {
            meshRenderer.material = GameManager.instance.female;
        }

        transform.localScale = new Vector3(1, (float)((data.height - 60f) / 25f) + 0.75f, 1);

        UpdateStat();
    }

    private void Update()
    {
        if (GameManager.instance.coupleOnly && data.coupleWith == -1)
        {
            meshRenderer.enabled = false;
            statText.enabled = false;
        }
        else
        {
            meshRenderer.enabled = true;
            statText.enabled = true;
        }
    }

    public void UpdateStat()
    {
        string race;

        if (data.race == 1)
        {
            race = "asian";
        }
        else if (data.race == 2)
        {
            race = "hispanic";
        }
        else if (data.race == 3)
        {
            race = "caucasian";
        }
        else
        {
            race = "black";
        }

        string coupleName = "None";
        if (data.coupleWith != -1)
        {
            coupleName = GameManager.instance.humanDatas[data.coupleWith].name;
        }

        string attractive = "Average";

        if (data.appearance == 4)
        {
            attractive = "Attractive";
        }
        else if (data.appearance == 3)
        {
            attractive = "Above Avg";
        }
        else if (data.appearance == 2)
        {
            attractive = "Avg";
        }
        else
        {
            attractive = "Ugly";
        }

        string heightStr = "";
        int ft = (int)data.height / 12;
        int inch = data.height - (ft * 12);
        heightStr = ft.ToString() + "ft " + inch.ToString();

        statText.text = "Name: " + data.name + "\nAge: " + data.age.ToString() + "\nIncome: " + data.income.ToString() + "k/yr\nAttractive: " + attractive + "\nRace: " + race + "\nHeight: " + heightStr + "\nWeight: " + data.weight.ToString() + "\nGender: " + (data.gender ? "Male" : "Female") + "\nCoupleWith: " + coupleName;
    }

    // Receive Message sent during collision. Let the male handle all of the logic because im too stupid to figure out how to make it separate.
    private void ReceiveMatch(HumanData human)
    {
        if (Time.time - startTime < 2f)
        {
            Debug.Log("Too early");
            return; // Don't date within the first 2 seconds
        }
        if (!data.gender)
        {
            Debug.Log("Won't run logic in female");
            return;
        };
        if (human.gender == data.gender)
        {
            Debug.Log("Same Gender nahh");
            return;
        };
        if (data.coupleWith != -1 || human.coupleWith != -1)
        {
            Debug.Log("Already has a couple");
            return;
        };

        HumanData female;
        HumanData male;

        if (data.gender)
        {
            female = human;
            male = data;
        }
        else
        {
            female = data;
            male = human;
        }

        if (Mathf.Abs(male.age - female.age) > 10)
        {
            Debug.Log("Too Big Age Gap");
            return;
        }

        // same race pref
        if (male.sameRacePref || female.sameRacePref)
        {
            if (male.race != female.race)
            {
                Debug.Log("Wont date. different race");
                return;
            }
        }

        // too big status gap makes it impossible to have a good relationship
        float interactionChance = ((Mathf.Abs(4 - male.appearance + female.appearance) * 25f) / 100f);
        if (Random.Range(0f, 1f) > interactionChance) return;

        float datingChanceMale = 0.4f; // Will the male like the female
        float datingChanceFemale = 0.4f; // Will the female like the male 

        int maleIncome = 0;
        if (male.income < 50)
        {
            maleIncome = 1;
        }
        else if (male.income < 70)
        {
            maleIncome = 2;
        }
        else if (male.income < 100)
        {
            maleIncome = 3;
        }
        else
        {
            maleIncome = 4;
        }

        // Income (doesn't affect females)
        datingChanceFemale *= incomeMultipliers[maleIncome - 1];
        // BMI
        datingChanceMale *= (float)(17 / Mathf.Abs(female.bmi - 17)) * 1.2f;
        datingChanceFemale *= (float)(27 / Mathf.Abs(male.bmi - 27)) * 1.2f;

        // Height
        if (male.height >= 72) datingChanceFemale *= 1.65f;
        if (female.height <= 68) datingChanceMale *= 1.65f;

        // Appearance
        datingChanceFemale *= (male.appearance / female.appearance);
        datingChanceMale *= (female.appearance / male.appearance);

        float datingChance = (datingChanceMale + datingChanceFemale) / 2;
        Debug.Log(datingChance);
        if (datingChance > 0.7f)
        {
            // They will date!
            Debug.Log(data.name + "(" + data.num + ") and " + human.name + "(" + human.num + ") are dating!");

            Color c = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));

            human.objRef.GetComponent<Human>().data.coupleWith = data.num;
            human.objRef.GetComponent<Human>().UpdateStat();
            human.objRef.GetComponent<MeshRenderer>().material.SetColor("_Color", c);
            human.objRef.layer = LayerMask.NameToLayer("Taken");

            data.coupleWith = human.num;
            UpdateStat();
            meshRenderer.material.SetColor("_Color", c);
            gameObject.layer = LayerMask.NameToLayer("Taken");
        }
        else
        {
            // They will not date :(
            Debug.Log("They won't date :(");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Actor")
        {
            collision.collider.SendMessage("ReceiveMatch", data);
        }
    }
}
