using Microsoft.DirectX.Direct3D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGC.Group.Model
{
    class ControladorIA
    {

        private Moto jugador;
        private List<Oponente> oponentes;

        private List<CustomVertex.PositionColored[]> obstaculosPath;

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

        public void atacarJugador(float ElapsedTime)
        {
            foreach(Oponente op in this.oponentes)
            {
                op.seguirObjetivo(this.jugador, ElapsedTime, obstaculosPath);
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
            
            return false;
        }

        public void updateOponentes(float ElapsedTime)
        {
            foreach(Oponente op in this.oponentes)
            {
                op.update(ElapsedTime);
            }

            this.generarPathObstaculo();
        }


        public void renderOponentes()
        {
            oponentes.ForEach(renderOponente);
        }

        private void renderOponente(Oponente o)
        {
            o.render();
        }

        public void disposeOponentes()
        {
            oponentes.ForEach(disposeOponente);
        }

        private void disposeOponente(Oponente o)
        {
            o.dispose();
        }
    }
}
