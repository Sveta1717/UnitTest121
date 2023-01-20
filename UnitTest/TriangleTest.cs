using App;

namespace AppTest // ���� ������ App
{
    [TestClass]
    public class TriangleTest  // �����������: ���� �������� ����� - ��� ������ ������ �������
    {
        [TestMethod]
        public void Test_DefCtor()  // ��������� ����������� (��� ����������)
        { 
            Triangle triangle = new() { A = 1 };        // ������������� - ��� �� ���������
            Assert.IsNotNull(triangle);
            Triangle triangle2 = new();                 // Same - ���� ������ ( ��������� 
            Assert.AreNotSame(triangle2, triangle);     // ������). ������������ ���
                                                        // Singleton / non-Singleton           
        }

        [TestMethod]
        public void Test_ParCtor()  // ��������� ����������� (c �����������)
        {
            // ���� ���������� �������� ���������� (�->�, b->B. c->C)
            Triangle triangle = new(2, 3, 4);       
            Assert.IsNotNull(triangle);
            Assert.AreEqual(2, triangle.A, "new(2, 3, 4) --> A == 2");
            Assert.AreEqual(3, triangle.B, "new(2, 3, 4) --> B == 3");
            Assert.AreEqual(4, triangle.C, "new(2, 3, 4) --> B == 4");

            /* ������������ �������������� ��������.
            * ����������� - ���� ���������� ������� �� ����� ������� ���������, ��
            * ����� ������ ������ �� ����������
            * Assert....( new(1,2,3) )
            * ������� ��� � ����������� "�����������" � ������ ����� ������������
            * Assert....( () => new(1,2,3)  )
            * ������ ���������� � Assert � ��� �����������. ��� ���������� ����� �������������
            */
            Assert.ThrowsException<ArgumentException>(      // ��������� ���� ���������� - �������,
                () => triangle = new(1, 2, 3),              // <Exception> - �������� � ������� �����
                "new(1,2,3) Throws ArgumentException"       // 
            );

            // ThrowsExceptio - ���������� ���� ����������, ������� ���� ��������� � ������
            var ex = Assert.ThrowsException<ArgumentException>(
                () => triangle = new(-1, 2, 3));
            // ��� ��������� ��������� ��������� ����������, �������� ����� ���������
            Assert.AreEqual(
                "(-1,2,3) is not valid triangle",
                ex.Message,
                "Exceotion (-1,2,3) message test");

            // ��� �������� (���������� � ��� ���������) ����� ����������
            Assert.AreEqual(
               "(1,-2,3) is not valid triangle",
                Assert.ThrowsException<ArgumentException>(
                () => triangle = new(1, -2, 3)).Message,
               "Exceotion (1,-2,3) message test");
        }
        [TestMethod]
        public void Test_IsValid() // �� ������ ����� Triangle - ����� �����
        {
            Triangle triangle = new() { A = 2, B = 3, C = 4 };
            // ����������� - ������� ��� �������: ��� ������������ ��� ������������
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
            Assert.AreEqual(                       // ����������� ��������� (�� ������ � Equals )
                9,                                 // ��������� (����������) ��������
                triangle.Perimeter(),              // ����������� (����������) ��������
                "Perimeter{2,3,4} == 9");          // �������� ��� ������� ��������

            triangle = new() { A = 2 * Math.Sqrt(2), B= 3 * Math.Sqrt(2), C = 4 * Math.Sqrt(2)};
            // Assert.AreEqual(9 * Math.Sqrt(2), triangle.Perimeter());  // ���������: <12,727922061357857>. ����������: <12,727922061357855>
           
            /* ����������� �������� ������� �����.
             * ������� �����, �������� � ����������� ������� ������, ��� ������ ����������
             * �� ������� ���� (64 ����). ����� �����, ��� �������� � ������� ���������� �� �������.
             * ������� 2v2 + 3v2 + 4v2 =/= 9v2
             * ������� ����� ������ ����� ���������� � ��������� ����������� (��������)
             * ���������� �������� ����������� ����� ������
             * double - 1e-15
             * float - 1e-7
             * ��� ���������� ����������� ����������, ��������,
             * 1.001 * 100 -> 100.1
             * ������� ��� ��������� ���������� �������� ����� �������������� �� ���������
             */
            Assert.AreEqual(                      // ��� ������� ����� - ��������� � ������������
               9 * Math.Sqrt(2),                  // ��������� (����������) �������� <12,727922061357857>
               triangle.Perimeter(),              // ����������� (����������) ��������  <12,727922061357855> (������� � ���������� �����)
               1e-15 * triangle.Perimeter(),      // ������ - ����������� ���������
               "Perimeter{2,3,4}v2 == 9");        // �������� ��� ������� ��������
        }

        
    }
}