using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacMan : MonoBehaviour
{

    public float pacmanSpeed = 4.0f;  //pacmanin h�z�

    public Sprite idleSprite;  //pacmanin durma animasyonu 

    private Vector2 direction = Vector2.zero; //pacmanin y�n� bunu default olarak  0 kabul ediyoruz

    private Vector2 nextDirection;  //pacmanin s�radaki y�n�

    private Node currentNode /*bulundu�umuz d���m */, 
                 previousNode /* �nceki d���m */, 
                 targetNode /* gidece�imiz d���m */; 


    void Start()
    {
        Node node = GetNodeAtPosition(transform.localPosition); // pacmanin pozisyonu veriyoruz ki yak�n�ndaki pozisyonlar� ��renebilelim

        if (node != null) // e�er  node bo� de�ilse �uanda bir d���mdeyiz demektir 
        {
            currentNode = node; // ve bu d���m� "currentNode" 'un i�inde tutuyoruz
        }

        direction = Vector2.left; // default olarak y�n�m�z� sol belirliyoruz ��nk� pacman oynunda klasik olarak pacman sola bak�yor

        ChangePosition(direction);
    }


    void Update()
    {
        CheckInput();

        Move();

        UpdateOrientation();

        UpdateAnimationState();
    }

    void CheckInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            ChangePosition(Vector2.left);
        }

        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            
            ChangePosition(Vector2.right);
        }

        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            
            ChangePosition(Vector2.up);
        }

        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            
            ChangePosition(Vector2.down);
        }
    }

    void ChangePosition(Vector2 directionChangePosition)  //pozisyon de�i�tirme
    {
        if (directionChangePosition != direction)  // e�er de�i�tirdi�imiz yada yol ald���m�z pozisyon asl�nda directionumzua e�it de�ilse
        {
            nextDirection = directionChangePosition;  // bir sonraki pozisyonumuzdur
        }

        if (currentNode != null)  // bulundu�umuz d���m bo� de�ilse yani node sa
        {
            Node moveToNode = CanMove(directionChangePosition); // �nce bu node da ilerleyebilirmiyiz kontrol ediyoruz 

            if (moveToNode != null)  // ilerliyece�imiz node bo� de�ilse
            {
                direction = directionChangePosition; // y�n�m�z girmi� oldu�umuz
                targetNode = moveToNode;   // hedef node'miz �n�m�zdeki node
                previousNode = currentNode; // �nceki node'muz �uanki node'muz
                currentNode = null; // ve hareket edece�i i�in bulundu�umuz node null d�r.

            }
        }
    }

    void Move()
    {
        if (targetNode != currentNode && targetNode != null)        // hedef node'muzun �uanda bulundu�umuz node olamaz ve hedef nodemuz bo� olamaz
        {

            if (nextDirection == direction * -1)        // burda ilerledi�imiz y�n �uanda gitmek istedi�imiz y�n�n tersiyse yani -1 le �arp�lm�� haliyle ayn�ysa y�n�m�z� de�i�tirmek istiyoruz demektir
            {
                direction *= -1;        // -1 le �arp�nca e�er sola gidiyorsak sa�a gitmeye ba�l�caz e�er yukar� gidiyorsak a�a�� gidiyoruz
                
                Node tempNode = targetNode;     // �uanki hedef node'u ge�ici olarak depoluyoruz

                targetNode = previousNode;      // �uanki hedef nodemuzu geldi�imiz eski node 'u diyoruz

                previousNode = tempNode;       // ge�i�i node'muzu �nceki node'muz yap�yoruz

                /* burda olay �nce gitmek istedi�imiz y�n� anl�yoruz �rne�in sa�a giderken sola gitmek istiyoruz. Biz bu hareketleri node dan node'a yapt���m�z i�in y�n kontrol� i�inde hedefimizde olan node 'u kullan�yoruz
                 �nce tekrar kullanmak i�in hedef nodemuzu depoluyoruz ard�ndan hedefimizi de�i�tiriyoruz ve gitmek istedi�imiz yeni y�n� at�yoruz ki bu y�nde geldi�imiz y�n yani �nceki previous y�n� ard�ndan eski y�n�m�z�de tempNode'e att���m�z
                 ilk ba�taki hedef nodemuz oluyor.
                 
                 */

            }

            if (OverShotTarget()) // pacman�n s�n�rlar�n� belirliyoruz yani gidebilece�i yerleri
            {

                currentNode = targetNode;  // default olarak gidece�imiz node hedef nodedur
                transform.localPosition = currentNode.transform.position; // pozisyonumuz da gidece�imiz node a e�ittir


                GameObject otherPortal = GetPortal(currentNode.transform.position); // other portal objesinin i�ine getportaldaki kontrol�m�z� �a�r�p portal�m�z� istip depoluyoruz

                if (otherPortal != null) // e�er other portal bo� de�ilse i�lemlere ba�l�yoruz
                {
                    transform.localPosition = otherPortal.transform.position;  // �nce globaldeki konumumuzu receiver portal�n�n mazedeki konumuna d�n��t�r�yoruz

                    currentNode = otherPortal.GetComponent<Node>(); // ard�ndan bulundu�umuz konumu yani current konumunu gidece�imiz receiver portal�na g�re ayarl�yoruz ve onun node scrpiti 
                }

                
                Node moveToNode = CanMove(nextDirection);  // gidece�imiz nodelar harekete uygunmu kontrol edip moveToNode a depoluyoruz

                if (moveToNode != null)  // e�er bo� de�ilse �n�m�zdeki y�n do�rudur ve gidece�imiz y�n olabilir
                {
                    direction = nextDirection;
                }

                if (moveToNode == null) // e�er bo�sa bu sefer kendi direction�m�z� kontrol ediyoruz harekete uygun mu
                {
                    moveToNode = CanMove(direction);
                }

                if (moveToNode != null) // e�er yinede bo�sa bu sefer hedef nodemiz gidebilece�imiz node olur ve �nceki nodemuz asl�nda �uan bulundu�umuz konumdad�r ve harekete devam edince �uanki nodemuz bo�tur 
                {
                    targetNode = moveToNode;
                    previousNode = currentNode;
                    currentNode = null;
                }
                else
                {
                    direction = Vector2.zero; // hi�birine uymazsa pacman durmal�d�r
                }
            }
            else  // e�er bunlar�n sonucunda hedefi a�m�yorsak yani s�n�r� ge�miyorsak harekete devam edebiliriz
            {
                transform.localPosition += (Vector3)(direction * pacmanSpeed) * Time.deltaTime;
            }
        }
        
    }

    void MoveToNode(Vector2 directionMoveToNode)
    {
        Node moveToNode = CanMove(directionMoveToNode);

        if (moveToNode != null)
        {
            transform.localPosition = moveToNode.transform.position;

            currentNode = moveToNode;
        }
    }

    void UpdateOrientation()  // pacmanin y�n�n� g�ncelliyoruz
    {
        if (direction == Vector2.left)  // e�er sola gidiyorsa
        {
            transform.localScale = new Vector3(-1, 1, 1);  
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        else if (direction == Vector2.right)    // e�er sa�a gidiyorsa
        {
            transform.localScale = new Vector3(1, 1, 1);
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        else if (direction == Vector2.up)   // e�er yukar� gidiyorsa
        {
            transform.localScale = new Vector3(1, 1, 1);
            transform.localRotation = Quaternion.Euler(0, 0, 90);
        }
        else if (direction == Vector2.down)  // e�er a�a�� gidiyorsa
        {
            transform.localScale = new Vector3(1, 1, 1);
            transform.localRotation = Quaternion.Euler(0, 0, 270);
        }
    }

    void UpdateAnimationState() // durma animsyonu
    {
        if (direction == Vector2.zero)
        {
            GetComponent<Animator>().enabled = false;
            GetComponent<SpriteRenderer>().sprite = idleSprite;
        }
        else
        {
            GetComponent<Animator>().enabled = true;
        }
    }

    Node CanMove(Vector2 directionCanMove) //hareket edebilece 
    {
        Node moveToNode = null; // default olarak gidebilece�imiz node'u bo�� olarak i�aretliyoruz

        for (int i = 0; i < currentNode.neighbors.Length; i++)
        {
            if (currentNode.validDirections[i] == directionCanMove) // for d�ng�s�nden gelen index i kontrol ediyoruz bizim y�n�m�zle do�rumu diye
            {
                moveToNode = currentNode.neighbors[i]; // e�er do�ru ve orda bir node varsa belirtiyoruz hareket edilebilir
                break; // ve for d�ng�s�n� durduruyoruz
            }
        }

        return moveToNode; // e�er d�ng�den sonu� ��karsa y�n�m�z ��kan sonu� olur fakat sonu� ��kmazsa bo� d�nd�r�r
    }

    

    Node GetNodeAtPosition(Vector2 position)  // d���m�n pozisyonunu alma methodu
    {
        GameObject tile = GameObject.Find("Game").GetComponent<GameBoard>().board[(int)position.x,(int)position.y]; // gameboard i�ersinde depolanan �n�m�zdeki node'�n konumunu istiyoruz

        if (tile != null)  // e�er �n�m�zdeki node gameboard da ise ve konumu depolanm��sa bo� de�ildir ve e�er bo� de�ilse
        {
            return tile.GetComponent<Node>(); // bu objenin konumunu g�nderiyoruz.
        }

        return null; // e�er kar��m�zdaki node gameboard da bir kar��l��� yoksa bo� d�nd�r�yoruz
    }

    bool OverShotTarget() 
    {
        float nodeToTarget = LengthFromNode(targetNode.transform.position); // ilerledi�imiz de�il ilereyelebilece�imiz node mesafesi
        float nodeToSelf = LengthFromNode(transform.localPosition);  // ilerledi�imiz node mesafesi
        
        return nodeToSelf > nodeToTarget; // e�er ilerledi�imiz node mesafesi gidebilece�imizden b�y�k hata var demektir bu nedenle true d�ner
    }

    float LengthFromNode(Vector2 targetPosition) // hedefteki nodela bulundu�umuz node aras�ndaki mesafeyi buluyor. negatif ��kma ihtimaline kar��n karesini al�yoruz
    {
        Vector2 vector = targetPosition - (Vector2) previousNode.transform.position; // bulundu�umuz pozisyondan d���m pozisyonunu ��kart�yoruz ortaya 
        return vector.sqrMagnitude;  
    }

    GameObject GetPortal(Vector2 position)  //portalin konumunu ��renme
    {
        GameObject tile = GameObject.Find("Game").GetComponent<GameBoard>().board[(int)position.x,(int)position.y];  // tile objesinde portal�n gameboardda depolanan konumunu alma

        if (tile != null) // tile�n bo� olup olmad�n� yani o konumda varm� yokmu kontrol ediyoruz varsa devam ediyoruz
        {
            if (tile.GetComponent<Tile>() != null) // tile'�n i�inde tile scrpiti varm� kontrol ediyoruz varsa devam ediyoruz
            {
                if (tile.GetComponent<Tile>().isPortal)  // tilein portal olup olmad���n� kontrol ediyoruz
                {
                    GameObject otherPortal = tile.GetComponent<Tile>().portalReceiver; // e�er portalsa di�er portal�n hangisi oldu�unu ��reniyoruz
                    return otherPortal; // ve o portal� veriyoruz
                }
            }
        }

        return null; // fakat sonu� portal de�ilse bo� d�nd�r�yoruz
    }

}
