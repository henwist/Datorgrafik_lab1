using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Components
{
    public class ModelComponent : Component
    {
        public Model model { get; protected set; }
        public Matrix world { get; set; }

        public Matrix scale { get; protected set; }
        public Matrix translation { get; protected set; }
        public Matrix rotation { get; protected set; }

        public ModelComponent(Model m, float scale, Vector3 translation, 
                              float rotationx, float rotationy, float rotationz)
        {
            model = m;
            this.scale = Matrix.CreateScale(scale);
            this.translation = Matrix.CreateTranslation(translation);
            rotation = Matrix.CreateRotationX(rotationx)
                     * Matrix.CreateRotationY(rotationy)
                     * Matrix.CreateRotationZ(rotationz);
        }

        public override void Update(GameTime gametime)
        {

        }
    }
}
