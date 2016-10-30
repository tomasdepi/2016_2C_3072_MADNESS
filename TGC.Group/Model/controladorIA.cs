using Microsoft.DirectX.Direct3D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.SceneLoader;

namespace TGC.Group.Model
{
    class ControladorIA
    {

        private Moto jugador;
        private List<Oponente> oponentes;

        private List<CustomVertex.PositionColored[]> obstaculosPath;
        private List<TgcMesh> obstaculosEscenario;

        public ControladorIA()
        {
            oponentes = new List<Oponente>();
        }

        public void agregarOponente(Oponente o)
        {
            oponentes.Add(o);
        }

        public void setJugador(Moto j)
        {
            this.jugador = j;
        }

        public void setObstaculosEscenario(List<TgcMesh> obs)
        {
            this.obstaculosEscenario = obs;
        }

        public void atacarJugador(float ElapsedTime)
        {
            foreach(Oponente op in this.oponentes)
            {
                if(!op.haPerdido())op.seguirObjetivo(this.jugador, ElapsedTime, obstaculosPath);
            }
        }
        

        private void generarPathObstaculo()
        {

            List<CustomVertex.PositionColored[]> listaPath = new List<CustomVertex.PositionColored[]>();

            foreach (Oponente op in this.oponentes)
            {
                CustomVertex.PositionColored[] path = new CustomVertex.PositionColored[op.generarPathLight().Length];
                path = op.generarPathLight();
                listaPath.Add(path);
            }

            listaPath.Add(this.jugador.generarPathLight());

            this.obstaculosPath = listaPath;
        }

        public bool comprobarColisionPathLight()
        {
            
            if(this.jugador.coomprobarColisionPathLight(obstaculosPath)) return true;
            if (this.jugador.coomprobarColisionObstaculoEscenario(obstaculosEscenario)) return true;
            return false;
        }

        public void updateOponentes(float ElapsedTime)
        {
            foreach(Oponente op in this.oponentes)
            {
               if(!op.haPerdido()) op.update(ElapsedTime);
            }

            this.generarPathObstaculo();

            foreach (Oponente op in this.oponentes)
            {
                if (op.coomprobarColisionPathLight(obstaculosPath)) op.perder();
                if (op.coomprobarColisionObstaculoEscenario(obstaculosEscenario)) op.perder();
            }
        }

        public void renderOponentes()
        {
            foreach (Oponente o in oponentes)
            {
                o.render();
            }
        }

        public void disposeOponentes()
        {
            foreach(Oponente o in oponentes)
            {
                o.dispose();
            }
        }

        public List<Oponente> getOponentes()
        {
            return this.oponentes;
        }

        
    }
}
