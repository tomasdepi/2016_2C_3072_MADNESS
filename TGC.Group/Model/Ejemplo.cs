﻿using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX.DirectInput;
using System.Drawing;
using TGC.Core.Collision;
using TGC.Core.Direct3D;
using TGC.Core.Example;
using TGC.Core.Geometry;
using TGC.Core.SceneLoader;
using TGC.Core.Terrain;
using TGC.Core.Text;
using TGC.Core.Textures;
using TGC.Core.Utils;
using TGC.Examples.Camara;

namespace TGC.Group.Model
{
    public class Ejemplo : TgcExample
    {


        public Ejemplo(string mediaDir, string shadersDir) : base(mediaDir, shadersDir)
        {
            Category = Game.Default.Category;
            Name = Game.Default.Name;
            Description = Game.Default.Description;
        }

        //declaro el mesh que representa a la moto
        private Moto moto;
        private Oponente oponente;


        private TgcThirdPersonCamera camaraInterna;

        private SkyBox skyBoxTron;

        private bool keyLeftRightPressed;

        //variables de camara
        private int velocidadRotacionCamara;
        private float anguloRotado;
        private bool camaraRotando;
        private int sentidoRotacion;

        //variables path
        private VertexBuffer vertexBuffer;

        private TgcText2D texto;

        private bool perdido;

        public override void Init()
        {
            var d3dDevice = D3DDevice.Instance.Device;

            moto = new Moto(MediaDir, new Vector3(0, -5000, 0));
            moto.init();

            oponente = new Oponente(MediaDir, new Vector3(100, -5000, 0));
            oponente.init();
            oponente.getPathLight().cambiarColor(Color.Red.ToArgb());


            //defino una camara de tercera persona que sigue a la moto
            camaraInterna = new TgcThirdPersonCamera(moto.getPosicion(), 60, -170);
            Camara = camaraInterna;
            camaraInterna.rotateY(FastMath.ToRad(180));

            keyLeftRightPressed = false;
            velocidadRotacionCamara = 100; //grados

            anguloRotado = 0;
            camaraRotando = false;

            skyBoxTron = new SkyBox(MediaDir);
            skyBoxTron.init();

            texto = new TgcText2D();
            texto.Color = Color.Red;
            texto.Align = TgcText2D.TextAlign.LEFT;
            texto.Text = "Perdiste";
            texto.Size = new Size(500, 200);
            texto.Position = new Point(650, 250);

            perdido = false;

            vertexBuffer = new VertexBuffer(typeof(CustomVertex.PositionColored), 3, D3DDevice.Instance.Device,
               Usage.Dynamic | Usage.WriteOnly, CustomVertex.PositionColored.Format, Pool.Default);

        }

        private void rotarCamaraIzquierda()
        {
            anguloRotado = 0;
            camaraRotando = true;
            sentidoRotacion = -1;
        }

        private void rotarCamaraDerecha()
        {
            anguloRotado = 0;
            camaraRotando = true;
            sentidoRotacion = 1;
        }

        private void corregirDesfasaje(float rotacion)
        {
            camaraInterna.rotateY(FastMath.ToRad(sentidoRotacion * rotacion));
        }

        private void rotarCamara()
        {
            if (camaraRotando)
            {
                var rotacion = velocidadRotacionCamara * ElapsedTime;
                camaraInterna.rotateY(sentidoRotacion * FastMath.ToRad(rotacion));
                anguloRotado += rotacion;
                if(anguloRotado >= 90)
                {
                    camaraRotando = false;
                    corregirDesfasaje(anguloRotado - 90);
                }
            }
            
        }

        private void validarGiroIzquierda()
        {
            if (Input.keyDown(Key.Left) && !keyLeftRightPressed && !camaraRotando)
            {

                moto.girarIzquierda();

                rotarCamaraIzquierda();

                keyLeftRightPressed = true;
            }

        }

        private void validarGiroDerecha()
        {
            if (Input.keyDown(Key.Right) && !keyLeftRightPressed && !camaraRotando)
            {

                moto.girarDerecha();
                
                //camaraInterna.rotateY(-rotAngle);
                rotarCamaraDerecha();

                keyLeftRightPressed = true;
            }
        }
        
        private void validarTeclasGiroLevantadas()
        {
            if (keyLeftRightPressed)
            {
                if (Input.keyUp(Key.Right) || Input.keyUp(Key.Left))
                {
                    keyLeftRightPressed = false;
                }
            }
        }

        public override void Update()
        {
            PreUpdate();

            if (!perdido) { 

                validarGiroDerecha();
                validarGiroIzquierda();
                validarTeclasGiroLevantadas();

                rotarCamara();

                if (Input.keyDown(Key.A))
                {
                    oponente.acelerar(ElapsedTime);
                }

                if (Input.keyDown(Key.Up))
                {
                    moto.acelerar(ElapsedTime);
                }

            }
            //actualizo la camara para que siga a la moto
            camaraInterna.Target = moto.getPosicion();

            //actualizo vertex buffer
            moto.actualizarPuntoPathLight();
            oponente.actualizarPuntoPathLight();

            CustomVertex.PositionColored[] path = new CustomVertex.PositionColored[moto.generarPathLight().Length + oponente.generarPathLight().Length];
            moto.generarPathLight().CopyTo(path, 0);
            oponente.generarPathLight().CopyTo(path, moto.generarPathLight().Length);

            //for(int i=0; i<path.Length; i += 3)
            // {
            //     Vector3 a = new Vector3(path[i].X, path[i].Y, path[i].Z);
            //     Vector3 b = new Vector3(path[i+1].X, path[i+1].Y, path[i+1].Z);
            //     Vector3 c = new Vector3(path[i+2].X, path[i+2].Y, path[i+2].Z);

            //     if (TgcCollisionUtils.testTriangleAABB(a, b, c, moto.getBoundingBox())){
            //         path[i] = new CustomVertex.PositionColored(a.X, a.Y, a.Z, Color.Red.ToArgb());
            //         path[i+1] = new CustomVertex.PositionColored(b.X, b.Y, b.Z, Color.Red.ToArgb());
            //         path[i+2] = new CustomVertex.PositionColored(c.X, c.Y, c.Z, Color.Red.ToArgb());
            //     }
            // }

            vertexBuffer.SetData(path, 0, LockFlags.None);
            if (moto.coomprobarColisionPathLight(path))
            {
                perdido = true;
            }

            oponente.seguirObjetivo(moto, ElapsedTime);
           
        }


        public override void Render()
        {
            PreRender();

            //renderizo mi moto en la pantalla
            moto.render();

            oponente.render();
            
            skyBoxTron.render();

            if(perdido)
            texto.render();        

            PostRender();
        }

        public override void Dispose()
        {
            //destruyo mi moto
            moto.dispose();

            oponente.dispose();
            
            skyBoxTron.dispose();

            texto.Dispose();
            
        }
    }
}
