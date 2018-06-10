using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Genop.UnitTests
{
    [TestClass]
    public class SimulatorTests
    {
        [TestMethod]
        public void Simulate_Calculating_GetValue()
        {
            // Arrange
            long number = 100;
            Simulator simulator1 = new Simulator();

            // Act
            var result = simulator1.Simulate(number);

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
