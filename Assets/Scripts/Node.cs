using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Node : MonoBehaviour
{
    public Node[] neighbors; //her düðümün komþularýný depoluyor
    public Vector2[] validDirections;  // düðümün hangi yönde olduðunu belirtiyor



    void Start()
    {
        validDirections = new Vector2[neighbors.Length]; // default olarak komþulara olan uzaklýðýmýzý alýyor fakat bu hareket gerçekleþene kadar 0 0 0 oluýyor

        for (int i = 0; i < neighbors.Length; i++)
        {
            Node neighbor = neighbors[i]; //düðümleri belirliyor
            Vector2 tempVector = neighbor.transform.localPosition - transform.localPosition; //geçiçi olarak düðümlerle aramýzdaki mesafeyi belirliyor
            validDirections[i] = tempVector.normalized;  // düðümlerle aramýzdaki mesafeye göre default olan deðerle birlikte mesafeyi normalized edip yani 1 1 1  lik bir vektöre çeviripi yönümüzü belirtiyor
            
        }
    }
}