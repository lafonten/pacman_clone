using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacMan : MonoBehaviour
{

    public float pacmanSpeed = 4.0f;  //pacmanin hýzý

    public Sprite idleSprite;  //pacmanin durma animasyonu 

    private Vector2 direction = Vector2.zero; //pacmanin yönü bunu default olarak  0 kabul ediyoruz

    private Vector2 nextDirection;  //pacmanin sýradaki yönü

    private Node currentNode /*bulunduðumuz düðüm */, 
                 previousNode /* önceki düðüm */, 
                 targetNode /* gideceðimiz düðüm */; 


    void Start()
    {
        Node node = GetNodeAtPosition(transform.localPosition); // pacmanin pozisyonu veriyoruz ki yakýnýndaki pozisyonlarý öðrenebilelim

        if (node != null) // eðer  node boþ deðilse þuanda bir düðümdeyiz demektir 
        {
            currentNode = node; // ve bu düðümü "currentNode" 'un içinde tutuyoruz
        }

        direction = Vector2.left; // default olarak yönümüzü sol belirliyoruz çünkü pacman oynunda klasik olarak pacman sola bakýyor

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

    void ChangePosition(Vector2 directionChangePosition)  //pozisyon deðiþtirme
    {
        if (directionChangePosition != direction)  // eðer deðiþtirdiðimiz yada yol aldýðýmýz pozisyon aslýnda directionumzua eþit deðilse
        {
            nextDirection = directionChangePosition;  // bir sonraki pozisyonumuzdur
        }

        if (currentNode != null)  // bulunduðumuz düðüm boþ deðilse yani node sa
        {
            Node moveToNode = CanMove(directionChangePosition); // önce bu node da ilerleyebilirmiyiz kontrol ediyoruz 

            if (moveToNode != null)  // ilerliyeceðimiz node boþ deðilse
            {
                direction = directionChangePosition; // yönümüz girmiþ olduðumuz
                targetNode = moveToNode;   // hedef node'miz önümüzdeki node
                previousNode = currentNode; // önceki node'muz þuanki node'muz
                currentNode = null; // ve hareket edeceði için bulunduðumuz node null dýr.

            }
        }
    }

    void Move()
    {
        if (targetNode != currentNode && targetNode != null)        // hedef node'muzun þuanda bulunduðumuz node olamaz ve hedef nodemuz boþ olamaz
        {

            if (nextDirection == direction * -1)        // burda ilerlediðimiz yön þuanda gitmek istediðimiz yönün tersiyse yani -1 le çarpýlmýþ haliyle aynýysa yönümüzü deðiþtirmek istiyoruz demektir
            {
                direction *= -1;        // -1 le çarpýnca eðer sola gidiyorsak saða gitmeye baþlýcaz eðer yukarý gidiyorsak aþaðý gidiyoruz
                
                Node tempNode = targetNode;     // þuanki hedef node'u geçici olarak depoluyoruz

                targetNode = previousNode;      // þuanki hedef nodemuzu geldiðimiz eski node 'u diyoruz

                previousNode = tempNode;       // geçiçi node'muzu önceki node'muz yapýyoruz

                /* burda olay önce gitmek istediðimiz yönü anlýyoruz örneðin saða giderken sola gitmek istiyoruz. Biz bu hareketleri node dan node'a yaptýðýmýz için yön kontrolü içinde hedefimizde olan node 'u kullanýyoruz
                 önce tekrar kullanmak için hedef nodemuzu depoluyoruz ardýndan hedefimizi deðiþtiriyoruz ve gitmek istediðimiz yeni yönü atýyoruz ki bu yönde geldiðimiz yön yani önceki previous yönü ardýndan eski yönümüzüde tempNode'e attýðýmýz
                 ilk baþtaki hedef nodemuz oluyor.
                 
                 */

            }

            if (OverShotTarget()) // pacmanýn sýnýrlarýný belirliyoruz yani gidebileceði yerleri
            {

                currentNode = targetNode;  // default olarak gideceðimiz node hedef nodedur
                transform.localPosition = currentNode.transform.position; // pozisyonumuz da gideceðimiz node a eþittir


                GameObject otherPortal = GetPortal(currentNode.transform.position); // other portal objesinin içine getportaldaki kontrolümüzü çaðrýp portalýmýzý istip depoluyoruz

                if (otherPortal != null) // eðer other portal boþ deðilse iþlemlere baþlýyoruz
                {
                    transform.localPosition = otherPortal.transform.position;  // önce globaldeki konumumuzu receiver portalýnýn mazedeki konumuna dönüþtürüyoruz

                    currentNode = otherPortal.GetComponent<Node>(); // ardýndan bulunduðumuz konumu yani current konumunu gideceðimiz receiver portalýna göre ayarlýyoruz ve onun node scrpiti 
                }

                
                Node moveToNode = CanMove(nextDirection);  // gideceðimiz nodelar harekete uygunmu kontrol edip moveToNode a depoluyoruz

                if (moveToNode != null)  // eðer boþ deðilse önümüzdeki yön doðrudur ve gideceðimiz yön olabilir
                {
                    direction = nextDirection;
                }

                if (moveToNode == null) // eðer boþsa bu sefer kendi directionýmýzý kontrol ediyoruz harekete uygun mu
                {
                    moveToNode = CanMove(direction);
                }

                if (moveToNode != null) // eðer yinede boþsa bu sefer hedef nodemiz gidebileceðimiz node olur ve önceki nodemuz aslýnda þuan bulunduðumuz konumdadýr ve harekete devam edince þuanki nodemuz boþtur 
                {
                    targetNode = moveToNode;
                    previousNode = currentNode;
                    currentNode = null;
                }
                else
                {
                    direction = Vector2.zero; // hiçbirine uymazsa pacman durmalýdýr
                }
            }
            else  // eðer bunlarýn sonucunda hedefi aþmýyorsak yani sýnýrý geçmiyorsak harekete devam edebiliriz
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

    void UpdateOrientation()  // pacmanin yönünü güncelliyoruz
    {
        if (direction == Vector2.left)  // eðer sola gidiyorsa
        {
            transform.localScale = new Vector3(-1, 1, 1);  
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        else if (direction == Vector2.right)    // eðer saða gidiyorsa
        {
            transform.localScale = new Vector3(1, 1, 1);
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        else if (direction == Vector2.up)   // eðer yukarý gidiyorsa
        {
            transform.localScale = new Vector3(1, 1, 1);
            transform.localRotation = Quaternion.Euler(0, 0, 90);
        }
        else if (direction == Vector2.down)  // eðer aþaðý gidiyorsa
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
        Node moveToNode = null; // default olarak gidebileceðimiz node'u boþþ olarak iþaretliyoruz

        for (int i = 0; i < currentNode.neighbors.Length; i++)
        {
            if (currentNode.validDirections[i] == directionCanMove) // for döngüsünden gelen index i kontrol ediyoruz bizim yönümüzle doðrumu diye
            {
                moveToNode = currentNode.neighbors[i]; // eðer doðru ve orda bir node varsa belirtiyoruz hareket edilebilir
                break; // ve for döngüsünü durduruyoruz
            }
        }

        return moveToNode; // eðer döngüden sonuç çýkarsa yönümüz çýkan sonuç olur fakat sonuç çýkmazsa boþ döndürür
    }

    

    Node GetNodeAtPosition(Vector2 position)  // düðümün pozisyonunu alma methodu
    {
        GameObject tile = GameObject.Find("Game").GetComponent<GameBoard>().board[(int)position.x,(int)position.y]; // gameboard içersinde depolanan önümüzdeki node'ýn konumunu istiyoruz

        if (tile != null)  // eðer önümüzdeki node gameboard da ise ve konumu depolanmýþsa boþ deðildir ve eðer boþ deðilse
        {
            return tile.GetComponent<Node>(); // bu objenin konumunu gönderiyoruz.
        }

        return null; // eðer karþýmýzdaki node gameboard da bir karþýlýðý yoksa boþ döndürüyoruz
    }

    bool OverShotTarget() 
    {
        float nodeToTarget = LengthFromNode(targetNode.transform.position); // ilerlediðimiz deðil ilereyelebileceðimiz node mesafesi
        float nodeToSelf = LengthFromNode(transform.localPosition);  // ilerlediðimiz node mesafesi
        
        return nodeToSelf > nodeToTarget; // eðer ilerlediðimiz node mesafesi gidebileceðimizden büyük hata var demektir bu nedenle true döner
    }

    float LengthFromNode(Vector2 targetPosition) // hedefteki nodela bulunduðumuz node arasýndaki mesafeyi buluyor. negatif çýkma ihtimaline karþýn karesini alýyoruz
    {
        Vector2 vector = targetPosition - (Vector2) previousNode.transform.position; // bulunduðumuz pozisyondan düðüm pozisyonunu çýkartýyoruz ortaya 
        return vector.sqrMagnitude;  
    }

    GameObject GetPortal(Vector2 position)  //portalin konumunu öðrenme
    {
        GameObject tile = GameObject.Find("Game").GetComponent<GameBoard>().board[(int)position.x,(int)position.y];  // tile objesinde portalýn gameboardda depolanan konumunu alma

        if (tile != null) // tileýn boþ olup olmadýný yani o konumda varmý yokmu kontrol ediyoruz varsa devam ediyoruz
        {
            if (tile.GetComponent<Tile>() != null) // tile'ýn içinde tile scrpiti varmý kontrol ediyoruz varsa devam ediyoruz
            {
                if (tile.GetComponent<Tile>().isPortal)  // tilein portal olup olmadýðýný kontrol ediyoruz
                {
                    GameObject otherPortal = tile.GetComponent<Tile>().portalReceiver; // eðer portalsa diðer portalýn hangisi olduðunu öðreniyoruz
                    return otherPortal; // ve o portalý veriyoruz
                }
            }
        }

        return null; // fakat sonuç portal deðilse boþ döndürüyoruz
    }

}
