using UnityEngine;
using UnityEngine.PlayerLoop;

public class GameBoard : MonoBehaviour
{

    private static int boardWidth = 28;  //labiretimizin geniþliði
    private static int boardHeight = 31; //labirentimizin boyu

    public GameObject[,] board = new GameObject[boardWidth, boardHeight];

    void Start()
    {
        Object[] objects = GameObject.FindObjectsOfType(typeof(GameObject));// labirentteki bütün gameobjeleri depoluyor

        foreach (GameObject objectGameObject in objects)  // objelerin üstünden geziyor
        {
            Vector2 position = objectGameObject.transform.position; // objelerin pozisyonunu alýyor

            if (objectGameObject.name != "PacMan") // eðer objenin adý pacman deðilse 
            {
                board[(int)position.x, (int)position.y] = objectGameObject; //objenin pozisyonunu alýp "objectGameobjectte" depoluyor
            }
            else
            {
                Debug.Log("Pacman is here:" + position); // eðer objenin adý pacmanse konumunu istiyoruz denemek için yaptýk bunu
            }
        }
    }


}