using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Akira

    {
        public enum UseForCollisionDetection { Triangles, Rectangles, Circles, PerPixel }

        public static class Globals
        {
            public static UseForCollisionDetection CDPerformedWith { get; set; }
        }
}
