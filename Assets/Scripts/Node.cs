using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Node : MonoBehaviour
{
    public Node[] neighbors; //her d���m�n kom�ular�n� depoluyor
    public Vector2[] validDirections;  // d���m�n hangi y�nde oldu�unu belirtiyor



    void Start()
    {
        validDirections = new Vector2[neighbors.Length]; // default olarak kom�ulara olan uzakl���m�z� al�yor fakat bu hareket ger�ekle�ene kadar 0 0 0 olu�yor

        for (int i = 0; i < neighbors.Length; i++)
        {
            Node neighbor = neighbors[i]; //d���mleri belirliyor
            Vector2 tempVector = neighbor.transform.localPosition - transform.localPosition; //ge�i�i olarak d���mlerle aram�zdaki mesafeyi belirliyor
            validDirections[i] = tempVector.normalized;  // d���mlerle aram�zdaki mesafeye g�re default olan de�erle birlikte mesafeyi normalized edip yani 1 1 1  lik bir vekt�re �eviripi y�n�m�z� belirtiyor
            
        }
    }
}