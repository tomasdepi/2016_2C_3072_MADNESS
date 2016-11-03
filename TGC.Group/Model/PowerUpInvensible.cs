using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.DirectX;

namespace TGC.Group.Model
{
    class PowerUpInvensible : PowerUp
    {
        public PowerUpInvensible(Vector3 pos) : base(pos)
        {
          
        }

        public override void finalizarEfecto()
        {
            moto.activarModoDios();
            moto.modoDios = false;
        }

        public override void tomar(Moto moto)
        {
            this.moto = moto;

            moto.modoDios = true;
        }
    }
}
