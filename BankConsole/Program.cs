
using BankConsole;
using System;
using System.Threading;

if (args.Length == 0)
    EmailService.SendMail();
else
    ShowMenu();

void ShowMenu()
{
    Console.Clear();
    Console.WriteLine("Selecciona una opcion:");
    Console.WriteLine("1 - Crear un Usuario nuevo.");
    Console.WriteLine("2 - Elimiar un Usuario existente.");
    Console.WriteLine("3 - Salir.");

    int option = 0;
    do
    {
        string input = Console.ReadLine();

        if (!int.TryParse(input, out option))
            Console.WriteLine("Debes ingresar un numero (1, 2 o 3).");
        else if (option > 3)
            Console.WriteLine("Debes agregar un numero valido (1, 2 o 3).");
    }
    while (option == 0 || option > 3);

    switch (option)
    {
        case 1:
            CreatUser();
            break;
        case 2:
            DeleteUser();
            break;
        case 3:
            Environment.Exit(0);
            break;
    }
}

void CreatUser()
{
    Console.Clear();
    Console.WriteLine("Ingresa la informacion del usuario:");

    int ID;
    do
    {
        Console.WriteLine("Ingresa un numero entero y positivo.");
        Console.Write("ID: ");
    } while (!int.TryParse(Console.ReadLine(), out ID) || ID <= 0);

    string Name;
    do
    {
        Console.Write("Nombre: ");
        Name = Console.ReadLine();
    } while (string.IsNullOrWhiteSpace(Name));

    string Email;
    do
    {
        Console.WriteLine("Por favor ingresa un correo valido.");
        Console.Write("Email: ");
        Email = Console.ReadLine();
    } while (!IsValidEmail(Email));

    decimal Balance;
    do
    {
        Consle.WriteLine("Por favor ingresa un saldo mayor a 0");
        Console.Write("Saldo: ");
    } while (!decimal.TryParse(Console.ReadLine(), out Balance) || Balance < 0);

    char userType;
    do
    {
        Console.WriteLine("Por favor asegurate que hayas usado 'c' o 'e' para este espacio.");
        Console.Write("Escribe 'c' si el usuario es Cliente, 'e' si es Empleado: ");
    } while (!char.TryParse(Console.ReadLine(), out userType) || (userType != 'c' && userType != 'e'));

    User newUser;

    if (userType == 'c')
    {
        char TaxRegime;
        do
        {
            Console.Write("Regimen Fiscal: ");
        } while (!char.TryParse(Console.ReadLine(), out TaxRegime));

        newUser = new Client(ID, Name, Email, Balance, TaxRegime);
    }
    else
    {
        string Department;
        do
        {
            Console.Write("Departamento: ");
            Department = Console.ReadLine();
        } while (string.IsNullOrWhiteSpace(Department));

        newUser = new Employee(ID, Name, Email, Balance, Department);
    }

    Storage.AddUser(newUser);

    Console.WriteLine("Usuario creado.");
    Thread.Sleep(2000);
    ShowMenu();
}

void DeleteUser()
{
    Console.Clear();

    int ID;
    do
    {
        Console.Write("Ingresa el ID del usuario a eleminar: ");
    } while (!int.TryParse(Console.ReadLine(), out ID));

    string result = Storage.DeleteUser(ID);

    if (result.Equals("Success"))
    {
        Console.Write("Usuario eliminado.");
        Thread.Sleep(2000);
        ShowMenu();
    }
}

bool IsValidEmail(string email)
{
    try
    {
        var addr = new System.Net.Mail.MailAddress(email);
        return addr.Address == email;
    }
    catch
    {
        return false;
    }
}
