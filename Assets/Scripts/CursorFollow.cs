using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorFollow : MonoBehaviour
{
    private bool isToggled = false;

    // Update is called once per frame
    void Update()
    {
        if (StateCtrl.isPaused == true && isToggled == true) {
            Cursor.visible = true;
            isToggled = false;
        } else if (StateCtrl.isPaused == false && isToggled == false) {
            Cursor.visible = false;
            isToggled = true; 
        }

        transform.position = Input.mousePosition;  
        
    }
}
