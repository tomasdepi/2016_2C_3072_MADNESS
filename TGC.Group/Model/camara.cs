using Microsoft.DirectX;
using TGC.Core.Camara;
using TGC.Core.SceneLoader;
using TGC.Core.Utils;
using TGC.Group.Model;

namespace TGC.Examples.Camara
{
    /// <summary>
    ///     Camara en tercera persona que sigue a un objeto a un determinada distancia.
    /// </summary>
    public class camara : TgcCamera
    {
        private Vector3 position;
        private TgcMesh objetivo;

        private int velocidadRotacionCamara;
        private float anguloRotado;
        private bool camaraRotando;
        private int sentidoRotacion;

        /// <summary>
        ///     Crear una nueva camara
        /// </summary>
        public camara()
        {
            resetValues();
        }

        public camara(Moto objetivo) : this()
        {
            Target = objetivo.getPosicion();
            OffsetHeight = 45;
            OffsetForward = -170;

            velocidadRotacionCamara = 100; //grados
            anguloRotado = 0;
            camaraRotando = false;
        }

        public camara(Vector3 target, float offsetHeight, float offsetForward) : this()
        {
            Target = target;
            OffsetHeight = offsetHeight;
            OffsetForward = offsetForward;
        }

        public camara(Vector3 target, Vector3 targetDisplacement, float offsetHeight, float offsetForward)
            : this()
        {
            Target = target;
            TargetDisplacement = targetDisplacement;
            OffsetHeight = offsetHeight;
            OffsetForward = offsetForward;
        }

        /// <summary>
        ///     Desplazamiento en altura de la camara respecto del target
        /// </summary>
        public float OffsetHeight { get; set; }

        /// <summary>
        ///     Desplazamiento hacia adelante o atras de la camara repecto del target.
        ///     Para que sea hacia atras tiene que ser negativo.
        /// </summary>
        public float OffsetForward { get; set; }

        /// <summary>
        ///     Desplazamiento final que se le hace al target para acomodar la camara en un cierto
        ///     rincon de la pantalla
        /// </summary>
        public Vector3 TargetDisplacement { get; set; }

        /// <summary>
        ///     Rotacion absoluta en Y de la camara
        /// </summary>
        public float RotationY { get; set; }

        /// <summary>
        ///     Objetivo al cual la camara tiene que apuntar
        /// </summary>
        public Vector3 Target { get; set; }

        public override void UpdateCamera(float elapsedTime)
        {
            Vector3 targetCenter;
            CalculatePositionTarget(out position, out targetCenter);
            SetCamera(position, targetCenter);
        }

        /// <summary>
        ///     Carga los valores default de la camara y limpia todos los cálculos intermedios
        /// </summary>
        public void resetValues()
        {
            OffsetHeight = 20;
            OffsetForward = -120;
            RotationY = 0;
            TargetDisplacement = Vector3.Empty;
            Target = Vector3.Empty;
            position = Vector3.Empty;
        }

        /// <summary>
        ///     Configura los valores iniciales de la cámara
        /// </summary>
        /// <param name="target">Objetivo al cual la camara tiene que apuntar</param>
        /// <param name="offsetHeight">Desplazamiento en altura de la camara respecto del target</param>
        /// <param name="offsetForward">Desplazamiento hacia adelante o atras de la camara repecto del target.</param>
        public void setTargetOffsets(Vector3 target, float offsetHeight, float offsetForward)
        {
            Target = target;
            OffsetHeight = offsetHeight;
            OffsetForward = offsetForward;
        }

        /// <summary>
        ///     Genera la proxima matriz de view, sin actualizar aun los valores internos
        /// </summary>
        /// <param name="pos">Futura posicion de camara generada</param>
        /// <param name="pos">Futuro centro de camara a generada</param>
        public void CalculatePositionTarget(out Vector3 pos, out Vector3 targetCenter)
        {
            //alejarse, luego rotar y lueg ubicar camara en el centro deseado
            targetCenter = Vector3.Add(Target, TargetDisplacement);
            var m = Matrix.Translation(0, OffsetHeight, OffsetForward) * Matrix.RotationY(RotationY) *
                    Matrix.Translation(targetCenter);

            //Extraer la posicion final de la matriz de transformacion
            pos.X = m.M41;
            pos.Y = m.M42;
            pos.Z = m.M43;
        }

        /// <summary>
        ///     Rotar la camara respecto del eje Y
        /// </summary>
        /// <param name="angle">Ángulo de rotación en radianes</param>
        public void rotateY(float angle)
        {
            RotationY += angle;
        }

        public void rotarCamaraIzquierda()
        {
            anguloRotado = 0;
            camaraRotando = true;
            sentidoRotacion = -1;
        }

        public void rotarCamaraDerecha()
        {
            anguloRotado = 0;
            camaraRotando = true;
            sentidoRotacion = 1;
        }

        private void corregirDesfasaje(float rotacion)
        {
            this.rotateY(FastMath.ToRad(sentidoRotacion * rotacion));
        }

        public void rotarCamara(float ElapsedTime)
        {
            if (camaraRotando)
            {
                var rotacion = velocidadRotacionCamara * ElapsedTime;
                this.rotateY(sentidoRotacion * FastMath.ToRad(rotacion));
                anguloRotado += rotacion;
                if (anguloRotado >= 90)
                {
                    camaraRotando = false;
                    corregirDesfasaje(anguloRotado - 90);
                }
            }

        }

        public void seguirObjetivo(Moto objetivo)
        {
            this.Target = objetivo.getPosicion();
        }

        public bool estaCamaraRotando()
        {
            return this.camaraRotando;
        }
    }
}