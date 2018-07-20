using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Snake
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private MySnake _snake; // referencja naszego weza
        private static readonly int SIZE = 10; // rozmiar pojedynczej komórki naszej planszy w stałej
        private int _directionX = 1;
        private int _directionY = 0;
        private DispatcherTimer _timer;
        private SnakePart _food;
        private int _partsToAdd;
        private List<Wall> _walls;

        public MainWindow()
        {
            InitializeComponent(); // wew konstruktora MainWindow() za wywołaniem metody InitializeComponent() dodajemy wywołanie metod inicjujących
            InitBoard();
            InitSnake();
            InitTimer(); // wywolanie metody
            InitFood();
            InitWall();

        }


        void InitBoard() // metoda inicjująca planszę
        {
            for (int i = 0; i < grid.Width / SIZE; i++)
            {
                ColumnDefinition columnDefinitions = new ColumnDefinition();
                columnDefinitions.Width = new GridLength(SIZE);
                grid.ColumnDefinitions.Add(columnDefinitions);
            }
            for (int j = 0; j < grid.Height / SIZE; j++)
            {
                RowDefinition rowDefinition = new RowDefinition();
                rowDefinition.Height = new GridLength(SIZE);
                grid.RowDefinitions.Add(rowDefinition);
            }
            _snake = new MySnake();
        }
        void InitSnake() // metoda inicjująca węża
        {
            grid.Children.Add(_snake.Head.Rect);
            foreach (SnakePart snakePart in _snake.Parts)
                grid.Children.Add(snakePart.Rect);
            _snake.RedrawSnake();

        }

        private void MoveSnake() // wąż będzie przesuwał się w zadanym kierunku, przesuwamy każdą poszczególną część o jedno pole
        {
            int snakePartCount = _snake.Parts.Count; // zapisujemy  do zmiennej snakePartCount ile wąż ma części
            if (_partsToAdd > 0) // w każdym kroku dodajemy jedną część jeśli wartość zmiennej jest większa od zera, po dodaniu jednej części wartość zmiennej zmniejszymy o jeden, aby wąż nie rósł w nieskończoność
            {
                SnakePart newPart = new SnakePart(_snake.Parts[_snake.Parts.Count -1].X, // po utworzeniu nowej części newPart i dodaniu jej na plansze przesuwamy wyłącznie części jakie wąż miał wcześniej
                _snake.Parts[_snake.Parts.Count - 1].Y);
                grid.Children.Add(newPart.Rect);
                _snake.Parts.Add(newPart);
                _partsToAdd--;
            }
            for (int i = snakePartCount - 1; i >= 1;  i-- ) 
                {
                _snake.Parts[i].X = _snake.Parts[i - 1].X; // przesuwamy na miejsce części poprzedzającej
                _snake.Parts[i].Y = _snake.Parts[i - 1].Y;
            }
            _snake.Parts[0].X = _snake.Head.X; // głowa przesuwa się zgodnie z zadanym kierunkiem zapisanym w zmiennych _directionX oraz _directionY
            _snake.Parts[0].Y = _snake.Head.Y;
            _snake.Head.X += _directionX;
            _snake.Head.Y += _directionY;
            if (CheckCollision())
                EndGame();
            else
            {
                if (CheckFood())
                    RedrawFood();
                _snake.RedrawSnake();
            }

        }
        
        void InitTimer()
        {
            _timer = new DispatcherTimer();
            _timer.Tick += new EventHandler(_timer_Tick);
            _timer.Interval = new TimeSpan(0, 0, 0, 0, 80); // 100ms
            _timer.Start();
        }

        void _timer_Tick(object sender, EventArgs e)
        {
            MoveSnake();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left)
            {
                _directionX = -1;
                _directionY = 0;
            }

            if (e.Key == Key.Right)
            {
                _directionX = 1;
                _directionY = 0;
            }

            if (e.Key == Key.Up)
            {
                _directionX = 0;
                _directionY = -1;
            }

            if (e.Key == Key.Down)
            {
                _directionX = 0;
                _directionY = 1;
            }
        }
        void InitFood() // jedzonko
        {
            _food = new SnakePart(10, 10); // pierwszy pokarm w komórce o współrzędnych x=10 oraz y=10
            _food.Rect.Width = _food.Rect.Height = 10; //   kwadrat o boku 10
            _food.Rect.Fill = Brushes.LightPink; // niebieski
            grid.Children.Add(_food.Rect);
            Grid.SetColumn(_food.Rect, _food.X);
            Grid.SetRow(_food.Rect, _food.Y);
        }

        private bool CheckFood() // zjadanie 
        {
            Random rand = new Random();
            if (_snake.Head.X == _food.X && _snake.Head.Y == _food.Y) // jeśli głowa węża w danym kroku najechała na pożywienie, należy powiększyć snake i znaleźć inne miejsce na nowe pożywienie
            {
                _partsToAdd += 1; 
                for (int i = 0; i < 20; i++) // Losowanie odbywa się poprzez wywołanie metody Next() na obiekcie Typu Random i jest powtarzane 20 razy
                {
                    int x = rand.Next(0, (int)(grid.Width / SIZE));
                    int y = rand.Next(0, (int)(grid.Height / SIZE));
                    if (IsFieldFree(x, y)) //  sprawdzamy czy wylosowane pole jest wolne; tak = konczymy dzialanie, nie = losujemy dalej
                    {
                        _food.X = x;
                        _food.Y = y;
                        return true; // czy najechano na pożywienie i jeśli tak to zwraca wartość true 
                    }
                }
                for (int i = 0; i < grid.Width / SIZE; i++) // Losowanie powtarzamy 20 razy (na tyle jest ustawiony licznik pętli for), a jeśli nie przyniesie poprawnego wyniku to szukamy pierwszego wolnego miejsca w którym można umieścić pożywienie i je tam wstawiamy
                    for (int j = 0; j < grid.Height / SIZE; j++)
                    {
                        if (IsFieldFree(i, j))
                        {
                            _food.X = i;
                            _food.Y = j;
                            return true; 
                        }
                    }
                EndGame(); // jeśli nie ma żadnych wolnych miejsc na pozywienie np. wąż za dlugi to the end
            }
            return false;
        }

        private bool IsFieldFree(int x, int y)
        {
            if (_snake.Head.X == x && _snake.Head.Y == y)
                return false;
            foreach (SnakePart snakePart in _snake.Parts)
            {
                if (snakePart.X == x && snakePart.Y == y)
                    return false;
            }
            return true;
        }

        void EndGame()
        {
            _timer.Stop();

            MessageBox.Show("KONIEC GRY");
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }

        private void RedrawFood() // przerysowanie na nowej pozycji
        {
            Grid.SetColumn(_food.Rect, _food.X);
            Grid.SetRow(_food.Rect, _food.Y);
        }

        bool CheckCollision()
        {
            if (CheckBoardCollision())
                return true;
            if (CheckItselfCollision())
                return true;
            if (CheckWallCollision())
                return true;

            return false;
        }

        bool CheckBoardCollision()
        {
            if (_snake.Head.X < 0 || _snake.Head.X > grid.Width / SIZE)
                return true;
            if (_snake.Head.Y < 0 || _snake.Head.Y > grid.Height / SIZE)
                return true;
            return false;
        }

        bool CheckItselfCollision()
        {
            foreach (SnakePart snakePart in _snake.Parts)
            {
                if (_snake.Head.X == snakePart.X && _snake.Head.Y == snakePart.Y)
                    return true;
            }
            return false;
        }

        void InitWall() // 3 ściany umieszczone po środku planszy w równych odstępach
        {
            _walls = new List<Wall>();
            Wall wall1 = new Wall(19, 15, 3, 30);
            grid.Children.Add(wall1.Rect);
            Grid.SetColumn(wall1.Rect, wall1.X);
            Grid.SetRow(wall1.Rect, wall1.Y);
            Grid.SetColumnSpan(wall1.Rect, wall1.Width);
            Grid.SetRowSpan(wall1.Rect, wall1.Height);
            _walls.Add(wall1);

            Wall wall2 = new Wall(39, 15, 3, 30);
            grid.Children.Add(wall2.Rect);
            Grid.SetColumn(wall2.Rect, wall2.X);
            Grid.SetRow(wall2.Rect, wall2.Y);
            Grid.SetColumnSpan(wall2.Rect, wall2.Width);
            Grid.SetRowSpan(wall2.Rect, wall2.Height);
            _walls.Add(wall2);

            Wall wall3 = new Wall(59, 15, 3, 30);
            grid.Children.Add(wall3.Rect);
            Grid.SetColumn(wall3.Rect, wall3.X);
            Grid.SetRow(wall3.Rect, wall3.Y);
            Grid.SetColumnSpan(wall3.Rect, wall3.Width);
            Grid.SetRowSpan(wall3.Rect, wall3.Height);
            _walls.Add(wall3);
        }

        bool CheckWallCollision() // sprawdzi, czy głowa węża nie weszła w konflikt z przeszkodą
        {
            foreach (Wall wall in _walls)
            {
                if (_snake.Head.X >= wall.X && _snake.Head.X < wall.X + wall.Width &&
                _snake.Head.Y >= wall.Y && _snake.Head.Y < wall.Y + wall.Height)
                    return true;
            }
            return false;
        }


    }
}
