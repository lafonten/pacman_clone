using UnityEngine;
using UnityEngine.PlayerLoop;

public class GameBoard : MonoBehaviour
{

    private static int boardWidth = 28;  //labiretimizin geni�li�i
    private static int boardHeight = 31; //labirentimizin boyu

    public GameObject[,] board = new GameObject[boardWidth, boardHeight];

    void Start()
    {
        Object[] objects = GameObject.FindObjectsOfType(typeof(GameObject));// labirentteki b�t�n gameobjeleri depoluyor

        foreach (GameObject objectGameObject in objects)  // objelerin �st�nden geziyor
        {
            Vector2 position = objectGameObject.transform.position; // objelerin pozisyonunu al�yor

            if (objectGameObject.name != "PacMan") // e�er objenin ad� pacman de�ilse 
            {
                board[(int)position.x, (int)position.y] = objectGameObject; //objenin pozisyonunu al�p "objectGameobjectte" depoluyor
            }
            else
            {
                Debug.Log("Pacman is here:" + position); // e�er objenin ad� pacmanse konumunu istiyoruz denemek i�in yapt�k bunu
            }
        }
    }


}