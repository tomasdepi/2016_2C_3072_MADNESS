using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.SceneLoader;
using TGC.Core.Utils;

namespace TGC.Group.Model
{
    class Moto
    {

        private TgcMesh moto;
        private string MediaDir;
        private int velocidad;

        private PathLight pathlight;


        public Moto(string mediaPath)
        {
            this.MediaDir = mediaPath;
        }

        public void init()
        {
            moto = new TgcSceneLoader().loadSceneFromFile(MediaDir + Game.Default.pathAuto).Meshes[0];
            moto.Scale = new Vector3(0.5f, 0.5f, 0.5f);
            moto.move(new Vector3(0, -5000, 0));

            velocidad = 100;

            pathlight = new PathLight(moto.Position);
        }

        public void setVelocidad(int v)
        {
            this.velocidad = v;
        }

        public void render()
        {
            moto.render();
        }


        public void dispose()
        {
            moto.dispose();
        }

        public void girarIzquierda()
        {
            var rotAngle = FastMath.ToRad(-90);
            moto.rotateY(rotAngle);
            pathlight.agregarSegmento(moto.Position);
        }

        public void girarDerecha()
        {
            var rotAngle = FastMath.ToRad(90);
            moto.rotateY(rotAngle);
            pathlight.agregarSegmento(moto.Position);
        }

        public Vector3 getPosicion()
        {
            return moto.Position;
        }

        public void acelerar(float ElepsedTime)
        {
            moto.moveOrientedY(-1 * velocidad * ElepsedTime);
        }

        public CustomVertex.PositionColored[] generarPathLight()
        {
            return pathlight.crearTriangulos();
        }

        public void actualizarPuntoPathLight()
        {
            pathlight.setSegmentoActual(moto.Position);
        }

        public PathLight getPathLight()
        {
            return pathlight;
        }

    }
}
