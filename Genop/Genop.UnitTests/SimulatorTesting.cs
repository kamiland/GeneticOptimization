using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Genop.UnitTests
{
    /**
     * Klasa wykonująca testy jednostkowe
     * niektórych z metod występujących w
     * klasie Simulator.
     */
    [TestClass]
    public class SimulatorTests
    {
        /**
         * Metoda testująca metodę
         * Simulate(), kalkulującą dane
         * i zwracającą wartość liczbową
         */
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
