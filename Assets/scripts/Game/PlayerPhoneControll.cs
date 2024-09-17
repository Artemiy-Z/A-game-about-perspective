using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPhoneControll : MonoBehaviour
{
    public PlayerMovement playerMovement;
    public PlayerSynchronization playerSynchronization;

    public void MoveRight(bool val)
    {
        if (val)
            playerMovement.inp.x = 1;
        else
            playerMovement.inp.x = 0;
        print("mR");
    }

    public void MoveLeft(bool val)
    {
        if (val)
            playerMovement.inp.x = -1;
        else
            playerMovement.inp.x = 0;
        print("mL");
    }

    public void Jump(bool val)
    {
        if (val)
            playerMovement.inp.y = 1;
    }

    public void RotateRight()
    {
        playerSynchronization.RotateByExternalControll(-90);
    }

    public void RotateLeft()
    {
        playerSynchronization.RotateByExternalControll(90);
    }

    private void Update()
    {
        
    }
}
