using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;
namespace Lab01;
public class Program
{
    public static void Main()
    {
        try
        {
            AVLTree<Persona> arbolPersonas = new AVLTree<Persona>();
            string route = @"C:\Users\AndresLima\Desktop\input.csv";
            if (File.Exists(route))
            {
                string[] FileData = File.ReadAllLines(route);
                foreach (var item in FileData)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        string[] fila = item.Split(";");
                        Persona? persona = JsonSerializer.Deserialize<Persona>(fila[1]);
                        if (fila[0] == "INSERT")
                        {
                            arbolPersonas.Add(persona!, Delegates.NameComparison, Delegates.DPIComparison);
                        }
                        else if (fila[0] == "DELETE")
                        {
                            arbolPersonas.Delete(persona!, Delegates.NameComparison, Delegates.DPIComparison);
                        }
                        else if (fila[0] == "PATCH")
                        {
                            arbolPersonas.Patch(persona!, Delegates.NameComparison, Delegates.DPIComparison);
                        }
                    }                    
                }

            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Ha ocurrido un error inesperado");
        }
        Console.ReadKey();
    }
}
