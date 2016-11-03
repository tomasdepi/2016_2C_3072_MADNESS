using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGC.Group.Model
{
    public class GestorPowerUps
    {

        List<PowerUp> poderes;
        List<PowerUp> poderesUsados;

        private int cantidad;
        private int cantMaxima;

        private Random random;

        public GestorPowerUps()
        {
            poderes = new List<PowerUp>();
            poderesUsados = new List<PowerUp>();

            cantidad = 0;
            cantMaxima = 10;

            random = new Random();

        }

        public void actualizar(float ElapsedTime, List<Moto> motos)
        {
            if(cantidad < cantMaxima)
            {
                var pos = new Vector3 (random.Next(-3000, 3000), 0, random.Next(-3000, 3000));
                var tipo = random.Next();

                if(tipo%2 == 0)
                {
                    poderes.Add(new PowerUpBoost(pos));
                }
                else
                {
                    poderes.Add(new PowerUpInvensible(pos));
                }

              
                cantidad++;
            }


            foreach(PowerUp poder in poderesUsados)
            {
                poder.actualizarTiempo(ElapsedTime);
            }

            foreach(PowerUp poder in poderes)
            {
                foreach(Moto moto in motos)
                {
                    if (poder.comprobarColisionConMoto(moto)){
                        poderes.Remove(poder);
                        cantidad--;
                        poderesUsados.Add(poder);
                        return;
                    }   
                }
            }

        }

        public void render(float ElapsedTime)
        {
            foreach(PowerUp poder in poderes)
            {
                poder.animar(ElapsedTime);
                poder.render();
            }      
        }

        public void dispose()
        {
            foreach (PowerUp poder in poderes)
            {
                poder.dispose();
            }

            foreach (PowerUp poder in poderesUsados)
            {
                poder.dispose();
            }
        }

    }
}
