using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ProyectoF.cuentas;
using Newtonsoft.Json;
using System.Security.Principal;
using static ProyectoF.cuentas.Cuenta;


namespace ProyectoF
{
    internal class Program
    {
        static void Main()
        {
            //se crea el menu de inicio dando tres posibles opciones.
            //la variable de seleccion se almacena en un espacio de memoria
            //se llama al metodo seleccionado mediante un switch
            Console.WriteLine("Cajero Automatico");

            while (true)
            {
                Console.WriteLine("\nSeleccione una opción:");
                Console.WriteLine("1. Crear cuenta");
                Console.WriteLine("2. Iniciar sesión");
                Console.WriteLine("3. Salir");

                int opcion = Convert.ToInt32(Console.ReadLine());



                switch (opcion)
                {
                    case 1:
                        CrearCuenta();
                        break;
                    case 2:
                        IniciarSesion();
                        break;
                    case 3:
                        Console.WriteLine("Gracias por preferirnos");
                        return;
                    default:
                        Console.WriteLine("Opción inválida. Por favor, seleccione una opción válida.");
                        break;
                }
            }
        }

        //se crean metodos publicos por su accesibilidad
        public static void CrearCuenta()
        {
            //se crea una variable cadena definiendo la ruta donde se almacenan las cuentas
            string path = @"C:\Users\User\Desktop\Proyecto Final\ProyectoF\ProyectoF\cuentas\cuentas.json";

            // se le solicita al usuario ingresar PIN y DNI, asignandolos como variables de tipo cadena
            Console.WriteLine("\nCreación de una nueva cuenta.");

            Console.WriteLine("\nCreación de una nueva cuenta.");

            Console.WriteLine("Ingrese su DNI:");
            string dni = Console.ReadLine();

            Console.WriteLine("Ingrese su PIN:");
            string pin = Console.ReadLine();

            //el saldo inicial se almacena como dato de tipo decimal
            Console.WriteLine("Ingrese su saldo inicial:");
            decimal saldoInicial = Convert.ToDecimal(Console.ReadLine());

            //se crea una nueva lista llamada cuentas que contendrá objetos del tipo Cuenta
            List<Cuenta> cuentas = new List<Cuenta>();

            try
            {
                //comprueva la existencia del archivo
                if (File.Exists(path))

                //lee y deserializa en la lista cuentas
                {
                    string json = File.ReadAllText(path);
                    cuentas = JsonConvert.DeserializeObject<List<Cuenta>>(json);
                    //lee y verifica la existencia de los datos ingresados en una cuenta ya creada
                    foreach (var cuenta in cuentas)
                    {
                        if (cuenta.DNI == dni && cuenta.PIN == pin)
                        {
                            Console.WriteLine("La cuenta ya existe, elige otro PIN");
                            return;
                        }
                    }
                }

                //se crea una nueva instancia de la clase cuenta con los datos ingresados
                Cuenta nuevaCuenta = new Cuenta
                {
                    DNI = dni,
                    PIN = pin,
                    Saldo = saldoInicial
                };

                //se agreaga la nueva cuenta a cuentas
                cuentas.Add(nuevaCuenta);

                // Se convierte la lista de cuentas a formato JSON y se escribe en el archivo.
                string jsonToWrite = JsonConvert.SerializeObject(cuentas);
                File.WriteAllText(path, jsonToWrite);


                Console.WriteLine("Cuenta creada exitosamente.");
            }
            catch (IOException e)
            {
                Console.WriteLine("Error al escribir en el archivo: " + e.Message);
            }
        }



        public static void IniciarSesion()
        {

            //solicita los datos nesesarios para el ingreso de secion
            string path = @"C:\Users\User\Desktop\Proyecto Final\ProyectoF\ProyectoF\cuentas\cuentas.json";
            Console.WriteLine("\nInicio de sesión.");

            Console.WriteLine("Ingrese su DNI:");
            string dni = Console.ReadLine();

            Console.WriteLine("Ingrese su PIN:");
            string pin = Console.ReadLine();

            try
            {
                //muestra la esepcion si los datos ingresados no se encuentran en el archivo
                if (!File.Exists(path))
                {
                    return;
                }

                // Lee el contendo del archivo cuentas.json
                string json = File.ReadAllText(path);

                //Transforma el JSON. en una lista de objetos 
                List<Cuenta> cuentas = JsonConvert.DeserializeObject<List<Cuenta>>(json);

                //lee y verifica la existencia de los datos ingresados en una cuenta ya creada
                foreach (var cuenta in cuentas)
                {
                    if (cuenta.DNI == dni && cuenta.PIN == pin)
                    {
                        //crea un bucle en el menu para que este se presenta tras realizar una transaccion
                        while (true)
                        {
                            Console.WriteLine("\nInicio exitoso");
                            Console.WriteLine("1. Consulta de saldo");
                            Console.WriteLine("2. Retiro");
                            Console.WriteLine("3. Deposito");
                            Console.WriteLine("4. Historial");
                            Console.WriteLine("5. Cerrar secion");

                            int opcion = Convert.ToInt32(Console.ReadLine());

                            // Se utiliza un switch para determinar qué acción realizar según la opción elegida
                            switch (opcion)
                            {
                                case 1:
                                    ConsultarSaldo(cuenta);
                                    break;
                                case 2:
                                    RealizarRetiro(cuenta, cuentas);
                                    break;
                                case 3:
                                    RealizarDeposito(cuenta, cuentas);
                                    break;
                                case 4:
                                    VerHistorial(cuenta);

                                    break;
                                case 5:
                                    // Se convierte la lista de cuentas a formato JSON y se escribe en el archivo
                                    string nuevoJson = JsonConvert.SerializeObject(cuentas);
                                    File.WriteAllText(path, nuevoJson);
                                    return;
                                default:
                                    Console.WriteLine("Opción inválida. Por favor, seleccione una opción válida.");
                                    break;
                            }
                        }
                    }
                }
                //errores

                Console.WriteLine("Cuenta no encontrada.");
            }
            catch (IOException e)
            {
                Console.WriteLine("Error: " + e.Message);
            }
        }

        //se llama a este método con una instancia de la clase Cuenta como argumento
        public static void ConsultarSaldo(Cuenta cuenta)
        {
            Console.WriteLine($"Tu saldo actual es: Q. {cuenta.Saldo}");

            Cuenta.transaccion consulta = new Cuenta.transaccion(cuenta.Saldo, Cuenta.TipoTransaccion.Consulta);
            cuenta.RegistrarTransaccion(consulta);


        }

        // se crea un metodo y se llama a cuenta y a cuentas como argumentos
        public static void RealizarRetiro(Cuenta cuenta, List<Cuenta> cuentas)
        {

            // se ingresa la cantidad que se desea retirar
            Console.WriteLine("Ingrese la cantidad que desea retirar:");
            decimal cantidad = Convert.ToDecimal(Console.ReadLine());

            // se verifica que la cantdidad sea correcta
            if (cantidad > 0 && cantidad <= cuenta.Saldo)


            {
                //se resta la cantidad depositada a el saldo de cuenta
                cuenta.Saldo -= cantidad;
                //se guardan los datos de la transaccion en el metodo "Registrar transaccion"
                cuenta.RegistrarTransaccion(new Cuenta.transaccion(cantidad, Cuenta.TipoTransaccion.Retiro));
                // se muestra el mensaje de la operacion realizada
                Console.WriteLine($"Has retirado Q{cantidad} Tu saldo actual es: Q. {cuenta.Saldo}");
                // se agregan al JSON los cambios y movimientos 
                ActualizarArchivoJson(cuentas);
            }
            else
            {
                Console.WriteLine("Cantidad no válida o insuficiente saldo.");
            }
        }
        //cuenta es un parametro de tipo cuenta
        //cuentas es un parametro de tipo lista que es una clase genérica
        //que representa una colección de objetos de tipo Cuenta.
        public static void RealizarDeposito(Cuenta cuenta, List<Cuenta> cuentas)
        {
            // ingresa la cantidad que se desea depositar
            Console.WriteLine("Ingrese la cantidad que desea depositar:");
            decimal cantidad = Convert.ToDecimal(Console.ReadLine());

            // verifica si la cantidad es correcta
            if (cantidad > 0)
            {
                cuenta.Saldo += cantidad;
                cuenta.RegistrarTransaccion(new Cuenta.transaccion(cantidad, Cuenta.TipoTransaccion.Deposito));
                Console.WriteLine($"Has depositado Q.{cantidad} Tu saldo actual es: Q{cuenta.Saldo}");
                ActualizarArchivoJson(cuentas);
            }
            else
            {
                Console.WriteLine("Cantidad no válida.");
            }
        }
        public static void VerHistorial(Cuenta cuenta)
        {
            //lee historialtransacciones
            Console.WriteLine("Historial de Transacciones:");
            foreach (var transaccion in cuenta.HistorialTransacciones)
            {
                //muestra los datos guardados
                Console.WriteLine($"Fecha: {transaccion.Fecha}, Monto: {transaccion.Monto}, Tipo: {transaccion.Tipo}");
            }
        }

        //cuentas es un parametro de tipo list que a su ves resive valore de la clase cuentas
        //representa la lista de cuentas bancarias que se quiere guardar en el archivo JSON.
        public static void ActualizarArchivoJson(List<Cuenta> cuentas)
        {
            string path = @"C:\Users\User\Desktop\Proyecto Final\ProyectoF\ProyectoF\cuentas\cuentas.json";

            try
            {
                //crea una variable que conviete a JSON los valores de cuentas
                //escribre los datos JSON el path espesificado
                string nuevoJson = JsonConvert.SerializeObject(cuentas);
                File.WriteAllText(path, nuevoJson);
            }
            catch (IOException e)
            {
                Console.WriteLine("Error al actualizar el archivo JSON: " + e.Message);
            }
        }
    }
}
