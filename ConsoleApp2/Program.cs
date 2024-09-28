using System;
using System.Collections.Generic;

class Piece
{
    public int Width { get; set; }
    public int Height { get; set; }
    public int Quantity { get; set; }
    public int Placed { get; set; } = 0; 

    public Piece(int width, int height, int quantity)
    {
        Width = width;
        Height = height;
        Quantity = quantity;
    }
}

class Program
{
    const int BoardWidth = 1200;
    const int MaxHeight = 3000;  
    static bool[,] board = new bool[MaxHeight, BoardWidth]; 

    static void Main()
    {
        try {
            Console.Write("Введите количество типов деталей: ");
            int numTypes = int.Parse(Console.ReadLine());
            List<Piece> pieces = new List<Piece>();
            List<Piece> pieces2 = new List<Piece>();

            for (int i = 0; i < numTypes; i++)
            {
                Console.Write("Введите ширину детали: ");
                int width = int.Parse(Console.ReadLine());
                Console.Write("Введите высоту детали: ");
                int height = int.Parse(Console.ReadLine());
                Console.Write("Введите количество деталей данного типа: ");
                int quantity = int.Parse(Console.ReadLine());

                pieces.Add(new Piece(width, height, quantity));
                pieces2.Add(new Piece(width, height, quantity));
            }
            PlacePieces(pieces, BoardWidth);
            Console.WriteLine("вариант2");
            Array.Clear(board, 0, board.Length);
            pieces2.Sort((piece1, piece2) => piece1.Width.CompareTo(piece2.Width));
            PlacePieces(pieces2, BoardWidth);
        }
        catch(Exception ex) { Console.WriteLine(ex); }
        }

    static void PlacePieces(List<Piece> pieces, int boardWidth)
    {
        List<(int width, int height, int x, int y)> coordinates = new List<(int, int, int, int)>();
        int finalHeight = 0; 

        for (int y = 0; y < MaxHeight; y++)  
        {
            for (int x = 0; x < boardWidth; x++)  
            {
                foreach (var piece in pieces)
                {
                    if (piece.Placed < piece.Quantity && CanPlacePiece(piece, x, y))
                    {
                        
                        for (int prevX = 0; prevX < boardWidth; prevX++)
                        {
                            if (y > 0 && board[y - 1, prevX] && prevX + piece.Width <= boardWidth)
                            {
                                foreach (var prevPiece in pieces)
                                {
                                    if (prevPiece.Placed > 0 && prevPiece.Width != 0 &&
                                       
                                       ((prevPiece.Width / piece.Height == 2 || prevPiece.Width / piece.Height == 3)))
                                    {
                                        
                                        int temp = piece.Width;
                                        piece.Width = piece.Height;
                                        piece.Height = temp;
                                        break;
                                    }
                                }
                            }
                        }

                        // Устанавливаем деталь
                        PlacePiece(piece, x, y);
                        coordinates.Add((piece.Width, piece.Height, x, y));
                        piece.Placed++;
                        finalHeight = Math.Max(finalHeight, y + piece.Height); 
                        x += piece.Width - 1;  
                        break;
                    }
                }
            }
        }

        Console.WriteLine($"Длина плиты: {finalHeight}");
        foreach (var coord in coordinates)
        {
            Console.WriteLine($"Деталь: {coord.width}x{coord.height} на позиции: ({coord.x}, {coord.y})");
        }

        if (finalHeight > 3000)
        {
            Console.WriteLine("Ошибка: Длина плиты превышает 3000!");
        }
    }

    static bool CanPlacePiece(Piece piece, int x, int y)
    {
        
        if (x + piece.Width > BoardWidth || y + piece.Height > MaxHeight) return false;

        
        for (int i = 0; i < piece.Height; i++)
        {
            for (int j = 0; j < piece.Width; j++)
            {
                if (board[y + i, x + j]) return false;
            }
        }
        return true;
    }

    static void PlacePiece(Piece piece, int x, int y)
    {
       
        for (int i = 0; i < piece.Height; i++)
        {
            for (int j = 0; j < piece.Width; j++)
            {
                board[y + i, x + j] = true;
            }
        }
    }

}