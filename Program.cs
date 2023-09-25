using System;

Random random = new Random();
Console.CursorVisible = false;
int height = Console.WindowHeight - 1;
int width = Console.WindowWidth - 5;
bool shouldExit = false;

// Console position of the player
int playerX = 0;
int playerY = 0;

// Console position of the food
int foodX = 0;
int foodY = 0;


// Available player and food strings
string[] states = { "('-')", "(^-^)", "(X_X)" };
string[] foods = { "@@@@@", "$$$$$", "#####" };

// Current player string displayed in the Console
string player = states[0];

// Index of the current food
int food = 0;

bool CollisionDetector()
{
    int[] playerOccupiedSpacesX = { playerX, playerX + 1, playerX + 2, playerX + 3, playerX + 4 };
    int[] foodOccupiedSpacesX = { foodX, foodX + 1, foodX + 2, foodX + 3, foodX + 4 };
    if (foodY == playerY)
    {
        return foodOccupiedSpacesX.Intersect(playerOccupiedSpacesX).Count() == 5;
    }
    return false;
}

InitializeGame();
while (!shouldExit)
{
    Move(true);
}

// Returns true if the Terminal was resized 
bool TerminalResized()
{
    return height != Console.WindowHeight - 1 || width != Console.WindowWidth - 5;
}

// Displays random food at a random location
void ShowFood()
{
    // Update food to a random index
    food = random.Next(0, foods.Length);

    // Update food position to a random location
    foodX = random.Next(0, width - player.Length);
    foodY = random.Next(0, height - 1);

    // Display the food at the location
    Console.SetCursorPosition(foodX, foodY);
    Console.Write(foods[food]);
}

// Changes the player to match the food consumed
void ChangePlayer()
{
    player = states[food];
    Console.SetCursorPosition(playerX, playerY);
    Console.Write(player);
}

bool IsSlowedByPoison() => player == states[2];
bool XMovementIsIncreased() => player == states[1];

// Temporarily stops the player from moving
void FreezePlayer()
{
    System.Threading.Thread.Sleep(1000);
    player = states[0];
}

// Reads directional input from the Console and moves the player
void Move(bool anyKeyToExit = false, bool speedOption = false)
{
    int lastX = playerX;
    int lastY = playerY;
    int xMoveSpeed = 1;

    if (speedOption && XMovementIsIncreased())
        xMoveSpeed = 4;

    if (TerminalResized())
    {
        Console.Clear();
        Console.WriteLine("\n\nConsole was resized. Program exiting.\n\n");
        System.Threading.Thread.Sleep(3000);
        shouldExit = true;
        return;
    }

    if (IsSlowedByPoison())
        FreezePlayer();

    if (anyKeyToExit)
    {
        switch (Console.ReadKey(true).Key)
        {
            case ConsoleKey.UpArrow:
                playerY--;
                break;
            case ConsoleKey.DownArrow:
                playerY++;
                break;
            case ConsoleKey.LeftArrow:
                playerX -= xMoveSpeed;
                break;
            case ConsoleKey.RightArrow:
                playerX += xMoveSpeed;
                break;
            default:
                shouldExit = true;
                break;
        }
    }
    else
    {
        switch (Console.ReadKey(true).Key)
        {
            case ConsoleKey.UpArrow:
                playerY--;
                break;
            case ConsoleKey.DownArrow:
                playerY++;
                break;
            case ConsoleKey.LeftArrow:
                playerX -= xMoveSpeed;
                break;
            case ConsoleKey.RightArrow:
                playerX += xMoveSpeed;
                break;
            case ConsoleKey.Escape:
                shouldExit = true;
                break;
        }
    }

    // Clear the characters at the previous position
    Console.SetCursorPosition(lastX, lastY);
    for (int i = 0; i < player.Length; i++)
    {
        Console.Write(" ");
    }

    // Keep player position within the bounds of the Terminal window
    playerX = (playerX < 0) ? 0 : (playerX >= width ? width : playerX);
    playerY = (playerY < 0) ? 0 : (playerY >= height ? height : playerY);

    // Draw the player at the new location
    Console.SetCursorPosition(playerX, playerY);
    if (CollisionDetector())
    {
        ChangePlayer();
        ShowFood();
    }
    else
    {
        Console.Write(player);
    }
}

// Clears the console, displays the food and player
void InitializeGame()
{
    Console.Clear();
    ShowFood();
    Console.SetCursorPosition(0, 0);
    Console.Write(player);
}
