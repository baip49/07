using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;
using System.Windows.Forms;
using MySql.Data.MySqlClient; // Referencias necesarias para trabajar con localhost

namespace _05
{
    public partial class Form1 : Form
    {
        // Datos de conexión a MySQL
        string conexionSQL = "Server=localhost;Port=3306;Database=pavanzada;Uid=root;Pwd=baip49399;";
        public Form1()
        {
            InitializeComponent();

            // Agregar controladores de eventos TextChanged a los campos
            tbNombre.TextChanged += ValidarNombre;
            tbApellidos.TextChanged += ValidarApellidos;
            tbTelefono.TextChanged += ValidarTelefono;
            tbEstatura.TextChanged += ValidarEstatura;
            tbEdad.TextChanged += ValidarEdad;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string nombres = tbNombre.Text;
            string apellidos = tbApellidos.Text;
            string telefono = tbTelefono.Text;
            string estatura = tbEstatura.Text;
            string edad = tbEdad.Text;
            string genero = "";
            if (rbHombre.Checked)
            {
                genero = "Hombre";
                rbMujer.Checked = false;
            }
            else if (rbMujer.Checked)
            {
                genero = "Mujer";
                rbHombre.Checked = false;
            }

            if (EsEnteroValido(edad) && EsDecimalValido(estatura) && EsEnteroValidoDe10Digitos(telefono) && EsTextoValido(nombres) && EsTextoValido(apellidos))
            {
                // Crear una cadena con los datos
                string datos = $"Nombres: {nombres}\r\nApellidos: {apellidos}\r\nTeléfono: {telefono}\r\nEstatura: {estatura}\r\nEdad: {edad}\r\nGénero: {genero}";

                string rutaArchivo = ("C:\\Users\\byp3n\\Documents\\.UNACH\\Programación avanzada\\07\\datos.txt");
                bool archivoExiste = File.Exists(rutaArchivo);
                Console.WriteLine(archivoExiste);

                // Verificar si el archivo ya existe
                using (StreamWriter writer = new StreamWriter(rutaArchivo, true))
                if (archivoExiste)
                {
                        writer.WriteLine();
                        // Programación de fucionalidad de insert SQL
                        insertarRegistro(nombres, apellidos, int.Parse(edad), decimal.Parse(estatura), telefono, genero);
                        File.WriteAllText(rutaArchivo, datos);
                }
                else
                {
                        writer.WriteLine(datos);
                        // Programación de fucionalidad de insert SQL
                        insertarRegistro(nombres, apellidos, int.Parse(edad), decimal.Parse(estatura), telefono, genero);
                        MessageBox.Show("Datos ingresados correctamente");
                }

                //Mostrar un mensaje con los datos capturados
                MessageBox.Show($"Datos guardados con éxito\n\n{datos}", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Por favor, ingrese datos válidos en los campos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            tbNombre.Clear();
            tbApellidos.Clear();
            tbTelefono.Clear();
            tbEstatura.Clear();
            tbEdad.Clear();
            rbHombre.Checked = false;
            rbMujer.Checked = false;
        }

        private void insertarRegistro(string nombre, string apellidos, int edad, decimal estatura, string telefono, string genero)
        {
            using (MySqlConnection connection = new MySqlConnection(conexionSQL))
            {
                connection.Open();
                string insertQuery = "INSERT INTO persona (Nombre, Apellidos, Telefono, Estatura, Edad, Genero) " + "VALUES (@Nombre, @Apellidos, @Telefono, @Estatura, @Edad, @Genero)";

                using (MySqlCommand command = new MySqlCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@Nombre", nombre);
                    command.Parameters.AddWithValue("@Apellidos", apellidos);
                    command.Parameters.AddWithValue("@Telefono", telefono);
                    command.Parameters.AddWithValue("@Estatura", estatura);
                    command.Parameters.AddWithValue("@Edad", edad);
                    command.Parameters.AddWithValue("@Genero", genero);

                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }


        private bool EsEnteroValido(string valor)
        {
            int resultado;
            return int.TryParse(valor, out resultado);
        }
        private bool EsDecimalValido(string valor)
        {
            decimal resultado;
            return decimal.TryParse(valor, out resultado);
        }
        private bool EsEnteroValidoDe10Digitos(string valor)
        {
            long resultado;
            return long.TryParse(valor, out resultado);
        }
        private bool EsTextoValido(string valor)
        {
            return Regex.IsMatch(valor, @"^[a-zA-Z\s]+$"); // Solo letras y espacios
        }

        private void ValidarEdad(object sender, EventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (!EsEnteroValido(textBox.Text))
            {
                MessageBox.Show("Por favor, ingrese una edad válida", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox.Clear();
            }
        }
        private void ValidarEstatura(object sender, EventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (!EsDecimalValido(textBox.Text))
            {
                MessageBox.Show("Por favor, ingrese una estatura válida", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox.Clear();
            }
        }
        private void ValidarTelefono(object sender, EventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            string input = textBox.Text;
            if (input.Length<10)
            {
                if (!EsEnteroValidoDe10Digitos(input))
                {
                    MessageBox.Show("Por favor, ingrese una estatura válida", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    textBox.Clear();
                }
            } else if (!EsEnteroValidoDe10Digitos(input))
            {
                MessageBox.Show("Por favor, ingrese una estatura válida", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox.Clear();
            }
        }

        private void ValidarNombre(object sender, EventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (!EsTextoValido(textBox.Text))
            {
                MessageBox.Show("Por favor, ingrese un nombre válido (sólo letras y espacios)", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox.Clear();
            }
        }
        private void ValidarApellidos(object sender, EventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (!EsTextoValido(textBox.Text))
            {
                MessageBox.Show("Por favor, ingrese apellidos válidos (sólo letras y espacios)", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox.Clear();
            }
        }
    }
}