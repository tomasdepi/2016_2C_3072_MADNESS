using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Direct3D;

namespace TGC.Group.Model
{
    public class PathLight
    {

        private Vector3[] puntos = new Vector3[2000];
        private int cantPuntos;
        private Vector3 posMoto = new Vector3();
        private int altura;

        private int color = Color.Blue.ToArgb();

        public PathLight(Vector3 posMoto)
        {
            altura = 15;
            cantPuntos = 2;
            puntos[0] = posMoto;
            puntos[1] = posMoto;
        }

        public CustomVertex.PositionColored[] crearTriangulos()
        {
            var data = new CustomVertex.PositionColored[6 * (cantPuntos - 1)];

            for (int i = 1; i < cantPuntos; i++)
            {
                data[0 + (i - 1) * 6] = new CustomVertex.PositionColored(puntos[i - 1].X, puntos[i - 1].Y, puntos[i - 1].Z, color);
                data[1 + (i - 1) * 6] = new CustomVertex.PositionColored(puntos[i - 1].X, puntos[i - 1].Y + altura, puntos[i - 1].Z, color);
                data[2 + (i - 1) * 6] = new CustomVertex.PositionColored(puntos[i].X, puntos[i].Y, puntos[i].Z, color);

                data[3 + (i - 1) * 6] = new CustomVertex.PositionColored(puntos[i - 1].X, puntos[i - 1].Y + altura, puntos[i - 1].Z, color);
                data[4 + (i - 1) * 6] = new CustomVertex.PositionColored(puntos[i].X, puntos[i].Y + altura, puntos[i].Z, color);
                data[5 + (i - 1) * 6] = new CustomVertex.PositionColored(puntos[i].X, puntos[i].Y, puntos[i].Z, color);
            }

            return data;
        }

        public void setSegmentoActual(Vector3 posMoto)
        {
            puntos[cantPuntos - 1] = posMoto;
        }

        public void agregarSegmento(Vector3 posMoto)
        {
            puntos[cantPuntos - 1] = posMoto;
            cantPuntos++;
        }

        public int getCantPuntos()
        {
            return cantPuntos;
        }

        public int getCantTriangulos()
        {
            return (cantPuntos - 1) * 2;
        }

        public void cambiarColor(int c)
        {
            this.color = c;
        }
    }
}
