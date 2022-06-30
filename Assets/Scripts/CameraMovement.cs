using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private float speed = 10f;
    private Vector2 turn;

    private void Update()
    {
        if (GameManager.instance.followPlayer)
        {
            transform.position = GameManager.instance.playerData.objRef.transform.position + new Vector3(0, 2, -5);
            transform.localRotation = Quaternion.identity;  
        }
        else
        {

            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");
            float up = 0;

            if (Input.GetMouseButton(1))
            {
                turn.x += Input.GetAxis("Mouse X");
                turn.y += Input.GetAxis("Mouse Y");
                transform.localRotation = Quaternion.Euler(-turn.y, turn.x, 0);
            }

            if (Input.GetKey(KeyCode.Space)) up++;
            if (Input.GetKey(KeyCode.LeftShift)) up--;

            Vector3 movement = new Vector3(x, up, z);
            transform.Translate(movement * speed * Time.deltaTime);


            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit raycastHit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out raycastHit, 100f, LayerMask.GetMask("Taken")))
                {
                    if (raycastHit.transform != null)
                    {
                        ClickedGameObject(raycastHit.transform.gameObject);
                    }
                }
            }
        }
    }

    private void ClickedGameObject(GameObject go)
    {
        if (go.tag == "Actor")
        {
            if (GameManager.instance.coupleOnly)
            {
                

                HumanData humanFiance;

                if (go.name == "Player")
                {
                    Player human = go.GetComponent<Player>();
                    humanFiance = GameManager.instance.humanDatas[human.data.coupleWith];
                }
                else {
                    Human human = go.GetComponent<Human>();
                    humanFiance = GameManager.instance.humanDatas[human.data.coupleWith];
                }

                transform.position = humanFiance.objRef.transform.position + new Vector3(0, 2, -5);
                transform.localRotation = Quaternion.identity;
            }
        }
    }   
}
