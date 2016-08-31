using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Direct3D;
using TGC.Core.Example;
using TGC.Core.SceneLoader;
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

        private TgcScene currentScene;

        public override void Init()
        {
            var d3dDevice = D3DDevice.Instance.Device;

            moto = new TgcSceneLoader().loadSceneFromFile(MediaDir + Game.Default.pathAuto).Meshes[0];
            moto.Scale = new Vector3(0.5f, 0.5f, 0.5f);

            //declaro mi camara
            //var cameraPosition = new Vector3(0, 0, 125);
            //Quiero que la camara mire hacia el origen (0,0,0).
            //var lookAt = Vector3.Empty;
            //Configuro donde esta la posicion de la camara y hacia donde mira.
            //Camara.SetCamera(cameraPosition, lookAt);

            //defino una camara de tercera persona que sigue a la moto
            camaraInterna = new TgcThirdPersonCamera(moto.Position, 100, -150);
            Camara = camaraInterna;

            //cargo el escenario
            var pathEscenario = MediaDir + "PatioDeJuegos\\PatioDeJuegos-TgcScene.xml";
        }

        public override void Update()
        {
            PreUpdate();
            
        }
        public override void Render()
        {
            PreRender();

            //renderizo mi moto en la pantalla
            moto.render();

            PostRender();
        }

        public override void Dispose()
        {
            //destruyo mi moto
            moto.dispose();
        }
    }
}
