using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Components
{
    public class ChopperComponent : Component
    {
        public readonly bool isChopper = true;
        public readonly float rotorAngle = .15f;
    }
}
