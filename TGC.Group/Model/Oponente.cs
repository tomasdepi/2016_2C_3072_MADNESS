using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Geometry;
using TGC.Core.Text;
using TGC.Core.Utils;

namespace TGC.Group.Model
{
    public class Oponente : Moto
    {

        private String MediaDir;

        public Oponente(string mediaPath, Vector3 posInicial) : base(mediaPath, posInicial)
        {
            MediaDir = mediaPath;
        }

        public void seguirObjetivo(Vector3 posicion, float ElapsedTime)
        {

            Vector3 vectorEnX = new Vector3( posicion.X, -5000, this.getPosicion().Z);
            Vector3 vectorEnZ = new Vector3(this.getPosicion().X, -5000, posicion.Z);
            Vector3 direccion = new Vector3(posicion.X - this.getPosicion().X, 0, posicion.Z - this.getPosicion().Z);

            var angulo1 = Vector3.Dot(vectorEnX, direccion);
            var angulo2 = Vector3.Dot(vectorEnZ, direccion);

            if(angulo1 > angulo2)
            {
                //X
                //this.acelerar(ElapsedTime);
            }
            else
            {
                //Y
                //this.acelerar(ElapsedTime);
            }

        }

        public new void render()
        {
            base.render();
        }

        public new void dispose()
        {
            base.render();
        }

    }
}
