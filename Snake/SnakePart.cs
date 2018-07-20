using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Snake
{
    class SnakePart
    {
        public int X { get; set; } // get w chwili gdy chcemy pobrać wartość właściwości, set w chwili nadania wartości właściwości
        public int Y { get; set; }
        public Rectangle Rect { get; private set; } // graficznie

        public SnakePart(int x, int y) // konstruktor = część wężą 9/9, czarny prostokat
        {
            X = x;
            Y = y;
            Rect = new Rectangle();
            Rect.Width = Rect.Height = 9;
            Rect.Fill = Brushes.Black;
    
        }
    }
}
