using System;
using System.IO;

namespace TransactionsApps
{
  class Program
  {
    private readonly TransaksiCRUDRepository transaksiCRUDRepository = new();
    private readonly TransaksiSortRepository transaksiSortRepository = new();
    private string menu, command;

    static void Main(string[] args)
    {
      Program p = new();
      if (!File.Exists("pembelian.csv"))
      {
        CreateCsv();
      }

      while (p.menu != "exit" && p.command != "exit" )
      {
        if (p.command == "" || p.command == null || p.command == "menu")
        {
          p.InitMenu();
        } else
        {
          p.InitCommand();
        }
      }     
    }

    private static async void CreateCsv()
    {
      string[] pembelianHeader =
        {
            "ID;Tanggal;Keterangan Pembelian;Sebesar"
        };

      await File.WriteAllLinesAsync("pembelian.csv", pembelianHeader);

      string[] penjualanHeader =
        {
            "ID;Tanggal;Keterangan Penjualan;Sebesar"
        };

      await File.WriteAllLinesAsync("penjualan.csv", penjualanHeader);
    }

    private void InitMenu()
    {
      Console.Write("\nMenu (pembelian|penjualan|exit): ");
      menu = Console.ReadLine();
      if (menu == "pembelian" || menu == "penjualan")
      {
        InitCommand();
      } else if (menu == "exit")
      {

      } else
      {
        Console.Write("\nMenu tidak tersedia. Silahkan coba kembali.\n");
        InitMenu();
      }
    }

    private void InitCommand()
    {
      Console.Write($"\nPerintah pada menu {menu} (create|read|update|delete|sort|menu|exit): ");
      command = Console.ReadLine();
      if (command == "create" || command == "read" || command == "update" || command == "delete"|| command == "sort")
      {
        Console.Write("\n");
        switch (command)
        {
          case "create":
            transaksiCRUDRepository.Create(menu);
            break;
          case "read":
            transaksiCRUDRepository.Read(menu);
            break;          
          case "update":
            transaksiCRUDRepository.Update(menu);
            break;
          case "delete":
            transaksiCRUDRepository.Delete(menu);
            break;

          case "sort":
            transaksiSortRepository.Sort(menu);
            break;
          default:
            break;
        }
      }
      else if (command == "menu" || command == "exit")
      {

      }
      else
      {
        Console.Write("\nPerintah tidak tersedia. Silahkan coba kembali.\n");
        InitCommand();
      }
    }
  }
}
