using UnityEngine;

public class HumanData
{
    public string name;
    public int age; // 18-40
    public int income; // 20 - 150 (in thousands per year)
    public int appearance; // 1-4
    public int race; // 1 - asian, 2 - hispanic, 3 - caucasian 4, - black
    public int height; // 5'0 (60 inch) -> 6'5 (77 inch)
    public int weight; // 100 lbs -> 250lbs
    public int bmi;
    public bool gender; // True is male, false is female
    public bool sameRacePref; // Will they only date the same race
    public int num;
    public int coupleWith;
    public GameObject objRef;

    public void engage(int n)
    {
        coupleWith = n;
    }
}
