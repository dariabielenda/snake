using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Snake
{
    class MySnake
    {
        public SnakePart Head { get; private set; }
        public List<SnakePart> Parts { get; private set; }

        public MySnake() // konstruktor - glowa
        {
            Head = new SnakePart(20, 0);
            Head.Rect.Width = Head.Rect.Height = 20;
            Head.Rect.Fill = System.Windows.Media.Brushes.Pink;
            Parts = new List<SnakePart>();

            Parts.Add(new SnakePart(19, 0)); // tworzenie kolejnych czesci snake
            Parts.Add(new SnakePart(18, 0));
            Parts.Add(new SnakePart(17, 0));
            Parts.Add(new SnakePart(16, 0));
            Parts.Add(new SnakePart(15, 0));
            Parts.Add(new SnakePart(14, 0));
            Parts.Add(new SnakePart(13, 0));
            Parts.Add(new SnakePart(12, 0));
            Parts.Add(new SnakePart(11, 0));
            Parts.Add(new SnakePart(10, 0));
       
        }

        public void RedrawSnake() //przerysowanie snake, każda kolejna część wstawi w odpowiednią komórkę planszy zgodnie ze współrzędnymi 
        {
            Grid.SetColumn(Head.Rect, Head.X); // grid to wlasciwa plansza (okno window) - ustawia wartość właściwości przyłączonej do kolumny na konkretny element
            Grid.SetRow(Head.Rect, Head.Y);
            foreach (SnakePart snakePart in Parts)
            {
                Grid.SetColumn(snakePart.Rect, snakePart.X);
                Grid.SetRow(snakePart.Rect, snakePart.Y);
            }
        }


    }
}
