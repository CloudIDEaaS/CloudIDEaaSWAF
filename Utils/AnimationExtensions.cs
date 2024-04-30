using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

namespace Utils
{
    public delegate void OnFrameHandler(int frame);

    public class Animation
    {
        private int delay;
        private bool useCustomTimer;
        private int currentFrame;
        public bool Stopped { get; private set; }
        public bool Started { get; private set; }
        public int Frames { get; private set; }
        private int totalTime;
        private int startingTick;
        private bool autoStop;
        private Form form;
        private Timer timer;
        private int lastFrame;
        private int lastTick;
        public event OnFrameHandler OnFrame;
        public event EventHandler OnStop;

        public Animation(Form form, int delay)
        {
            this.delay = delay;
            this.form = form;

            form.FormClosing += (sender, e) =>
            {
                if (timer != null && timer.Enabled)
                {
                    Stop();
                }
            };
        }

        public Animation(int delay, bool useCustomTimer = false)
        {
            this.delay = delay;
            this.useCustomTimer = useCustomTimer;
        }

        private float Lerp(float x, float y, float t)
        {
            return x + (t * (y - x));
        }

        public void Stop()
        {
            if (timer != null)
            {
                timer.Tick -= Tick;
                timer.Stop();

                timer = null;
            }

            this.Stopped = true;
            this.Started = false;
        }

        public void Start(int frames, bool autoStop = false)
        {
            if (!useCustomTimer)
            {
                timer = new Timer();

                timer.Tick += Tick;

                timer.Start();
            }

            this.Started = true;
            this.Frames = frames;

            totalTime = frames * delay;
            startingTick = Environment.TickCount;
            this.autoStop = autoStop;
        }

        public void Tick()
        {
            var tick = Environment.TickCount - startingTick;
            var time = tick % Math.Max(1, totalTime);
            var percentOfTime = ((float)time) / ((float)totalTime);

            currentFrame = (int)Math.Round(Lerp(1, Frames, percentOfTime), 0);

            // Debug.WriteLine("curentFrame: {0}, percentOfTime: {1}", currentFrame, percentOfTime);

            if (OnFrame != null)
            {
                OnFrame(currentFrame);
            }

            if (this.autoStop && currentFrame < lastFrame)
            {
                this.Stop();
                OnStop?.Invoke(this, EventArgs.Empty);
            }

            lastFrame = currentFrame;
            lastTick = tick;
        }

        private void Tick(object sender, EventArgs e)
        {
            Tick();
        }
    }

    static public class Easings
    {
        /// <summary>
        /// Constant Pi.
        /// </summary>
        private const double PI = Math.PI;

        /// <summary>
        /// Constant Pi / 2.
        /// </summary>
        private const double HALFPI = Math.PI / 2.0f;

        /// <summary>
        /// Easing Functions enumeration
        /// </summary>
        public enum Functions
        {
            Linear,
            QuadraticEaseIn,
            QuadraticEaseOut,
            QuadraticEaseInOut,
            CubicEaseIn,
            CubicEaseOut,
            CubicEaseInOut,
            QuarticEaseIn,
            QuarticEaseOut,
            QuarticEaseInOut,
            QuinticEaseIn,
            QuinticEaseOut,
            QuinticEaseInOut,
            SineEaseIn,
            SineEaseOut,
            SineEaseInOut,
            CircularEaseIn,
            CircularEaseOut,
            CircularEaseInOut,
            ExponentialEaseIn,
            ExponentialEaseOut,
            ExponentialEaseInOut,
            ElasticEaseIn,
            ElasticEaseOut,
            ElasticEaseInOut,
            BackEaseIn,
            BackEaseOut,
            BackEaseInOut,
            BounceEaseIn,
            BounceEaseOut,
            BounceEaseInOut
        }

        /// <summary>
        /// Interpolate using the specified function.
        /// </summary>
        static public double Interpolate(double p, Functions function)
        {
            switch (function)
            {
                default:
                case Functions.Linear: return Linear(p);
                case Functions.QuadraticEaseOut: return QuadraticEaseOut(p);
                case Functions.QuadraticEaseIn: return QuadraticEaseIn(p);
                case Functions.QuadraticEaseInOut: return QuadraticEaseInOut(p);
                case Functions.CubicEaseIn: return CubicEaseIn(p);
                case Functions.CubicEaseOut: return CubicEaseOut(p);
                case Functions.CubicEaseInOut: return CubicEaseInOut(p);
                case Functions.QuarticEaseIn: return QuarticEaseIn(p);
                case Functions.QuarticEaseOut: return QuarticEaseOut(p);
                case Functions.QuarticEaseInOut: return QuarticEaseInOut(p);
                case Functions.QuinticEaseIn: return QuinticEaseIn(p);
                case Functions.QuinticEaseOut: return QuinticEaseOut(p);
                case Functions.QuinticEaseInOut: return QuinticEaseInOut(p);
                case Functions.SineEaseIn: return SineEaseIn(p);
                case Functions.SineEaseOut: return SineEaseOut(p);
                case Functions.SineEaseInOut: return SineEaseInOut(p);
                case Functions.CircularEaseIn: return CircularEaseIn(p);
                case Functions.CircularEaseOut: return CircularEaseOut(p);
                case Functions.CircularEaseInOut: return CircularEaseInOut(p);
                case Functions.ExponentialEaseIn: return ExponentialEaseIn(p);
                case Functions.ExponentialEaseOut: return ExponentialEaseOut(p);
                case Functions.ExponentialEaseInOut: return ExponentialEaseInOut(p);
                case Functions.ElasticEaseIn: return ElasticEaseIn(p);
                case Functions.ElasticEaseOut: return ElasticEaseOut(p);
                case Functions.ElasticEaseInOut: return ElasticEaseInOut(p);
                case Functions.BackEaseIn: return BackEaseIn(p);
                case Functions.BackEaseOut: return BackEaseOut(p);
                case Functions.BackEaseInOut: return BackEaseInOut(p);
                case Functions.BounceEaseIn: return BounceEaseIn(p);
                case Functions.BounceEaseOut: return BounceEaseOut(p);
                case Functions.BounceEaseInOut: return BounceEaseInOut(p);
            }
        }

        /// <summary>
        /// Modeled after the line y = x
        /// </summary>
        static public double Linear(double p)
        {
            return p;
        }

        /// <summary>
        /// Modeled after the parabola y = x^2
        /// </summary>
        static public double QuadraticEaseIn(double p)
        {
            return p * p;
        }

        /// <summary>
        /// Modeled after the parabola y = -x^2 + 2x
        /// </summary>
        static public double QuadraticEaseOut(double p)
        {
            return -(p * (p - 2));
        }

        /// <summary>
        /// Modeled after the piecewise quadratic
        /// y = (1/2)((2x)^2)             ; [0, 0.5)
        /// y = -(1/2)((2x-1)*(2x-3) - 1) ; [0.5, 1]
        /// </summary>
        static public double QuadraticEaseInOut(double p)
        {
            if (p < 0.5f)
            {
                return 2 * p * p;
            }
            else
            {
                return (-2 * p * p) + (4 * p) - 1;
            }
        }

        /// <summary>
        /// Modeled after the cubic y = x^3
        /// </summary>
        static public double CubicEaseIn(double p)
        {
            return p * p * p;
        }

        /// <summary>
        /// Modeled after the cubic y = (x - 1)^3 + 1
        /// </summary>
        static public double CubicEaseOut(double p)
        {
            double f = (p - 1);
            return f * f * f + 1;
        }

        /// <summary>	
        /// Modeled after the piecewise cubic
        /// y = (1/2)((2x)^3)       ; [0, 0.5)
        /// y = (1/2)((2x-2)^3 + 2) ; [0.5, 1]
        /// </summary>
        static public double CubicEaseInOut(double p)
        {
            if (p < 0.5f)
            {
                return 4 * p * p * p;
            }
            else
            {
                double f = ((2 * p) - 2);
                return 0.5f * f * f * f + 1;
            }
        }

        /// <summary>
        /// Modeled after the quartic x^4
        /// </summary>
        static public double QuarticEaseIn(double p)
        {
            return p * p * p * p;
        }

        /// <summary>
        /// Modeled after the quartic y = 1 - (x - 1)^4
        /// </summary>
        static public double QuarticEaseOut(double p)
        {
            double f = (p - 1);
            return f * f * f * (1 - p) + 1;
        }

        /// <summary>
        // Modeled after the piecewise quartic
        // y = (1/2)((2x)^4)        ; [0, 0.5)
        // y = -(1/2)((2x-2)^4 - 2) ; [0.5, 1]
        /// </summary>
        static public double QuarticEaseInOut(double p)
        {
            if (p < 0.5f)
            {
                return 8 * p * p * p * p;
            }
            else
            {
                double f = (p - 1);
                return -8 * f * f * f * f + 1;
            }
        }

        /// <summary>
        /// Modeled after the quintic y = x^5
        /// </summary>
        static public double QuinticEaseIn(double p)
        {
            return p * p * p * p * p;
        }

        /// <summary>
        /// Modeled after the quintic y = (x - 1)^5 + 1
        /// </summary>
        static public double QuinticEaseOut(double p)
        {
            double f = (p - 1);
            return f * f * f * f * f + 1;
        }

        /// <summary>
        /// Modeled after the piecewise quintic
        /// y = (1/2)((2x)^5)       ; [0, 0.5)
        /// y = (1/2)((2x-2)^5 + 2) ; [0.5, 1]
        /// </summary>
        static public double QuinticEaseInOut(double p)
        {
            if (p < 0.5f)
            {
                return 16 * p * p * p * p * p;
            }
            else
            {
                double f = ((2 * p) - 2);
                return 0.5f * f * f * f * f * f + 1;
            }
        }

        /// <summary>
        /// Modeled after quarter-cycle of sine wave
        /// </summary>
        static public double SineEaseIn(double p)
        {
            return Math.Sin((p - 1) * HALFPI) + 1;
        }

        /// <summary>
        /// Modeled after quarter-cycle of sine wave (different phase)
        /// </summary>
        static public double SineEaseOut(double p)
        {
            return Math.Sin(p * HALFPI);
        }

        /// <summary>
        /// Modeled after half sine wave
        /// </summary>
        static public double SineEaseInOut(double p)
        {
            return 0.5f * (1 - Math.Cos(p * PI));
        }

        /// <summary>
        /// Modeled after shifted quadrant IV of unit circle
        /// </summary>
        static public double CircularEaseIn(double p)
        {
            return 1 - Math.Sqrt(1 - (p * p));
        }

        /// <summary>
        /// Modeled after shifted quadrant II of unit circle
        /// </summary>
        static public double CircularEaseOut(double p)
        {
            return Math.Sqrt((2 - p) * p);
        }

        /// <summary>	
        /// Modeled after the piecewise circular function
        /// y = (1/2)(1 - Math.Sqrt(1 - 4x^2))           ; [0, 0.5)
        /// y = (1/2)(Math.Sqrt(-(2x - 3)*(2x - 1)) + 1) ; [0.5, 1]
        /// </summary>
        static public double CircularEaseInOut(double p)
        {
            if (p < 0.5f)
            {
                return 0.5f * (1 - Math.Sqrt(1 - 4 * (p * p)));
            }
            else
            {
                return 0.5f * (Math.Sqrt(-((2 * p) - 3) * ((2 * p) - 1)) + 1);
            }
        }

        /// <summary>
        /// Modeled after the exponential function y = 2^(10(x - 1))
        /// </summary>
        static public double ExponentialEaseIn(double p)
        {
            return (p == 0.0f) ? p : Math.Pow(2, 10 * (p - 1));
        }

        /// <summary>
        /// Modeled after the exponential function y = -2^(-10x) + 1
        /// </summary>
        static public double ExponentialEaseOut(double p)
        {
            return (p == 1.0f) ? p : 1 - Math.Pow(2, -10 * p);
        }

        /// <summary>
        /// Modeled after the piecewise exponential
        /// y = (1/2)2^(10(2x - 1))         ; [0,0.5)
        /// y = -(1/2)*2^(-10(2x - 1))) + 1 ; [0.5,1]
        /// </summary>
        static public double ExponentialEaseInOut(double p)
        {
            if (p == 0.0 || p == 1.0) return p;

            if (p < 0.5f)
            {
                return 0.5f * Math.Pow(2, (20 * p) - 10);
            }
            else
            {
                return -0.5f * Math.Pow(2, (-20 * p) + 10) + 1;
            }
        }

        /// <summary>
        /// Modeled after the damped sine wave y = sin(13pi/2*x)*Math.Pow(2, 10 * (x - 1))
        /// </summary>
        static public double ElasticEaseIn(double p)
        {
            return Math.Sin(13 * HALFPI * p) * Math.Pow(2, 10 * (p - 1));
        }

        /// <summary>
        /// Modeled after the damped sine wave y = sin(-13pi/2*(x + 1))*Math.Pow(2, -10x) + 1
        /// </summary>
        static public double ElasticEaseOut(double p)
        {
            return Math.Sin(-13 * HALFPI * (p + 1)) * Math.Pow(2, -10 * p) + 1;
        }

        /// <summary>
        /// Modeled after the piecewise exponentially-damped sine wave:
        /// y = (1/2)*sin(13pi/2*(2*x))*Math.Pow(2, 10 * ((2*x) - 1))      ; [0,0.5)
        /// y = (1/2)*(sin(-13pi/2*((2x-1)+1))*Math.Pow(2,-10(2*x-1)) + 2) ; [0.5, 1]
        /// </summary>
        static public double ElasticEaseInOut(double p)
        {
            if (p < 0.5f)
            {
                return 0.5f * Math.Sin(13 * HALFPI * (2 * p)) * Math.Pow(2, 10 * ((2 * p) - 1));
            }
            else
            {
                return 0.5f * (Math.Sin(-13 * HALFPI * ((2 * p - 1) + 1)) * Math.Pow(2, -10 * (2 * p - 1)) + 2);
            }
        }

        /// <summary>
        /// Modeled after the overshooting cubic y = x^3-x*sin(x*pi)
        /// </summary>
        static public double BackEaseIn(double p)
        {
            return p * p * p - p * Math.Sin(p * PI);
        }

        /// <summary>
        /// Modeled after overshooting cubic y = 1-((1-x)^3-(1-x)*sin((1-x)*pi))
        /// </summary>	
        static public double BackEaseOut(double p)
        {
            double f = (1 - p);
            return 1 - (f * f * f - f * Math.Sin(f * PI));
        }

        /// <summary>
        /// Modeled after the piecewise overshooting cubic function:
        /// y = (1/2)*((2x)^3-(2x)*sin(2*x*pi))           ; [0, 0.5)
        /// y = (1/2)*(1-((1-x)^3-(1-x)*sin((1-x)*pi))+1) ; [0.5, 1]
        /// </summary>
        static public double BackEaseInOut(double p)
        {
            if (p < 0.5f)
            {
                double f = 2 * p;
                return 0.5f * (f * f * f - f * Math.Sin(f * PI));
            }
            else
            {
                double f = (1 - (2 * p - 1));
                return 0.5f * (1 - (f * f * f - f * Math.Sin(f * PI))) + 0.5f;
            }
        }

        /// <summary>
        /// </summary>
        static public double BounceEaseIn(double p)
        {
            return 1 - BounceEaseOut(1 - p);
        }

        /// <summary>
        /// </summary>
        static public double BounceEaseOut(double p)
        {
            if (p < 4 / 11.0f)
            {
                return (121 * p * p) / 16.0f;
            }
            else if (p < 8 / 11.0f)
            {
                return (363 / 40.0f * p * p) - (99 / 10.0f * p) + 17 / 5.0f;
            }
            else if (p < 9 / 10.0f)
            {
                return (4356 / 361.0f * p * p) - (35442 / 1805.0f * p) + 16061 / 1805.0f;
            }
            else
            {
                return (54 / 5.0f * p * p) - (513 / 25.0f * p) + 268 / 25.0f;
            }
        }

        /// <summary>
        /// </summary>
        static public double BounceEaseInOut(double p)
        {
            if (p < 0.5f)
            {
                return 0.5f * BounceEaseIn(p * 2);
            }
            else
            {
                return 0.5f * BounceEaseOut(p * 2 - 1) + 0.5f;
            }
        }
    }
}
