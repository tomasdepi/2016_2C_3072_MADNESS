using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX.DirectInput;
using TGC.Core.Direct3D;
using TGC.Core.Example;
using TGC.Core.Geometry;
using TGC.Core.SceneLoader;
using TGC.Core.Terrain;
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
        private TgcMesh moto;

        private TgcThirdPersonCamera camaraInterna;

        private SkyBox skyBoxTron;

        private bool keyLeftRightPressed;

        //variables de camara
        private int velocidadRotacionCamara;
        private float anguloRotado;
        private bool camaraRotando;
        private int sentidoRotacion;

        public override void Init()
        {
            var d3dDevice = D3DDevice.Instance.Device;

            moto = new TgcSceneLoader().loadSceneFromFile(MediaDir + Game.Default.pathAuto).Meshes[0];
            moto.Scale = new Vector3(0.5f, 0.5f, 0.5f);
            moto.move(new Vector3(0, -5000, 0));

            //declaro mi camara
            //var cameraPosition = new Vector3(0, 0, 125);
            //Quiero que la camara mire hacia el origen (0,0,0).
            //var lookAt = Vector3.Empty;
            //Configuro donde esta la posicion de la camara y hacia donde mira.
            //Camara.SetCamera(cameraPosition, lookAt);

            //defino una camara de tercera persona que sigue a la moto
            camaraInterna = new TgcThirdPersonCamera(moto.Position, 80, -150);
            Camara = camaraInterna;
            camaraInterna.rotateY(FastMath.ToRad(180));

            keyLeftRightPressed = false;
            velocidadRotacionCamara = 100; //grados

            anguloRotado = 0;
            camaraRotando = false;

            skyBoxTron = new SkyBox(MediaDir);
            skyBoxTron.init();
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
                var rotAngle = FastMath.ToRad(-90);
                moto.rotateY(rotAngle);
                //camaraInterna.rotateY(-rotAngle);
                rotarCamaraIzquierda();

                keyLeftRightPressed = true;
            }

        }

        private void validarGiroDerecha()
        {
            if (Input.keyDown(Key.Right) && !keyLeftRightPressed && !camaraRotando)
            {
                var rotAngle = FastMath.ToRad(90);
                moto.rotateY(rotAngle);
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

            validarGiroDerecha();
            validarGiroIzquierda();
            validarTeclasGiroLevantadas();

            rotarCamara();

            if (Input.keyDown(Key.Up))
            {
                moto.moveOrientedY(-100 * ElapsedTime);
            }

            if (Input.keyDown(Key.Down))
            {
                moto.moveOrientedY(100 * ElapsedTime);
            }
         


            //actualizo la camara para que siga a la moto
            camaraInterna.Target = moto.Position;


        }
        public override void Render()
        {
            PreRender();

            //renderizo mi moto en la pantalla
            moto.render();
            
            skyBoxTron.render();
            PostRender();
        }

        public override void Dispose()
        {
            //destruyo mi moto
            moto.dispose();
            
            skyBoxTron.dispose();
        
        }
    }
}
