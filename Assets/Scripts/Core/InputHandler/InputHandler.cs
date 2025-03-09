using UnityEngine;

namespace Match3.InputHandler
{
    public class InputHandler : MonoBehaviour
    {

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
                    // add first selection and define current type of fruit
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
                    // continue adding selections, if added dont add again, check if the same type of fruit,
                    // check if the adjacent of the selected fruits (on 8 direction)
                }
            }
        }
        private void OnSelectionEnd()
        {
            if (Input.GetMouseButtonUp(0))
            {
                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);
                if (hit.collider != null && hit.collider.CompareTag("TileView"))
                {
                    // end the selection, if selected items in the list is 3 or more, add it to another list,
                    // make them selected to make sure they are not selectable again
                }
            }
        }

        

        
    }
}