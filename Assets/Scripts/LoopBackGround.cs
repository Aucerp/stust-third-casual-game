using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopBackGround : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] Transform[] backgroundImage;
    [SerializeField] Transform[] walls;
    float distanceBetween;
    float overThanGenerateNext;
    int currentMoveBG = 0;
    int bgImageCount = 4;
    // Start is called before the first frame update
    void Start()
    {
        overThanGenerateNext = backgroundImage[2].position.y;
        distanceBetween = backgroundImage[1].position.y - backgroundImage[0].position.y;
    }

    // Update is called once per frame
    void Update()
    {
        walls[0].transform.position = new Vector3(walls[0].transform.position.x, player.position.y + .5f, walls[0].transform.position.z);
        walls[1].transform.position = new Vector3(walls[1].transform.position.x, player.position.y + .5f, walls[1].transform.position.z);
        if (player.transform.position.y > overThanGenerateNext)
        {
            backgroundImage[currentMoveBG].position += new Vector3(0, distanceBetween * bgImageCount, 0);
            if (currentMoveBG < (bgImageCount - 1))
            { currentMoveBG += 1; }
            else { currentMoveBG = 0; }
            overThanGenerateNext += distanceBetween;
            //Debug.Log("第" + currentMoveBG + "張場景，向上移動");
        }
    }
}
