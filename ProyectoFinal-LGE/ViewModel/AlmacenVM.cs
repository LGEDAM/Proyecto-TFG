using ProyectoFinal_LGE.Data;
using ProyectoFinal_LGE.Model;
using ProyectoFinal_LGE.View.VentanasAddEdit;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.IO;
using Application = System.Windows.Application;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Windows.Media.Imaging;

namespace ProyectoFinal_LGE.ViewModel
{
    /// <summary>
    /// Vista modelo para almacenes.
    /// </summary>
    public class AlmacenVM : ObservableCollection<Almacen>
    {
        private ObservableCollection<Almacen> listaAlmacenes;

        /// <summary>
        /// Lista de almacenes.
        /// </summary>
        public ObservableCollection<Almacen> ListaAlmacenes
        {
            get { return listaAlmacenes; }
        }

        // Titulo de la cabecera del MessageBox
        string cabeceraInfo = (string)Application.Current.FindResource("informacion");
        // Mensaje del MessageBox
        string mensajeBox = null;

        /// <summary>
        /// Constructor de AlmacenVM.
        /// </summary>
        public AlmacenVM()
        {
            //Creo la lista de almacenes
            listaAlmacenes = new ObservableCollection<Almacen>();

            // LLamo a la instancia y la conecto a la DB
            MySQLDB.Instance.ConnectDB("Almacenes");

            // Recorrer el array de almacenes y coger los almacenes y asginarlos a la lista
            if (MySQLDB.Instance.ArrayAlmacenes != null)
            {
                foreach (Almacen a in MySQLDB.Instance.ArrayAlmacenes)
                {
                    listaAlmacenes.Add(a);
                }
            }
        }

        #region Comandos

        private ICommand btnDeleteAlmacen;
        /// <summary>
        /// Comando del botón eliminar almacén. Llama al método BorrarAlmacen(Almacen).
        /// </summary>
        public ICommand BtnDeleteAlmacen
        {
            get
            {
                if (btnDeleteAlmacen == null)
                {
                    btnDeleteAlmacen = new RelayCommand(param => this.BorrarAlmacen((Almacen)param));
                }
                return btnDeleteAlmacen;
            }
        }


        private ICommand btnAddAlmacen;
        /// <summary>
        ///  Comando del botón añadir almacén. Llama al método AccionAdd().
        /// </summary>
        public ICommand BtnAddAlmacen
        {
            get
            {
                if (btnAddAlmacen == null)
                {
                    btnAddAlmacen = new RelayCommand(param => this.AccionAdd(), null);
                }
                return btnAddAlmacen;
            }
        }

        private ICommand btnEditAlmacen;
        /// <summary>
        /// Comando del botón editar almacén. Llama al método AccionEdit(Almacen).
        /// </summary>
        public ICommand BtnEditAlmacen
        {
            get
            {
                if (btnEditAlmacen == null)
                {
                    btnEditAlmacen = new RelayCommand(param => this.AccionEdit((Almacen)param));
                }
                return btnEditAlmacen;
            }
        }

        private ICommand exportAlmacen;
        /// <summary>
        ///  Comando del botón exportar almacenes. Llama al método AccionExportar(Datagrid).
        /// </summary>
        public ICommand ExportAlmacen
        {
            get
            {
                if (exportAlmacen == null)
                {
                    exportAlmacen = new RelayCommand(param => this.AccionExportar((DataGrid)param));
                }
                return exportAlmacen;
            }
        }


        private ICommand importAlmacen;
        /// <summary>
        ///  Comando del botón importar almacenes. Llama al método AccionImportar().
        /// </summary>
        public ICommand ImportAlmacen
        {
            get
            {
                if (importAlmacen == null)
                {
                    importAlmacen = new RelayCommand(param => this.AccionImportar(), null);
                }
                return importAlmacen;
            }
        }

        private ICommand reportAlmacen;
        /// <summary>
        /// Comando del botón generar reporte de las transacciones de un almacén. Llama al método GenerarReporte(Almacen).
        /// </summary>
        public ICommand ReportAlmacen
        {
            get
            {
                if (reportAlmacen == null)
                {
                    reportAlmacen = new RelayCommand(param => this.GenerarReporte((Almacen)param));
                }
                return reportAlmacen;
            }
        }


        #endregion

        #region Acciones
        /// <summary>
        /// Metodo que abre la ventana de añadir almacen (AddEditAlmacen).
        /// </summary>
        private void AccionAdd()
        {
            AddEditAlmacen page = new AddEditAlmacen(this);
            page.ShowDialog();
        }

        /// <summary>
        /// Método que abre la ventana de modificar almacén (AddEditAlmacen).
        /// </summary>
        /// <param name="a">Almacén a modificar</param>
        private void AccionEdit(Almacen a)
        {
            if (a != null) // Debe estar seleccionado un almacen
            {
                AddEditAlmacen page = new AddEditAlmacen(this, a);
                page.ShowDialog();
            }
            else
            {
                mensajeBox = (string)Application.Current.FindResource("seleccionarAlmacen");
                MessageBox.Show(mensajeBox, cabeceraInfo, MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        /// <summary>
        /// Método que exporta los almacenes a un fichero CSV abriendo el explorador de archivos
        /// para seleccionar donde guardar el fichero. Exportado correctamente, se pregunta si se quiere abrir la información
        /// en Microsoft Excel.
        /// </summary>
        /// <param name="da">Datagrid de almacenes a exportar</param>
        private void AccionExportar(DataGrid da)
        {
            if (da.Items.Count != 0) // debe haber informacion para exportar
            {
                // Abrir explorador de archivos para seleccionar donde guardar el fichero
                System.Windows.Forms.SaveFileDialog saveFileDialog = new System.Windows.Forms.SaveFileDialog();

                // Filtrar fichero por CSV
                saveFileDialog.Filter = "CSV Files| *.csv";
                saveFileDialog.Title = (string)Application.Current.FindResource("guardarFichero");
                saveFileDialog.ShowDialog();

                if (saveFileDialog.FileName != "") // si se selecciona un fichero
                {

                    StringBuilder builder = new StringBuilder();
                    // se escribe en la primera linea del fichero 'Almacenes'
                    string almacenes = (string)Application.Current.FindResource("Almacenes");
                    builder.AppendLine(almacenes);
                    // se añade cada almacen separando los campos por ';'
                    foreach (Almacen a in ListaAlmacenes)
                    {
                        string registro = string.Format("{0};{1};{2}", a.Id, a.Nombre, a.Direccion);
                        builder.AppendLine(registro);
                    }
                    // escribir todo el texto al fichero
                    File.WriteAllText(saveFileDialog.FileName, builder.ToString());

                    // preguntar si se quiere abrir el fichero en Excel
                    mensajeBox = (string)Application.Current.FindResource("exportarExcel");
                    if (MessageBox.Show(mensajeBox, cabeceraInfo, MessageBoxButton.OKCancel, MessageBoxImage.Information) == MessageBoxResult.OK)
                    {
                        ExportarExcel(da); // Exportar a Excel
                    }
                }
            }
            else
            {
                mensajeBox = (string)Application.Current.FindResource("noInfoCSV");
                MessageBox.Show(mensajeBox, cabeceraInfo, MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        /// <summary>
        /// Método que importa almacenes desde un fichero CSV. Abre el explorador de archivos para obtener el fichero
        /// a importar y controla que sea un fichero de almacenes, esten todos los campos, no haya almacenes repetidos y
        /// no esten ciertos campos vacios o con formatos erroneos.
        /// </summary>
        private void AccionImportar()
        {
            // abrir el explorador de archivos
            System.Windows.Forms.OpenFileDialog dialogoBuscarArchivo = new System.Windows.Forms.OpenFileDialog();
            // filtrar for ficheros csv
            dialogoBuscarArchivo.Filter = "CSV Files|*.csv";
            dialogoBuscarArchivo.Title = (string)Application.Current.FindResource("abrirFicheroCSV");
            dialogoBuscarArchivo.FilterIndex = 1;
            dialogoBuscarArchivo.Multiselect = false;
            if (dialogoBuscarArchivo.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                using (Stream stream = dialogoBuscarArchivo.OpenFile()) // abrir el fichero
                {
                    string ruta = dialogoBuscarArchivo.FileName;

                    StreamReader file = new StreamReader(ruta);
                    string line;
                    // Fichero debe comenzar en la primera linea con 'Almacenes'
                    string m = (string)Application.Current.FindResource("Almacenes");
                    if (file.ReadLine().StartsWith(m))
                    {
                        int error = 0; // controlar errores
                        int numLinea = 0; // controlar el numero de linea
                        List<Almacen> arrayAlmacenes = new List<Almacen>(); // almacenes a añadir

                        while ((line = file.ReadLine()) != null) // leer cada linea del fichero
                        {
                            String[] l = line.Split(';'); // obtener campos

                            numLinea++;

                            Almacen a;
                            MySQLDB.Instance.ConnectDB("IdAlmacenes");
                            String[] almacenes = MySQLDB.Instance.almacenes; // obtener id de los almacenes en la BD

                            try
                            {
                                // Campos no esten vacios y tengan el formato correcto
                                if (!l[0].Equals("") && l[0].Length <= 5 && !l[0].Trim().Equals("")
                                    && !l[1].Equals("") && l[1].Length <= 20 && !l[1].Trim().Equals("")
                                    && !l[2].Equals("") && l[2].Length <= 40 && !l[2].Trim().Equals(""))
                                {
                                    // comprobar que el id del almacen no exista ya en la BD
                                    int count = 0;
                                    for (int i = 0; i < almacenes.Count(); i++)
                                    {
                                        if (l[0].Trim().Equals(almacenes[i]))
                                        {
                                            count++;
                                        }
                                    }

                                    if (count == 0) // si no existe
                                    {
                                        a = new Almacen(l[0].Trim(), l[1], l[2]); // creamos almacen
                                        arrayAlmacenes.Add(a); // añadimos a la lista
                                    }
                                    else { error++; break; }
                                }
                                else { error++; break; }
                            }
                            catch (Exception e)
                            {
                                error++;
                                FicheroLog.Instance.EscribirFichero(e.Message);
                                break;

                            }
                        }

                        if (error > 0) // si hay errores se deja de leer el archivo y se muestra el mensaje con la linea del error
                        {
                            mensajeBox = (string)Application.Current.FindResource("errorImportar");
                            string mensajeBox1 = (string)Application.Current.FindResource("errorImportar2");
                            string mensajeBox2 = (string)Application.Current.FindResource("almacenCSV");
                            MessageBox.Show(mensajeBox + " " + numLinea + "\n\n" + mensajeBox1 + "\n\n" + mensajeBox2
                                , cabeceraInfo, MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else // si no hay errores por cada almacen en la lista lo añadimos a la BD
                        {
                            foreach (Almacen a in arrayAlmacenes)
                            {
                                if (MySQLDB.Instance.InsertarAlmacen(a))
                                {
                                    listaAlmacenes.Add(a);
                                }
                            }

                            mensajeBox = (string)Application.Current.FindResource("importarCSVCorrecto");
                            MessageBox.Show(mensajeBox, cabeceraInfo, MessageBoxButton.OK, MessageBoxImage.Information);
                            UIGlobal.PageAlmacen.datagrid.Items.Refresh();

                        }

                    }
                    else // si el fichero no corresponde a almacenes, debe comenzar con 'Almacenes'
                    {
                        mensajeBox = (string)Application.Current.FindResource("importarCSVIncorrecto");
                        string mensajeBox2 = (string)Application.Current.FindResource("Almacenes");
                        MessageBox.Show(mensajeBox + " '" + mensajeBox2 + "'", cabeceraInfo,
                                MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    //cerrar fichero
                    file.Close();
                }
            }
        }

        /// <summary>
        /// Método que elimina el almacén pasado como parámetro.
        /// </summary>
        /// <param name="a">Almacén a eliminar</param>
        public void BorrarAlmacen(Almacen a)
        {
            if (a != null) // debe estar seleccionado un almacen
            {
                // preguntar si desea eliminar el almacen
                mensajeBox = (string)Application.Current.FindResource("deseaEliminarAlmacen");
                if (MessageBox.Show(mensajeBox + "\n" + a.Id + " - " + a.Nombre, cabeceraInfo,
                        MessageBoxButton.OKCancel, MessageBoxImage.Information) == MessageBoxResult.OK)
                {

                    if (MySQLDB.Instance.BorrarAlmacen(a))// eliminar el almacen de la BD
                    {
                        listaAlmacenes.Remove(a);

                        mensajeBox = (string)Application.Current.FindResource("almacenEliminado");
                        MessageBox.Show(mensajeBox, cabeceraInfo, MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            else // si no hay almacen seleccionado
            {
                mensajeBox = (string)Application.Current.FindResource("seleccionarAlmacen");
                MessageBox.Show(mensajeBox, cabeceraInfo, MessageBoxButton.OK, MessageBoxImage.Information);

            }
        }

        /// <summary>
        /// Método que exporta a Microsoft Excel el datagrid con la información de los almacenes. 
        /// </summary>
        /// <param name="da">Datagrid de almacenes a exportar</param>
        private void ExportarExcel(DataGrid da)
        {
            try
            {
                // Abrir Microsoft Excel
                Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
                excel.Visible = true;
                Workbook workbook = excel.Workbooks.Add(System.Reflection.Missing.Value);
                Worksheet sheet = (Worksheet)workbook.Sheets[1];
                // Escribir los nombres de las columnas en negrita
                for (int j = 0; j < da.Columns.Count; j++)
                {
                    sheet.Range["A1"].Offset[0, j].Value = da.Columns[j].Header;
                    sheet.Cells[1, j + 1].Font.Bold = true;
                }
                // Por cada almacen se escriben sus campos en las celdas
                var row = 1;
                foreach (Almacen a in ListaAlmacenes)
                {
                    row++;
                    sheet.Cells[row, "A"] = a.Id;
                    sheet.Cells[row, "B"] = a.Nombre;
                    sheet.Cells[row, "C"] = a.Direccion;
                }

                sheet.Columns.AutoFit(); // Ajustar el contenido
            }
            catch (Exception e) // No existe el programa Microsoft Excel
            {
                mensajeBox = (string)Application.Current.FindResource("microsoftExcel");
                MessageBox.Show(mensajeBox, cabeceraInfo, MessageBoxButton.OK, MessageBoxImage.Information);
                FicheroLog.Instance.EscribirFichero(e.Message);
            }
        }


        /// <summary>
        /// Método que añade el almacén pasado como parámetro a la base de datos comprobando que ciertos campos
        /// no esten vacios y tengan el formato correcto.
        /// </summary>
        /// <param name="a">Almacén a añadir</param>
        public void AddAlmacen(Almacen a)
        {
            // codigo no este vacio y ocupe mas de 5 caracteres
            if (a.Id != null && !a.Id.Trim().Equals("") && a.Id.Length <= 5)
            {
                a.Id = a.Id.Trim();
                // nombre no este vacio y ocupe mas de 20 caracteres
                if (a.Nombre != null && !a.Nombre.Trim().Equals("") && a.Nombre.Length <= 20)
                {
                    // direccion no este vacia y ocupe mas de 40 caracteres
                    if (a.Direccion != null && !a.Direccion.Trim().Equals("") && a.Direccion.Length <= 40)
                    {
                        // codigo no este repetido
                        bool repetidoId = false;
                        for (int i = 0; i < listaAlmacenes.Count; i++)
                        {
                            Almacen am = listaAlmacenes[i];
                            if (am.Id.Equals(a.Id))
                            {
                                repetidoId = true;
                            }
                        }
                        if (!repetidoId) // si el almacen no existe en la BD
                        {
                            // Añadir almacen a la base de datos
                            if (MySQLDB.Instance.InsertarAlmacen(a))
                            {
                                listaAlmacenes.Add(a);// añadir a la lista
                                                      // cerrar ventana
                                UIGlobal.AddEditAlmacen.Close();
                                mensajeBox = (string)Application.Current.FindResource("almacenAñadido");
                                MessageBox.Show(mensajeBox, cabeceraInfo, MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                        }
                        else
                        {
                            mensajeBox = (string)Application.Current.FindResource("almacenIdRepetido");
                            cabeceraInfo = (string)Application.Current.FindResource("error");
                            MessageBox.Show(mensajeBox, cabeceraInfo, MessageBoxButton.OK, MessageBoxImage.Error);
                            UIGlobal.AddEditAlmacen.id.Focus();
                        }

                    }
                    else
                    {
                        mensajeBox = (string)Application.Current.FindResource("almacenDirNoVacio");
                        cabeceraInfo = (string)Application.Current.FindResource("error");
                        MessageBox.Show(mensajeBox, cabeceraInfo, MessageBoxButton.OK, MessageBoxImage.Error);
                        UIGlobal.AddEditAlmacen.dir.Focus();
                    }

                }
                else
                {
                    mensajeBox = (string)Application.Current.FindResource("almacenNombreNoVacio");
                    cabeceraInfo = (string)Application.Current.FindResource("error");
                    MessageBox.Show(mensajeBox, cabeceraInfo, MessageBoxButton.OK, MessageBoxImage.Error);
                    UIGlobal.AddEditAlmacen.nombre.Focus();
                }

            }
            else
            {
                mensajeBox = (string)Application.Current.FindResource("almacenIdNoVacio");
                cabeceraInfo = (string)Application.Current.FindResource("error");
                MessageBox.Show(mensajeBox, cabeceraInfo, MessageBoxButton.OK, MessageBoxImage.Error);
                UIGlobal.AddEditAlmacen.id.Focus();
            }

        }

        /// <summary>
        /// Método que modifica el almacén pasado como parámetro en la base de datos comprobando que ciertos campos
        /// no esten vacios y tengan el formato correcto.
        /// </summary>
        /// <param name="a">Almacén a modificar</param>
        public void EditAlmacen(Almacen a)
        {
            // nombre no este vacio y ocupe mas de 20 caracteres
            if (a.Nombre != null && !a.Nombre.Trim().Equals("") && a.Nombre.Length <= 20)
            {
                // direccion no este vacia y ocupe mas de 40 caracteres
                if (a.Direccion != null && !a.Direccion.Trim().Equals("") && a.Direccion.Length <= 40)
                {
                    if (MySQLDB.Instance.ModificarAlmacen(a))// modificar almacen en la base de datos
                    {
                        //Comprobar cual es el almacen de la lista que tenemos que modificar
                        for (int i = 0; i < listaAlmacenes.Count; i++)
                        {
                            Almacen am = listaAlmacenes[i];
                            if (am.Id.Equals(a.Id))
                            {
                                listaAlmacenes[i] = a;
                            }
                        }
                        // cerra ventana
                        UIGlobal.AddEditAlmacen.Close();
                        mensajeBox = (string)Application.Current.FindResource("almacenActualizado");
                        MessageBox.Show(mensajeBox, cabeceraInfo, MessageBoxButton.OK, MessageBoxImage.Information);
                    }

                }
                else
                {
                    mensajeBox = (string)Application.Current.FindResource("almacenDirNoVacio");
                    cabeceraInfo = (string)Application.Current.FindResource("error");
                    MessageBox.Show(mensajeBox, cabeceraInfo, MessageBoxButton.OK, MessageBoxImage.Error);
                    UIGlobal.AddEditAlmacen.dir.Focus();
                }

            }
            else
            {
                mensajeBox = (string)Application.Current.FindResource("almacenNombreNoVacio");
                cabeceraInfo = (string)Application.Current.FindResource("error");
                MessageBox.Show(mensajeBox, cabeceraInfo, MessageBoxButton.OK, MessageBoxImage.Error);
                UIGlobal.AddEditAlmacen.nombre.Focus();
            }
        }

        /// <summary>
        /// Método que genera un reporte pdf de las transacciones del almacén pasado como parámetro. Comprueba
        /// que este seleccionado un almacén y que el almacén tenga transacciones registradas.
        /// </summary>
        /// <param name="a">Almacén del que realizar el reporte</param>
        private void GenerarReporte(Almacen a)
        {
            if (a != null) // debe seleccionarse un almacen
            {
                MovimientosVM m = new MovimientosVM();
                // Comprobamos todas las transacciones existentes y las que contengan el almacen pasado 
                // las dividimos en dos listas: transacciones de entrada y transacciones de salida
                List<Transaccion> transaccions = m.ListaTransacciones.ToList();
                List<Transaccion> transaccionesEntrada = new List<Transaccion>();
                List<Transaccion> transaccionesSalida = new List<Transaccion>();
                foreach (Transaccion t in transaccions)
                {
                    if (t.AlmacenN.Equals(a.Id))
                    {
                        transaccionesEntrada.Add(t);
                    }
                    if (t.AlmacenV.Equals(a.Id))
                    {
                        transaccionesSalida.Add(t);
                    }

                }

                // Si existen transaccioness ya sean de entrada o salida
                if (transaccionesSalida.Count != 0 || transaccionesEntrada.Count != 0)
                {
                    // Abrimeos el explorador de archivos
                    // Indicamos donde vamos a guardar el documento
                    System.Windows.Forms.SaveFileDialog saveFileDialog = new System.Windows.Forms.SaveFileDialog();
                    // filtar por ficheros pdf
                    saveFileDialog.Filter = "PDF Files| *.pdf";
                    saveFileDialog.Title = (string)Application.Current.FindResource("guardarFichero");
                    saveFileDialog.ShowDialog();

                    try
                    {
                        if (saveFileDialog.FileName != "") // elegimos un fichero
                        {
                            // crear documento A4 con margenes
                            Document doc = new Document(PageSize.A4, 50f, 50f, 60f, 50f);
                            // indicamos donde guardar el documento
                            PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(saveFileDialog.FileName, FileMode.Create));
                            writer.PageEvent = new PageEventHelper();

                            // título y el autor del fichero
                            doc.AddTitle(saveFileDialog.FileName);
                            doc.AddCreator(Usuario.InstanciaUser.User);

                            // Abrimos el archivo
                            doc.Open();

                            //Fuentes
                            iTextSharp.text.Font font = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 11, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
                            iTextSharp.text.Font fontTitulo = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 15, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
                            iTextSharp.text.Font fontSub = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
                            iTextSharp.text.Font fontCellHeaders = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.UNDEFINED, 11, iTextSharp.text.Font.BOLD, BaseColor.BLACK);

                            //Titulo
                            string mensaje = (string)Application.Current.FindResource("TRANSACCIONES");
                            doc.Add(new Paragraph(mensaje + " - " + a.Nombre, fontTitulo));
                            doc.Add(new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(2.0F, 65.3F, BaseColor.GRAY, Element.ALIGN_LEFT, 1))));

                            // Imagen
                            try
                            {
                                string currentAssemblyPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                                System.Windows.Media.Imaging.BitmapImage bitMapImagen =
                                    new System.Windows.Media.Imaging.BitmapImage(new Uri(String.Format("file:///{0}/KarmaIcon.png", currentAssemblyPath)));
                                System.Drawing.Bitmap imagen;
                                using (MemoryStream outStream = new MemoryStream())
                                {
                                    BitmapEncoder enc = new BmpBitmapEncoder();
                                    enc.Frames.Add(BitmapFrame.Create(bitMapImagen));
                                    enc.Save(outStream);
                                    System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(outStream);

                                    imagen = new System.Drawing.Bitmap(bitmap);
                                }
                                iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance((System.Drawing.Image)imagen, BaseColor.BLACK);
                                jpg.ScaleToFit(180f, 130f); //escalar
                                jpg.SpacingBefore = 10f;
                                jpg.SpacingAfter = 1f;
                                jpg.SetAbsolutePosition(370, 715);
                                doc.Add(jpg);

                            }
                            catch (Exception ex)
                            {
                                FicheroLog.Instance.EscribirFichero(ex.Message);
                            }


                            //Fecha - Autor
                            Paragraph p = new Paragraph(DateTime.Now.ToLongDateString() + "  |  " + Usuario.InstanciaUser.User, font);
                            p.Alignment = Element.ALIGN_LEFT;
                            doc.Add(p);
                            doc.Add(Chunk.NEWLINE);
                            doc.Add(Chunk.NEWLINE);
                            doc.Add(Chunk.NEWLINE);
                            doc.Add(Chunk.NEWLINE);
                            doc.Add(new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(2.0F, 100.0F, BaseColor.GRAY, Element.ALIGN_LEFT, 1))));

                            //TABLA SALIDAS
                            int columnas = 4;
                            PdfPTable tablaS = new PdfPTable(columnas);
                            tablaS.TotalWidth = 495f;
                            tablaS.LockedWidth = true;
                            tablaS.DefaultCell.Padding = 5f;
                            // titulo salidas
                            mensaje = (string)Application.Current.FindResource("SALIDAS");
                            doc.Add(new Paragraph(mensaje, fontSub));
                            doc.Add(Chunk.NEWLINE);
                            if (transaccionesSalida.Count != 0) // si hay transacciones
                            {
                                // Escribimos los titulos de las columnas
                                mensaje = (string)Application.Current.FindResource("AlmacenDestino");
                                tablaS.AddCell(new Phrase(mensaje, fontCellHeaders));
                                mensaje = (string)Application.Current.FindResource("Fecha");
                                tablaS.AddCell(new Phrase(mensaje, fontCellHeaders));
                                mensaje = (string)Application.Current.FindResource("Producto");
                                tablaS.AddCell(new Phrase(mensaje, fontCellHeaders));
                                mensaje = (string)Application.Current.FindResource("Unidades");
                                tablaS.AddCell(new Phrase(mensaje, fontCellHeaders));

                                foreach (Transaccion t in transaccionesSalida)
                                // por cada transaccion en las lista la añadimos a la tabla
                                {
                                    tablaS.AddCell(new Phrase(t.AlmacenN, font));
                                    tablaS.AddCell(new Phrase(t.Fecha.ToShortDateString(), font));
                                    tablaS.AddCell(new Phrase(t.Producto, font));
                                    tablaS.AddCell(new Phrase(t.Unidades.ToString(), font));

                                }
                            }
                            // añadir tabla de salidas al documento
                            doc.Add(tablaS);
                            doc.Add(Chunk.NEWLINE);
                            doc.Add(Chunk.NEWLINE);
                            doc.Add(Chunk.NEWLINE);
                            doc.Add(Chunk.NEWLINE);
                            doc.Add(Chunk.NEWLINE);

                            //TABLA ENTRADAS
                            PdfPTable tablaE = new PdfPTable(columnas);
                            tablaE.TotalWidth = 495f;
                            tablaE.LockedWidth = true;
                            tablaE.DefaultCell.Padding = 5f;
                            // titulo entradas
                            mensaje = (string)Application.Current.FindResource("ENTRADAS");
                            doc.Add(new Paragraph(mensaje, fontSub));
                            doc.Add(Chunk.NEWLINE);
                            if (transaccionesEntrada.Count != 0)// si hay transacciones
                            {
                                // Escribimos los titulos de las columnas
                                mensaje = (string)Application.Current.FindResource("AlmacenOrigen");
                                tablaE.AddCell(new Phrase(mensaje, fontCellHeaders));
                                mensaje = (string)Application.Current.FindResource("Fecha");
                                tablaE.AddCell(new Phrase(mensaje, fontCellHeaders));
                                mensaje = (string)Application.Current.FindResource("Producto");
                                tablaE.AddCell(new Phrase(mensaje, fontCellHeaders));
                                mensaje = (string)Application.Current.FindResource("Unidades");
                                tablaE.AddCell(new Phrase(mensaje, fontCellHeaders));

                                foreach (Transaccion t in transaccionesEntrada)
                                // por cada transaccion en las lista la añadimos a la tabla
                                {
                                    tablaE.AddCell(new Phrase(t.AlmacenV, font));
                                    tablaE.AddCell(new Phrase(t.Fecha.ToShortDateString(), font));
                                    tablaE.AddCell(new Phrase(t.Producto, font));
                                    tablaE.AddCell(new Phrase(t.Unidades.ToString(), font));
                                }
                            }
                            // añadir al documento
                            doc.Add(tablaE);
                            doc.Add(Chunk.NEWLINE);

                            // cerrar
                            doc.Close();
                            writer.Close();

                            // mensaje del reporte se ha creado correctamente
                            mensajeBox = (string)Application.Current.FindResource("reporteAlmacen");
                            cabeceraInfo = (string)Application.Current.FindResource("informacion");
                            MessageBox.Show(mensajeBox, cabeceraInfo, MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                    catch (Exception e)// error al escribir el reporte
                    {
                        mensajeBox = (string)Application.Current.FindResource("reporteAbierto");
                        cabeceraInfo = (string)Application.Current.FindResource("error");
                        MessageBox.Show(mensajeBox, cabeceraInfo, MessageBoxButton.OK, MessageBoxImage.Error);
                        FicheroLog.Instance.EscribirFichero(e.Message);
                    }


                }
                else // no hay transacciones asociadas al almacen pasado
                {
                    mensajeBox = (string)Application.Current.FindResource("almacenNoReporte");
                    MessageBox.Show(mensajeBox, cabeceraInfo, MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else // si no se ha seleccionado un almacen
            {
                mensajeBox = (string)Application.Current.FindResource("seleccionarAlmacenReporte");
                MessageBox.Show(mensajeBox, cabeceraInfo, MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        /// <summary>
        /// Clase que hereda de PdfPageEventHelper y contiene los método necesarios para modificar los 
        /// encabezados y pies de página de un documento pdf.
        /// </summary>
        public class PageEventHelper : PdfPageEventHelper
        {

            /// <summary>
            /// Método que permite establecer un pie de página del documento y writer pasado como parámetros.
            /// </summary>
            /// <param name="writer"></param>
            /// <param name="document"></param>
            public override void OnEndPage(PdfWriter writer, Document document)
            {
                Paragraph footer = new Paragraph("- " + writer.PageNumber + " -", FontFactory.GetFont(FontFactory.HELVETICA, 10, iTextSharp.text.Font.NORMAL));

                PdfContentByte canvas = writer.DirectContent;

                writer.SetMargins(20f, 20f, 20f, 20f);
                document.SetMargins(20f, 20f, 20f, 20f);
                ColumnText.ShowTextAligned(canvas, Element.ALIGN_CENTER, footer, 300, 30, 0);


            }

        }
        #endregion

    }
}
