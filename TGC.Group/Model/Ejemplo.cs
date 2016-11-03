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

        private Moto moto;
        private Oponente oponente;
        private Oponente oponente2;
        private Oponente oponente3;

        private ControladorIA controladorIA;

        private camara camaraInterna;

        private SkyBox skyBoxTron;

        private bool keyLeftRightPressed;

        private TgcText2D texto;
        private TgcText2D textoModoDios;

        private bool perdido;

        private List<TgcMesh> cajas;

        private TgcPlane pisoPlane;
        private TgcMesh piso;
        private TgcTexture texturaPiso;

        private TgcMesh cajaConLuz;
        private Microsoft.DirectX.Direct3D.Effect efectoLuz;
        private Vector3[] posLuz;
        private Color[] colorLuz;

        private GestorPowerUps gestorPowerUps;
        

        public override void Init()
        {
            var d3dDevice = D3DDevice.Instance.Device;

            moto = new Moto(MediaDir, new Vector3(0, 0, 0));
            moto.init();

            texturaPiso = TgcTexture.createTexture(D3DDevice.Instance.Device, MediaDir + "SkyBoxTron\\bottom.png");
            pisoPlane = new TgcPlane();
            pisoPlane.Origin = new Vector3(-5000, 0, -5000);
            pisoPlane.Size = new Vector3(10000, 0, 10000);
            pisoPlane.Orientation = TgcPlane.Orientations.XZplane;
            pisoPlane.setTexture(texturaPiso);
            pisoPlane.updateValues();

            piso = pisoPlane.toMesh("piso");
            piso.AutoTransformEnable = true;

            camaraInterna = new camara(moto);
            Camara = camaraInterna;
            camaraInterna.rotateY(FastMath.ToRad(180));

            skyBoxTron = new SkyBox(MediaDir);
            skyBoxTron.init();

            texto = new TgcText2D();
            texto.Color = Color.Red;
            texto.Align = TgcText2D.TextAlign.LEFT;
            texto.Text = "Perdiste, toca la tecla R para reiniciar";
            texto.Size = new Size(700, 400);
            texto.Position = new Point(550, 150);

            textoModoDios = new TgcText2D();
            textoModoDios.Color = Color.Red;
            textoModoDios.Text = "Modo Dios Activado";
            textoModoDios.Position = new Point(0, 30);
            textoModoDios.Size = new Size(500, 200);

            controladorIA = new ControladorIA();
             
            this.generarOponentes(); 

            perdido = false;

            cajas = new List<TgcMesh>();
            cajaConLuz = new TgcSceneLoader().loadSceneFromFile(MediaDir + Game.Default.pathCajaMetalica).Meshes[0];
            cajaConLuz.Position = new Vector3(0, 0, -200);
            cajaConLuz.Scale = new Vector3(0.8f, 0.8f, 0.8f);
            efectoLuz = TgcShaders.loadEffect(ShadersDir + "MultiDiffuseLights.fx");

            this.generarCajas(20);

            controladorIA.setObstaculosEscenario(cajas);

            gestorPowerUps = new GestorPowerUps();

        }

        private void generarCajas(int cantidad)
        {
            for (int i = 0; i < cantidad; i++)
            {

                Random randomizador = new Random();
                var z = randomizador.Next(-1000, 1000);
                var x = randomizador.Next(-1000, 1000);

                TgcMesh caja = new TgcSceneLoader().loadSceneFromFile(MediaDir + Game.Default.pathCajaMetalica).Meshes[0];
                caja.Scale = new Vector3(0.5f, 0.5f, 0.5f);
                caja.move(new Vector3(x, 0, z));
                caja.setColor(Color.Blue);

                cajas.Add(caja);
            }
        }

        private void generarOponentes()
        {
            oponente = new Oponente(MediaDir, new Vector3(2000, 0, -1000));
            oponente2 = new Oponente(MediaDir, new Vector3(-2000, 0, -2200));
            oponente3 = new Oponente(MediaDir, new Vector3(1500, 0, -1600));

            oponente.init();
            oponente2.init();
            oponente3.init();

            oponente.getPathLight().cambiarColor(Color.Red.ToArgb());
            oponente2.getPathLight().cambiarColor(Color.Green.ToArgb());
            oponente3.getPathLight().cambiarColor(Color.Yellow.ToArgb());
            
            controladorIA.setJugador(moto);
            controladorIA.agregarOponente(oponente);
            controladorIA.agregarOponente(oponente2);
            controladorIA.agregarOponente(oponente3);
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

            var motos = new List<Moto>();
            motos.Add(moto);

            gestorPowerUps.actualizar(ElapsedTime, motos);

            if (!perdido) { 

                validarGiroDerecha();
                validarGiroIzquierda();
                validarSalto();
                validarTeclasGiroLevantadas();

                camaraInterna.rotarCamara(ElapsedTime);

                if (Input.keyUp(Key.G))
                {
                    moto.activarModoDios();
                }
            
                if (Input.keyDown(Key.Up))
                {
                    moto.acelerar(ElapsedTime);
                }

            }
            else
            {
                if (Input.keyUp(Key.R))
                {
                    this.reiniciarJuego();
                }
            }

            camaraInterna.seguirObjetivo(moto);
            
            if (controladorIA.comprobarColisionPathLight())
            {
                perdido = true;
            }
            
            controladorIA.atacarJugador(ElapsedTime);
        }

        public override void Render()
        {
            PreRender();
            /*
            piso.Effect = efectoLuz;
            piso.Technique = "MultiDiffuseLightsTechnique";

            var lightColors = new ColorValue[1];
            var pointLightPositions = new Vector4[1];
            var pointLightIntensity = new float[1];
            var pointLightAttenuation = new float[1];

            lightColors[0] = ColorValue.FromColor(Color.White);
            pointLightPositions[0] = TgcParserUtils.vector3ToVector4(cajaConLuz.Position);
            pointLightIntensity[0] = 40;
            pointLightAttenuation[0] = (float)0.15;

            piso.UpdateMeshTransform();

            piso.Effect.SetValue("lightColor", lightColors);
            piso.Effect.SetValue("lightPosition", pointLightPositions);
            piso.Effect.SetValue("lightIntensity", pointLightIntensity);
            piso.Effect.SetValue("lightAttenuation", pointLightAttenuation);
            piso.Effect.SetValue("materialEmissiveColor", Color.Black.ToArgb());
            piso.Effect.SetValue("materialDiffuseColor", Color.White.ToArgb());
           */

            piso.render();

            moto.render();

            controladorIA.renderOponentes();
            
            skyBoxTron.render();

            foreach(TgcMesh caja in cajas)
                caja.render();

            if(perdido)
            texto.render();
            

            if (moto.esDios()) textoModoDios.render();

            //cajaConLuz.render();

            gestorPowerUps.render(ElapsedTime);

            PostRender();
        }

        public override void Dispose()
        {
            moto.dispose();

            foreach (TgcMesh caja in cajas) caja.dispose();

            controladorIA.disposeOponentes();
            
            skyBoxTron.dispose();

            texto.Dispose();
            textoModoDios.Dispose();

            cajaConLuz.dispose();

            gestorPowerUps.dispose();

            pisoPlane.dispose();
            piso.dispose();
            
        }


        public void reiniciarJuego()
        {
            
            cajas.Clear();
            this.generarCajas(30);
            
            controladorIA.getOponentes().Clear();
            
            generarOponentes();

            camaraInterna = new camara(moto);
            Camara = camaraInterna;
            camaraInterna.rotateY(FastMath.ToRad(180));

            moto = new Moto(MediaDir, new Vector3(0, 0, 0));
            moto.init();
            controladorIA.setJugador(moto);

            perdido = false;
        }
    }
}
