using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Genop.UnitTests
{
    /**
     * Klasa wykonująca testy jednostkowe
     * niektórych z metod występujących w
     * klasie Controller.
     */
    [TestClass]
    public class ControlerTests
    {
        /**
         * Metoda testująca metodę
         * CalculateOutput(), kalkulującą dane
         * i zwracającą wartość liczbową
         */
        [TestMethod]
        public void CalculateOutput_Calculating_GetingValue()
        {
            // Arrange
            double Kp = 1;
            double Ki = 1;
            double Kd = 1;
            double sp = 1;
            double pv = 10;
            double dt = 0.001;
            Controller controller1 = new Controller(Kp, Ki, Kd);

            // Act
            var resoult = controller1.CalculateOutput(sp, pv, dt);

            // Assert
            Assert.IsNotNull(resoult);
        }
    }
}
