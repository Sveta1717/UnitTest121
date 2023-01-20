using App;

namespace AppTest // тест проета App
{
    [TestClass]
    public class TriangleTest  // традиционно: один тестовый класс - для одного класса проекта
    {
        [TestMethod]
        public void Test_DefCtor()  // тестируем конструктор (без параметров)
        { 
            Triangle triangle = new() { A = 1 };        // инициализация - это не параметры
            Assert.IsNotNull(triangle);
            Triangle triangle2 = new();                 // Same - один объект ( равенство 
            Assert.AreNotSame(triangle2, triangle);     // ссылок). Используется для
                                                        // Singleton / non-Singleton           
        }

        [TestMethod]
        public void Test_ParCtor()  // тестируем конструктор (c параметрами)
        {
            // тест правильной передачи параметров (а->А, b->B. c->C)
            Triangle triangle = new(2, 3, 4);       
            Assert.IsNotNull(triangle);
            Assert.AreEqual(2, triangle.A, "new(2, 3, 4) --> A == 2");
            Assert.AreEqual(3, triangle.B, "new(2, 3, 4) --> B == 3");
            Assert.AreEqual(4, triangle.C, "new(2, 3, 4) --> B == 4");

            /* Тестирование исключительных ситуаций.
            * Особенность - если исключение брошено во время расчета аргумента, то
            * вызоы метода вообще не произойдет
            * Assert....( new(1,2,3) )
            * Поэтому код с исключением "оборачивают" в лямбду перед утверждением
            * Assert....( () => new(1,2,3)  )
            * Лямбда передается в Assert и там исполняется. Так исключение может перехватиться
            */
            Assert.ThrowsException<ArgumentException>(      // Сравнение типа исключений - строгое,
                () => triangle = new(1, 2, 3),              // <Exception> - приведет к провалу теста
                "new(1,2,3) Throws ArgumentException"       // 
            );

            // ThrowsExceptio - возвращает само исключение, которое было выброшено в лямбду
            var ex = Assert.ThrowsException<ArgumentException>(
                () => triangle = new(-1, 2, 3));
            // это позволяет проверить параметры исключения, например текст сообщение
            Assert.AreEqual(
                "(-1,2,3) is not valid triangle",
                ex.Message,
                "Exceotion (-1,2,3) message test");

            // Две проверки (исключение и его параметры) можно объеденить
            Assert.AreEqual(
               "(1,-2,3) is not valid triangle",
                Assert.ThrowsException<ArgumentException>(
                () => triangle = new(1, -2, 3)).Message,
               "Exceotion (1,-2,3) message test");
        }
        [TestMethod]
        public void Test_IsValid() // на каждый метод Triangle - метод теста
        {
            Triangle triangle = new() { A = 2, B = 3, C = 4 };
            // Утверждение - элемент для поверки: или подтверждено или опровергнуто
            Assert.IsTrue(triangle.IsValid(), "2-3-4 is valid triangle");

            triangle = new() { A = 1, B = 2, C = 3 };
            Assert.IsFalse(triangle.IsValid(), "1-2-3 is invalid triangle");

            triangle = new() { A = 3, B = 2, C = 1 };
            Assert.IsFalse(triangle.IsValid(), "3-2-1 is invalid triangle");

            triangle = new() { A = 2, B = 3, C = 1 };
            Assert.IsFalse(triangle.IsValid(), "2-3-1 is invalid triangle");

            triangle = new() { A = -2, B = -3, C = -4 };
            Assert.IsFalse(triangle.IsValid(), "-2 -3 -4 is invalid triangle");

            Random rand = new ();
            for (int i = 0; i<20; i++) 
            {
                double c = rand.NextDouble();
                triangle = new() { A = 2, B = 3, C = c };
                Assert.IsFalse(triangle.IsValid(), $"2-3-{c} is invalid triangle");
            }
        }

        [TestMethod]
        public void Test_Perimeter()
        {
            Triangle triangle = new() { A = 2, B = 3, C = 4 };
            Assert.AreEqual(                       // Утверждение равенства (не путать с Equals )
                9,                                 // Ожидаемое (правильное) значение
                triangle.Perimeter(),              // Фактическое (полученное) значение
                "Perimeter{2,3,4} == 9");          // Собщение при провале проверки

            triangle = new() { A = 2 * Math.Sqrt(2), B= 3 * Math.Sqrt(2), C = 4 * Math.Sqrt(2)};
            // Assert.AreEqual(9 * Math.Sqrt(2), triangle.Perimeter());  // Ожидается: <12,727922061357857>. Фактически: <12,727922061357855>
           
            /* Особенность проверки дробных чисел.
             * Дробные числа, особенно с бесконечной дробной частью, при записи обрезаются
             * до размера типа (64 бита). После этого, все операции с числами становятся не точными.
             * Поэтому 2v2 + 3v2 + 4v2 =/= 9v2
             * Дробные числа всегда нужно сравнивать с указанием погрешности (точности)
             * Предельная точность опредеяется типом данных
             * double - 1e-15
             * float - 1e-7
             * При умножениях погрешность ухудшается, например,
             * 1.001 * 100 -> 100.1
             * Поэтому при проверках предельную точность нужно корректировать на множитель
             */
            Assert.AreEqual(                      // Для дробных чисел - сравнение с погрешностью
               9 * Math.Sqrt(2),                  // Ожидаемое (правильное) значение <12,727922061357857>
               triangle.Perimeter(),              // Фактическое (полученное) значение  <12,727922061357855> (отличие в последенем знаке)
               1e-15 * triangle.Perimeter(),      // дельта - погрешность сравнения
               "Perimeter{2,3,4}v2 == 9");        // Собщение при провале проверки
        }

        
    }
}