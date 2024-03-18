using System;
using System.Collections.Generic;
namespace EightPuzzleSolver

{
    public class EightPuzzleSolver

    {
        // Define the goal state of the 8-puzzle
        static int[,] goalState = {
        {1, 2, 3},
        {4, 5, 6},
        {7, 8, 0} // 0 represents the empty space
    };

        // Method to generate new states (neighbors) based on the current state
       public static List<int[,]> GenerateNewStates(int[,] currentState)
        {
            List<int[,]> newStates = new List<int[,]>();
            int emptyRow = 0, emptyCol = 0;

            // Find the position of the empty space (0) in the current state
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (currentState[i, j] == 0)
                    {
                        emptyRow = i;
                        emptyCol = j;
                        break;
                    }
                }
            }

            // Generate new states by swapping the empty space with its valid neighbors
            if (emptyRow > 0)
                newStates.Add(SwapAndCopy(currentState, emptyRow, emptyCol, emptyRow - 1, emptyCol)); // Up
            if (emptyRow < 2)
                newStates.Add(SwapAndCopy(currentState, emptyRow, emptyCol, emptyRow + 1, emptyCol)); // Down
            if (emptyCol > 0)
                newStates.Add(SwapAndCopy(currentState, emptyRow, emptyCol, emptyRow, emptyCol - 1)); // Left
            if (emptyCol < 2)
                newStates.Add(SwapAndCopy(currentState, emptyRow, emptyCol, emptyRow, emptyCol + 1)); // Right

            return newStates;
        }

        // Method to create a new state by swapping two positions and copying the current state
        static int[,] SwapAndCopy(int[,] currentState, int row1, int col1, int row2, int col2)
        {
            int[,] newState = (int[,])currentState.Clone(); // Create a copy of the current state
            int temp = newState[row1, col1]; // Swap values
            newState[row1, col1] = newState[row2, col2];
            newState[row2, col2] = temp;
            return newState;
        }

        // Method to check if a state is the goal state
        public static bool IsGoalState(int[,] state)
        {
            return CompareStates(state, goalState);
        }

        // Method to compare two states to check if they are equal
        static bool CompareStates(int[,] state1, int[,] state2)
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (state1[i, j] != state2[i, j])
                        return false;
                }
            }
            return true;
        }

        // Method to get a description of the move made from the current state to the new state
        static string GetMoveDescription(int[,] currentState, int[,] newState)
        {
            int emptyRowCurrent = 0, emptyColCurrent = 0;
            int emptyRowNew = 0, emptyColNew = 0;

            // Find the positions of the empty space in both states
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (currentState[i, j] == 0)
                    {
                        emptyRowCurrent = i;
                        emptyColCurrent = j;
                    }
                    if (newState[i, j] == 0)
                    {
                        emptyRowNew = i;
                        emptyColNew = j;
                    }
                }
            }

            // Determine the direction of the move based on the change in positions
            int deltaRow = emptyRowNew - emptyRowCurrent;
            int deltaCol = emptyColNew - emptyColCurrent;

            // Construct and return a string describing the move
            if (deltaRow == -1)
                return $"Move Up (Tile {currentState[emptyRowCurrent, emptyColCurrent]})";
            if (deltaRow == 1)
                return $"Move Down (Tile {currentState[emptyRowCurrent, emptyColCurrent]})";
            if (deltaCol == -1)
                return $"Move Left (Tile {currentState[emptyRowCurrent, emptyColCurrent]})";
            return $"Move Right (Tile {currentState[emptyRowCurrent, emptyColCurrent]})";
        }

        // Method to print the puzzle board and solution path
        static void PrintPuzzle(int[,] puzzle, List<string> moves)
        {
            Console.WriteLine("## 8-Puzzle Game\n");

            Console.WriteLine("**Initial State:**\n");
            PrintBoard(puzzle);

            // Print recorded moves during solving
            if (moves.Count > 0)
            {
                Console.WriteLine("\n**Moves (recorded during solving):**\n");
                for (int i = 0; i < moves.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {moves[i]}");
                }
            }

            // Print the solved state
            Console.WriteLine("\n**Solved State:**\n");
            PrintBoard(goalState);

            // Print congratulations message
            Console.WriteLine($"\n**Congratulations! You solved the puzzle in {moves.Count} moves.**");
        }

        // Method to print the puzzle board
        static void PrintBoard(int[,] board)
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (board[i, j] == 0)
                        Console.Write("   (Empty Space)");
                    else
                        Console.Write(board[i, j].ToString().PadLeft(2) + " | ");

                    if (j == 2)
                        Console.WriteLine();
                }
                if (i < 2)
                    Console.WriteLine("-------");
            }
        }

        // Method to prompt the user to input the initial state of the puzzle
        static int[,] GetUserInitialState()
        {
            int[,] initialState = new int[3, 3];
            Console.WriteLine("Enter the initial state of the puzzle (enter 0 for empty space):");

            // Get values for each position from the user
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Console.Write($"Enter value for row {i + 1}, column {j + 1}: ");
                    int value = int.Parse(Console.ReadLine());
                    initialState[i, j] = value;
                }
            }

            return initialState;
        }

        // Method to solve the puzzle using breadth-first search
        static List<string> SolvePuzzle(int[,] initialState)
        {
            Queue<SearchNode> queue = new Queue<SearchNode>();
            SearchNode initialNode = new SearchNode(initialState, null, "");
            queue.Enqueue(initialNode);

            HashSet<string> visitedStates = new HashSet<string>();

            while (queue.Count > 0)
            {
                SearchNode currentNode = queue.Dequeue();
                visitedStates.Add(GetStateString(currentNode.state));

                // Check if the current state is the goal state
                if (IsGoalState(currentNode.state))
                {
                    List<string> moves = new List<string>();
                    SearchNode node = currentNode;
                    while (node.parent != null)
                    {
                        moves.Insert(0, node.move);
                        node = node.parent;
                    }
                    return moves; // Return the solution path
                }

                // Generate new states and add them to the queue if they haven't been visited
                List<int[,]> newStates = GenerateNewStates(currentNode.state);
                foreach (int[,] newState in newStates)
                {
                    string stateString = GetStateString(newState);
                    if (!visitedStates.Contains(stateString))
                    {
                        string move = GetMoveDescription(currentNode.state, newState);
                        SearchNode childNode = new SearchNode(newState, currentNode, move);
                        queue.Enqueue(childNode);
                    }
                }
            }

            return new List<string>(); // No solution found
        }

        // Method to convert a state into a string for hashing and comparison
        static string GetStateString(int[,] state)
        {
            string stateString = "";
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    stateString += state[i, j];
                }
            }
            return stateString;
        }

        // Main method
        static void Main()
        {
            // Prompt user to input the initial state
            int[,] initialState = GetUserInitialState();

            // Solve the puzzle using breadth-first search
            List<string> moves = SolvePuzzle(initialState);

            // Print the solution path
            PrintPuzzle(initialState, moves);
        }

        // Nested class to represent a node in the search tree
        class SearchNode
        {
            public int[,] state { get; set; }
            public SearchNode parent { get; set; }
            public string move { get; set; }

            // Constructor
            public SearchNode(int[,] state, SearchNode parent, string move)
            {
                this.state = state;
                this.parent = parent;
                this.move = move;
            }
        }
    }
}
