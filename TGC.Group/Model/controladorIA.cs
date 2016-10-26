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
                op.seguirObjetivo(this.jugador, ElapsedTime, null);
            }
        }
        

        private List<CustomVertex.PositionColored[]> generarPathObstaculo()
        {

            List<CustomVertex.PositionColored[]> listaPath = new List<CustomVertex.PositionColored[]>();

            foreach (Oponente op in this.oponentes)
            {
                CustomVertex.PositionColored[] path = new CustomVertex.PositionColored[op.generarPathLight().Length];
                path = op.generarPathLight();
                listaPath.Add(path);
            }

            return listaPath;
        }

        public bool comprobarColisionPathLight()
        {
            if (this.jugador.coomprobarColisionPathLight(jugador.generarPathLight())) return true;

            List<CustomVertex.PositionColored[]> obstaculos = generarPathObstaculo();

            foreach (CustomVertex.PositionColored[] path in obstaculos)
            {
                if(this.jugador.coomprobarColisionPathLight(path)) return true;
            }
            return false;
        }

        public void updateOponentes(float ElapsedTime)
        {
            foreach(Oponente op in this.oponentes)
            {
                op.update(ElapsedTime);
            }
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
