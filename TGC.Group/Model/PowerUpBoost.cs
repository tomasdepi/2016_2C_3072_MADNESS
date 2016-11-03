using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.DirectX;
using System.Drawing;

namespace TGC.Group.Model
{
    class PowerUpBoost : PowerUp
    {
        public PowerUpBoost(Vector3 pos) : base(pos)
        {
            this.esfera.setColor(Color.Green);
            this.esfera.updateValues();

        }

        public override void finalizarEfecto()
        {
            moto.velocidadMaxima = 250;
        }

        public override void tomar(Moto moto)
        {
            moto.velocidadMaxima = 600;
            this.moto = moto;
        }
    }
}
