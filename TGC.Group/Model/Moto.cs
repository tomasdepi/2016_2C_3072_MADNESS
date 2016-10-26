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
        

        private PathLight pathlight;

        private VertexBuffer vertexbuffer;
        private Vector3 posInicial;

        private int rotando;
        private float velocidadRotacion;
        private float anguloRotado;

        private int velocidadMaxima;
        private float velocidad;

        private float posY; //para saltar
        private int distMaxSalto;
        private int velocidadSalto;
        private int saltando; //0 no salta, 1 sube, -1 baja 


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
            
            velocidad = 0;
            velocidadMaxima = 250;

            rotando = 0;
            velocidadRotacion = 25;
            anguloRotado = 0;

            posY = 0;
            distMaxSalto = 50;
            velocidadSalto = 40;
            saltando = 0;

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

            rotar(-1);
        }

        public void girarDerecha()
        {
            moto.moveOrientedY(35);
            var rotAngle = FastMath.ToRad(90);
            moto.rotateY(rotAngle);

            pathlight.agregarSegmento(moto.Position);
            moto.moveOrientedY(-35);

            rotar(1);
        }

        private void rotar(int sentido)
        {
            moto.rotateZ(FastMath.ToRad(sentido * (45 - sentido * anguloRotado)));
            rotando = sentido;
            anguloRotado = 45 * sentido;
        }

        public void update(float ElepsedTime)
        {

            if (rotando != 0)
            {
                moto.rotateZ(FastMath.ToRad(-rotando * velocidadRotacion * ElepsedTime));
                if (rotando * anguloRotado < 0)
                {
                    rotando = 0;
                }
            }

            anguloRotado += ElepsedTime * velocidadRotacion * -rotando;

            if (this.velocidad > 0) this.velocidad -= (float)0.3;
            moto.moveOrientedY(-1 * velocidad * ElepsedTime);
            this.actualizarPuntoPathLight();


            if (saltando != 0)
            {
                this.moto.move(new Vector3(0, saltando * velocidadSalto * ElepsedTime, 0));
                posY += saltando * velocidadSalto * ElepsedTime;

                if (saltando == 1 && posY > distMaxSalto)
                {
                    saltando = -1;
                    this.retroceder((float)0.3);
                    pathlight.agregarSegmento(moto.Position);
                    this.avanzar((float)0.3);
                }
                if (saltando == -1 && posY < 0) saltando = 0;
            }
            
            
        }


        public Vector3 getPosicion()
        {
            return moto.Position;
        }

        public void acelerar(float ElepsedTime)
        {
            this.velocidad = velocidadMaxima < this.velocidad + 1 ? velocidadMaxima : this.velocidad + 1; 
            //moto.moveOrientedY(-1 * velocidad * ElepsedTime);
            //this.actualizarPuntoPathLight();
        }

        public void avanzar(float ElepsedTime)
        {
            moto.moveOrientedY(-1 * velocidad * ElepsedTime);
        }

        public void retroceder(float ElepsedTime)
        {
            moto.moveOrientedY(velocidad * ElepsedTime);
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

        public float getVelocidad()
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

        public bool coomprobarColisionPathLight(List<CustomVertex.PositionColored[]> path)
        {
            foreach(CustomVertex.PositionColored[] p in path)
            {
                for (int i = 0; i < p.Length; i += 3)
                {
                    Vector3 a = new Vector3(p[i].X, p[i].Y, p[i].Z);
                    Vector3 b = new Vector3(p[i + 1].X, p[i + 1].Y, p[i + 1].Z);
                    Vector3 c = new Vector3(p[i + 2].X, p[i + 2].Y, p[i + 2].Z);

                    if (TgcCollisionUtils.testTriangleAABB(a, b, c, this.getBoundingBox()))
                    {
                        return true;
                    }
                }
               
            }
            return false;
        }


        public double distancia(Vector3 punto)
        {
            return Math.Sqrt(
                Math.Pow(this.getPosicion().X - punto.X, 2) +
                Math.Pow(this.getPosicion().Z - punto.Z, 2)
            );
        }

        public void saltar(float ElapsedTime)
        {
            if(saltando == 0)
            {
                saltando = 1;
                this.retroceder((float)0.3);
                pathlight.agregarSegmento(moto.Position);
                this.avanzar((float)0.3);
            }
            
        }

        public bool estaSaltando()
        {
            return saltando == 0 ? true : false; 
        }
        


    }
}
