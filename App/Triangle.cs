using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App
{
    public class Triangle
    {
        // Стороны треуголника (длины)
        public double A { get; set; }
        public double B { get; set; }
        public double C { get; set; }

        public Triangle()
        {

        }

        public Triangle(double a, double b, double c)
        {
            A = a;
            B = b;
            C = c;
            if( ! this.IsValid())
            {
                throw new ArgumentException($"({a},{b},{c}) is not valid triangle");
            }
        }



        // Проверка на допустимость (можно ли собрать тр-к их данных A, B, C)
        public bool IsValid()
        {
            // return true;        // проходят тесты, в которых только Assert.IsTrue
            //return A + B > C;   // проходят тесты, в которых только A + B > C
           
            return A + B > C
                && B + C > A
                && A + C > B;
        }

        //  Периметр - сумма сторон треугольника

        public double Perimeter()
        {
            return A + B + C;
        }
    }
}
