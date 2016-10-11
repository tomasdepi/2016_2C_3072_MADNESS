using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.BoundingVolumes;
using TGC.Core.Collision;
using TGC.Core.Direct3D;
using TGC.Core.Geometry;
using TGC.Core.SceneLoader;
using TGC.Core.Utils;

namespace TGC.Group.Model
{
    public class Moto
    {

        private TgcMesh moto;
        private string MediaDir;
        private int velocidad;

        private PathLight pathlight;

        private VertexBuffer vertexbuffer;
        private Vector3 posInicial;

        public Moto(string mediaPath, Vector3 posInicial)
        {
            this.MediaDir = mediaPath;
            this.posInicial = posInicial;
        }

        public void init()
        {

            moto = new TgcSceneLoader().loadSceneFromFile(MediaDir + Game.Default.pathMoto).Meshes[0];
            moto.Scale = new Vector3(0.5f, 0.5f, 0.5f);
            moto.move(posInicial);

            
            velocidad = 100;

            moto.moveOrientedY(35);
            pathlight = new PathLight(moto.Position);
            moto.moveOrientedY(-35);


            vertexbuffer = new VertexBuffer(typeof(CustomVertex.PositionColored), 3, D3DDevice.Instance.Device,
               Usage.Dynamic | Usage.WriteOnly, CustomVertex.PositionColored.Format, Pool.Default);
        }

        public void setVelocidad(int v)
        {
            this.velocidad = v;
        }

        public void render()
        {
            moto.render();

            vertexbuffer.SetData(this.pathlight.crearTriangulos(), 0, LockFlags.None);

            D3DDevice.Instance.Device.VertexFormat = CustomVertex.PositionColored.Format;
            //Cargar VertexBuffer a renderizar
            D3DDevice.Instance.Device.SetStreamSource(0, vertexbuffer, 0);
            D3DDevice.Instance.Device.Transform.World = Matrix.Translation(2.5f, 0, 0);

            D3DDevice.Instance.Device.DrawPrimitives(PrimitiveType.TriangleList, 0, this.getPathLight().getCantTriangulos());

        }


        public void dispose()
        {
            moto.dispose();
        }

        public void girarIzquierda()
        {
            moto.moveOrientedY(35);
            var rotAngle = FastMath.ToRad(-90);
            moto.rotateY(rotAngle);

            
            pathlight.agregarSegmento(moto.Position);
            moto.moveOrientedY(-35);
        }

        public void girarDerecha()
        {
            moto.moveOrientedY(25);
            var rotAngle = FastMath.ToRad(90);
            moto.rotateY(rotAngle);

            
            pathlight.agregarSegmento(moto.Position);
            moto.moveOrientedY(-35);
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
            moto.moveOrientedY(35);
            pathlight.setSegmentoActual(moto.Position);
            moto.moveOrientedY(-35);
        }

        public PathLight getPathLight()
        {
            return pathlight;
        }

        public int getVelocidad()
        {
            return this.velocidad;
        }

        public TgcBoundingAxisAlignBox getBoundingBox()
        {
            return moto.BoundingBox;
        }

        public void mover(Vector3 matriz)
        {
            moto.move(matriz);
        }

        public bool coomprobarColisionPathLight(CustomVertex.PositionColored[] path)
        {
            for (int i = 0; i < path.Length; i += 3)
            {
                Vector3 a = new Vector3(path[i].X, path[i].Y, path[i].Z);
                Vector3 b = new Vector3(path[i + 1].X, path[i + 1].Y, path[i + 1].Z);
                Vector3 c = new Vector3(path[i + 2].X, path[i + 2].Y, path[i + 2].Z);

                if (TgcCollisionUtils.testTriangleAABB(a, b, c, this.getBoundingBox()))
                {
                    return true;
                }
            }
            return false;
        }


    }
}
