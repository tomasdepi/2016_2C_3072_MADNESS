using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX.DirectInput;
using System;
using System.Collections.Generic;
using System.Drawing;
using TGC.Core.Collision;
using TGC.Core.Direct3D;
using TGC.Core.Example;
using TGC.Core.Geometry;
using TGC.Core.SceneLoader;
using TGC.Core.Shaders;
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
        private Oponente oponente2;
        private Oponente oponente3;

        private ControladorIA controladorIA;

        private camara camaraInterna;

        private SkyBox skyBoxTron;

        private bool keyLeftRightPressed;
        

        private TgcText2D texto;

        private bool perdido;
        
        private List<TgcMesh> cajas;
        private Microsoft.DirectX.Direct3D.Effect efectoLuzCaja;

        public override void Init()
        {
            var d3dDevice = D3DDevice.Instance.Device;

            moto = new Moto(MediaDir, new Vector3(0, -5000, 0));
            moto.init();

            oponente = new Oponente(MediaDir, new Vector3(1000, -5000, 0));
            oponente.init();
            oponente.getPathLight().cambiarColor(Color.Red.ToArgb());

            oponente2 = new Oponente(MediaDir, new Vector3(500, -5000, 0));
            oponente2.init();
            oponente2.getPathLight().cambiarColor(Color.Green.ToArgb());

            oponente3 = new Oponente(MediaDir, new Vector3(200, -5000, 0));
            oponente3.init();
            oponente3.getPathLight().cambiarColor(Color.Yellow.ToArgb());

            controladorIA = new ControladorIA();
            controladorIA.setJugador(moto);
            controladorIA.agregarOponente(oponente);
            //controladorIA.agregarOponente(oponente2);
            //controladorIA.agregarOponente(oponente3);

            //defino una camara de tercera persona que sigue a la moto
            camaraInterna = new camara(moto);
            Camara = camaraInterna;
            camaraInterna.rotateY(FastMath.ToRad(180));

            skyBoxTron = new SkyBox(MediaDir);
            skyBoxTron.init();

            texto = new TgcText2D();
            texto.Color = Color.Red;
            texto.Align = TgcText2D.TextAlign.LEFT;
            texto.Text = "Perdiste";
            texto.Size = new Size(500, 200);
            texto.Position = new Point(650, 250);

            perdido = false;

            cajas = new List<TgcMesh>();

            for(int i=0; i<20; i++)
            {

                Random randomizador = new Random();
                var z = randomizador.Next(-1000, 1000);
                var x = randomizador.Next(-1000, 1000);

                TgcMesh caja = new TgcSceneLoader().loadSceneFromFile(MediaDir + Game.Default.pathCajaMetalica).Meshes[0];
                caja.Scale = new Vector3(0.5f, 0.5f, 0.5f);
                caja.move(new Vector3(x, -4970, z));
                caja.setColor(Color.Blue);

                cajas.Add(caja);
            }

            controladorIA.setObstaculosEscenario(cajas);

            efectoLuzCaja = TgcShaders.Instance.TgcMeshPointLightShader;
        }


        private void validarGiroIzquierda()
        {
            if (Input.keyDown(Key.Left) && !keyLeftRightPressed && !camaraInterna.estaCamaraRotando())
            {
                if (moto.estaSaltando())
                {
                    moto.girarIzquierda();
                    camaraInterna.rotarCamaraIzquierda();
                    keyLeftRightPressed = true;
                }
               
            }

        }

        private void validarGiroDerecha()
        {
            if (Input.keyDown(Key.Right) && !keyLeftRightPressed && !camaraInterna.estaCamaraRotando())
            {
                if (moto.estaSaltando())
                {
                    moto.girarDerecha();
                    camaraInterna.rotarCamaraDerecha();
                    keyLeftRightPressed = true;
                }
              
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

        private void validarSalto()
        {
            if (Input.keyDown(Key.Space))
            {
                moto.saltar(ElapsedTime);
            }
        }

        public override void Update()
        {
            PreUpdate();
            
            moto.update(ElapsedTime);
            controladorIA.updateOponentes(ElapsedTime);

            if (!perdido) { 

                validarGiroDerecha();
                validarGiroIzquierda();
                validarSalto();
                validarTeclasGiroLevantadas();

                camaraInterna.rotarCamara(ElapsedTime);

                if (Input.keyDown(Key.Up))
                {
                    moto.acelerar(ElapsedTime);
                }

            }

            camaraInterna.seguirObjetivo(moto);
            

            //CustomVertex.PositionColored[] path = new CustomVertex.PositionColored[moto.generarPathLight().Length + oponente.generarPathLight().Length];
            ///moto.generarPathLight().CopyTo(path, 0);
            //oponente.generarPathLight().CopyTo(path, moto.generarPathLight().Length);

            //vertexBuffer.SetData(path, 0, LockFlags.None);
            if (controladorIA.comprobarColisionPathLight())
            {
                perdido = true;
            }

            //oponente.seguirObjetivo(moto, ElapsedTime, path);
            controladorIA.atacarJugador(ElapsedTime);
        }

        public override void Render()
        {
            PreRender();

            //renderizo mi moto en la pantalla
            moto.render();

            controladorIA.renderOponentes();
            
            skyBoxTron.render();

            foreach(TgcMesh caja in cajas)
                caja.render();

            if(perdido)
            texto.render();        

            PostRender();
        }

        public override void Dispose()
        {
            //destruyo mi moto
            moto.dispose();

            foreach (TgcMesh caja in cajas) caja.dispose();

            controladorIA.disposeOponentes();
            
            skyBoxTron.dispose();

            texto.Dispose();

        }
    }
}
