using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoF.cuentas
{
    internal class Cuenta
    {


        public string DNI { get; set; }
        public string PIN { get; set; }
        public decimal Saldo { get; set; }

        // es una lista de objetos del tipo transaccion.
        //La lista se inicializa como vacia cunado se modifica cuenta
        public List<transaccion> HistorialTransacciones { get; set; } = new List<transaccion>();


        public class transaccion
        {
            public DateTime Fecha { get; set; }
            public decimal Monto { get; set; }
            public TipoTransaccion Tipo { get; set; }

            //se crea un constructor que le asigne valores a transaccion
            public transaccion(decimal monto, TipoTransaccion tipo)
            {
                Fecha = DateTime.Now;
                Monto = monto;
                Tipo = tipo;
            }
        }

        //un enumerador que defina el tipo de transaccion que puede ser 0 1 o 2
        public enum TipoTransaccion
        {
            Retiro,
            Deposito,
            Consulta
        }

        public void RegistrarTransaccion(transaccion transaccion)
        {
            HistorialTransacciones.Add(transaccion);
        }

    }
}
