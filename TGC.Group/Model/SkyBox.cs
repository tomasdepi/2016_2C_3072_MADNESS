using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Terrain;

namespace TGC.Group.Model
{
    public class SkyBox
    {
        private TgcSkyBox skybox;
        private string MediaDir;

        public SkyBox(string mediaPath)
        {
            this.MediaDir = mediaPath;
        }


        public void init()
        {
            skybox = new TgcSkyBox();
            skybox.Center = new Vector3(0, 0, 0);
            skybox.Size = new Vector3(10000, 10000, 10000);

            skybox.setFaceTexture(TgcSkyBox.SkyFaces.Up, MediaDir + "SkyBoxTron\\bottom.png");
            skybox.setFaceTexture(TgcSkyBox.SkyFaces.Down, MediaDir + "SkyBoxTron\\bottom.png");
            skybox.setFaceTexture(TgcSkyBox.SkyFaces.Left, MediaDir + "SkyBoxTron\\Pared.jpg");
            skybox.setFaceTexture(TgcSkyBox.SkyFaces.Right, MediaDir + "SkyBoxTron\\Pared.jpg");
            skybox.setFaceTexture(TgcSkyBox.SkyFaces.Front, MediaDir + "SkyBoxTron\\Pared.jpg");
            skybox.setFaceTexture(TgcSkyBox.SkyFaces.Back, MediaDir + "SkyBoxTron\\Pared.jpg");
            skybox.SkyEpsilon = 25f;
            skybox.Init();

        }

        public void render()
        {
            skybox.render();
        }

        public void dispose()
        {
            skybox.dispose();
        }
    }
}
