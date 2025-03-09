using Match3.Model;
using Match3.Presenter;
using Match3.SO;
using Match3.View;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Match3.InputHandler
{
    public class InputHandler : MonoBehaviour
    {

        [SerializeField] private GridPresenter gridPresenter;

        private FruitDataSO currentFruitType;
        [SerializeField] private List<TileView> currentTileViews= new List<TileView>();
        [SerializeField] private List<TileView> selectedTileViews = new List<TileView>();

        private void Update()
        {
            OnSelectionStart();
            OnSelectionContinue();
            OnSelectionEnd();
        }
        private void OnSelectionStart()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);
                if(hit.collider != null && hit.collider.CompareTag("TileView"))
                {
                    
                    TileView tileView = hit.collider.GetComponent<TileView>();
                    Tile tile = gridPresenter.GetTileAt(tileView.GridPositionX, tileView.GridPositionY);
                    if (!tile.IsSelected) 
                    {
                        currentFruitType = tile.Fruit;
                        currentTileViews.Add(tileView);
                    }
                }
            }
        }
        private void OnSelectionContinue()
        {
            if (Input.GetMouseButton(0))
            {
                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);
                if (hit.collider != null && hit.collider.CompareTag("TileView"))
                {
                    
                    // check if the adjacent of the selected fruits (on 8 direction)
                    TileView tileView = hit.collider.GetComponent<TileView>();
                    Tile tile = gridPresenter.GetTileAt(tileView.GridPositionX, tileView.GridPositionY);
                    List<Tile> tileNeighbors = gridPresenter.GetNeighborsAt(tile);
                    Debug.Log(tileNeighbors.Count);
                    foreach(Tile tileNeighbor in tileNeighbors)
                    {
                        if (!tile.IsSelected && currentTileViews.Contains(gridPresenter.GetTileViewAt(tileNeighbor.PositionX,tileNeighbor.PositionY)))
                        {
                            if (tile.Fruit == currentFruitType)
                            {
                                if (!currentTileViews.Contains(tileView))
                                {
                                    currentTileViews.Add(tileView);
                                    break;
                                }
                            }
                            
                        }
                    }
                    
                    
                }
            }
        }
        private void OnSelectionEnd()
        {
            if (Input.GetMouseButtonUp(0))
            {
                // end the selection, if selected items in the list is 3 or more, add it to another list,
                // make them selected to make sure they are not selectable again
                if(currentTileViews.Count >= 3)
                {
                    foreach(TileView tileView in currentTileViews)
                    {
                        gridPresenter.GetTileAt(tileView.GridPositionX, tileView.GridPositionY).SetSelected(true);
                    }
                    selectedTileViews.AddRange(currentTileViews);
                    currentTileViews.Clear();
                }
                else
                {
                    currentTileViews.Clear();
                }
            }
        }

        

        
    }
}