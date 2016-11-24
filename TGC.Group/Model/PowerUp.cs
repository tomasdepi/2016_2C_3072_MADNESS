using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Collision;
using TGC.Core.Geometry;
using TGC.Core.Interpolation;

namespace TGC.Group.Model
{
    public abstract class PowerUp
    {

        public TgcSphere esfera { set; get; }
        private InterpoladorVaiven interpolador;
        private float tiempo;

        public Moto moto { set; get; }

        public PowerUp(Vector3 pos)
        {
            interpolador = new InterpoladorVaiven();
            interpolador.Current = 8;
            interpolador.Min = 7;
            interpolador.Max = 28;
            interpolador.Speed = 40f;

            esfera = new TgcSphere();
            esfera.Position = pos;
            esfera.setColor(Color.Red);
            esfera.Radius = 10;
            esfera.LevelOfDetail = 3;

            esfera.AutoTransformEnable = true;

            esfera.updateValues();

            tiempo = 10f;

            moto = null;
        } 

        public void animar(float ElapsedTime)
        {

            var move = new Vector3(esfera.Position.X, interpolador.update(ElapsedTime), esfera.Position.Z);
            esfera.Position = move;
            esfera.AutoTransformEnable = true;
            esfera.updateValues();
        }

        public abstract void tomar(Moto moto);

        public abstract void finalizarEfecto();
        
        public void actualizarTiempo(float ElapsedTime)
        {
            tiempo -= ElapsedTime;
            if (tiempo < 0) this.finalizarEfecto();
        }

        public bool comprobarColisionConMoto(Moto moto)
        {
            if(TgcCollisionUtils.testSphereAABB(esfera.BoundingSphere, moto.getBoundingBox()))
            {
                this.tomar(moto);
                return true;
            }
            return false;
        }

        public void render()
        {
            esfera.render();
        }

        public void dispose()
        {
            esfera.dispose();
        }

    }
}
