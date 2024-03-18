using NUnit.Framework;
using EightPuzzleSolver; // Import the correct namespace

namespace UnitTest1
{
    [TestFixture]
    public class EightPuzzleSolverTests
    {
        [Test]
        public void TestIsGoalState()
        {
            int[,] goalState = {
                { 1, 2, 3 },
                { 4, 5, 6 },
                { 7, 8, 0 }
            };

            Assert.IsTrue(EightPuzzleSolver.EightPuzzleSolver.IsGoalState(goalState)); // Corrected method call
        }

        [Test]
        public void TestGenerateNewStatesCount()
        {
            int[,] initialState = {
                { 1, 2, 3 },
                { 4, 0, 6 },
                { 7, 5, 8 }
            };

            var newStates = EightPuzzleSolver.EightPuzzleSolver.GenerateNewStates(initialState); // Corrected method call
            Assert.AreEqual(4, newStates.Count);
        }

        [Test]
        public void TestGenerateNewStates_Up()
        {
            int[,] initialState = {
                { 1, 2, 3 },
                { 4, 0, 6 },
                { 7, 5, 8 }
            };

            var newStates = EightPuzzleSolver.EightPuzzleSolver.GenerateNewStates(initialState); // Corrected method call
            bool hasUpState = false;

            foreach (var state in newStates)
            {
                if (state[0, 1] == 0 && state[1, 1] == 4) // Check for swapped positions (0 up with 4)
                {
                    hasUpState = true;
                    break;
                }
            }

            Assert.IsTrue(hasUpState); // Ensure a state with the empty space moved up exists
        }

        // Add more test methods as needed
    }
}
