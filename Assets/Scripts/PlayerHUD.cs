using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    public Image toggleBtn;
    public Image coupleOnly;
    public Image followPlayer;

    public void Update()
    {
        if (GameManager.instance.paused)
        {
            toggleBtn.color = Color.green;
        }
        else {
            toggleBtn.color = Color.white;
        }

        if (GameManager.instance.coupleOnly)
        {
            coupleOnly.color = Color.green;
        }
        else {
            coupleOnly.color = Color.white;
        }

        if (GameManager.instance.followPlayer)
        {
            followPlayer.color = Color.green;
        }
        else {
            followPlayer.color = Color.white;
        }
    }

    public void toggle()
    {
        GameManager.instance.toggle();
    }

    public void toggleCoupleOnly()
    {
        GameManager.instance.toggleCoupleOnly();
    }

    public void ResetSimulation()
    {
        GameManager.instance.ResetSimulation();
    }

    public void toggleFollowPlayer()
    {
        GameManager.instance.toggleFollowPlayer();
    }
}
