using Match3.SO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Unity.Mathematics;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;


namespace Match3.Model
{
    public class LogicalGrid
    {
        public event Action<List<Tile>> OnColumnShifted;

        public int GridWidth { get; private set; }
        public int GridHeight { get; private set; }
        public FruitDataSO[] Fruits;
        public Tile[,] Tiles;

        public void InitializeGrid(int gridWidth, int gridHeight, FruitDataSO[] fruits)
        {
            GridWidth = gridWidth;
            GridHeight = gridHeight;
            Fruits = fruits;
            Tiles = new Tile[GridWidth, GridHeight];

            for(int x = 0; x < GridWidth; x++)
            {
                for(int y = 0; y < GridHeight; y++)
                {
                    FruitDataSO randomFruit = Fruits[UnityEngine.Random.Range(0, Fruits.Length)];
                    CreateTile(x, y, false, randomFruit);
                }
            }

            GetPossibleSelections();
        }

        private void CreateTile(int x, int y, bool isSelected, FruitDataSO fruit)
        {
            Tiles[x, y] = new Tile(x, y, isSelected, fruit);
        }

        public Tile GetTile(int x, int y)
        {
            if (Tiles[x,y] != null)
            {
                return Tiles[x, y];
            }
            else
            {
                return null;
            }
        }

        public List<Tile> GetNeighbors(Tile tile)
        {
            int[,] directions = new int[,]
            {
                {-1,-1 },
                {-1, 0 },
                {-1, 1 },
                { 0,-1 },
                { 0, 1 },
                { 1,-1 },
                { 1, 0 },
                { 1, 1 }
            };

            List<Tile> neighbors = new List<Tile>();

            for(int i = 0; i < directions.GetLength(0); i++)
            {
                int newX = tile.PositionX + directions[i, 0];
                int newY = tile.PositionY + directions[i, 1];

                if (newX >= 0 && newY >= 0 && newX < GridWidth && newY < GridHeight)
                {
                    Tile neighbor = GetTile(newX, newY);
                    if (neighbor != null && neighbor.Fruit == tile.Fruit)
                    {
                        neighbors.Add(neighbor);
                    }
                }
            }

            return neighbors;
        }

        public bool IsNeighbor(Tile tile1, Tile tile2)
        {
            int[,] directions = new int[,]
            {
                {-1,-1 },
                {-1, 0 },
                {-1, 1 },
                { 0,-1 },
                { 0, 1 },
                { 1,-1 },
                { 1, 0 },
                { 1, 1 }
            };

            for (int i = 0; i < directions.GetLength(0); i++)
            {
                int newX = tile1.PositionX + directions[i, 0];
                int newY = tile1.PositionY + directions[i, 1];

                if (newX == tile2.PositionX && newY == tile2.PositionY)
                {
                    return tile1.Fruit == tile2.Fruit;
                }
            }
            return false;
        }


        public List<List<Tile>> GetPossibleSelections()
        {
            List<List<Tile>> possibleSelections = new List<List<Tile>>();

            for (int x = 0; x < GridWidth; x++)
            {
                for (int y = 0; y < GridHeight; y++)
                {
                    List<Tile> possibleSelection = new List<Tile>();
                    Tile tile = Tiles[x, y];
                    List<Tile> neighbors = GetNeighbors(tile);

                    if(possibleSelections.Count > 0)
                    {
                        bool isNeighbor = false;
                        foreach (List<Tile> selection in possibleSelections)
                        {
                            foreach (Tile tileInSelection in selection)
                            {
                                if (neighbors.Contains(tileInSelection))
                                {
                                    isNeighbor = true;
                                    selection.Add(tile);
                                    break;
                                }     
                            }
                            if (isNeighbor)
                            {
                                break;
                            }
                        }

                        if (!isNeighbor)
                        {
                            possibleSelection.Add(tile);
                            possibleSelections.Add(possibleSelection);
                        }

                    }
                    else
                    {
                        possibleSelection.Add(tile);
                        possibleSelections.Add(possibleSelection);
                    }
                }
            }


            for (int i = 0; i < possibleSelections.Count; i++)
            {
                for (int j = i + 1; j < possibleSelections.Count; j++)
                {
                    bool merged = false;

                    for (int k = 0; k < possibleSelections[i].Count; k++)
                    {
                        for (int l = 0; l < possibleSelections[j].Count; l++)
                        {
                            if (IsNeighbor(possibleSelections[i][k], possibleSelections[j][l]))
                            {
                                possibleSelections[i].AddRange(possibleSelections[j]);
                                possibleSelections.RemoveAt(j);
                                merged = true;
                                break;
                            }
                        }

                        if (merged)
                            break;
                    }
                    if (merged)
                    {
                        j--;
                    }
                }
            }

            for (int i = possibleSelections.Count - 1; i >= 0; i--)
            {
                if (possibleSelections[i].Count < 3)
                {
                    possibleSelections.RemoveAt(i);
                }
            }


            possibleSelections = possibleSelections.OrderByDescending(s => s.Count).ToList();

            int tileCount = 0;
            foreach (List<Tile> selection in possibleSelections)
            {
                foreach (Tile tileInSelection in selection)
                {
                    Debug.Log(tileInSelection.Fruit.name + " " + tileInSelection.PositionX + " " + tileInSelection.PositionY);
                    tileCount++;
                }

                Debug.Log("---------------------------------------------");
            }
            Debug.Log(tileCount);

            return possibleSelections;
        }

        public List<Tile> GetSelectedTiles()
        {
            List<Tile> selectedTiles = new List<Tile>();
            for (int x = 0; x < GridWidth; x++)
            {
                for (int y = 0; y < GridHeight; y++)
                {
                    if (Tiles[x, y].IsSelected)
                    {
                        selectedTiles.Add(Tiles[x, y]);
                    }
                }
            }
            return selectedTiles;
        }

        public void ShiftSelectedTilesUp(FruitDataSO[] fruits)
        {
            for (int x = 0; x < GridWidth; x++)
            {
                List<Tile> columnTiles = new List<Tile>();
                for (int y = 0; y < GridHeight; y++)
                {
                    columnTiles.Add(Tiles[x, y]);
                }
                columnTiles.Sort((a, b) => a.IsSelected.CompareTo(b.IsSelected));
                
                for (int y = 0; y < GridHeight; y++)
                {
                    Tiles[x, y] = columnTiles[y];
                    if (Tiles[x,y].IsSelected)
                    {
                        Tiles[x, y].SetFruit(fruits[UnityEngine.Random.Range(0, fruits.Length)]);
                    }
                }
                OnColumnShifted?.Invoke(columnTiles);
                for (int y = 0; y < GridHeight; y++)
                {
                    Tiles[x, y].SetPosition(x, y);
                }


            }
            GetPossibleSelections();
        }
    }
}