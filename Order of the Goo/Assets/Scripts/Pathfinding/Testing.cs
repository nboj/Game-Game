using System;
using UnityEngine;
using TMPro;

namespace Pathfinding {
    public class Testing : MonoBehaviour {
        [SerializeField] 
        private Vector2 gridSize;
        [SerializeField]
        private float cellSize;
        

        private void Start() {
            PF_Grid grid = new PF_Grid((int)gridSize.x, (int)gridSize.y, cellSize);
            CreateText(grid);
        }

        private void CreateText(PF_Grid grid) {
            int[,] gridArray = grid.GridArray;
            Canvas canvas = gameObject.AddComponent<Canvas>();
            RectTransform rectTransform = GetComponent<RectTransform>(); 
            rectTransform.sizeDelta = gridSize;
            canvas.renderMode = RenderMode.WorldSpace;
            canvas.sortingLayerName = "Collider";
            for (int y = 0; y < gridArray.GetLength(1); y++) {
                for (int x = 0; x < gridArray.GetLength(0); x++) {
                    GameObject child = new GameObject(); 
                    child.name = x + " " + y;
                    child.transform.parent = transform;
                    TextMeshProUGUI text = child.AddComponent<TextMeshProUGUI>();
                    RectTransform childRect = child.GetComponent<RectTransform>();
                    childRect.sizeDelta = new Vector2(1, 1) * grid.CellSize;
                    text.horizontalAlignment = HorizontalAlignmentOptions.Center;
                    text.verticalAlignment = VerticalAlignmentOptions.Geometry;
                    text.enableAutoSizing = true;
                    text.fontSizeMin = 0f;
                    text.transform.localPosition = new Vector3(x, y, 0) * grid.CellSize;
                    RaycastHit2D hit = Physics2D.BoxCast(childRect.position, Vector2.one * grid.CellSize, 0f, Vector2.down, 1f, LayerMask.GetMask(("Collider")));
                    if (hit.collider != null) {
                        text.text = "1";
                    } else {
                        text.text = "0";
                    }
                }
            }
        }
    }
}