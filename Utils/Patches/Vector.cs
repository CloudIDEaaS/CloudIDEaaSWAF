using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Reflection;
using System.Text;
using System.Collections;
using System.Globalization;
using System.Windows;
using System.Runtime.InteropServices;

namespace System.Windows
{
    /// 

    /// Vector - A value type which defined a vector in terms of X and Y
    /// 

    public partial struct Vector
    {
        public double X { get; set; } = 0;
        public double Y { get; set; } = 0;

        #region Constructors 

        /// 

        /// Constructor which sets the vector's initial values
        /// 

        ///  double - The initial X 
        ///  double - THe initial Y  
        public Vector(double x, double y)
        {
            X = x;
            Y = y;
        }

        #endregion Constructors

        #region Public Methods 

        /// 

        /// Length Property - the length of this Vector 
        /// 

        public double Length
        {
            get
            {
                return Math.Sqrt(X * X + Y * Y);
            }
        }

        /// 

        /// LengthSquared Property - the squared length of this Vector 
        /// 

        public double LengthSquared
        {
            get
            {
                return X * X + Y * Y;
            }
        }

        /// 

        /// Normalize - Updates this Vector to maintain its direction, but to have a length
        /// of 1.  This is equivalent to dividing this Vector by Length
        /// 

        public void Normalize()
        {
            // Avoid overflow 
            this /= Math.Max(Math.Abs(X), Math.Abs(Y));
            this /= Length;
        }

        /// 

        /// CrossProduct - Returns the cross product: vector1.X*vector2.Y - vector1.Y*vector2.X 
        /// 

        ///  
        /// Returns the cross product: vector1.X*vector2.Y - vector1.Y*vector2.X 
        /// 
        ///  The first Vector  
        ///  The second Vector 
        public static double CrossProduct(Vector vector1, Vector vector2)
        {
            return vector1.X * vector2.Y - vector1.Y * vector2.X;
        }

        /// 

        /// AngleBetween - the angle between 2 vectors
        /// 

        /// 
        /// Returns the the angle in degrees between vector1 and vector2
        /// 
        ///  The first Vector  
        ///  The second Vector 
        public static double AngleBetween(Vector vector1, Vector vector2)
        {
            double sin = vector1.X * vector2.Y - vector2.X * vector1.Y;
            double cos = vector1.X * vector2.X + vector1.Y * vector2.Y;

            return Math.Atan2(sin, cos) * (180 / Math.PI);
        }

        #endregion Public Methods

        #region Public Operators 
        /// 

        /// Operator -Vector (unary negation) 
        /// 

        public static Vector operator -(Vector vector)
        {
            return new Vector(-vector.X, -vector.Y);
        }

        /// 

        /// Negates the values of X and Y on this Vector
        /// 

        public void Negate()
        {
            X = -X;
            Y = -Y;
        }

        /// 

        /// Operator Vector + Vector
        /// 

        public static Vector operator +(Vector vector1, Vector vector2)
        {
            return new Vector(vector1.X + vector2.X,
                              vector1.Y + vector2.Y);
        }

        /// 

        /// Add: Vector + Vector
        /// 

        public static Vector Add(Vector vector1, Vector vector2)
        {
            return new Vector(vector1.X + vector2.X,
                              vector1.Y + vector2.Y);
        }

        /// 

        /// Operator Vector - Vector
        /// 

        public static Vector operator -(Vector vector1, Vector vector2)
        {
            return new Vector(vector1.X - vector2.X,
                              vector1.Y - vector2.Y);
        }

        /// 

        /// Subtract: Vector - Vector
        /// 

        public static Vector Subtract(Vector vector1, Vector vector2)
        {
            return new Vector(vector1.X - vector2.X,
                              vector1.Y - vector2.Y);
        }

        /// 

        /// Operator Vector + Point
        /// 

        public static Point operator +(Vector vector, Point point)
        {
            return new Point((int)(point.X + vector.X), (int)(point.Y + vector.Y));
        }

        /// 

        /// Add: Vector + Point 
        /// 

        public static Point Add(Vector vector, Point point)
        {
            return new Point((int)(point.X + vector.X), (int)(point.Y + vector.Y));
        }

        /// 

        /// Operator Vector * double 
        /// 

        public static Vector operator *(Vector vector, double scalar)
        {
            return new Vector(vector.X * scalar,
                              vector.Y * scalar);
        }

        /// 

        /// Multiply: Vector * double 
        /// 

        public static Vector Multiply(Vector vector, double scalar)
        {
            return new Vector(vector.X * scalar,
                              vector.Y * scalar);
        }

        /// 

        /// Operator double * Vector 
        /// 

        public static Vector operator *(double scalar, Vector vector)
        {
            return new Vector(vector.X * scalar,
                              vector.Y * scalar);
        }

        /// 

        /// Multiply: double * Vector 
        /// 

        public static Vector Multiply(double scalar, Vector vector)
        {
            return new Vector(vector.X * scalar,
                              vector.Y * scalar);
        }

        /// 

        /// Operator Vector / double 
        /// 

        public static Vector operator /(Vector vector, double scalar)
        {
            return vector * (1.0 / scalar);
        }

        /// 

        /// Multiply: Vector / double
        /// 

        public static Vector Divide(Vector vector, double scalar)
        {
            return vector * (1.0 / scalar);
        }

        /// 

        /// Operator Vector * Matrix 
        /// 

        //public static Vector operator *(Vector vector, Matrix matrix)
        //{
        //    return matrix.Transform(vector);
        //}

        ///// 

        ///// Multiply: Vector * Matrix
        ///// 

        //public static Vector Multiply(Vector vector, Matrix matrix)
        //{
        //    return matrix.Transform(vector);
        //}

        /// 

        /// Operator Vector * Vector, interpreted as their dot product
        /// 

        public static double operator *(Vector vector1, Vector vector2)
        {
            return vector1.X * vector2.X + vector1.Y * vector2.Y;
        }

        /// 

        /// Multiply - Returns the dot product: vector1.X*vector2.X + vector1.Y*vector2.Y
        /// 

        /// 
        /// Returns the dot product: vector1.X*vector2.X + vector1.Y*vector2.Y 
        /// 
        ///  The first Vector  
        ///  The second Vector  
        public static double Multiply(Vector vector1, Vector vector2)
        {
            return vector1.X * vector2.X + vector1.Y * vector2.Y;
        }

        /// 

        /// Determinant - Returns the determinant det(vector1, vector2)
        /// 

        ///  
        /// Returns the determinant: vector1.X*vector2.Y - vector1.Y*vector2.X
        ///  
        ///  The first Vector 
        ///  The second Vector 
        public static double Determinant(Vector vector1, Vector vector2)
        {
            return vector1.X * vector2.Y - vector1.Y * vector2.X;
        }

        /// 

        /// Explicit conversion to Size.  Note that since Size cannot contain negative values, 
        /// the resulting size will contains the absolute values of X and Y
        /// 

        /// 
        /// Size - A Size equal to this Vector 
        /// 
        ///  Vector - the Vector to convert to a Size  
        public static explicit operator Size(Vector vector)
        {
            return new Size((int) Math.Abs(vector.X), (int)Math.Abs(vector.Y));
        }

        /// 

        /// Explicit conversion to Point 
        /// 

        ///  
        /// Point - A Point equal to this Vector 
        /// 
        ///  Vector - the Vector to convert to a Point  
        public static explicit operator Point(Vector vector)
        {
            return new Point((int)vector.X, (int)vector.Y);
        }
        #endregion Public Operators
    }
}
