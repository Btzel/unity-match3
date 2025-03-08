using Match3.SO;

namespace Match3.Model
{
    public class Tile
    {
        public int PositionX { get; private set; }
        public int PositionY { get; private set; }
        public bool IsSelected { get; private set; }
        public FruitDataSO Fruit { get; private set; }

        public Tile(int positionX, int positionY, bool isSelected, FruitDataSO fruit)
        {
            PositionX = positionX;
            PositionY = positionY;
            IsSelected = isSelected;
            Fruit = fruit;
        }

        public void SetPosition(int positionX, int positionY)
        {
            PositionX = positionX;
            PositionY = positionY;
        }

        public void SetSelected(bool isSelected)
        {
            IsSelected = isSelected;
        }

        public void SetFruit(FruitDataSO fruit)
        {
            Fruit = fruit;
        }
    }
}
