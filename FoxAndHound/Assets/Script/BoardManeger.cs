using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManeger : MonoBehaviour
{
  public Piece[,] Pieces{set;get;}
  private Piece selectedPiece;
  private const float TILE_SIZE= 1.0f;
  private const float TILE_OFFSET= 0.5f;
  private int selectionX = -1;
  private int selectionY = -1;

  public List<GameObject> piecePrefabs;
  private List<GameObject> activePiece = new List<GameObject>();
  private Quaternion orientation = Quaternion.Euler(-90,180,0);

  public bool isFoxTurn = true;

    private void Start()
    {
        Pieces = new Piece[8,8];
        SpawnPiece(0, 3,0);
        SpawnPiece(1, 0,7 );
        SpawnPiece(2, 2,7 );
        SpawnPiece(3, 4,7 );
        SpawnPiece(4, 6,7 );
    }

//Funcion que se ejecuta en cada Frame del juego
    private void Update()
    {
        DrawBoard();
        UpdateSelection();
        if(Input.GetMouseButtonDown (0))
        {
            if(selectionX >= 0 && selectionY >= 0)
            {
                if(selectedPiece == null)
                {
                    //seleccinoar la pieza
                    SelectPiece(selectionX,selectionY);
                }
                else
                {
                    //mover la pieza
                    MovePiece(selectionX,selectionY);
                }
            }
        }
        
    }

    private void SelectPiece(int x, int y)
    {
        if(Pieces[x,y] == null)
            return;
        if(Pieces[x,y].isFox != isFoxTurn)
            return;
        selectedPiece = Pieces[x,y];


    }
    private void MovePiece(int x, int y)
    {
        if(selectedPiece.PossibleMove(x,y))
        {
            Pieces[selectedPiece.CurrentX,selectedPiece.CurrentY] = null;
            selectedPiece.transform.position = GetTileCenter(x,y);
            Pieces[x,y]=selectedPiece;

        }
        
        selectedPiece = null;

    }

//Funcion que muestra la ubicacion en el tablero donde el mouse esta apuntando
    private void UpdateSelection()
    {
        if(!Camera.main)
            return;
        
        RaycastHit hit;
        if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition),out hit, 25.0f,LayerMask.GetMask ("ChessPlane")))
        {
            selectionX = (int)hit.point.x;
            selectionY = (int)hit.point.z;

        }
        else
        {
            selectionX = -1;
            selectionY = -1;
        }

    }

//Funcion que Dibuja el tablero
    private void DrawBoard()
    {
        Vector3 widthLine = Vector3.right * 8;
        Vector3 heigthLine = Vector3.forward * 8;
        
        for(int i = 0; i<=8; i++)
        {
            Vector3 start = Vector3.forward * i;
            Debug.DrawLine (start,start + widthLine); 
            for(int j = 0; j<=8; j++)
            {
                start = Vector3.right * j;
                Debug.DrawLine (start,start + heigthLine);


            }
        }
        //Draw the selection
        if(selectionX >= 0 && selectionY >= 0)
        {
            Debug.DrawLine(
                Vector3.forward * selectionY +Vector3.right*selectionX,
                Vector3.forward *(selectionY +1)+Vector3.right*(selectionX+1));
            
             Debug.DrawLine(
                Vector3.forward * (selectionY + 1) +Vector3.right*selectionX,
                Vector3.forward *selectionY +Vector3.right*(selectionX+1));
        }

    }

    private void SpawnPiece(int index,int x, int y)
    {   
        GameObject go = Instantiate (piecePrefabs [index], GetTileCenter(x,y), orientation) as GameObject;
        go.transform.SetParent(transform);
        Pieces[x,y]=go.GetComponent<Piece>();
        Pieces [x,y].SetPosition(x,y);
        activePiece.Add (go);

    }

    private Vector3 GetTileCenter(int x,int y)
    {
        Vector3 origin = Vector3.zero;
        origin.x += (TILE_SIZE * x) +TILE_OFFSET;
        origin.z += (TILE_SIZE * y) +TILE_OFFSET;
        return origin;
    }

}
